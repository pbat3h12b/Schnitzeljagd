#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# Implementation of the RestAPI
#


__author__ = "Patrick Meyer"

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
from playhouse.pool import *

import logging
logging.basicConfig(level=logging.DEBUG)
log = logging.getLogger(__name__)

db_host = '10.0.3.75'
#	db_host = 'localhost'
db_database = 'geo'
db_user = 'berry_pink'
db_password = 'mellow_yellow'

db_conn = PooledMySQLDatabase(db_database,
							  max_connections=0,
							  stale_timeout=600,
							  host=db_host, 
							  user=db_user, 
							  passwd=db_password,
							  threadlocals=True)

class Api(object):
	def __init__(self):
		self.session = dict()

		db_conn.connect()

		#setup_db()

# {{{ Unexposed Methods - These methods are not exposed in the API.
	def now(self):
		"""
		Returns UTC Unixtimestamp.
		"""
		return int(datetime.datetime.utcnow().strftime('%s'))

	def generateRandomString(self, length=20):
		"""
		Returns random string with only printable ascii characters of the length, length.
		This Function is used to generate session secrets or the password salt.
		"""
		character_set = string.digits + string.letters + string.punctuation
		return ''.join(random.choice(character_set) for c in range(length))

	def checkUserExists(self, username):
		"""
		Checks if username has account in database.
		"""
		if ( User.select().where(User.username == username).exists() ):
			return (True)
		else: 
			return (False)

	def checkToken(self, username, token):
		"""
		Checks if session is valid and updates session.
		"""
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
		"""
		Returns logbook entries from database by user.
		"""		
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
		"""
		Returns last logbook entry from database by user.
		In case no cache was found yet, None is returned.
		"""
		all_entries = self.allLogbookEntriesByUser(username)
		if all_entries == []:
			return None
		else:
			return all_entries[-1]

	def allCachesFoundByUser(self, username):
		"""
		Returns the caches from the logbook entries returned by allLogbookEntriesByUser.
		"""
		entries = self.allLogbookEntriesByUser(username)
		caches = []
		for e in entries:
			caches.append(e.cache)

		return caches

	def nextCache(self, username):
		"""
		Returns the next cache after the last found cache by the specified user.
		In case no cache was found yet, the first cache is returned.
		In case the last cache was found, None is returned.
		"""
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

	def html_escape(self, text):
		"""
		Mitigate HTML / JS / CSS / $Whatever in text string.
		This method is used to sanitise guestbook entries.
		"""

		escape_table = {
			"&": "&amp;",
			'"': "&quot;",
			"'": "&apos;",
			">": "&gt;",
			"<": "&lt;",
		}

		return "".join(escape_table.get(char,char) for char in text)

# }}}

