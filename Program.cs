using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace search
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetLinks();
        }

        public static void GetLinks()
        {
            try
            {
                string folderPath = @"D:\GoogleSearch\";
                
                string KeywordCount = @"D:\GoogleSearch\CountKeyword\";
                Directory.CreateDirectory(folderPath);
                Directory.CreateDirectory(KeywordCount);
                Console.WriteLine("Enter Your Search Keyword:-");
                string query = Console.ReadLine();
                string apiKey = "AIzaSyCtj9-Q5SEuFA43dUmjRLU7pezJlNjwup8";
                string cx = "551e5e1323162447f";

                // Make the HTTP request to the Google Search API
                string url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cx}&q={WebUtility.UrlEncode(query)}";
                WebClient webClient = new WebClient();
                string jsonResponse = webClient.DownloadString(url);

                // Parse the JSON response
                JObject response = JObject.Parse(jsonResponse);
                JArray items = (JArray)response["items"];

                // Create a list to store the search results
                List<string> searchResults = new List<string>();

              
                // Extract the search result titles and URLs
                foreach (JToken item in items)
                {
                    //string title = (string)item["title"];
                    string url1 = (string)item["link"];
                    searchResults.Add(url1);

                }

                // Display the search results
                //foreach (string result in searchResults)
                //{

                //    Console.WriteLine(result);

                //}

                // string path = @"D:\GoogleSearch\search_results.txt";


                using (StreamWriter writer = new StreamWriter(folderPath + "" + "search_results.txt"))
                {
                    foreach (string result in searchResults)
                    {
                        writer.WriteLine(result);
                    }
                }



                using (StreamWriter writer = new StreamWriter(KeywordCount + "" + "search_results_withcount.txt"))
                {
                    string url1 = "";
                    string text = "";
                    string keyword = "";
                   
                    foreach (string result in searchResults)
                    {
                        url1 = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cx}&q={WebUtility.UrlEncode(result)}";
                        WebClient webClient1 = new WebClient();
                        string jsonResponse1 = webClient.DownloadString(url);

                        // Parse the JSON response
                        JObject response1 = JObject.Parse(jsonResponse1);

                        text = response1.ToString();
                        //Console.WriteLine("Enter the keyword to search:");
                        keyword = query;

                        int count = CountKeyword(text, keyword);

                        writer.WriteLine(result+" "+" Keyword Count=" +count);
                        //writer.WriteLine(jsonResponse1);
                    }
                }

                Console.WriteLine("Search results written to file.");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ReadLine();
            }
           
        }
        static int CountKeyword(string text, string keyword)
        {
            // Use case-insensitive search by specifying RegexOptions.IgnoreCase
            Regex regex = new Regex(keyword, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(text);

            return matches.Count;
        }
    }



}
