#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# RestAPI Unittests
#
# Some tests have race conditions and may fail 
# when run against a server with active users.
#

__author__ = "Patrick Meyer"

import unittest
import random
import hashlib
import uuid
import json
import requests
import time

import logging
logging.basicConfig(level=logging.WARNING) # INFO
log = logging.getLogger(__name__)

config = {
    "api_url" : "http://btcwash.de:8080/api/",
#	"api_url" : "http://localhost:8000/api/",
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

	def submitGameScore(self, session, payload, expected_error = None):
		pos_url=(config["api_url"] + "submitGameScore")

		token, session = self.token(session)
		payload.update( {	'username': session["username"], 
							'token': token } )

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]):																	 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return session


	def getTopTenScoresForAllMinigames(self, payload = {}, expected_error = None):
		pos_url=(config["api_url"] + "getTopTenScoresForAllMinigames")


		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]): 
			if expected_error != response_json["error"]: logging.warning(response_json["error"])
			return None
		else: return response_json



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
			pos_payload   = {	'longitude' : ("%s%d" %  ("51.7", random.randint(2903727, 3363555))),
								'latitude'  : ("%s%d" %  ("8.7", random.randint (3138427, 4317526))) }

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


		payload = {	"cache_secret": "irgendwas", 
					"username": 	login_payload["username"]}

		response_json = self.secretValidForNextCache(payload, "All caches already solved.")
		self.assertIsNone(response_json)		

	def testSecretValidForNextCacheWithWrongUsername(self):

			payload = {	"cache_secret": "8e71bee3", 
						"username": 	"nenenenenene"}

			response_json = self.secretValidForNextCache(payload, "Username doesn't exist.")
			self.assertIsNone(response_json)

	def testGetAllLogbookEntriesByUserWithWrongUsername(self):

			payload = {	"username": "nenenenenene"}

			response_json = self.getAllLogbookEntriesByUser(payload, "Username doesn't exist.")
			self.assertIsNone(response_json)

	def testSecretValidForNextCacheWithWrongSecret(self):
		login_payload = {	'username': ("to_delete" + str(uuid.uuid4()))[0:30], 
							'password': uuid.uuid4() }
		
		self.register(login_payload)
		session = self.login(login_payload)

		payload = {	"cache_secret": "falsch", 
					"username": 	login_payload["username"]}

		response_json = self.secretValidForNextCache(payload, "Wrong secret.")
		self.assertIsNone(response_json)		

	def testMakeLogbookEntryWrongToken(self):
		login_payload = {	'username': ("to_delete" + str(uuid.uuid4()))[0:30], 
							'password': uuid.uuid4() }

		self.register(login_payload)
		session = self.login(login_payload)
		session["session_secret"] = "Eris the younger"

		payload = {	"secret":		"8e71bee3", 
					"message_str":	"blub",
					"username": 	login_payload["username"]}

		session = self.makeLogbookEntry(session, payload, "Invalid Authentication Token.")
		self.assertIsNone(session)

	def testSecretValidForNextCacheWithWrongSecret(self):
		login_payload = {	'username': ("to_delete" + str(uuid.uuid4()))[0:30], 
							'password': uuid.uuid4() }
		
		self.register(login_payload)
		session = self.login(login_payload)

		payload = {	"secret":		"falsch", 
					"message_str":	"blub",
					"username": 	login_payload["username"]}

		response_json = self.makeLogbookEntry(session, payload, "Wrong secret.")
		self.assertIsNone(response_json)		

class GuestbookTests(unittest.TestCase, BaseFunctions):
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

	def testforwardMakeAndRetriveGuestbookEntryWithHTML(self):
		entry_payload		= {	'author' : "Axel Stoll",
								'message_str' : "<ne> &\"'Magie = Physik / Wollen <ne/>" }

		expected_response	= {	'author' : "Axel Stoll",
								'message_str' : "&lt;ne&gt; &amp;&quot;&apos;Magie = Physik / Wollen &lt;ne/&gt;" }

		response_json = self.makeGuestbookEntry(entry_payload)
		response_json = self.getGuestbookIndex()
		self.assertIsNotNone(response_json)

		query_payload = {	'id' : response_json["index"][0] }
		response_json = self.getGuestbookEntryById(query_payload)

		self.assertEqual(expected_response['author'], response_json['author'])
		self.assertEqual(expected_response['message_str'], response_json['message'])
		self.assertEqual(response_json['id'], query_payload['id'])

	def testforwardMakeAndRetriveGuestbookEntryWithUnicode(self):
		entry_payload = {	'author' : u"☃",
							'message_str'  : u"‽" }

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
	def testfoarwardSubmitAndRetrieveGameScores(self):
		expected_map  = dict()

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
			session = self.markPuzzleSolved(session)
			session = self.makeLogbookEntry(session, {"secret": cache['secret'], "message_str": str(uuid.uuid4())})	

		for i in xrange(10):
			cache = random.choice(caches)["name"]
 			game_payload   = {	'points' : ("%d" %  (random.randint(100000, 999999))),
								'cache'  :  cache}

			session = self.submitGameScore(session, game_payload)
			self.assertIsNotNone(session)

			response_json = self.getTopTenScoresForAllMinigames({ "username": login_payload["username"]})

			found = False
			for score in response_json["game"][cache]:
				if(	score["username"] == login_payload["username"]
				and score["points"] == int(game_payload["points"])):
					found = True
					break

			self.assertTrue(found)

	def testSecretGetTopTenScoresForAllMinigamesWithWrongUsername(self):

			payload = {	"username": "nenenenenene"}

			response_json = self.getTopTenScoresForAllMinigames(payload, "Username doesn't exist.")
			self.assertIsNone(response_json)

	def testSubmitGameScoreWithWrongCache(self):
		login_payload = {	'username': ("to_delete" + str(uuid.uuid4()))[0:30], 
							'password': uuid.uuid4() }
		
		self.register(login_payload)
		session = self.login(login_payload)

		payload = { 'points':	342344, 
					'cache':	"herebedragons"	}

		response_json = self.submitGameScore(session, payload, "Cache not in database.")
		self.assertIsNone(response_json)	

	def testSubmitGameScoreWithUnfoundCache(self):
		login_payload = {	'username': ("to_delete" + str(uuid.uuid4()))[0:30], 
							'password': uuid.uuid4() }
		
		self.register(login_payload)
		session = self.login(login_payload)

		payload = { 'points':	342344, 
					'cache':	"bib-Eingang"	}

		response_json = self.submitGameScore(session, payload, "User didn't find that cache yet.")
		self.assertIsNone(response_json)

if __name__ == '__main__':
	unittest.main()

#	suite = unittest.TestSuite()
#	suite.addTest(GeoTests("testfoarwardUpdateAndGetPositionsMap"))
#	runner = unittest.TextTestRunner()
#	runner.run(suite)
