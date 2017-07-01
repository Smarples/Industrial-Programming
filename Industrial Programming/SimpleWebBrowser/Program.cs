using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading;

namespace Coursework1IP
{
    partial class Form1 : Form
    {
        //VARIABLES:
        //Label variables:
        private Label URLLabel = new Label();
        private Label HomeLabel = new Label();
        private Label FavLabel = new Label();
        private Label FavURLLabel = new Label();
        private Label CurFavLabel = new Label();
        Label[] labels = new Label[50];
        Label[] favlabels = new Label[100];
        Label[] hislabels = new Label[50];

        //Textbox Variables
        private TextBox InputTb = new TextBox();
        private TextBox OutputTb = new TextBox();
        private TextBox HomeURLTb = new TextBox();
        private TextBox HomeChangedTb = new TextBox();
        private TextBox FavNameTb = new TextBox();
        private TextBox FavURLTb = new TextBox();
        private TextBox FavEditTb = new TextBox();
        TextBox[] textboxes = new TextBox[100];

        //Button Variables
        private Button HTMLButton = new Button();
        private Button ChangeHomeB = new Button();
        private Button ViewHistoryB = new Button();
        private Button ViewFavB = new Button();
        private Button AddFavB = new Button();
        private Button RemoveFavB = new Button();
        private Button EditFavB = new Button();
        private Button BackButton = new Button();
        private Button ForwardButton = new Button();
        private Button closeHome = new Button();
        private Button closeFav = new Button();
        private Button closeHis = new Button();
        Button[] buttons = new Button[150];
        Button[] closebuttons = new Button[51];

        //Tab Variables
        private TabControl tabControl1 = new TabControl();
        private TabPage[] myTabPage = new TabPage[50];
        private TabPage HTMLTab = new TabPage();
        private TabPage HomeTab = new TabPage();
        private TabPage HistoryTab = new TabPage();
        private TabPage FavTab = new TabPage();
        private TabPage tabPagenew = new TabPage();
        private TabControl dynamicTabControl;

        //Other Global Variables
        Thread[] threads = new Thread[50];
        Stack[] forward = new Stack[50];
        Stack[] backwards = new Stack[50];
        MainMenu mMenu = new MainMenu();
        private string[,] fav = new string[50, 2];
        private string[] history = new string[50];
        string homeURL = "http://www.example.com";
        string cPage;
        string tb1 = null;
        string editname = "";
        string editURL = "";
        int closeCount = 0;
        int count = 0;
        int tabCount = 1;
        int tbCount = 1;
        int btcount = 0;
        bool favTab = false;
        bool nav = false;
        int x = 5;
        int y = 100;
        bool faved = false;
        string historyfile = "history.txt";
        string favouritesfile = "Favourites.txt";
        string homepagefile = "homePage.txt";

