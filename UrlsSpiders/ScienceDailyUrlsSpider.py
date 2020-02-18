import scrapy
import os
import re
import csv
import datetime
import operator
from string import printable
from scrapy.spiders import CrawlSpider, Rule
from scrapy.linkextractors import LinkExtractor
from scrapy.linkextractors.lxmlhtml import LxmlLinkExtractor
from scrapy.signalmanager import SignalManager
from scrapy.exceptions import CloseSpider
from scrapy import signals
from pydispatch import dispatcher
from distinctUrls import distinct

class WebSite(scrapy.Item):
	url = scrapy.Field()

class ScienceDailyUrlsSpider(CrawlSpider):
	name = "ScienceDailyUrlsSpider"
	allowed_domains = ["sciencedaily.com"]

	YEAR = '2019'
	LIMIT_URLS_COUNT = 1000
	MATCH = r'.*\bwww.sciencedaily.com/releases/%s.*$' % YEAR

	visitedUrls = []
	start_urls  = ['https://www.sciencedaily.com']
	
	def __init__(self, *args, **kwargs):
		super(ScienceDailyUrlsSpider, self).__init__(*args, **kwargs)
		SignalManager(dispatcher.Any).connect(self.spiderClosed, signals.spider_closed)

	rules = (	
		Rule(LinkExtractor(allow=(), deny=(), restrict_xpaths="//body"), callback='parsePage', follow=True),
	)

				
	def parsePage(self, response):
		item = WebSite()
		item['url'] = response.url		

		if re.match(self.MATCH, item['url']) != None:
			self.visitedUrls.append(response.url)

			print("Found", len(self.visitedUrls), "urls")
			print(len(self.visitedUrls) == self.LIMIT_URLS_COUNT)
			if len(self.visitedUrls) == self.LIMIT_URLS_COUNT:
				raise CloseSpider('LIMIT_URLS_COUNT')

			return item
	
	def spiderClosed(self, spider):
		if not os.path.exists("urls-results"):
			os.makedirs("urls-results")

		self.printUrlsToFiles("ScienceDaily-visitedUrls1.txt", "urls-results")

		distinct("urls-results" + self.YEAR + "-" + "ScienceDaily-visitedUrls1.txt", "ScienceDaily-visitedUrls-distinct.txt")

	def printUrlsToFiles(self, filename, folder = ""):
		folderPath = folder + "/" if folder else ""

		with open(folderPath + self.YEAR + "-" + filename, 'a') as f:
			for url in self.visitedUrls:
				f.write(url+"\n")
			f.close()
