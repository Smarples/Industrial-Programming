#my python files
import myConstants
import Coursework

#libraries being used
import json #for parsing
import matplotlib.pyplot as plt #visualising the result

#json file for parsing
#jfile = 'issuu_sample.json'
#jfile = 'issuu_cw2.json'
jfile = '/tmp/sample_800k_lines.json'

#global variable that will see if alsoLikes is being run and if so then it is set to true to not cause errors while using other functions
alsoLikes = False

#will find the countries of the users that have viewed the document---------------------------------------------------------------------
def findCountry(UUID):
	if UUID == "":
		Coursework.noDUUID()
	else:
		#stores the countries visited from in a dictionary
		countries = {}
		with open(jfile) as f:
			#line is a single "user" in the json file
			for line in f:
				ind = json.loads(line)	
				#will attempt to find the documents UUID and if one is not supplied it will be stored as "unknown"
				try:
					docId = ind['env_doc_id']
				except KeyError:
					try:
						docId = ind['subject_doc_id']
					except KeyError:
						docId = "Unknown"
				#if the document UUID that the users requested is found it will add it to the dictionary or increase the count for it if it already exists in the dictionary
				if docId == UUID:
					country = ind['visitor_country']
					existCountry = False
					for c in countries:
						if c == country:
							countries[country] = countries[country] + 1
							existCountry = True
					if existCountry == False:
						countries.update({country : 1})
			#if no results are found an error message will be displayed, otherwise a histogram is displayed
			if not bool(countries):
				Coursework.noResults()
			else:
				show_histo(countries,orient="vert",label = "Number of Users", title="Number of countries")

#will find the browsers used by the users that have viewed the document---------------------------------------------------------------------
def findBrowser():
	#stores the browsers visited from in dictionary
	browsers = {}
	with open(jfile) as f:
		#line is a single "user" in the json file
		for line in f:
			ind = json.loads(line)
			#finds the browser 
			browser = ind['visitor_useragent']
			#browser.split was used to find the name of the browsers as the name was always stored before a "/"
			start, end = browser.split('/', 1)
			existsBrowser = False
			for b in browsers:
				if b == start:
					browsers[start] = browsers[start] + 1
					existsBrowser = True
			if existsBrowser == False:
				browsers.update({start : 1})
		if not bool(browsers):
			Coursework.noResults()
		else:
			show_histo(browsers,orient="vert", label = "Number of Users", title="Number of Browsers")
			
#will find the time a user has spent reading a specific document---------------------------------------------------------------------
def findTime(DUUID, VUUID):
	with open(jfile) as f:
		#line is a single "user" in the json file
		for line in f:
			ind = json.loads(line)
			visId = ind['visitor_uuid']
			#will try to find a document UUID but if one is not available the time will be set to 0
			try:
				docId = ind['env_doc_id']
			except KeyError:
				try:
					docId = ind['subject_doc_id']
				except KeyError:
					return 0
			#if the document and visitor UUID match the ones requested, return the time spent reading
			if DUUID == docId and visId == VUUID:
				try:
					time = ind['pagereadtime']
				except:
					try:
						time = ind['event_readtime']
					except:
						time = 0
				return time
	return 0

#will find the documents that have been read by users that have viewed the original document---------------------------------------------------------------------
def alsoLike(DUUID, VUUID, rp, nr):
	#global variable to dictate when this function is being used so findVisitor and findDocument do not give incorrect output
	global alsoLikes
	alsoLikes = True
	#dictionaries to store the documents UUID, visitors UUID and store the time each viewer spent reading a document
	documents = {}
	visitors = {}
	rpTime = {}
	#checks to see if both checkboxes were checked, and if they have not entered the mandatory document UUID and if so display error
	if nr and rp == 1:
		Coursework.tooManySort()
	elif DUUID == "" :
		Coursework.noDUUID()
	else:
		if VUUID == "":
			vis = findVisitor(DUUID)
		else:
			#will run other functions to find a list of document and visitor UUIDs with appropriate counts
			doc = findDocument(VUUID)
			vis = findVisitor(DUUID)
			for d in doc:
				#only find the time spent reading if the user wants to sort it in that manner
				if rp == 1:
					rpTime.update({d: findTime(d, VUUID)})
				visitors.update({VUUID : 1})
				documents.update({d : doc[d]})
		existsVis = False
		#will go through the visitors from each document and add them to a file or update the count to stop multiple counts of the same document
		for v in vis:
			try:
				visitors[v] = visitors[v] + 1
				existsVis = True
			except:
				doc = findDocument(v)
				existsDoc = False
				for d in doc:
					existsDoc = False
					#reads through the known documents and the new ones to find the amount of reads to add or to add the new document to the dictionary
					for docu in documents:
						if d == docu:
							if rp == 1:
								rpTime[d] = rpTime[d] + findTime(d,v)
							documents[d] = documents[d] + doc[d]
							existsDoc = True
					if existsDoc == False:
						if rp ==1:
							rpTime.update({d: findTime(d, v)})
						documents.update({d : doc[d]})
			if existsVis == False:
				visitors.update({v : 1})
		if not bool(documents):
			Coursework.noResults()
		else:
			#this is used to sort the number of readers into the top 10 and then send it to the lbSortInsert function to be displayed
			if nr == 1:
				sortReader = {}
				temp = ([(k, documents[k]) for k in sorted(documents, key=documents.get, reverse=True)][:10])
				for t in temp:
					sortReader.update({t[0] : t[1]})
				Coursework.lbSortInsert(sortReader)
			elif rp == 1:
				sortReader = {}
				temp = ([(k, rpTime[k]) for k in sorted(rpTime, key=rpTime.get, reverse=True)][:10])
				for t in temp:
					sortReader.update({t[0] : t[1]})
				Coursework.lbSortRpInsert(sortReader)
			else:
				Coursework.lbDocInsert(documents)
		alsoLikes = False

