#!/usr/bin/env python
#main function to run the project
import sys
import Coursework
import myFunctions

DUUID = ""
VUUID = ""
#task 1 - used to find countries of readers
#task 2 - used to find continents of readers
#task 3 - used to find browsers of readers
#task 4 - used to find top 10 readers
#task 5 - used to find Document UUIDs from a user
#task 6 - used to find Visitor UUIDs in a document
#task 7 - used to find also liked documents from a document UUID
#function to find the task requested by user and run the appropriate task.
def taskNumber(task):
	if task == "1":
		if DUUID == "":
			print("You must enter a Document UUID to run this task")
		else:
			myFunctions.findCountry(DUUID)
			Coursework.GUI()
	elif task == "2":
		if DUUID == "":
			print("You must enter a Document UUID to run this task")
		else:
			myFunctions.findContinent(DUUID)
	elif task == "3":
		myFunctions.findBrowser()
	elif task == "4":
		myFunctions.findReader()
	elif task == "5":
		if VUUID == "":
			print("You must enter a Visitor UUID to run this task")
		else:
			myFunctions.findDocument(VUUID)
			Coursework.GUI()
	elif task == "6":
		if DUUID == "":
			print("You must enter a Document UUID to run this task")
		else:
			myFunctions.findVisitor(DUUID)
			Coursework.GUI()
	elif task == "7":
		if DUUID == "":
			print("You must enter a Document UUID  to run this task")
		else:
			myFunctions.alsoLike(DUUID, VUUID, 0, 0)
			Coursework.GUI()
	else:
		print("there was some sort of error, please choose a task number between 1-7")

#will try to assign the first argument in sys.argv and if it fails, it just runs the program normally
try:
	#made up of if statements to see what the user has entered and to save the details appropriately and run tasks correctly
	if sys.argv[1] == "-u":
		VUUID = sys.argv[2]
		if sys.argv[3] == "-t":
			task = sys.argv[4]
			taskNumber(task)
		elif sys.argv[3] == "-d":
			DUUID = sys.argv[4]
			if sys.argv[5] == "-t":
				task = sys.argv[6]
				taskNumber(task)
			else:
				print("please enter a task number")
		else:
			print("please enter a task number")
	elif sys.argv[1] == "-d":
		DUUID = argv[2]
		if sys.argv[3] == "-t":
			task = sys.argv[4]
			taskNumber(task)
		else:
			print("please enter a task number")
	elif sys.argv[1] == "-t":
		task = sys.argv[2]
		if task == "3":
			myFunctions.findBrowser()
		if task == "4":
			myFunctions.findReader()
		else:
			print("please enter task 3 or 4, or enter a document UUID or visitor UUID to complete more tasks")
except:
	Coursework.GUI()
