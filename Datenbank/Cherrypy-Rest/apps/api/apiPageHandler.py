#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# Implementation of the RestAPI
#

# sql injection
# Allowed charaters
# Guestbook XSS


__author__ = "space"

from IPython.core.debugger import Tracer

import re
import uuid
import hashlib
import datetime
import md5
import json
import string
import random
import cherrypy
import peewee
from peewee import *

import logging
logging.basicConfig(level=logging.DEBUG)
log = logging.getLogger(__name__)

db_conn = MySQLDatabase(None)


class Api(object):
	def __init__(self, db_host, db_database, db_user, db_password):
		self.db_host = db_host
		self.db_database = db_database
		self.db_user = db_user
		self.db_password = db_password

		self.session = dict()

		db_conn.init (db_database,
					  host=db_host, 
					  user=db_user, 
					  passwd=db_password)
		db_conn.connect()

		#setup_db()

# {{{ Unexposed Methods
	def now(self):
		return int(datetime.datetime.utcnow().strftime('%s'))

	def generateRandomString(self, length=20):
		character_set = string.digits + string.letters + string.punctuation
		return ''.join(random.choice(character_set) for c in range(length))

	def checkUserExists(self, username):
		if ( User.select().where(User.username == username).exists() ):
			return (True)
		else: 
			return (False)

	def checkToken(self, username, token):
		if username not in self.session:
			return (False)

		session_secret          = self.session[username]["session_secret"]
		session_command_counter = self.session[username]["command_counter"]
		check_token = hashlib.md5((session_secret + str(session_command_counter)).encode('utf-8')).hexdigest()

		if (token == check_token):
			self.session[username]["command_counter"] += 1
			return (True)
		else: return (False)

	def allLogbookEntriesByUser(self, username):
		logbook_select = """
		SELECT *
		FROM logbook
		WHERE user_id = '%s'
		ORDER BY id;
		""" % (username)

		logbook_query = Logbook.raw(logbook_select)
		all_entries = list()
		for log in logbook_query:
			all_entries.append(log)

		return all_entries

	def lastLogbookEntryByUser(self, username):
		all_entries = self.allLogbookEntriesByUser(username)
		print(all_entries)
		if all_entries == []:
			return None
		else:
			print(all_entries)
			return all_entries[-1]

	def nextCache(self, username):

		lastLogbook = self.lastLogbookEntryByUser(username)

		nextCache = None
		if lastLogbook is None:
			# TODO: Maybe this could be made less static.
			nextCache = Geocache.select().where( Geocache.cachename == "bib-Eingang").get()
		elif lastLogbook.cache == lastLogbook.cache.next_cache:
			nextCache = None
		else:
			nextCache = lastLogbook.cache.next_cache

		return nextCache

# }}}

