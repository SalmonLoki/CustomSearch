using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CustomSearch
{
    class DBConnector
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomSearch.Properties.Settings.CustomSearchDBConnectionString"].ConnectionString;


        public List<SearchResult> getOldResultsFromDB()
        {
            List<SearchResult> oldResults = new List<SearchResult>();

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                string sqlQuery = "SELECT * FROM SearchResults";
                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                char[] charsToTrim = { ' ' };
                                oldResults.Add(
                                    new SearchResult(reader["url"].ToString().Trim(charsToTrim), reader["name"].ToString().Trim(charsToTrim))
                                    );
                            }
                        }
                    }
                }
            }
            return oldResults;
        }

        public List<SearchResult> searchInDB(string keyword)
        {
            List<SearchResult> results = new List<SearchResult>();

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                string sqlQuery = "SELECT * FROM SearchResults WHERE name LIKE '%@key%' OR url LIKE concat('%', @key, '%')";
                using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection))
                {
                    command.Parameters.AddWithValue("@key", keyword);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                char[] charsToTrim = { ' ' };
                                results.Add(
                                    new SearchResult(reader["url"].ToString().Trim(charsToTrim), reader["name"].ToString().Trim(charsToTrim))
                                    );
                            }
                        }
                    }
                }
            }
            return results;
        }

        public void updateDataInDB(List<SearchResult> oldResults, List<SearchResult> newResults,
            TextBox textBox, CustomSearchDBDataSetTableAdapters.SearchResultsTableAdapter searchResultsTableAdapter, CustomSearchDBDataSet customSearchDBDataSet)
        {
            if (oldResults.SequenceEqual(newResults))
            {
                textBox.Text = "Данные не устарели";
            }
            else
            {
                textBox.Text = "Данные устарели...Обновление данных. ";

                using (SqlConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    string sqlQuery = "TRUNCATE TABLE SearchResults";
                    using (SqlCommand command = new SqlCommand(sqlQuery, dbConnection))
                    {
                        command.ExecuteNonQuery();
                        searchResultsTableAdapter.Fill(customSearchDBDataSet.SearchResults);

                    }

                    foreach (SearchResult result in newResults)
                    {
                        string sqlQueryInsert = "INSERT INTO SearchResults (url, name) VALUES (@url, @name)";
                        string url = result.Link.Substring(0, Math.Min(result.Link.Length, 500));
                        string name = result.Name.Substring(0, Math.Min(result.Name.Length, 500));
                        using (SqlCommand command = new SqlCommand(sqlQueryInsert, dbConnection))
                        {
                            command.Parameters.AddWithValue("@url", url);
                            command.Parameters.AddWithValue("@name", name);
                            command.ExecuteNonQuery();
                            searchResultsTableAdapter.Fill(customSearchDBDataSet.SearchResults);
                        }
                    }
                }
            }
        }   
    }
}
