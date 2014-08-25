#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# RestAPI Unittests
#
# Some tests have race conditions and may fail 
# when run against a server with active users.
#

__author__ = "space"

import unittest
import random
import hashlib
import uuid
import json
import requests
import time

import logging
logging.basicConfig(level=logging.WARNING)#INFO)
log = logging.getLogger(__name__)

config = {
#    "api_url" : "http://btcwash.de:8080/api/",
	"api_url" : "http://localhost:8000/api/",
	"existing_user" : "testUser",
	"existing_users_password" : "foobar",
	"new_user" : ("to_delete" + str(uuid.uuid4()))[0:30],
	"new_users_password" : uuid.uuid4(),
	"new_user2" : ("to_delete" + str(uuid.uuid4()))[0:30],
	"new_user2s_password" : uuid.uuid4()	
}

class BaseFunctions(object):
	def token(self, session):
		username 				= session["username"]
		session_secret          = session["session_secret"]
		session_command_counter = session["command_counter"]
		token = hashlib.md5((session_secret + str(session_command_counter)).encode('utf-8')).hexdigest()
		session["command_counter"] += 1

		return (token, session)	

	def genericRestCall(self, command_url, payload, expected_error = None):
		response  = requests.post(command_url, data=payload)
		log.info(response.text)
		response_json = json.loads(response.text)
		
		if (expected_error):
			self.assertFalse(response_json["success"])
			self.assertEqual(response_json["error"], expected_error)

		else:
			self.assertTrue(response_json["success"])
		
		return response_json

	def login(self, payload, expected_error = None):
		login_url = (config["api_url"] + "login")
		response_json = self.genericRestCall(login_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None

		self.assertIsInstance(response_json["session_secret"], unicode)

		session = { "username" : payload["username"],
					"command_counter" : 0,
					"session_secret":  response_json["session_secret"]}

		return session

	def register(self, payload, expected_error = None):
		register_url = (config["api_url"] + "register")
		response_json = self.genericRestCall(register_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return False
		else: return True	

	def nop(self, session, expected_error = None):
		nop_url=(config["api_url"] + "nop")

		token, session = self.token(session)
		payload = {	'username': session["username"], 
					'token': token }

		response_json = self.genericRestCall(nop_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return session

	def updatePosition(self, session, payload, expected_error = None):
		pos_url=(config["api_url"] + "updatePosition")

		token, session = self.token(session)
		payload.update( {	'username': session["username"], 
							'token': token } )

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]):																	 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return session

	def getPositionsMap(self, expected_error = None):
		pos_url=(config["api_url"] + "getPositionsMap")

		response_json = self.genericRestCall(pos_url, {}, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json	

	def makeGuestbookEntry(self, payload, expected_error = None):
		pos_url=(config["api_url"] + "makeGuestbookEntry")

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json

	def getGuestbookIndex(self, expected_error = None):
		pos_url=(config["api_url"] + "getGuestbookIndex")

		response_json = self.genericRestCall(pos_url, {}, expected_error)
		if not (response_json["success"]):
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json	

	def getGuestbookEntryById(self, payload, expected_error = None):
		pos_url=(config["api_url"] + "getGuestbookEntryById")

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json

	def getUsers(self, expected_error = None):
		pos_url=(config["api_url"] + "getUsers")

		response_json = self.genericRestCall(pos_url, {}, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json

	def getAllLogbookEntriesByUser(self, payload, expected_error = None):
		pos_url=(config["api_url"] + "getAllLogbookEntriesByUser")

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json	

	def secretValidForNextCache(self, payload, expected_error = None):
		pos_url=(config["api_url"] + "secretValidForNextCache")

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json

	def makeLogbookEntry(self, session, payload, expected_error = None):
		pos_url=(config["api_url"] + "makeLogbookEntry")

		token, session = self.token(session)
		payload.update( {	'username': session["username"], 
							'token': token } )

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]):																	 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return session


	def markPuzzleSolved(self, session, expected_error = None):
		pos_url=(config["api_url"] + "markPuzzleSolved")

		token, session = self.token(session)
		payload = dict()
		payload.update( {	'username': session["username"], 
							'token': token } )

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]):																	 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return session		

		

class RegisterTests(unittest.TestCase, BaseFunctions):
	def testUsernametaken(self):
		payload = {	'username': config["existing_user"], 
					'password': config["existing_users_password"] }

		self.register(payload, "Username already taken.")

	def testUsernameTooLong(self):
		payload = {	'username': uuid.uuid4(), 
					'password': uuid.uuid4() }
		
		self.register(payload, "Username too long.")	

	def testforwardRegister(self):
		payload = {	'username': config["new_user2"], 
					'password': config["new_user2s_password"] }

		self.register(payload)

		response_json = self.getUsers()
		self.assertTrue( payload["username"] in response_json["users"] )

class LoginTests(unittest.TestCase, BaseFunctions):
	def testUserDoesNotExist(self):
		payload = {	'username': "to_delete" + str(uuid.uuid4()), 
					'password': uuid.uuid4() }

		session = self.login(payload, "Username doesn't exist.")

	def testWrongPassword(self):
		payload = {	'username': config["existing_user"], 
					'password': "wrongpw" }

		self.login(payload, "Wrong Password.")		

	def testforwardLogin(self):
		payload = {	'username': config["existing_user"], 
					'password': config["existing_users_password"] }

		self.login(payload)

	def testforwardLoginWithFreshUser(self):
		payload = {	'username': config["new_user"], 
					'password': config["new_users_password"] }

		self.register(payload)
		self.login(payload)