        public Form1()
        {
            //reads the data from the various files into the arrays
            loadHistory();
            loadHomePage();
            loadFavourites();

            //initialise the first two stacks for the back and forward buttons of the first tab
            forward[0] = new Stack();
            backwards[0] = new Stack();
            dynamicTabControl = new TabControl();
            SuspendLayout();

            // Creates the labels for displaying the history and assigns them a location
            int i = 0;
            while(i < hislabels.Length)
            {
                hislabels[i] = new Label();
                hislabels[i].Location = new Point(x, y);
                if(i == (hislabels.Length / 2) - 1)
                {
                    x = 700;
                    y = 70;
                }
                y = y + 20;
                HistoryTab.Controls.Add(hislabels[i]);
                hislabels[i].Click += new EventHandler(label_Click);
                i++;
            }
            i = 0;

            // Creates the labels for displaying the Favourites and assigns them a location
            i = 0;
            x = 5;
            y = 100;
            while (i < favlabels.Length)
            {
                favlabels[i] = new Label();
                favlabels[i].Location = new Point(x, y);
                if (i != 0 && i % 2 != 0)
                {
                    x = x - 100;
                    y = y + 20;
                }
                else
                {
                    if (i == 0)
                    {
                        x = x + 100;
                    }else
                    {
                        x = x + 100;
                    }
                }
                if (i == (favlabels.Length / 2) - 1)
                {
                    x = 300;
                    y = 100;
                }
                //Adds the labels to the Favourites Tab
                FavTab.Controls.Add(favlabels[i]);
                if (i % 2 == 0 || i == 0)
                {
                    //Opens link in new tab if they click on the name of a favourite
                    favlabels[i].Click += new EventHandler(fav_Click);
                }
                else
                {
                    //Opens link in new tab if they click on the URL of a favourite
                    favlabels[i].Click += new EventHandler(label_Click);
                }
                i++;
            }
            i = 0;
            
            //SET UP LABELS WITH VALUES 
            // URLLabel
            URLLabel.AutoSize = true;
            URLLabel.Location = new Point(5, 40);
            URLLabel.Name = "URLLabel";
            URLLabel.Size = new Size(78, 13);
            URLLabel.Text = "Please enter a URL:    ";
   
            // HomeLabel
            HomeLabel.AutoSize = true;
            HomeLabel.Location = new Point(5, 40);
            HomeLabel.Name = "HomeLabel";
            HomeLabel.Size = new Size(78, 13);
            HomeLabel.Text = "Your home page URL:    ";
             
            // FavLabel
            FavLabel.AutoSize = true;
            FavLabel.Location = new Point(450, 40);
            FavLabel.Name = "FavLabel";
            FavLabel.Size = new Size(78, 13);
            FavLabel.Text = "Your Favourite page name:    ";
             
            // FavURLLabel
            FavURLLabel.AutoSize = true;
            FavURLLabel.Location = new Point(450, 80);
            FavURLLabel.Name = "FavURLLabel";
            FavURLLabel.Size = new Size(78, 13);
            FavURLLabel.Text = "Your Favourite page URL:    ";
             
            // CurFavLabel
            CurFavLabel.AutoSize = true;
            CurFavLabel.Location = new Point(500, 170);
            CurFavLabel.Name = "CurFavLabel";
            CurFavLabel.Size = new Size(78, 13);
            CurFavLabel.Text = "Name of favourite you would like to use:    ";

            //SET UP TEXTBOXES WITH VALUES 
            // InputTb
            InputTb.Text = homeURL;
            InputTb.Location = new Point(150, 37);
            InputTb.Name = "InputTb";
            InputTb.Size = new Size(200, 20);
            
            //OutputTb
            OutputTb.Location = new Point(25, 110);
            OutputTb.Multiline = true;
            OutputTb.ScrollBars = ScrollBars.Vertical;
            OutputTb.Name = "OutputTb";
            OutputTb.Size = new Size(700, 400);
            
            // HomeURLTb
            HomeURLTb.Text = homeURL;
            HomeURLTb.Location = new Point(150, 37);
            HomeURLTb.Name = "HomeURLTb";
            HomeURLTb.Size = new Size(200, 20);
            
            // FavNameTb
            FavNameTb.Text = "example";
            FavNameTb.Location = new Point(603, 37);
            FavNameTb.Name = "FavNameTb";
            FavNameTb.Size = new Size(200, 20);
            
            // FavURLTb
            FavURLTb.Text = "http://www.example.com";
            FavURLTb.Location = new Point(603, 75);
            FavURLTb.Name = "FavURLTb";
            FavURLTb.Size = new Size(200, 20);
            
            // FavEditTb
            FavEditTb.Text = "example";
            FavEditTb.Location = new Point(780, 168);
            FavEditTb.Name = "FavEditTb";
            FavEditTb.Size = new Size(300, 20);
            
            //HomeChangedTb
            HomeChangedTb.Location = new Point(25, 110);
            HomeChangedTb.Multiline = true;
            HomeChangedTb.ScrollBars = ScrollBars.Vertical;
            HomeChangedTb.Name = "HomeChangedTb";
            HomeChangedTb.Size = new Size(700, 400);

            //SET UP BUTTONS WITH VALUES 
            // HTMLButton
            HTMLButton.Location = new Point(75, 70);
            HTMLButton.Name = "HTMLButton";
            HTMLButton.Size = new Size(170, 23);
            HTMLButton.Text = "Send HTTP request message";
            HTMLButton.Click += new EventHandler(HTML_Click);
             
            // ChangeHomeB
            ChangeHomeB.Location = new Point(75, 70);
            ChangeHomeB.Name = "ChangeHomeB";
            ChangeHomeB.Size = new Size(170, 23);
            ChangeHomeB.Text = "Change your home page";
            ChangeHomeB.Click += new EventHandler(Home_Click);
             
            // ViewHistoryB
            ViewHistoryB.Location = new Point(75, 70);
            ViewHistoryB.Name = "ViewHistoryB";
            ViewHistoryB.Size = new Size(170, 23);
            ViewHistoryB.Text = "View your history";
            ViewHistoryB.Click += new EventHandler(History_Click);
             
            // ViewFavB
            ViewFavB.Location = new Point(75, 70);
            ViewFavB.Name = "ViewFavB";
            ViewFavB.Size = new Size(170, 23);
            ViewFavB.Text = "View your favourites";
            ViewFavB.Click += new EventHandler(ListFav_Click);
             
            // AddFavB
            AddFavB.Location = new Point(615, 130);
            AddFavB.Name = "AddFavB";
            AddFavB.Size = new Size(170, 23);
            AddFavB.Text = "Add to your favourites";
            AddFavB.Click += new EventHandler(EnterFav_Click);
             
            // RemoveFavB
            RemoveFavB.Location = new Point(750, 210);
            RemoveFavB.Name = "RemoveFavB";
            RemoveFavB.Size = new Size(170, 23);
            RemoveFavB.Text = "Remove from your favourites";
            RemoveFavB.Click += new EventHandler(DeleteFav_Click);

            // EditFavB
            EditFavB.Location = new Point(550, 210);
            EditFavB.Name = "EditFavB";
            EditFavB.Size = new Size(185, 23);
            EditFavB.Text = "Edit favourite with this name";
            EditFavB.Click += new EventHandler(EditFav_Click);
            
            // BackButton
            BackButton.Location = new Point(400, 70);
            BackButton.Name = "BackButton";
            BackButton.Size = new Size(30, 23);
            BackButton.Text = "<";
            BackButton.Click += new EventHandler(BackButton_Click);
            BackButton.Click += new EventHandler(HTML_Click);
            
            // ForwardButton
            ForwardButton.Location = new Point(450, 70);
            ForwardButton.Name = "ForwardButton";
            ForwardButton.Size = new Size(30, 23);
            ForwardButton.Text = ">";
            ForwardButton.Click += new EventHandler(ForwardButton_Click);
            ForwardButton.Click += new EventHandler(HTML_Click);

            //Close Home
            closeHome = new Button();
            closeHome.Location = new Point(500, 10);
            closeHome.Name = "closehome";
            closeHome.Size = new Size(100, 30);
            closeHome.Text = "CloseTab";
            closeHome.Click += new EventHandler(buttonClose_Click);

            //Close Fav
            closeFav = new Button();
            closeFav.Location = new Point(300, 10);
            closeFav.Name = "closefav";
            closeFav.Size = new Size(100, 30);
            closeFav.Text = "CloseTab";
            closeFav.Click += new EventHandler(buttonClose_Click);

            //Close his
            closeHis = new Button();
            closeHis.Location = new Point(500, 10);
            closeHis.Name = "closehis";
            closeHis.Size = new Size(100, 30);
            closeHis.Text = "CloseTab";
            closeHis.Click += new EventHandler(buttonClose_Click);

            //Close Button
            closebuttons[closeCount] = new Button();
            closebuttons[closeCount].Location = new Point(500, 10);
            closebuttons[closeCount].Name = "closebutton" + closeCount;
            closebuttons[closeCount].Size = new Size(100, 30);
            closebuttons[closeCount].Text = "CloseTab";
            closebuttons[closeCount].Click += new EventHandler(buttonClose_Click);
            closeCount++;

            //CREATE TABS AND OPTION MENU
            // Menu
            MenuItem menOptions = new MenuItem();
            MenuItem menHistory = new MenuItem();
            MenuItem menHome = new MenuItem();
            MenuItem menFav = new MenuItem();
            menOptions.Text = ("Options");
            menHistory.Text = ("History");
            menHome.Text = ("Change Your Home Page");
            menFav.Text = ("Favourites");
            menOptions.MenuItems.Add(menHistory);
            menOptions.MenuItems.Add(menHome);
            menOptions.MenuItems.Add(menFav);
            mMenu.MenuItems.Add(menOptions);
            menHistory.Click += new EventHandler(menHistory_Click);
            menHome.Click += new EventHandler(menHome_Click);
            menFav.Click += new EventHandler(menfav_Click);
            Menu = mMenu;
            
            // tab control
            dynamicTabControl.Name = "DynamicTabControl";
            dynamicTabControl.Width =  Width * 2;
            dynamicTabControl.Height = 680;
            dynamicTabControl.Dock = DockStyle.Top;
            dynamicTabControl.SelectedIndexChanged += new EventHandler(buttonNew_SelectedIndexChanged);
            dynamicTabControl.TabPages.Add(HTMLTab);
            dynamicTabControl.TabPages.Add(tabPagenew);

            // Add HTMLTab
            cPage = homeURL;
            HTMLTab.Name = "HTMLTab";
            HTMLTab.Text = cPage.Split('.')[1];

            HTMLTab.Controls.Add(HTMLButton);
            HTMLTab.Controls.Add(closebuttons[0]);
            HTMLTab.Controls.Add(BackButton);
            HTMLTab.Controls.Add(ForwardButton);
            HTMLTab.Controls.Add(URLLabel);
            HTMLTab.Controls.Add(InputTb);
            HTMLTab.Controls.Add(OutputTb);
            HTML_Click(HTMLButton, null);
            
            // Add TabPagenew
            tabPagenew.Name = "tabPagenew";
            tabPagenew.Text = "+";
            
            // Form1
            AutoSize = true;
            AutoScaleDimensions = new SizeF(4F, 11F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(292, 266);
            Width = 800;
            Height = 680;
            Controls.Add(dynamicTabControl);
            Name = "Form1";
            Text = "Simple Web Browser";
            ResumeLayout(false);
            PerformLayout();
        }

        //Code to open Home tab
        private void menHome_Click(object sender, EventArgs e)
        {
            HomeTab.Name = "HomeTab";
            HomeTab.Text = "Change Home Page";

            //populates the home tab with buttons, textboxes and labels
            HomeTab.Controls.Add(ChangeHomeB);
            HomeTab.Controls.Add(HomeURLTb);
            HomeTab.Controls.Add(HomeChangedTb);
            HomeChangedTb.Text = "";
            HomeTab.Controls.Add(HomeLabel);
            HomeTab.Controls.Add(closeHome);

            //Adds the home tab to the list of tabs, but keeps the "+" tab at the end
            dynamicTabControl.TabPages.Insert(dynamicTabControl.TabPages.Count - 1, HomeTab);

        }

        //Code to open favourite tab
        private void menfav_Click(object sender, EventArgs e)
        {
            FavTab.Name = "FavTab";
            FavTab.Text = "Favourites";

            //populates the favourites tab with buttons, textboxes and labels
            FavTab.Controls.Add(FavNameTb);
            FavTab.Controls.Add(FavURLTb);
            FavTab.Controls.Add(FavEditTb);
            FavTab.Controls.Add(ViewFavB);
            FavTab.Controls.Add(AddFavB);
            FavTab.Controls.Add(RemoveFavB);
            FavTab.Controls.Add(EditFavB);
            FavTab.Controls.Add(closeFav);
            FavTab.Controls.Add(FavLabel);
            FavTab.Controls.Add(FavURLLabel);
            FavTab.Controls.Add(CurFavLabel);
            //Adds the favourites tab to the list of tabs, but keeps the "+" tab at the end
            dynamicTabControl.TabPages.Insert(dynamicTabControl.TabPages.Count - 1, FavTab);
        }

        //Code to open History tab
        private void menHistory_Click(object sender, EventArgs e)
        {
            HistoryTab.Name = "HisTab";
            HistoryTab.Text = "History";
            //populates the history tab with buttons
            HistoryTab.Controls.Add(ViewHistoryB);
            HistoryTab.Controls.Add(closeHis);
            //Adds the history tab to the list of tabs, but keeps the "+" tab at the end
            dynamicTabControl.TabPages.Insert(dynamicTabControl.TabPages.Count - 1, HistoryTab);
        }

        //Code to close tabs
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Button temp = (Button)sender;
            //checks to see if the name is one of the options tabs and if so will close it
            if (temp.Name == "closehome")
            {
                dynamicTabControl.TabPages.Remove(HomeTab);
            }
            else if (temp.Name == "closefav")
            {
                dynamicTabControl.TabPages.Remove(FavTab);

            }
            else if (temp.Name == "closehis")
            {
                dynamicTabControl.TabPages.Remove(HistoryTab);

            }
            //if it is not an options tab it will find the name of the button thats closing and will close the tab with that button
            else
            {
                if (int.Parse(temp.Name.Split('n')[1]) != 0)
                {
                    int i = int.Parse(temp.Name.Split('n')[1]);
                    dynamicTabControl.TabPages.Remove(myTabPage[i]);
                    closeCount--;
                    tabCount--;
                    btcount = btcount - 3;
                    tbCount = tbCount - 2;
                }
                else
                {
                    dynamicTabControl.TabPages.Remove(HTMLTab);
                }
            }
        }

