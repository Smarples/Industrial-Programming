import sys
import json #for parsing
from collections import OrderedDict #ordering dictionary
from tkinter import *
from tkinter import ttk #GUI functionality
import tkinter as tk
from tkinter import messagebox

#---------------my python files------------------
import myFunctions

#------------------global GUI for access by functions---------------------------
root = Tk()
root.title("Data Analysis of a Document Tracker")

mainframe = ttk.Frame(root, padding="3 3 12 12")
mainframe.grid(column=0, row=0, sticky=(N, W, E, S))
mainframe.columnconfigure(0, weight=1)
mainframe.rowconfigure(0, weight=1)

#code for the scrollbar and the listbox
yScroll = tk.Scrollbar(mainframe, orient=tk.VERTICAL)
yScroll.grid(column=3, row=9, sticky=N+S+W)
lb = tk.Listbox(mainframe, width = 65, height = 20,yscrollcommand=yScroll.set)
yScroll['command'] = lb.yview

#------------------------------GUI-----------------------------------------------
def GUI():	 
	
	#label and entry box for the Document UUIDs
	UUID = StringVar()
	docUUID = ttk.Entry(mainframe, width=45, textvariable=UUID)
	#docUUID.insert(END, '140228202800-6ef39a241f35301a9a42cd0ed21e5fb0') #used for sample file
	docUUID.insert(END, '140228202800-6ef39a241f35301a9a42cd0ed21e5fb0') #used for cw2 file
	docUUID.grid(column=2, row=1, sticky=(W, E))
	ttk.Label(mainframe, text="Enter the Document UUID").grid(column=1, row=1, sticky=W)
	
	#label and entry box for the visitor UUIDs
	vUUID = StringVar()
	visUUID = ttk.Entry(mainframe, width=45, textvariable=vUUID)
	#visUUID.insert(END, '745409913574d4c6') #used for sample file
	visUUID.insert(END, 'b2a24f14bb5c9ea3') #used for cw2 file
	visUUID.grid(column=2, row=2, sticky=(W, E))
	ttk.Label(mainframe, text="Enter the Viewers UUID").grid(column=1, row=2, sticky=W)
	
	#Series of buttons and labels to run functions to complete the various tasks required
	ttk.Button(mainframe, width = 25, text="Countries", command= lambda: myFunctions.findCountry(docUUID.get())).grid(column=1, row=3, sticky=W)
	ttk.Label(mainframe, text="Please enter a Document UUID for this button").grid(column=2, row=3, sticky=W)
	ttk.Button(mainframe, width = 25, text="Continents", command=lambda: myFunctions.findContinent(docUUID.get())).grid(column=1, row=4, sticky=W)
	ttk.Label(mainframe, text="Please enter a Document UUID for this button").grid(column=2, row=4, sticky=W)
	ttk.Button(mainframe, width = 25, text="Browsers", command=lambda: myFunctions.findBrowser()).grid(column=1, row=5, sticky=W)
	ttk.Button(mainframe, width = 25, text="Readers", command=lambda: myFunctions.findReader()).grid(column=1, row=6, sticky=W)
	ttk.Button(mainframe, width = 25, text="Visitor UUIDs", command=lambda: myFunctions.findVisitor(docUUID.get())).grid(column=3, row=3, sticky=W)
	ttk.Label(mainframe, text="Please enter a Document UUID for this button").grid(column=4, row=3, sticky=W)
	ttk.Button(mainframe, width = 25, text="Document UUIDs", command=lambda: myFunctions.findDocument(visUUID.get())).grid(column=3, row=4, sticky=W)
	ttk.Label(mainframe, text="Please enter a Visitor UUID for this button").grid(column=4, row=4, sticky=W)
	ttk.Button(mainframe, width = 25, text="Also Likes", command=lambda: myFunctions.alsoLike(docUUID.get(),visUUID.get(),rp.get(),nr.get())).grid(column=3, row=5, sticky=W)
	ttk.Label(mainframe, text="Please enter a Document UUID for this button").grid(column=4, row=5, sticky=W)
	
	#code for label and checkbuttons for use in sorting in the Also Like function
	rp = IntVar()
	Checkbutton(mainframe, text ="Sort into top 10 by Readership Profile", variable = rp).grid(column=3, row = 6)
	nr = IntVar()
	Checkbutton(mainframe, text = "Sort into top 10 by Number of Readers", variable = nr).grid(column = 4, row = 6)
	ttk.Label(mainframe, text="To sort the Also Likes ouput, choose one of the checkboxes above").grid(column=3, columnspan = 2, row=9, sticky=N)
	
	#code for placement of listbox
	lb.grid(column = 2, row =9, sticky = W)
	lb.pack()
	
	for child in mainframe.winfo_children(): child.grid_configure(padx=5, pady=5)
	
	docUUID.focus()
	
	root.mainloop()

#function to insert the documents UUID data into the listbox
def lbDocInsert(values):
	#clears the listbox and then inserts a heading so user can understand what they are seeing
	lb.delete(0, lb.size())
	lb.insert(0,"Documents UUID:")
	#will take in the values and display them into the listbox
	for v in values:
		lb.insert(1,v + " visited " + str(values[v]) + " times")

#function to sort values readership values into listbox
def lbSortRpInsert(values):
	lb.delete(0, lb.size())
	lb.insert(0,"Documents UUID:")
	#will take in the values and use the OrderedDict library to order them for display by their secondary value
	ordvalues = OrderedDict(sorted(values.items(), key=lambda t: t[1]))
	for v in ordvalues:
		lb.insert(1,v + " been read for " + str(values[v]) + "(ms)")

#function to insert the sorted values into the listbox
def lbSortInsert(values):
	lb.delete(0, lb.size())
	lb.insert(0,"Documents UUID:")
	#will take in the values and use the OrderedDict library to order them for display by their secondary value
	ordvalues = OrderedDict(sorted(values.items(), key=lambda t: t[1]))
	for v in ordvalues:
		lb.insert(1,v + " visited " + str(values[v]) + " times")

#function to insert Users UUID data into the listbox 
def lbInsert(values):
	lb.delete(0, lb.size())
	lb.insert(0,"Users UUID:")
	for v in values:
		lb.insert(1,v + " - visited this document " + str(values[v]) + " times")

#function to insert Users UUID data with time into the listbox 
def lbTimeInsert(values):
	lb.delete(0, lb.size())
	lb.insert(0,"Users UUID:")
	ordvalues = OrderedDict(sorted(values.items(), key=lambda t: t[1]))
	for v in ordvalues:
		lb.insert(1,v + " - user has spent " + str(values[v]) + "(ms) reading  ")

#function to display error when the user is trying to run a function without the documents UUID when its required
def noDUUID():
	messagebox.showinfo("Error" , "You have not entered a Document UUID.")	

#function to display error when the user is trying to run a function without the visitors UUID when its required
def noVUUID():
	messagebox.showinfo("Error" , "You have not entered a Visitor UUID.")
	
#function to display error when the user is trying to sort the Also Likes in two different ways
def tooManySort():
	messagebox.showinfo("Error" , "You can not apply both sorting methods. Please only select one.")

#function to display error when no results have been found
def noResults():
	messagebox.showinfo("No Results" , "There are no results to your request. Please make sure you have entered any details correctly.")	
