using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CustomSearch
{
    public partial class Form1 : Form
    {
        HashSet<string> resultSet;
        List<SearchResult> newResultsList;
        DBService dbConnector = new DBService();

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
            IWebSearcher[] webSearchers;
            SearchResult[][] results;
            SearchResult[] newResults;            

            if (!string.IsNullOrEmpty(keyword))
            {               
                webSearchers = new IWebSearcher[] { new BingWebSearcher(), new GoogleWebSearcher(), new YandexWebSearcher() };

                results = new SearchResult[webSearchers.Length][];
                for (int i = 0; i < webSearchers.Length; i++)
                {
                    results[i] = webSearchers[i].Search(keyword, resultCount);
                }
                foreach (SearchResult[] array in results)
                    realCount = array != null ? (array.Length < realCount ? array.Length : realCount) : 0;


                for (int j = 0; j < realCount; j++)                    
                {
                    for (int i = 0; i < webSearchers.Length; i++)
                    {
                        addToCommonResultlist(results[i][j]);
                    }
                }
                newResults = newResultsList.ToArray();

                SearchResult[] oldResults = dbConnector.getOldResultsFromDB();

                displayInListBox(oldResults, listBox1);
                displayInListBox(newResults, ResultListBox);

                textBox1.Text = dbConnector.updateDataInDB(oldResults, newResults);
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

        private void displayInListBox(SearchResult[] array, ListBox box)
        {         
            box.DataSource = array;
            box.DisplayMember = "Name";
            box.ValueMember = "Link";
        }

        private void displayInListView(SearchResult[] array, ListView view)
        {
            view.Columns.Add("Link", -2, HorizontalAlignment.Left);
            view.Columns.Add("Text", -2, HorizontalAlignment.Left);
            view.Columns[0].Width = listView1.Width / 2;
            view.Columns[1].Width = listView1.Width / 2;
            view.Items.Clear();
            foreach (SearchResult result in array)
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
                SearchResult[] results = dbConnector.searchInDB(keyword);
                label5.Text = results.Length == 0 ? "Nothing \n found" : "";
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