        //code to open new tab from Favourites
        private void fav_Click(object sender, EventArgs e)
        {
            int i = 0;
            Label l = (Label)sender;
            //stops the user clicking the empty labels
            if (l.Text == "") { }
            else
            //will find the name in the favourites that matches the labels text and will open the URL in a new tab
            {
                string temp = homeURL;
                while (i < (fav.Length / 2) - 1)
                {
                    if (fav[i, 0] == l.Text)
                    {
                        homeURL = fav[i, 1];
                        break;
                    }
                    else { i++; }
                }
                favTab = true;
                buttonNew_SelectedIndexChanged(null, null);
                homeURL = temp;
            }
        }

        //code to open new tab from history
        private void label_Click(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            //stops the user clicking an empty label
            if (l.Text == "") { }
            else
            //opens a new tab with the URL from the labels text
            {
                string temp = homeURL;
                homeURL = l.Text;
                favTab = true;
                buttonNew_SelectedIndexChanged(null, null);
                homeURL = temp;
            }
        }

        //code to retrieve HTML code and to show error messages
        public void HTML_Click(object sender, EventArgs e)
        {
            Button temp = (Button)sender;
            //code for using textbox in first tab
            if (temp.Name == "HTMLButton" || temp.Name == "BackButton" || temp.Name == "ForwardButton")
                {
                tb1 = InputTb.Text;
            }
            else
                {
                //code for finding the appropriate textbox from the list of tabs
                    tb1 = textboxes[(((int.Parse(temp.Name.Split('n')[1])) / 3) * 2) + 1].Text;
                }
            try
            {
                //sends the users entry as a Http web request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tb1);
                if (nav != true)
                {
                    //will find the appropriate tabs stacks and add the URL to the stack
                    if (temp.Name == "HTMLButton" || temp.Name == "BackButton" || temp.Name == "ForwardButton")
                    {
                        backwards[0].Push(tb1);
                        //if the user has gone back and still has items in the forward stack, delete them
                        if (forward[0].Count != 0)
                        {
                            forward[0].Clear();
                        }
                    }
                    else
                    {
                        int bel = (int.Parse(temp.Name.Split('n')[1])) / 3 + 1;
                        backwards[(int.Parse(temp.Name.Split('n')[1])) / 3 + 1].Push(tb1);
                        if (forward[(int.Parse(temp.Name.Split('n')[1])) / 3 + 1].Count != 0)
                        {
                            forward[(int.Parse(temp.Name.Split('n')[1])) / 3 + 1].Clear();
                        }
                    }
                }
                //retrieves the response from the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //sets the response as a string for printing to a textbox
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                //if the history is full, then start at the beggining
                if(count == 50)
                {
                    count = 0;
                }
                using (StreamWriter sw = new StreamWriter(historyfile, true))
                { // open file
                    sw.WriteLine(tb1);
                }
                loadHistory();
                count++;

                //set the current page to be the text in the tab
                    cPage = tb1;
                //finds the appropriate textbox and outputs the HTML to it
                    if (temp.Name == "HTMLButton" || temp.Name == "BackButton" || temp.Name == "ForwardButton")
                    {
                    HTMLTab.Text = cPage.Split('.')[1];
                    OutputTb.Text = responseString;
                    }
                    else
                    {
                    //each page has 3 buttons, so you divide the button number by 3 to get 1 under the number of tabs, then add 1 to find tab number
                        myTabPage[((int.Parse(temp.Name.Split('n')[1])) / 3) + 1].Text = cPage.Split('.')[1];
                        textboxes[(((int.Parse(temp.Name.Split('n')[1])) / 3) * 2) + 2].Text = responseString;
                    }
                
            }
            //catches the various error messages and outputs the appropriate error message
            catch (UriFormatException)
            {
                if (temp.Name == "HTMLButton" || temp.Name == "BackButton" || temp.Name == "ForwardButton")
                {
                    OutputTb.Text = ("Error code 400 Bad Request.");
                }
                else
                {
                    textboxes[(((int.Parse(temp.Name.Split('n')[1])) / 3) * 2) + 2].Text = ("Error code 400 Bad Request.");
                }
            }
            catch (WebException web)
            {
                HttpWebResponse response = web.Response as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    if (temp.Name == "HTMLButton" || temp.Name == "BackButton" || temp.Name == "ForwardButton")
                    {
                        OutputTb.Text = ("Error code 403 Forbidden.");
                    }
                    else
                    {
                        textboxes[(((int.Parse(temp.Name.Split('n')[1])) / 3) * 2) + 2].Text = ("Error code 403 Forbidden.");
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    if (temp.Name == "HTMLButton" || temp.Name == "BackButton" || temp.Name == "ForwardButton")
                    {
                        OutputTb.Text = ("Error code 404 Not Found.");
                    } else
                    {
                        textboxes[(((int.Parse(temp.Name.Split('n')[1])) / 3) * 2) + 2].Text = ("Error code 404 Not Found.");
                    }
                }
            }
            nav = false;
        }

