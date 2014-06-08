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
logging.basicConfig(level=logging.DEBUG)
log = logging.getLogger(__name__)

config = {
#    "api_url" : "http://btcwash.de:8080/api/"
	"api_url" : "http://localhost:8000/api/",
	"existing_user" : "testUser",
	"existing_users_password" : "foobar",
	"new_user" : "to_delete" + str(uuid.uuid4()),
	"new_users_password" : uuid.uuid4()
}

class RegisterTests(unittest.TestCase):
	def setUp(self):
		self.register_url = config["api_url"] + "register"

	def testUsertaken(self):
		payload = {	'username': config["existing_user"], 
					'password': config["existing_users_password"] }
		response = requests.post(self.register_url, data=payload)

		self.assertIsInstance(response.text, unicode)
		response_json = json.loads(response.text)
		
		self.assertEqual(response_json["success"], False)
		self.assertEqual(response_json["error"], "Username already taken.")

	def testforwardRegister(self):
		payload = {	'username': "to_delete" + str(uuid.uuid4()), 
					'password': uuid.uuid4() }
		response = requests.post(self.register_url, data=payload)

		self.assertIsInstance(response.text, unicode)
		response_json = json.loads(response.text)
		
		self.assertEqual(response_json["success"], True)

class LoginTests(unittest.TestCase):
	def setUp(self):
		self.login_url = config["api_url"] + "login"

	def testUserDoesNotExist(self):
		payload = {	'username': "to_delete" + str(uuid.uuid4()), 
					'password': uuid.uuid4() }
		response = requests.post(self.login_url, data=payload)

		self.assertIsInstance(response.text, unicode)
		response_json = json.loads(response.text)
		
		self.assertEqual(response_json["success"], False)
		self.assertEqual(response_json["error"], "Username doesn't exist.")


	def testWrongPassword(self):
		payload = {	'username': config["existing_user"], 
					'password': "wrongpw" }
		response = requests.post(self.login_url, data=payload)

		self.assertIsInstance(response.text, unicode)
		log.debug(response.text)
		response_json = json.loads(response.text)
		
		self.assertEqual(response_json["success"], False)
		self.assertEqual(response_json["error"], "Wrong Password.")

	def login(self, payload):
		response = requests.post(self.login_url, data=payload)

		self.assertIsInstance(response.text, unicode)
		log.debug(response.text)
		response_json = json.loads(response.text)
		
		self.assertEqual(response_json["success"], True)
		self.assertIsInstance(response_json["session_secret"], unicode)

	def testforwardLogin(self):
		payload = {	'username': config["existing_user"], 
					'password': config["existing_users_password"] }
		self.login(payload)

	def testforwardLoginWithFreshUser(self):
		payload = {	'username': config["new_user"], 
					'password': config["new_users_password"] }

		response = requests.post(config["api_url"] + "register", data=payload)
		logging.debug("foo: " + config["new_user"] + response.text )
		time.sleep(1)
		self.login(payload)




if __name__ == '__main__':
    unittest.main()