#will find the visitors that have read a selected Document---------------------------------------------------------------------------------
def findVisitor(UUID):
	if UUID == "":
		Coursework.noDUUID()
	else:
		#stores the visitors in dictionary
		visitors = {}
		with open(jfile) as f:
			#line is a single "user" in the json file
			for line in f:
				ind = json.loads(line)	
				try:
					docId = ind['env_doc_id']
				except KeyError:
					try:
						docId = ind['subject_doc_id']
					except KeyError:
						docId = "Unknown"
				if docId == UUID:			
					visitor = ind['visitor_uuid']
					existsVisitor = False
					for v in visitors:
						if v == visitor:
							visitors[visitor] = visitors[visitor] + 1
							existsVisitor = True
					if existsVisitor == False:
						visitors.update({visitor : 1})
			if alsoLikes == False:
				if not bool(visitors):
					Coursework.noResults()
				else:
					if alsoLikes == False:
						Coursework.lbInsert(visitors)
			else:
				return visitors
				
#will find the Documents that a selected user has viewed---------------------------------------------------------------------------------
def findDocument(UUID):
	if UUID == "":
		Coursework.noVUUID()
	else:
		#stores the users in dictionary
		documents = {}
		with open(jfile) as f:
			#line is a single "user" in the json file
			for line in f:
				ind = json.loads(line)	
				visId = ind['visitor_uuid']
				if visId == UUID:	
					try:
						document = ind['env_doc_id']
					except KeyError:
						try:
							document = ind['subject_doc_id']
						except KeyError:
							document = "Unknown"		
					existsDocument = False
					for d in documents:
						if d == document:
							documents[document] = documents[document] + 1
							existsDocument = True
					if existsDocument == False:
						documents.update({document : 1})
			if alsoLikes == False:
				if not bool(documents):
					Coursework.noResults()
				else:
					Coursework.lbDocInsert(documents)
			else:
				return documents


#will find the browsers used by the users that have viewed the document------------------------------------------------------------------
def findReader():
	#stores the readers, top ten ouput and temporary data in dictionary
	readers = {}
	topTenr = {}
	temp = {}
	with open(jfile) as f:
		#line is a single "user" in the json file
		for line in f:
			ind = json.loads(line)	
			#sets the time to 0 if can not be found
			reader = ind['visitor_uuid']
			try:
				time = ind['pagereadtime']
			except:
				try:
					time = ind['event_readtime']
				except:
					time = 0
			existsReader = False
			for r in readers:
				if r == reader:
					readers[reader] = readers[reader] + time
					existsReader = True
			if existsReader == False:
				readers.update({reader : time})
		#if statement that will only find the top 10 if there is more than 10 readers, if not it will display whats available
		if len(readers) > 10:
			#stores the top 10 readers into a temporary variable to repopulate the topTenr in anappropriate manner for use in show_histo
			temp = ([(k, readers[k]) for k in sorted(readers, key=readers.get, reverse=True)][:10])
			for p in temp:
				topTenr.update({p[0] : p[1]})
			if not bool(topTenr):
				Coursework.noResults()
			else:
				Coursework.lbTimeInsert(topTenr)
				show_histo(topTenr, orient="vert", label = "Total Time Spent Reading (ms)", title = "Top 10 Readers")
		else:
			if not bool(readers):
				Coursework.noResults()
			else:
				Coursework.lbTimeInsert(readers)
				show_histo(readers, orient="vert", label = "Total Time Spent Reading (ms)", title = "Top Readers")

#create a histogram showing the various details required---------------------------------------------------------------------------------
def show_histo(dict, orient="horiz", label="counts", title="title"):
	#Takes in a dictionary and displays it as a histogram.
	if orient=="horiz":
		bar_fun = plt.barh     
		bar_ticks = plt.yticks
		bar_label = plt.xlabel
	elif orient=="vert":
		bar_fun = plt.bar
		bar_ticks = plt.xticks
		bar_label = plt.ylabel
	else:
		raise Exception("show_histo: Unknown orientation: %s ".format % orient)
	n = len(dict)
	bar_fun(range(n), list(dict.values()), align='center', alpha=0.4)
	bar_ticks(range(n), list(dict.keys())) 
	bar_label(label)
	plt.title(title)
	plt.show()

#will find the continent of the users who have viewed the document ---------------------------------------------------------------------------
def findContinent(UUID):
	if UUID == "":
		Coursework.noDUUID()
	else:
		conti = {}
		with open(jfile) as f:
			#line is a single "user" in the json file
			for line in f:
				ind = json.loads(line)	
				try:
					docId = ind['env_doc_id']
				except KeyError:
					try:
						docId = ind['subject_doc_id']
					except KeyError:
						docId = "Unknown"	
				if docId == UUID:
					country = ind['visitor_country']
					miniCont = myConstants.cntry_to_cont[country] #used to find the continent using the country
					continent = myConstants.continents[miniCont] #used to display Name of continent and not just NA or SA	
					i = 0
					existContinent = False
					for c in conti:
						if c == continent:
							conti[continent] = conti[continent] + 1
							existContinent = True
					if existContinent == False:
						conti.update({continent : 1})
			if not bool(conti):
				Coursework.noResults()
			else:
				show_histo(conti,orient="vert",label = "Number of Users", title="Number of continents represented")
	
