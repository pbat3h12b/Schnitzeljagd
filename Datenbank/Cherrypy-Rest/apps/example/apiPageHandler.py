import logging
import cherrypy

from jinja2 import Environment, PackageLoader
env = Environment(loader=PackageLoader('apps.example', 'templates'))

class Example(object):
	def __init__(self, ip, port):
		self.ip = ip
		self.port = port

		self.debug_template = env.get_template('debug.html')

# Generate Page {{{
	def index(self):
		return "You found the exmaple index"

	def debug(self, attribute_1, attribute_2):
		return self.debug_template.render(ip=self.ip, port=self.port, attribute_1=attribute_1, attribute_2=attribute_2)
# }}}