# {{{ No Authentication required
	def index(self):
		"""
		Exposed in API
		"""
		return ("You found the Rest-Api. Now try to actually use it...")

	def register(self, username, password):
		"""
		Exposed in API
		Creates user in database.
		Fails if username is already taken.
		Fails if username is too long.
		"""
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
		"""
		Exposed in API
		Returnes session secret in case the login succeeded.
		Fails if username doesn't exist.
		Fails if password is wrong.
		"""
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
		"""
		Exposed in API
		Returns a list of all registered users.
		"""
		message = dict()

		users_select = """
		SELECT *
		FROM user"""
		users_query = User.raw(users_select)

		usernames = []
		for user in users_query:
			usernames.append(user.username)

		message["users"] = usernames
		message["success"]  = True
		return (json.dumps(message))

	def getPositionsMap(self):
		"""
		Exposed in API
		Returns a dictionary, mapping usernames to their last known postitions.
		Only position updates made during the last five minutes are taken into account.
		"""

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
		AND p.recorded_date > %s"""

		user_map = dict()
		current_positions_query = PositionLog.raw(current_positions_select, recently)
		for pos in current_positions_query:
			user_map[pos.user.username] = (pos.longitude, pos.latitude)

		message["user_map"] = user_map
		message["success"]  = True
		return (json.dumps(message))

	def getUserPath(self, username):
		"""
		Exposed in API
		Returns a list of usernames postions during the last 20 Minutes.
		Fails if username doesn't exist.
		"""

		message = dict()
		if username != None and not ( self.checkUserExists(username) ):
			message["success"] = False
			message["error"]   = "Username doesn't exist."
			return (json.dumps(message))

		recently = (datetime.datetime.utcnow()-datetime.timedelta(minutes=20)).strftime('%s')
		positions_select = """
		SELECT p.id, p.latitude, p.longitude, p.recorded_date, p.user_id
		FROM positionlog p
		WHERE p.user_id = '%s'
		AND p.recorded_date > %s
		ORDER BY p.recorded_date"""

		positions = list()
		positions_query = PositionLog.raw(positions_select, username, recently)
		for pos in current_positions_query:
			positions[pos.user.username] = (pos.longitude, pos.latitude)

		message["positions"] = positions
		message["success"]  = True
		return (json.dumps(message))		

	def getTopTenScoresForAllMinigames(self, username = None ):
		"""
		Exposed in API
		Returns a dictionary, mapping each geocache-game to a list,
		containing the the top ten scores and the responsible player.
		The function can be limited to only take scores submitted by 
		username into account.
		Fails if username doesn't exist.		
		"""			
		message = dict()
		if username != None and not ( self.checkUserExists(username) ):
			message["success"] = False
			message["error"]   = "Username doesn't exist."
			return (json.dumps(message))

		cache_select = """
		SELECT *
		FROM geocache m
		"""	

		game = dict()
		geocache_query = Geocache.raw(cache_select)
		for cache in geocache_query:
			game[cache.cachename] = list()

			if username is not None:
				top_ten_select = """
				SELECT *
				FROM geocache g, score s
				WHERE g.cachename = '%s'
				AND s.cache_id = g.cachename
				AND s.user_id = '%s'
				ORDER BY s.points DESC
				LIMIT 10
				""" % (cache.get_id(), username)
			else:
				top_ten_select = """
				SELECT *
				FROM geocache g, score s
				WHERE g.cachename = '%s'
				AND s.cache_id = g.cachename
				ORDER BY s.points DESC
				LIMIT 10
				""" % (cache.get_id())

			top_ten_query = Score.raw(top_ten_select)
			for score in top_ten_query:
				game[cache.cachename].append({
					"username": score.user.username,
					"points":	score.points,
					"date":		score.play_date
				})

		message["game"] = game
		message["success"] = True
		return (json.dumps(message))

	def getAllLogbookEntriesByUser(self, username):
		"""
		Exposed in API
		Returns a list, containing usernames logbook entries.
		Fails if username doesn't exist.
		"""			
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
		"""
		Exposed in API
		Checks if cache_secret is indeed the needed secret to unlock the
		next unfound cache by username.
		This function can be used to validate a users entry before moving
		on to the cachebook entry view in the app.
		Fails if username doesn't exist.
		Fails if all caches were already solved.
		Fails if cache_secret is wrong.	
		"""			
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
		"""
		Exposed in API
		Saves new guestbook entry in database.

		Note that authors don't correlate to app users.
		"""			
		message = dict()

		author      = self.html_escape(author)
		message_str = self.html_escape(message_str)
		
		entry = Guestbook()
		entry.author        = author
		entry.message       = message_str
		entry.recorded_date = self.now()
		entry.save(force_insert=True)
		
		message["success"] = True
		return (json.dumps(message))

	def getGuestbookIndex(self):
		"""
		Exposed in API
		Returns a list of all available guestbook entry ids.
		This Function is supposed to be used in combination with
		getGuestbookEntryById for paged retrival of as many guestbook
		entries as needed.		
		"""
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
		"""
		Exposed in API
		Returns the guestbook entry with the matching id.
		Fails if no guestbook entry by that id exists.	
		"""			
		message = dict()

		index_select = """
		SELECT *
		FROM guestbook
		WHERE id = %s"""
 		index_query = Guestbook.raw(index_select, id)

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
		"""
		Exposed in API
		Checks if the token is the valid token for username.
		Fails if username is not logged in.
		Fails if token is not valid for username.

		This Function is meant to demonstrate the APIs
		authentication scheme. 
		"""

		message = dict()
		if not (self.checkToken(username, token)):
			message["success"] = False
			message["error"]   = "Invalid Authentication Token."
			return (json.dumps(message))
		else:
			message["success"] = True
			return (json.dumps(message))

	def updatePosition(self, username, token, longitude, latitude):
		"""
		Exposed in API
		Adds a new position for username in the database.
		Fails if username is not logged in.
		Fails if token is not valid for username.
		Fails if longitudes and/or latitudes data is flaky.
		"""

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
		"""
		Exposed in API
		Adds a new logbook entry for username in the database.
		Checks if token and secret are valid.
		Fails if username is not logged in.
		Fails if token is not valid for username.
		Fails if secret is not the secret for the next cache.
		Fails if the last caches puzzle was not yet solved.
		Fails if all caches were already found.
		"""

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
		"""
		Exposed in API
		Marks the puzzle of the last cache as solved in the last
		logbook entry.
		Fails if username is not logged in.
		Fails if token is not valid for username.
		"""
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
		"""
		Exposed in API
		Adds a new score entry for username in the database.
		Fails if username is not logged in.
		Fails if token is not valid for username.
		Fails if cache is not in database.
		Fails if user didn't find the cache whichs game the score relates to yet.
		"""
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
		if not relevant_cache in self.allCachesFoundByUser(username):
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

