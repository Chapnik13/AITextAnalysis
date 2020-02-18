def distinct(filePath, distinctFilePath):
	urls = []

	print('starting searching for duplications..')

	with open(filePath, 'r') as srcFile:
		for line in srcFile:
			url = line[:-1];
			
			if(url.startswith('https')):
				url = url.replace('https', 'http')

			if(url.startswith('http://pal.live.')):
				url = url.replace('http://pal.live.', 'http://www.')

			if(url not in urls):
				urls.append(url)
				
	print('finishing distinct urls, distinct urls number:', len(urls))

	with open(distinctFilePath, 'w') as distinctFile:
		for line in map(lambda x: x + '\n', urls):
			distinctFile.write(line)
			
	print('finished saving..')


def distinctByInput():
	filePath = input('urls file path:\n')
	distinctFilePath = input('distinct urls file path:\n')

	distinct(filePath, distinctFilePath)