class TokenTests(unittest.TestCase, BaseFunctions):
	def testWrongTokenUsage(self):
		payload = {	'username': config["existing_user"], 
					'password': config["existing_users_password"] }

		session = self.login(payload)
		session["session_secret"] = "Eris the younger"
		session = self.nop(session, "Invalid Authentication Token.")
		self.assertIsNone(session)

	def testNotLogedinUser(self):
		session = { 'username': "doesntexist",
					"command_counter" : -23,
					"session_secret":  "notasecret"}

		session = self.nop(session, "Invalid Authentication Token.")
		self.assertIsNone(session)

	def testforwardTokenUsage(self):
		payload = {	'username': config["existing_user"], 
					'password': config["existing_users_password"] }

		session = self.login(payload)
		session = self.nop(session)
		session = self.nop(session)
		self.assertIsNotNone(session)

class GeoTests(unittest.TestCase, BaseFunctions):
	def testforwardUpdatePosition(self):
		login_payload = {	'username': config["existing_user"], 
							'password': config["existing_users_password"] }

 		pos_payload   = {	'longitude' : "12.345678",
							'latitude'  : "98.765432" }

		session = self.login(login_payload)
		session = self.updatePosition(session, pos_payload)
		session = self.nop(session)			

	def testUpdatePositionNotEnoughAccurarcy(self):
		login_payload = {	'username': config["existing_user"], 
							'password': config["existing_users_password"] }

 		pos_payload   = {	'longitude' : "12.3456",
							'latitude'  : "98.7654" }

		session = self.login(login_payload)
		session = self.updatePosition(session, pos_payload, "Wrong Format or Insufficient position accurarcy.")
		self.assertIsNone(session)

	def testfoarwardGetPositionsMap(self):
		response_json = self.getPositionsMap()
		self.assertIsNotNone(response_json)


	def testfoarwardUpdateAndGetPositionsMap(self):
		expected_map  = dict()
		user_sessions = list()

		for idx in xrange(5):
			login_payload = {	'username': ("to_delete" + str(uuid.uuid4()))[0:30], 
								'password': uuid.uuid4() }
		
			self.register(login_payload)
			session = self.login(login_payload)	
			user_sessions.append(session)

		for i in xrange(len(user_sessions)*10):
 			pos_payload   = {	'longitude' : ("%d.%d" %  (random.randint(1, 99), random.randint(100000, 999999))),
								'latitude'  : ("%d.%d" %  (random.randint(1, 99), random.randint(100000, 999999))) }

			idx = random.randint(0, len(user_sessions)-1)
			user_sessions[idx] = self.updatePosition(user_sessions[idx], pos_payload)
			self.assertIsNotNone(user_sessions[idx])

			expected_map[user_sessions[idx]["username"]] = [float(pos_payload["longitude"]), float(pos_payload["latitude"])]

		response_json = self.getPositionsMap()
		for key in expected_map:
			self.assertEqual(expected_map[key], response_json["user_map"][key])

class CacheTest(unittest.TestCase, BaseFunctions):
	def testforwardWalkthroughCaches(self):
		login_payload = {	'username': ("to_delete" + str(uuid.uuid4()))[0:30], 
							'password': uuid.uuid4() }
		
		self.register(login_payload)
		session = self.login(login_payload)

		caches = [ {	'name':   'bib-Eingang',
						'secret': '8e71bee3'	},
					{	'name':   'Zukunftsmeile',
						'secret': 'd1741e41'	},
					{	'name':   'HNF',
						'secret': 'b7a34174'	},
					{	'name':   'Wohnheim',
						'secret': '8a1b32fa'	},
					{	'name':   'Fluss',
						'secret': '4f1fc70d'	},
					{	'name':   'Serverraum',
						'secret': 'df5a8617'	} ]

		for cache in caches:
			
			response_json = self.getAllLogbookEntriesByUser({"username": login_payload['username']})
			for le in response_json["entries"]:	self.assertTrue(cache['name'] != le['cache'])

			response_json = self.secretValidForNextCache({"cache_secret": cache['secret'], "username": login_payload['username']})
			self.assertTrue(response_json["success"])

			session = self.markPuzzleSolved(session)
			session = self.nop(session)

			session = self.makeLogbookEntry(session, {"secret": cache['secret'], "message_str": str(uuid.uuid4())})	
			session = self.nop(session)

			response_json = self.getAllLogbookEntriesByUser({"username": login_payload['username']})

			found = False
			for le in response_json["entries"]:
				if cache['name'] == le['cache']: found = True
			self.assertTrue(found)

class GuestbookTests(unittest.TestCase, BaseFunctions):
	# umlaute
	# sonderzeichen
	# non printable characters
	# XSS
	def testforwardMakeAndRetriveGuestbookEntry(self):
		entry_payload = {	'author' : "Axel Stoll",
							'message_str'  : "Magie = Physik / Wollen" }

		response_json = self.makeGuestbookEntry(entry_payload)
		response_json = self.getGuestbookIndex()
		self.assertIsNotNone(response_json)

		query_payload = {	'id' : response_json["index"][0] }
		response_json = self.getGuestbookEntryById(query_payload)

		self.assertEqual(entry_payload['author'], response_json['author'])
		self.assertEqual(entry_payload['message_str'], response_json['message'])
		self.assertEqual(response_json['id'], query_payload['id'])

	def testgetGuestbookEntryWithWrongId(self):
		query_payload = {	'id' : -1 }
		response_json = self.getGuestbookEntryById(query_payload, "No Entry by that id.")
		self.assertIsNone(response_json)

class MinigameTests(unittest.TestCase, BaseFunctions):
	pass


# getUsers
# markPuzzleSolved

if __name__ == '__main__':
    unittest.main()

#	suite = unittest.TestSuite()
#	suite.addTest(CacheTest("testforwardWalkthroughCaches"))
#	runner = unittest.TextTestRunner()
#	runner.run(suite)
