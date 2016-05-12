using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MoviesDirectory.Enums;
using MoviesDirectory.Model;


namespace MoviesDirectory.Service
{
/// <summary>
/// OMDB service.Service class to fetch the response
/// </summary>
    public class OMDBService
    {
        private const string OmdbUrl = "http://www.omdbapi.com/?"; // Base omdb api URL
        public string OmdbKey; // A key is required for poster images.
        public Movie Movie; // Initialize movie object
        public Movie MovieList; // Initialize movie list object
        private bool _loop;

		/// <summary>
		/// Gets the instanse.
		/// </summary>
		/// <returns>The instanse.</returns>
        public static OMDBService GetInstanse()
        {
            return new OMDBService();
        }

		/// <summary>
		/// Gets the movie details asynchronously
		/// </summary>
		/// <returns>Movie details</returns>
		/// <param name="query">search query passed from user</param>
		/// <param name="apiKey">API key(if available).</param>
        public async Task<Movie> GetMovie(string query, string apiKey = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OmdbUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(OmdbUrl + "t=" + query);
                if (response.IsSuccessStatusCode)
                {
                    Movie = await response.Content.ReadAsAsync<Movie>();
                    return Movie;
                }
                else
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// Gets the movie list response asynchronously
		/// </summary>
		/// <returns>List of movies based on search parameters</returns>
		/// <param name="query">search query entered by user</param>
		/// <param name="apiKey">API key(if available).</param>
        public async Task<Movie> GetMovieList(string query, string apiKey = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OmdbUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(OmdbUrl + "s=" + query);
                if (response.IsSuccessStatusCode)
                {
                    MovieList = await response.Content.ReadAsAsync<Movie>();
                    return MovieList;
                }
                else
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// Gets the episodes and series list based on type of movie and name asynchronously.
		/// </summary>
		/// <returns>The episodes and series list.</returns>
		/// <param name="query">Search text entered by user</param>
		/// <param name="episodeorseries">type - Episode or series.</param>
		/// <param name="apiKey">API key(if available).</param>
        public async Task<Movie> GetEpisodesAndSeriesList(string query, string episodeorseries, string apiKey = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OmdbUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(OmdbUrl + "s=" + query + "&type=" + episodeorseries);
                if (response.IsSuccessStatusCode)
                {
                    MovieList = await response.Content.ReadAsAsync<Movie>();
                    return MovieList;
                }
                else
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// Get the specified requestType, data and timeOut.
		/// </summary>
		/// <param name="requestType">Request type.Weather Movie name or details</param>
		/// <param name="data">key passed from the UI layer</param>
		/// <param name="timeOut">Response Time out.</param>
		public async Task<JsonValue> Get(RequestType requestType, Dictionary<string, string> data, int timeOut)
        {
            new HttpClient()
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
				Console.Write (e.Message);
                return null;
            }
            finally
            {
                Thread.Sleep(timeOut);
            }
        }

		/// <summary>
		/// Get the specified requestType and data.
		/// </summary>
		/// <param name="requestType">Request type.</param>
		/// <param name="data">Data.</param>
		public async Task<JsonValue> Get(RequestType requestType, Dictionary<string, string> data)
        {
            return await Get(requestType, data, 0);
        }

		/// <summary>
		/// Loops the request.
		/// </summary>
		/// <param name="requestType">Request type.</param>
		/// <param name="data">Data.</param>
		/// <param name="timeout">Timeout.</param>
		public async void loopRequest(RequestType requestType, Dictionary<string, string> data, int timeout)
        {
            _loop = true;
            while (_loop)
            {
                await Get(requestType, data, timeout);
            }
        }

		/// <summary>
		/// Stops the loop.
		/// </summary>
        public void stopLoop()
        {
            _loop = false;
        }

		/// <summary>
		/// Builds the parameters and append.
		/// </summary>
		/// <returns>The parameters.</returns>
		/// <param name="data">Data.</param>
        public string buildParams(Dictionary<string, string> data)
		{
		    if (data != null && data.Count > 0)
            {
                String parameters = data.Keys.Aggregate("?", (current, item) => current + (item + "=" + data[item] + "&"));
                parameters.Substring(0, parameters.Length - 2);
                return parameters;
            }
		    return "";
		}
    }
}