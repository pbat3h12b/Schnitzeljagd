#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# Implementation of the RestAPI
#

__author__ = "space"

import hashlib
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
		if not ( self.session[username] ):
			return (False)

		self.session[username][command_counter] += 1
		session_secret          = self.session[username][session_secret]
		session_command_counter = self.session[username][command_counter]
		check_token = hashlib.md5((session_secret + session_command_counter).encode('utf-8')).hexdigest()
		
		return (token == check_token)
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
		
		password_salt = self.generateRandomString()
		password_hash = hashlib.md5((password + password_salt).encode('utf-8')).hexdigest()

		user = User()
		user.username = username
		user.passwordSalt = password_salt
		user.passwordHash = password_hash
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
		password_salt = user.passwordSalt
		password_hash = hashlib.md5((password + password_salt).encode('utf-8')).hexdigest()

		if ( user.passwordHash != password_hash ):
			message["success"] = False
			message["error"]   = "Wrong Password."
			return (json.dumps(message))

		user_session = {"command_counter": 0,
						"session_secret":  self.generateRandomString()}

		self.session[username] = user_session

		message["success"]        = True
		message["session_secret"] = self.session[username]["session_secret"]
		return (json.dumps(message))

# }}}

# {{{ Authentication required
	def getAvailableMinigames(self, username, token):
		if not ( checkUserExists(username) ):
			return (False)
		if not ( checkToken(username, token) ):
			return (False)

		user = User.select().where(User.username == username)
		#doesn't work with current db moddel
# }}}

# {{{ Database wrapper
# Die folgenden Klassen nutzen die CamelCase Notation für Attribute, 
# da sie die SQL Tabellen  wieder spiegeln.
class BaseModel(peewee.Model):
	class Meta: 
		database = db_conn

class User(BaseModel):
	username     = CharField(primary_key=True)
	passwordHash = TextField()
	passwordSalt = TextField()

class Minigame(BaseModel):
	minigameId   = IntegerField(primary_key=True)
	name = CharField()

class Score(BaseModel):
	scoreId  = IntegerField(primary_key=True)
	username = ForeignKeyField(User, related_name='games')
	gameId   = ForeignKeyField(Minigame, related_name='scores')
	points   = IntegerField()
	playDate = DateTimeField()

class Geocache(BaseModel):
	geochacheId = IntegerField(primary_key=True)
	position    = TextField()
	secret      = CharField()
	nextCache   = ForeignKeyField('self', related_name='next')
	hint        = CharField()

class Logbook(BaseModel):
	logbookId    = IntegerField(primary_key=True)
	puzzleSolved = BooleanField()
	message      = CharField()
	foundDate    = DateTimeField()
	cacheId      = ForeignKeyField(Geocache, related_name='findings')
	username     = ForeignKeyField(User, related_name='logbookEntries')

class PositionLog(BaseModel):
	positionLogId = IntegerField(primary_key=True)
	username      = ForeignKeyField(User, related_name='positions')
	position      = TextField()
	recordedDate  = DateTimeField()
# }}}