# {{{ No Authentication required
	def index(self):
		return ("You found the Rest-Api. Now try to actually use it...")

	def register(self, username, password):
		message = dict()
		if ( self.checkUserExists(username) ):
			log.warning("Could not register %s. Username already taken." % (username))
			message["success"] = False
			message["error"]   = "Username already taken."
			return (json.dumps(message))

		if ( len(username) > 30):
			log.warning("Could not register %s. Username too long." % (username))
			message["success"] = False
			message["error"]   = "Username too long."
			return (json.dumps(message))
		
		password_salt = self.generateRandomString()
		password_hash = hashlib.md5((password + password_salt).encode('utf-8')).hexdigest()

		user = User()
		user.username = username
		user.password_salt = password_salt
		user.password_hash = password_hash
		user.save(force_insert=True)
		
		message["success"] = True
		return (json.dumps(message))

	def login(self, username, password):
		message = dict()		
		if not ( self.checkUserExists(username) ):
			message["success"] = False
			message["error"]   = "Username doesn't exist."
			return (json.dumps(message))

		user = User.select().where(User.username == username).get()
		password_salt = user.password_salt
		password_hash = hashlib.md5((password + password_salt).encode('utf-8')).hexdigest()

		if ( user.password_hash != password_hash ):
			message["success"] = False
			message["error"]   = "Wrong Password."
			return (json.dumps(message))

		user_session = {"command_counter": 0,
						"session_secret":  self.generateRandomString()}

		self.session[username] = user_session

		message["success"]        = True
		message["session_secret"] = self.session[username]["session_secret"]
		return (json.dumps(message))

	def getUsers(self):

		message = dict()

		users_select = """
		SELECT *
		FROM user"""
		users_query = User.raw(users_select)
		print(users_query)

		usernames = []
		for user in users_query:
			usernames.append(user.username)

		message["users"] = usernames
		message["success"]  = True
		return (json.dumps(message))

	def getPositionsMap(self):

		message = dict()

		recently = (datetime.datetime.utcnow()-datetime.timedelta(minutes=5)).strftime('%s')
		current_positions_select = """
		SELECT p.id, p.latitude, p.longitude, p.recorded_date, p.user_id
		FROM (
			SELECT user_id, MAX(recorded_date) "recorded_date"
			FROM positionlog
			GROUP BY user_id) AS a, positionlog p
		WHERE p.recorded_date = a.recorded_date
		AND p.user_id = a.user_id
		AND p.recorded_date > %s""" % (recently)

		user_map = dict()
		current_positions_query = PositionLog.raw(current_positions_select)
		for pos in current_positions_query:
			user_map[pos.user.username] = (pos.longitude, pos.latitude)

		message["user_map"] = user_map
		message["success"]  = True
		return (json.dumps(message))

	def getTopTenScoresForAllMinigames(self, username=None ):
		message = dict()
		if username != None and not ( self.checkUserExists(username) ):
			message["success"] = False
			message["error"]   = "Username doesn't exist."
			return (json.dumps(message))

		minigames_select = """
		SELECT *
		FROM minigame m
		"""	

		game = dict()
		minigame_query = Minigame.raw(minigames_select)
		for minigame in minigame_query:
			game[minigame.name] = dict()

			if username != None:
				player_restriction = "AND s.user_id = '%s'" % (username)
			else:
				player_restriction = ""

			top_ten_select = """
			SELECT *
			FROM minigame m, score s
			WHERE m.id = %s
			AND s.game_id = m.id
			%s
			ORDER BY s.points DESC
			LIMIT 10
			""" % (minigame.get_id(), player_restriction)

			top_ten_query = Score.raw(top_ten_select)
			for score in top_ten_query:
				game[minigame.name][score.user.username] = (score.points, score.play_date)

		message["game"] = game
		message["success"] = True
		return (json.dumps(message))

	def getAllLogbookEntriesByUser(self, username):
		message = dict()
		if username != None and not ( self.checkUserExists(username) ):
			message["success"] = False
			message["error"]   = "Username doesn't exist."
			return (json.dumps(message))

		entries = list()
		for entry in self.allLogbookEntriesByUser(username):
			entries.append({"puzzle_solved": entry.puzzle_solved, 
							"message": entry.message,
							"found_date": entry.found_date,
							"cache": entry.cache.cachename,
							"user": entry.user.username})

		message["entries"] = entries
		message["success"] = True
		return (json.dumps(message))

	def secretValidForNextCache(self, username, cache_secret):
		message = dict()
		if username != None and not ( self.checkUserExists(username) ):
			message["success"] = False
			message["error"]   = "Username doesn't exist."
			return (json.dumps(message))
		
		next_cache = self.nextCache(username)
		if next_cache is None:
			message["success"] = False
			message["error"]   = "All caches already solved."
		elif next_cache.secret == cache_secret:
			message["success"] = True
		else:
			message["success"] = False
			message["error"]   = "Wrong secret."

		return (json.dumps(message))

	def makeGuestbookEntry(self, author, message_str):
		message = dict()
		
		entry = Guestbook()
		entry.author        = author
		entry.message       = message_str
		entry.recorded_date = self.now()
		entry.save(force_insert=True)
		
		message["success"] = True
		return (json.dumps(message))

	def getGuestbookIndex(self):
		message = dict()

		index_select = """
		SELECT id
		FROM guestbook
		ORDER BY recorded_date DESC"""
		index_query = Guestbook.raw(index_select)

		index = []
		for entry in index_query:
			index.append(entry.id)

		message["index"] = index
		message["success"]  = True
		return (json.dumps(message))

	def getGuestbookEntryById(self, id):
		message = dict()

		index_select = """
		SELECT *
		FROM guestbook
		WHERE id = %s""" % (id)
 		index_query = Guestbook.raw(index_select)

		for entry in index_query:
			message["id"]      = entry.id
			message["author"]  = entry.author
			message["message"] = entry.message
			message["date"]    = entry.recorded_date

			message["success"]  = True
			return (json.dumps(message))

		message["success"]  = False
		message["error"]   = "No Entry by that id."
		return (json.dumps(message))