        //code to change the home page
        private void Home_Click(object sender, EventArgs e)
        {
            //sets the new homepage and then outputs a successfully changed message
            using (StreamWriter sw = new StreamWriter(homepagefile))
            {
                sw.WriteLine(HomeURLTb.Text);
            }
            loadHomePage();
            HomeChangedTb.Text = "Your home page has successfully been changed to: " + homeURL;
        }

        //code to show all items in the history
        private void History_Click(object sender, EventArgs e)
        {
            loadHistory();
            int lcount = 0;
            //starts the loop at the end of the array to output the history in an order that displays the most recent item first
            int i = history.Length - 1;
            //if nothings in the history output a message telling the user
            if (history[0] == null)
            {
                MessageBox.Show("You have no items in your history");
            }
            else
            {
                while(i >= 0)
                {
                    if (history[i] == null) { i--; } else
                    {
                        //sets the appropriate label to the item from the history
                        hislabels[lcount].Text = history[i];
                        hislabels[lcount].AutoSize = true;
                        i--;
                        lcount++;
                    }
                }
            }
        }

        //Code to list the favourites
        private void ListFav_Click(object sender, EventArgs e)
        {
            int lcount = 0;
            int j = 0;
            //starts at the end of the favourites array to print out the items in most recent at the top order
            int i = (fav.Length / 2) - 1;
            while (j < (fav.Length / 2))
            {
                if (fav[i, 0] == null) { i--; }
                else
                {
                    //uses label count to figure out which label to put the name and the URL from the favourites into
                    favlabels[lcount].AutoSize = true;
                    favlabels[lcount].Text = fav[i, 0];
                    lcount++;
                    favlabels[lcount].AutoSize = true;
                    favlabels[lcount].Text = fav[i, 1];
                    i--;
                    lcount++;
                }
                j++;
            }
            if (lcount == 0)
            {
                MessageBox.Show("You have no items in your favourites");
            }

        }

