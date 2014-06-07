#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# Implementation of the RestAPI
#

__author__ = "space"

import logging
import md5
import cherrypy
import peewee
from peewee import *

db_conn = None


class Api(object):
	def __init__(self, db_host, db_database, db_user, db_password):
		self.db_host = db_host
		self.db_database = db_database
		self.db_user = db_user
		self.db_password = db_password

		self.session = dict()

		db_conn = MySQLDatabase(db_database,
								host=db_host, 
								user=db_user, 
								passwd=db_password)
		db_conn.connect()

# {{{ Unexposed Methods
	def generateRandomString(length=20):
		character_set = string.digits + string.ascii_uppercase + string.ascii_lowercase
		return ''.join(random.choice(chars) for c in range(length))

	def checkUserExists(nickname):
		if ( User.select().where(User.nickname == nickname) ): return (True)
		else: return (False)

	def checkToken(nickname, token):
		if not ( self.session[nickname] ):
			return (False)

		self.session[nickname][command_counter] += 1
		session_secret          = self.session[nickname][session_secret]
		session_command_counter = self.session[nickname][command_counter]
		check_token = md5.new(session_secret + session_command_counter).digest()
		
		return (token == check_token)
# }}}

# {{{ No Authentication required
	def index(self):
		return ("You found the Rest-Api. Now try to actually use it...")

	def register(self, nickname, password):
		if ( checkUserExists(nickname) ):
			return (False)
		
		password_salt = generateSalt()
		password_hash = md5.new(password + password_salt).digest()

		User.create(nickname = nickname,
					passwordSalt = password_salt, 
					passwordHash = password_hash)

		return (True)

	def login(self, nickname, password):
		if not ( checkUserExists(nickname) ):
			return (False)

		user = User.select().where(User.nickname == nickname)
		password_salt = user.passwordSalt
		password_hash = md5.new(password + password_salt).digest()

		if ( user.passwordHash != password_hash ):
			return (False)

		user_session = {command_counter: 0,
						session_secret: generateRandomString()}

		self.session[nickname] = user_session

		return self.session[nickname][session_secret]

# }}}

# {{{ Authentication required
	def getAvailableMinigames(self, nickname, token):
		if not ( checkUserExists(nickname) ):
			return (False)
		if not ( checkToken(nickname, token) ):
			return (False)

		user = User.select().where(User.nickname == nickname)
		#doesn't work with current db moddel
# }}}

# {{{ Database wrapper
# Die folgenden Klassen nutzen die CamelCase Notation für Attribute, 
# da sie die SQL Tabellen  wieder spiegeln.
class BaseModel(peewee.Model):
	class Meta: 
		database = db_conn

class User(BaseModel):
	nickname     = CharField(primary_key=True)
	passwordHash = TextField()
	passwordSalt = TextField()

class Minigame(BaseModel):
	minigameId   = IntegerField(primary_key=True)
	name = CharField()

class Score(BaseModel):
	scoreId  = IntegerField(primary_key=True)
	nickname = ForeignKeyField(User, related_name='games')
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
	nickname     = ForeignKeyField(User, related_name='logbookEntries')

class PositionLog(BaseModel):
	positionLogId = IntegerField(primary_key=True)
	nickname      = ForeignKeyField(User, related_name='positions')
	position      = TextField()
	recordedDate  = DateTimeField()
# }}}
