using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            // TODO: данная строка кода позволяет загрузить данные в таблицу "customSearchDBDataSet1.SearchResults". При необходимости она может быть перемещена или удалена.
            this.searchResultsTableAdapter1.Fill(this.customSearchDBDataSet1.SearchResults);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var keyword = SearchOnlineTextBox.Text;
            var resultCount = 10;
            resultSet = new HashSet<string>();
            newResultsList = new List<SearchResult>();

            if (!string.IsNullOrEmpty(keyword))
            {
                BingWebSearcher bingSearcher = new BingWebSearcher();
                GoogleWebSearcher googleSearcher = new GoogleWebSearcher();
                YandexWebSearcher yandexSearcher = new YandexWebSearcher();
                var bingResults = bingSearcher.Search(keyword, resultCount);
                var googleResults = googleSearcher.Search(keyword, resultCount);
                var yandexResults = yandexSearcher.Search(keyword, resultCount);
                var newCount = Math.Min(bingResults.Count, Math.Min(googleResults.Count, yandexResults.Count));

                for (int i = 0; i < newCount; i++)
                {
                    addToCommonResultlist(yandexResults.ElementAt(i));
                    addToCommonResultlist(googleResults.ElementAt(i));
                    addToCommonResultlist(bingResults.ElementAt(i));
                }

                List<SearchResult> oldResults = dbConnector.getOldResultsFromDB();
                listBox1.DataSource = oldResults;
                listBox1.DisplayMember = "Name";
                listBox1.ValueMember = "Link";

                ResultListBox.DataSource = newResultsList;
                ResultListBox.DisplayMember = "Name";
                ResultListBox.ValueMember = "Link";

                dbConnector.updateDataInDB(oldResults, newResultsList, this.textBox1, this.searchResultsTableAdapter1, this.customSearchDBDataSet1);
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

        private void SearchOfflineButton_Click(object sender, EventArgs e)
        {
            var keyword = SearchOfflineTextBox.Text;
            if (!string.IsNullOrEmpty(keyword))
            {
                List<SearchResult> results = dbConnector.searchInDB(keyword);
                if (results.Count == 0)
                {
                    label5.Text = "Nothing \n found";
                }
                else
                {
                    label5.Text = "";
                }
                listView1.Columns.Add("Link", -2, HorizontalAlignment.Left);
                listView1.Columns.Add("Text", -2, HorizontalAlignment.Left);
                listView1.Columns[0].Width = listView1.Width / 2;
                listView1.Columns[1].Width = listView1.Width / 2;
                listView1.Items.Clear();
                foreach (SearchResult result in results)
                {
                    string[] row = { result.Name };
                    listView1.Items.Add(result.Link).SubItems.AddRange(row);
                }
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