# }}}

# {{{ Authentication required
	def nop(self, username, token):
		"""Diese Funktion demonstriert wie eine API Funktion mit Authentifizierung funktioniert.
		   Sie zählt den Befehlszähler hoch und überprüft das token."""

		message = dict()
		if not (self.checkToken(username, token)):
			message["success"] = False
			message["error"]   = "Invalid Authentication Token."
			return (json.dumps(message))
		else:
			message["success"] = True
			return (json.dumps(message))

	def updatePosition(self, username, token, longitude, latitude):

		message = dict()
		if not (self.checkToken(username, token)):
			message["success"] = False
			message["error"]   = "Invalid Authentication Token."
			return (json.dumps(message))

		accuracy_re = r"^\d+[.]\d{6,}$"
		if not ( re.match(accuracy_re, longitude) and
				 re.match(accuracy_re, latitude) ):
			message["success"] = False
			message["error"]   = "Wrong Format or Insufficient position accurarcy."
			return (json.dumps(message))

		pos = PositionLog()
		pos.user          = username
		pos.recorded_date = self.now()
		pos.latitude      = float(latitude)
		pos.longitude     = float(longitude)
		pos.save(force_insert=True)

		message["success"] = True
		return (json.dumps(message))

	def makeLogbookEntry(self, username, token, secret, message_str):
		message = dict()
		if not (self.checkToken(username, token)):
			message["success"] = False
			message["error"]   = "Invalid Authentication Token."
			return (json.dumps(message))

		next_cache = self.nextCache(username)
		if next_cache is None:
			message["success"] = False
			message["error"]   = "All caches already solved."
			return (json.dumps(message))
			
		if next_cache.secret != secret:
			message["success"] = False
			message["error"]   = "Wrong secret."
			return (json.dumps(message))

		if self.lastLogbookEntryByUser(username) != None:
			if self.lastLogbookEntryByUser(username).puzzle_solved == False:
				message["success"] = False
				message["error"]   = "Puzzle not yet solved."
				return (json.dumps(message))


		log = Logbook()
		log.puzzle_solved = False
		log.message       = message_str
		log.found_date    = self.now()
		log.cache         = next_cache
		log.user          = username
		log.save(force_insert=True)

		message["success"] = True
		return (json.dumps(message))

	def markPuzzleSolved(self, username, token):
		message = dict()
		if not (self.checkToken(username, token)):
			message["success"] = False
			message["error"]   = "Invalid Authentication Token."
			return (json.dumps(message))

		lastLogbook = self.lastLogbookEntryByUser(username)

		if lastLogbook is not None:
			lastLogbook.puzzle_solved = True
			lastLogbook.save()
			message["success"] = True		

		else:
			message["success"] = True
		return (json.dumps(message))

	def submitGameScore(self, username, token, points, cache):

		message = dict()
		if not (self.checkToken(username, token)):
			message["success"] = False
			message["error"]   = "Invalid Authentication Token."
			return (json.dumps(message))

		if not Geocache.select().where( Geocache.cachename == cache).exists():
			message["success"] = False
			message["error"]   = "Cache not in database."
			return (json.dumps(message))

		relevant_cache = Geocache.select().where( Geocache.cachename == cache).get()
		if not relevant_cache in self.allLogbookEntriesByUser(Username):
			message["success"] = False
			message["error"]   = "User didn't find that cache yet."
			return (json.dumps(message))

		sc = Score()
		sc.user      = username
		sc.cache     = cache
		sc.points    = points
		sc.play_date = self.now()
		sc.save(force_insert=True)

		message["success"] = True
		return (json.dumps(message))

