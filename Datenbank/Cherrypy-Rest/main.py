import sys
import os

import cherrypy

from apps.api.apiPageHandler import Api


class Root():
	def index(self):
		return "You found the index. There is nothing here. Go, use the API."
		#raise cherrypy.HTTPRedirect("/example/")


if __name__ == '__main__':
	ip = '127.0.0.1'
	port = 8000
#	ip = '81.169.244.213'
#	port = 8080

	db_host = 'localhost'
	db_database = 'geo'
	db_user = 'berry_pink'
	db_password = 'mellow_yellow'

	cherrypy.config.update({'server.socket_host': ip})
	cherrypy.config.update({'server.socket_port': port})

	index_controller = Root()
	api_controller = Api(db_host, db_database, db_user, db_password)

	d = cherrypy.dispatch.RoutesDispatcher()
	d.connect(name='root',		action='index',		controller=index_controller,	route='/')

	d.connect(name='api',		action='register',				controller=api_controller,	route='/api/register')
	d.connect(name='api',		action='login',					controller=api_controller,	route='/api/login')	
	d.connect(name='api',		action='nop',					controller=api_controller,	route='/api/nop')
	d.connect(name='api',		action='updatePosition',		controller=api_controller,	route='/api/updatePosition')
	d.connect(name='api',		action='getPositionsMap',		controller=api_controller,	route='/api/getPositionsMap')

	
	config_dict = {
		'/': {
			'request.dispatch': d
		}
	}

	cherrypy.tree.mount(None, config=config_dict)
	cherrypy.engine.start()
	cherrypy.engine.block()