        //Code for enetering a favourite
        private void EnterFav_Click(object sender, EventArgs e)
        {
            bool full = true;
            int i = 0;
            //a loop to find if there is a favourite with the name already in the array
            while (i < (fav.Length / 2))
            {
                if (fav[i, 0] == FavNameTb.Text && faved == false)
                {
                    MessageBox.Show("There is already a Favourite with this name, please enter a different name");
                    full = false;
                    i = 0;
                    break;
                }
                else
                {
                    if(fav[i, 0] != "")
                    {
                        full = false;
                    }
                    i++;
                }
            }
            if (i == (fav.Length / 2) && full == false)
            {
                i = 0;
                //loops through the favourites array to find an empty space to insert the users enteries
                while (i < (fav.Length / 2) - 1)
                {
                    //checks to see if favourites is being edited
                    if (faved == false)
                    {
                        if (fav[i, 0] == null)
                        {
                            using (StreamWriter sw = new StreamWriter(favouritesfile, true))
                            { // open file
                                sw.WriteLine(FavNameTb.Text);
                                sw.WriteLine(FavURLTb.Text);
                            }
                            loadFavourites();
                            i = 0;
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    } else
                    //if favourite is being edited, finds fav of that name and changes it
                    {
                        if (fav[i, 0] == editname)
                        {
                            fav[i, 0] = FavNameTb.Text;
                            fav[i, 1] = FavURLTb.Text;
                            break;
                        }else
                        {
                            i++;
                        }
                        
                    }
                }

                //if favourites are being edited you take the current edited favourites array and write it to the file to save edits
                if (faved == true)
                {
                    using (StreamWriter sw = new StreamWriter(favouritesfile))
                    { // open file
                        int count = 0;
                        while (count < fav.Length / 2)
                        {
                            if (fav[count, 0] != null)
                            {
                                sw.WriteLine(fav[count, 0]);
                                sw.WriteLine(fav[count, 1]);
                            }
                            count++;
                        }
                    }
                }
                faved = false;
            } else if(full == true)
            {
                MessageBox.Show("you have too many items in your favourites, please remove some to continue");
            }
        }

        //code to delete favourites
        private void DeleteFav_Click(object sender, EventArgs e)
        {
            int deleteCount = 0;
            int i = 0;
            //loops through to find the name of the favourite to delete, if it doesnt find it, output that they have no favourite to delete
            while (fav[i, 0] != FavEditTb.Text)
            {
                i++;
                if(i == (fav.Length / 2))
                {
                    MessageBox.Show("You do not have a favourite with this name to delete");
                    break;
                }
            }

            if (i < fav.Length / 2 && fav[i, 0] == FavEditTb.Text)
            {
                int j = 0;
                while(j < (fav.Length / 2))
                {
                    //finds the favourite in the array and sets the name and URL to null
                    if(fav[j,0] == FavEditTb.Text)
                    {
                        fav[j, 0] = null;
                        fav[j, 1] = null;
                        deleteCount++;
                    } else
                    {
                        j++;
                    }
                }
            }
            //after deletion from array, you write the new array of favourites to the file to save the deletion
            using (StreamWriter sw = new StreamWriter(favouritesfile))
            { // open file
                int count = 0;
                while (count < fav.Length / 2)
                {
                    if (fav[count, 0] != null)
                    {
                        sw.WriteLine(fav[count, 0]);
                        sw.WriteLine(fav[count, 1]);
                    }
                    count++;
                }
            }
            int n = 0;
            //after deletion of favourite, changes the labels which would have had a value back to blank
            if(deleteCount > 0)
            {
                while(favlabels[n].Text != "")
                {
                    n++;
                }
                while (deleteCount > 0)
                {
                    n--;
                    favlabels[n].Text = "";
                    n--;
                    favlabels[n].Text = "";
                    deleteCount--;
                }
            }
            loadFavourites();
            ListFav_Click(null, null);
        }
        
        //code to edit details from favourites
        private void EditFav_Click(object sender, EventArgs e)
        {
            int i = 0;
            //loop to find the location of the name in the fav array, if there is no name, then output an error message
            while(i < (fav.Length / 2) - 1)
            {
                if (fav[i, 0] != FavEditTb.Text)
                {
                    i++;
                    if(i == (fav.Length / 2) - 1)
                    {
                        MessageBox.Show("You have no favourites of this name to edit");
                    }
                }else
                //loads the favourite that is meant to be edited to show the user, then allows them to edit the details
                {
                    FavEditTb.Text = "Please edit the details for your favourite above";
                    FavNameTb.Text = fav[i, 0];
                    FavURLTb.Text = fav[i, 1];
                    editname = fav[i, 0];
                    editURL = fav[i, 1];
                    faved = true;
                    break;
                }
            }
        }

        //code to move backwards through pages
        private void BackButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            //checks if the user is using the first tab
            if (b.Name == "BackButton")
            {
                //checks to see if there is a page to go back to
                if(backwards[0].Count == 0 || backwards[0].Count == 1)
                {
                    MessageBox.Show("You have no pages to go back to");
                } else
                {
                    //if there is a page to go back to, pop the URL off the backwards stack and push it onto the forward stack
                    string temp = (string)backwards[0].Pop();
                    forward[0].Push(temp);
                    //ouput the next item in the backwards stack to the textbox to be loaded
                    InputTb.Text = (string)backwards[0].Peek();
                }
            }
            else
            //if the user is using more than 1 tab, the code does the same as above, but locates the correct textbox to output to
            {
                if (backwards[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Count == 0 || backwards[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Count == 1)
                {
                    MessageBox.Show("You have no pages to go back to");
                }
                else
                {
                    string temp = (string)backwards[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Pop();
                    forward[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Push(temp);

                    textboxes[((int.Parse(b.Name.Split('n')[1]) - 1) / 3) * 2 + 1].Text = (string)backwards[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Peek();
                }
            }
            nav = true;
        }

        //code to move forward through pages
        private void ForwardButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            //checks to see if user is using the first tab
            if (b.Name == "ForwardButton")
            {
                //checks if the user can move forward 
                if (forward[0].Count == 0)
                {
                    MessageBox.Show("You have no pages to go forward to");
                }
                else
                {
                    //pops the page off the forwards stack and pushes it onto the backwards stack
                    InputTb.Text = (string)forward[0].Pop();
                    backwards[0].Push(InputTb.Text);
                }
            }
            else
            {
                //does the same as above but locates the appropriate textbox from the amount of tabs
                if (forward[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Count == 0)
                {
                    MessageBox.Show("You have no pages to go forward to");
                }
                else
                {
                    textboxes[((int.Parse(b.Name.Split('n')[1]) - 1) / 3) * 2 + 1].Text = (string)forward[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Pop();
                    backwards[(int.Parse(b.Name.Split('n')[1]) - 1) / 3 + 1].Push(textboxes[((int.Parse(b.Name.Split('n')[1]) - 1) / 3) * 2 + 1].Text);
                }
            }
            nav = true;
        }

        //loads the history from the file into the array
        private void loadHistory()
        {
            //checks to see if file exists to read from, if not it will create the file
            if (File.Exists(historyfile))
            {
                int total = 0;
                int count = 0;
                using (StreamReader sr = new StreamReader(historyfile))
                {    // open file
                    string str = "";
                    while ((str = sr.ReadLine()) != null) // iterate over all lines
                    {
                        //if the history gets above 50, it will replace the earlier visits with more recent ones, when the history reaches 100, clears the file to stop the file getting too big.
                        if (count == 50)
                        {
                            count = 0;
                        }
                        if(total >= 100)
                        {
                            File.WriteAllText(historyfile, string.Empty);
                        }
                        //fills the history array with the items in the file.
                        history[count] = str;
                        count++;
                        total++;
                        
                    }
                }
            } else
            {
                File.Create("history.txt");
                loadHistory();
            }
        }

        //loads the home page url from the file into the variable
        private void loadHomePage()
        {
            //checks to see if theres a file to read from, if not then it creates one
            if (File.Exists(homepagefile))
            {
                int count = 0;
                using (StreamReader sr = new StreamReader(homepagefile))
                {    // open file
                    string str = "";
                    while ((str = sr.ReadLine()) != null) // iterate over all lines
                    {
                        //assigns whats in the file to the homepage for the user
                        homeURL = str;
                        count++;
                    }
                }
            }
            else
            {
                File.Create("homepage.txt");
                loadHomePage();
            }
        }

        //loads the favourites from the file into the array
        private void loadFavourites()
        {
            //checks to see if file exists and if not then it will create the file
            if (File.Exists(favouritesfile))
            {
                int count = 0;
                int scount = 0;
                string str = "";
                using (StreamReader sr = new StreamReader(favouritesfile))
                {    // open file
                    while ((str = sr.ReadLine()) != null) // iterate over all lines
                    {
                        //assigns the contents of the file to the array
                        if (scount == 0)
                        {
                            //first item is the name of the favourite
                            fav[count, scount] = str;
                            //increase scount so that it is ready to input the URL in the array with the name
                            scount++;
                        }
                        else
                        {
                            //assigns the URL to the array
                            fav[count, scount] = str;
                            //increases the count for the next favourite to be put in the array and decreases scount for the name to be entered first
                            count++;
                            scount--;
                        }
                    }
                }
            }
            else
            {
                File.Create("favourites.txt");
                loadFavourites();
            }
        }

        //Code to open new populated tab
        private void buttonNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Code for opening a favourite page in a tab
            if (dynamicTabControl.SelectedTab == tabPagenew || favTab == true)
            {
                //create the new Tab Page
                myTabPage[tabCount] = new TabPage();
                forward[tabCount] = new Stack();
                backwards[tabCount] = new Stack();
                //checks the tab count and outputs an error message if there is too many tabs
                if (tabCount == 50)
                {
                    MessageBox.Show("You have too many tabs open, please close some to continue");
                }
                else
                {
                    //adds a new tab to the tab control
                    dynamicTabControl.TabPages.Insert(dynamicTabControl.TabPages.Count - 1, myTabPage[tabCount]);

                    //sets the details for the tab page and sets the text of the tab to be the name of the current URL
                    cPage = homeURL;
                    myTabPage[tabCount].Name = "myTabPage" + tabCount;
                    myTabPage[tabCount].Text = homeURL.Split('.')[1];
                    myTabPage[tabCount].Font = new Font("Verdana", 12);
                    Controls.Add(dynamicTabControl);

                    //Generates labels, textboxes and buttons to fill new tab

                    textboxes[tbCount] = new TextBox();
                    labels[tabCount] = new Label();
                    buttons[btcount] = new Button();
                    buttons[btcount + 1] = new Button();
                    buttons[btcount + 2] = new Button();
                    closebuttons[closeCount] = new Button();

                    closebuttons[closeCount].Location = new Point(500, 10);
                    closebuttons[closeCount].Name = "closebutton" + closeCount;
                    closebuttons[closeCount].Size = new Size(100, 30);
                    closebuttons[closeCount].Text = "CloseTab";
                    closebuttons[closeCount].Click += new EventHandler(buttonClose_Click);

                    textboxes[tbCount].Text = homeURL;
                    textboxes[tbCount].Location = new Point(200, 37);
                    textboxes[tbCount].Name = "textBox" + tabCount;
                    textboxes[tbCount].Size = new Size(250, 20);
                    tbCount++;

                    textboxes[tbCount] = new TextBox();
                    textboxes[tbCount].Location = new Point(25, 110);
                    textboxes[tbCount].Multiline = true;
                    textboxes[tbCount].ScrollBars = ScrollBars.Vertical;
                    textboxes[tbCount].Name = "textBox" + tabCount + 1;
                    textboxes[tbCount].Size = new Size(700, 400);
                    tbCount++;

                    labels[tabCount].AutoSize = true;
                    labels[tabCount].Location = new Point(5, 40);
                    labels[tabCount].Name = "label" + tabCount;
                    labels[tabCount].Size = new Size(78, 13);
                    labels[tabCount].Text = "Please enter a URL:    ";

                    buttons[btcount].Location = new Point(75, 70);
                    buttons[btcount].Name = "button" + btcount;
                    buttons[btcount].Size = new Size(270, 30);
                    buttons[btcount].Text = "Send HTTP request message";
                    buttons[btcount].Click += new EventHandler(HTML_Click);
                    btcount++;

                    buttons[btcount].Location = new Point(400, 70);
                    buttons[btcount].Name = "button" + btcount;
                    buttons[btcount].Size = new Size(30, 23);
                    buttons[btcount].Text = "<";
                    buttons[btcount].Click += new EventHandler(BackButton_Click);
                    buttons[btcount].Click += new EventHandler(HTML_Click);
                    btcount++;

                    buttons[btcount].Location = new Point(450, 70);
                    buttons[btcount].Name = "button" + btcount;
                    buttons[btcount].Size = new Size(30, 23);
                    buttons[btcount].Text = ">";
                    buttons[btcount].Click += new EventHandler(ForwardButton_Click);
                    buttons[btcount].Click += new EventHandler(HTML_Click);
                    btcount++;

                    //adds all the generated buttons, labels and textboxes to the current tab
                    myTabPage[tabCount].Controls.Add(buttons[btcount - 1]);
                    myTabPage[tabCount].Controls.Add(closebuttons[closeCount]);
                    myTabPage[tabCount].Controls.Add(buttons[btcount - 3]);
                    myTabPage[tabCount].Controls.Add(buttons[btcount - 2]);
                    myTabPage[tabCount].Controls.Add(labels[tabCount]);
                    myTabPage[tabCount].Controls.Add(textboxes[tbCount-1]);
                    myTabPage[tabCount].Controls.Add(textboxes[tbCount-2]);
                    tabCount = tabCount + 1;
                    closeCount++;
                    favTab = false;
                    HTML_Click(buttons[btcount - 3], null);
                }
            }
        }
    }
    public class MainClass
    {
        static public void Main()
        {
            Application.Run(new Form1());
        }
    }
}