# }}}


# {{{ Database wrapper
class BaseModel(peewee.Model):
	class Meta: 
		database = db_conn

#class Minigame(BaseModel):
#	minigamename = CharField(primary_key=True)

class User(BaseModel):
	username      = CharField(primary_key=True)
	password_hash = TextField()
	password_salt = TextField()

class Geocache(BaseModel):
	cachename    = CharField(primary_key=True)
	latitude     = DoubleField()
	longitude    = DoubleField()
	secret       = CharField()
	next_cache   = ForeignKeyField('self', related_name='next')

class Logbook(BaseModel):
	logbook_id    = PrimaryKeyField
	puzzle_solved = BooleanField()
	message       = CharField()
	found_date    = IntegerField()
	cache         = ForeignKeyField(Geocache, related_name='findings')
	user          = ForeignKeyField(User, related_name='logbookEntries')

class PositionLog(BaseModel):
	position_log_id = PrimaryKeyField
	user            = ForeignKeyField(User, related_name='positions')
	latitude        = DoubleField()
	longitude       = DoubleField()
	recorded_date   = IntegerField()

class Score(BaseModel):
	score_id  = PrimaryKeyField
	cache     = ForeignKeyField(Geocache, related_name='gameScores')	
	user      = ForeignKeyField(User, related_name='playedRounds')
#	game      = ForeignKeyField(Minigame, related_name='gameScores')
	points    = IntegerField()
	play_date = IntegerField()

class Guestbook(BaseModel):
	guestbook_id  = PrimaryKeyField
	author        = CharField()
	message       = CharField()
	recorded_date = IntegerField()

# }}}



def setup_db():
	User.create_table()
	#Minigame.create_table()
	Geocache.create_table()
	Logbook.create_table()
	Score.create_table()

	PositionLog.create_table()
	Guestbook.create_table()

	user = User()
	user.username = "testUser"
	user.password_salt = "hb/ynyeWg'LHR%=cgQ~,"
	user.password_hash = "f4669ab43b879253c96375caa2349dde"
	user.save(force_insert=True)

	gc = Geocache()
	gc.cachename	= "Serverraum"
	gc.latitude		= 51.73106
	gc.longitude	= 8.73635
	gc.secret		= 'df5a8617'
	gc.next_cache	= "Serverraum" #  TODO: none?
	gc.save(force_insert=True)

	gc = Geocache()
	gc.cachename	= "Fluss"
	gc.latitude		= 51.73064
	gc.longitude	= 8.73554
	gc.secret		= '4f1fc70d'
	gc.next_cache	= "Serverraum"
	gc.save(force_insert=True)

	gc = Geocache()
	gc.cachename	= "Wohnheim"
	gc.latitude		= 51.72956
	gc.longitude	= 8.7374
	gc.secret		= '8a1b32fa'
	gc.next_cache	= "Fluss"
	gc.save(force_insert=True)

	gc = Geocache()
	gc.cachename	= "HNF"
	gc.latitude		= 51.73147
	gc.longitude	= 8.73618
	gc.secret		= 'b7a34174'
	gc.next_cache	= "Wohnheim"
	gc.save(force_insert=True)

	gc = Geocache()
	gc.cachename	= "Zukunftsmeile"
	gc.latitude		= 51.73057
	gc.longitude	= 8.73807
	gc.secret		= 'd1741e41'
	gc.next_cache	= "HNF"
	gc.save(force_insert=True)				

	gc = Geocache()
	gc.cachename	= "bib-Eingang"
	gc.latitude		= 51.73075
	gc.longitude	= 8.73707
	gc.secret		= '8e71bee3'
	gc.next_cache	= "Zukunftsmeile"
	gc.save(force_insert=True)
