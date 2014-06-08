#!/usr/bin/env python
# -*- coding: utf-8 -*-
# ✓ 

#
# RestAPI Unittests
#

__author__ = "space"

import unittest
import uuid
import json
import requests
import time

import logging
logging.basicConfig(level=logging.INFO)
log = logging.getLogger(__name__)

config = {
#    "api_url" : "http://btcwash.de:8080/api/"
	"api_url" : "http://localhost:8000/api/",
	"existing_user" : "testUser",
	"existing_users_password" : "foobar",
	"new_user" : ("to_delete" + str(uuid.uuid4()))[0:30],
	"new_users_password" : uuid.uuid4(),
	"new_user2" : ("to_delete" + str(uuid.uuid4()))[0:30],
	"new_user2s_password" : uuid.uuid4()	
}

class BaseFunctions(object):
	def login(self, payload, expected_error = None):
		login_url = (config["api_url"] + "login")
		response  = requests.post(login_url, data=payload)
		log.info(response.text)
		response_json = json.loads(response.text)
		
		if (expected_error):
			self.assertFalse(response_json["success"])
			self.assertEqual(response_json["error"], expected_error)

		else:
			self.assertTrue(response_json["success"])
			self.assertIsInstance(response_json["session_secret"], unicode)

	def register(self, payload, expected_error = None):
		register_url=(config["api_url"] + "register")
		response = requests.post(register_url, data=payload)
		log.info(response.text)
		response_json = json.loads(response.text)
		
		if (expected_error):
			self.assertFalse(response_json["success"])
			self.assertEqual(response_json["error"], expected_error)

		else:
			self.assertTrue(response_json["success"])


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

		self.login(payload, "Username doesn't exist.")

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


if __name__ == '__main__':
    unittest.main()

#	suite = unittest.TestSuite()
#	suite.addTest(LoginTests("testforwardLoginWithFreshUser"))
#	runner = unittest.TextTestRunner()
#	runner.run(suite)
