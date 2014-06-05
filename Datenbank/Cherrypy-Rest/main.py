import sys
import os

import cherrypy

from apps.example.examplePageHandler import Example


class Root():
	def index(self):
		return "You found the index"
		#raise cherrypy.HTTPRedirect("/example/")


if __name__ == '__main__':
	ip = '127.0.0.1'
	port = 8080
	static_root = os.path.abspath(os.path.join(os.path.dirname(__file__), 'static'))

	cherrypy.config.update({'server.socket_host': ip})
	cherrypy.config.update({'server.socket_port': port})
	cherrypy.config.update({'error_page.404': 'static/templates/404.html'})

	index_controller = Root()
	example_controller = Example(ip, port)

	d = cherrypy.dispatch.RoutesDispatcher()
	d.connect(name='root',		action='index',		controller=index_controller,	route='/')

	d.connect(name='example',	action='index',		controller=example_controller,	route='/example/')
	d.connect(name='example',				controller=example_controller,	route='/example/:action/:attribute_1/:attribute_2')

	config_dict = {
		'/': {
			'request.dispatch': d,
			'tools.staticdir.on': True,
			'tools.staticdir.root': static_root,
			'tools.staticdir.dir': '.'
		}
	}

	cherrypy.tree.mount(None, config=config_dict)
	cherrypy.engine.start()
	cherrypy.engine.block()
