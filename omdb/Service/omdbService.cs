using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MoviesDirectory.Model;


namespace MoviesDirectory
{
    public class OMDBService
    {
        private const string omdbUrl = "http://www.omdbapi.com/?"; // Base omdb api URL
        public string omdbKey; // A key is required for poster images.
        public Movie newMovie; // Initialize movie object
        public Movie newMovieList; // Initialize movie list object
        private bool loop = false;

        public static OMDBService getInstanse()
        {
            return new OMDBService();
        }

        public async Task<Movie> GetMovie(string query, string apiKey = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(omdbUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(omdbUrl + "t=" + query);
                if (response.IsSuccessStatusCode)
                {
                    newMovie = await response.Content.ReadAsAsync<Movie>();
                    return newMovie;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Movie> GetMovieList(string query, string apiKey = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(omdbUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(omdbUrl + "s=" + query);
                if (response.IsSuccessStatusCode)
                {
                    newMovieList = await response.Content.ReadAsAsync<Movie>();
                    return newMovieList;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Movie> GetEpisodesAndSeriesList(string query, string episodeorseries, string apiKey = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(omdbUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(omdbUrl + "s=" + query + "&type=" + episodeorseries);
                if (response.IsSuccessStatusCode)
                {
                    newMovieList = await response.Content.ReadAsAsync<Movie>();
                    return newMovieList;
                }
                else
                {
                    return null;
                }
            }
        }

		public async Task<JsonValue> get(RequestType requestType, Dictionary<string, string> data, int timeOut)
        {
            var httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(3000)
            };
            string url;
            string method;
            switch (requestType)
            {
                case RequestType.FIND_MOVIE:
                    url = "http://www.omdbapi.com/";
                    method = "GET";
                    break;

                case RequestType.MOVIE_DATA:
                    url = "http://www.omdbapi.com/";
                    method = "GET";
                    break;

                default:
                    url = "";
                    method = "GET";
                    break;
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url + buildParams(data)));
                request.ContentType = "application/json";
                request.Method = method;

                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Use this stream to build a JSON document object:
                        JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                        Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                        // Return the JSON document:
                        return JsonValue.Parse(jsonDoc.ToString());
                    }
                }
            }
            catch (HttpRequestException e)
            {
                //oops
                return null;
            }
            finally
            {
                Thread.Sleep(timeOut);
            }
        }

		public async Task<JsonValue> get(RequestType requestType, Dictionary<string, string> data)
        {
            return await get(requestType, data, 0);
        }

		public async void loopRequest(RequestType requestType, Dictionary<string, string> data, int timeout)
        {
            loop = true;
            while (loop)
            {
                await get(requestType, data, timeout);
            }
        }

        public void stopLoop()
        {
            loop = false;
        }

        public string buildParams(Dictionary<string, string> data)
        {
            if (data != null && data.Count > 0)
            {
                String s = "?";
                foreach (string item in data.Keys)
                {
                    s += item
                        + "=" + data[item] + "&";
                }
                s.Substring(0, s.Length - 2);
                return s;
            }
            else
            {
                return "";
            }
        }
    }
}