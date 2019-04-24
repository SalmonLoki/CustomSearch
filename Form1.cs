using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;

namespace CustomSearch
{
    public partial class Form1 : Form
    {
        HashSet<string> resultSet;
        List<SearchResult> newResultsList;
        DBConnector dbConnector = new DBConnector();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var keyword = SearchOnlineTextBox.Text;
            var resultCount = 10;
            var realCount = resultCount;
            resultSet = new HashSet<string>();
            newResultsList = new List<SearchResult>();
            List<IWebSearcher> webSearchers;
            List<List<SearchResult>> results = new List<List<SearchResult>>();

            if (!string.IsNullOrEmpty(keyword))
            {               
                webSearchers = new List<IWebSearcher>() { new BingWebSearcher(), new GoogleWebSearcher(), new YandexWebSearcher() };

                foreach (IWebSearcher webSearcher in webSearchers)
                {
                    results.Add( webSearcher.Search(keyword, resultCount).ToList());
                }
                foreach (List<SearchResult> list in results)
                    realCount = list != null ? (list.Count < realCount ? list.Count : realCount) : 0;
                for (int j = 0; j < realCount; j++)                    
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        addToCommonResultlist(results.ElementAt(i).ElementAt(j));
                    }
                }

                List<SearchResult> oldResults = dbConnector.getOldResultsFromDB();

                displayInListBox(oldResults, listBox1);
                displayInListBox(newResultsList, ResultListBox);

                dbConnector.updateDataInDB(oldResults, newResultsList, this.textBox1);
            }
            else
            {
                textBox1.Text = "Search query \n is empty";
            }
        }

        private void addToCommonResultlist(SearchResult result)
        {
            if (!resultSet.Contains(result.Link))
            {
                resultSet.Add(result.Link);
                newResultsList.Add(result);
            }
        }

        private void displayInListBox(List<SearchResult> list, ListBox box)
        {         
            box.DataSource = list;
            box.DisplayMember = "Name";
            box.ValueMember = "Link";
        }

        private void displayInListView(List<SearchResult> list, ListView view)
        {
            view.Columns.Add("Link", -2, HorizontalAlignment.Left);
            view.Columns.Add("Text", -2, HorizontalAlignment.Left);
            view.Columns[0].Width = listView1.Width / 2;
            view.Columns[1].Width = listView1.Width / 2;
            view.Items.Clear();
            foreach (SearchResult result in list)
            {
                string[] row = { result.Name };
                view.Items.Add(result.Link).SubItems.AddRange(row);
            }
        }
            
    private void SearchOfflineButton_Click(object sender, EventArgs e)
        {
            var keyword = SearchOfflineTextBox.Text;
            if (!string.IsNullOrEmpty(keyword))
            {
                List<SearchResult> results = dbConnector.searchInDB(keyword);
                label5.Text = results.Count == 0 ? "Nothing \n found" : "";
                displayInListView(results, listView1);
            }
            else
            {
                label5.Text = "Search query \n is empty";
            }
        }


        private void ResultListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var link = ResultListBox.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(link))
                ResultWebBrowser.Navigate(link);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var link = listBox1.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(link))
                ResultWebBrowser.Navigate(link);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                var selectedItems = listView1.SelectedItems;
                foreach (ListViewItem selectedItem in selectedItems)
                {
                    string link = selectedItem.SubItems[0].Text;
                    if (!string.IsNullOrEmpty(link))
                        ResultWebBrowser.Navigate(link);
                }                
            }
        }       
    }
}
