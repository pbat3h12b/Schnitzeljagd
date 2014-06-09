#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# RestAPI Unittests
#

__author__ = "space"

import unittest
import hashlib
import uuid
import json
import requests
import time

import logging
logging.basicConfig(level=logging.INFO)
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


	def login(self, payload, expected_error = None): # TODO: more asserts
		login_url = (config["api_url"] + "login")
		response_json = self.genericRestCall(login_url, payload, expected_error)
		if not (response_json["success"]): return None

		self.assertIsInstance(response_json["session_secret"], unicode)

		session = { "username" : payload["username"],
					"command_counter" : 0,
					"session_secret":  response_json["session_secret"]}

		return session

	def register(self, payload, expected_error = None):
		register_url = (config["api_url"] + "register")
		response_json = self.genericRestCall(register_url, payload, expected_error)
		if not (response_json["success"]): return False

		return response_json	

	def nop(self, session, expected_error = None):
		nop_url=(config["api_url"] + "nop")

		token, session = self.token(session)
		payload = {	'username': session["username"], 
					'token': token }

		response_json = self.genericRestCall(nop_url, payload, expected_error)
		if not (response_json["success"]): return None
		else: return session

	def updatePosition(self, session, payload, expected_error = None):
		pos_url=(config["api_url"] + "updatePosition")

		token, session = self.token(session)
		payload.update( {	'username': session["username"], 
							'token': token } )

		response_json = self.genericRestCall(pos_url, payload, expected_error)
		if not (response_json["success"]): return None
		else: return session

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


if __name__ == '__main__':
    unittest.main()

#	suite = unittest.TestSuite()
#	suite.addTest(LoginTests("testforwardLoginWithFreshUser"))
#	runner = unittest.TextTestRunner()
#	runner.run(suite)
