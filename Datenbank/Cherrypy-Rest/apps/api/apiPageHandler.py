#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# Implementation of the RestAPI
#

#active nutzer

#Top score aller spiele - für spiler

# gästebuch

# author 
# timestamp
# nachricht 

# sql injection


#	def getAvailableMinigames(self, username, token):
#		#doesn't work with current db moddel
# }}}

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

def setup_db():
	User.create_table()
	Minigame.create_table()
	Score.create_table()
	Geocache.create_table()
	Logbook.create_table()
	PositionLog.create_table()
	Guestbook.create_table()






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
#			if pos.user.username in user_map: pass
			user_map[pos.user.username] = (pos.longitude, pos.latitude)

		message["user_map"] = user_map
		message["success"]  = True
		return (json.dumps(message))

#message =
#{
#	success: True
#	user_map: {
#		"bernd": (längen, breiten)
#		"eva": (längen, breiten)
#		"eris": (längen, breiten)
#	}
#}

#message["user_map"]["bernd"]
#=> (43.234534, 23.423662)




#	def cachesAndUser(self, username):
#		message = dict()
#		if not ( self.checkUserExists(username) ):
#			message["success"] = False
#			message["error"]   = "Username doesn't exist."
#			return (json.dumps(message))
#
#		user = User.get(User.username == username)
#		user.logbookEntries.order_by(logbookEntries.recorded_date.desc()):

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

#		print(usernames)
		message["users"] = usernames
		message["success"]  = True
		return (json.dumps(message))


	def getTopTenScoresForAllMinigames(self, player=None ):
		message = dict()
		if player != None and not ( self.checkUserExists(player) ):
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

			if player != None:
				player_restriction = "AND s.user_id = %s" % (player)
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
#				Tracer()()
#				game[minigame.name].append((score.user.username, score.points, score.play_date))
				game[minigame.name][score.user.username] = (score.points, score.play_date)

		message["game"] = game
		message["success"] = True
		return (json.dumps(message))


	def makeGuestbookEntry(self, author, message_str):
		message = dict()
		
		now = int(datetime.datetime.utcnow().strftime('%s'))

		entry = Guestbook()
		entry.author        = author
		entry.message       = message_str
		entry.recorded_date = now
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
			break

		message["success"]  = True
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

		now = int(datetime.datetime.utcnow().strftime('%s'))

		pos = PositionLog()
		pos.user          = username
		pos.recorded_date  = now
		pos.latitude      = float(latitude)
		pos.longitude     = float(longitude)
		pos.save(force_insert=True)

		message["success"] = True
		return (json.dumps(message))

#	def makeLogbookEntry(self, username, token, cache_id, secret, message):
#		message = dict()
#		if not (self.checkToken(username, token)):
#			message["success"] = False
#			message["error"]   = "Invalid Authentication Token."
#			return (json.dumps(message))

		# TODO: Check user is allowed to solve



#class Logbook(BaseModel):
#	logbook_id    = PrimaryKeyField
#	puzzle_solved = BooleanField()
#	message       = CharField()
#	found_date    = IntegerField()
#	cache         = ForeignKeyField(Geocache, related_name='findings')
#	user          = ForeignKeyField(User, related_name='logbookEntries')	

#class Geocache(BaseModel):
#	geochache_id = PrimaryKeyField
#	latitude     = DoubleField()
#	longitude    = DoubleField()
#	secret       = CharField()
#	next_cache   = ForeignKeyField('self', related_name='next')
#	hint         = CharField()


# {{{ Database wrapper
class BaseModel(peewee.Model):
	class Meta: 
		database = db_conn

class User(BaseModel):
	username      = CharField(primary_key=True)
	password_hash = TextField()
	password_salt = TextField()

class Minigame(BaseModel):
	minigame_id = PrimaryKeyField
	name = CharField()

class Score(BaseModel):
	score_id  = PrimaryKeyField
	user      = ForeignKeyField(User, related_name='played_rounds')
	game   = ForeignKeyField(Minigame, related_name='scores')
	points    = IntegerField()
	play_date = IntegerField()

class Geocache(BaseModel):
	geochache_id = PrimaryKeyField
	name         = CharField()
	latitude     = DoubleField()
	longitude    = DoubleField()
	secret       = CharField()
	next_cache   = ForeignKeyField('self', related_name='next')
#	hint         = CharField()

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

class Guestbook(BaseModel):
	guestbook_id  = PrimaryKeyField
	author        = CharField()
	message       = CharField()
	recorded_date = IntegerField()

# }}}

