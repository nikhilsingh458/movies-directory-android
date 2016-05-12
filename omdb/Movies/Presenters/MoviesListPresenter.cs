using System;
using System.Collections.ObjectModel;
using MoviesDirectory.Model;
using System.Collections.Generic;
using System.Json;
using MoviesDirectory.Movies.Interfaces;

namespace MoviesDirectory
{
	public class MoviesListPresenter
	{

		protected ObservableCollection<Movie> Lstmovies = new ObservableCollection<Movie>();
		IMoviesListView moviesListView;

		public MoviesListPresenter (IMoviesListView view)
		{
			moviesListView = view;
		}

		public void GetMovieDetails(int position)
		{
			Movie selectedMovie = Lstmovies [position];
			moviesListView.GetMovieName (selectedMovie.Title,selectedMovie.IMDBID);
		}
		public async void GetMovieData(string Searchtext)
		{
			if (Searchtext.Length >= 3)
			{
				var jsondictionary = new Dictionary<string, string> { { "s", Searchtext } };
				JsonValue jsonresultset = await OMDBService.getInstanse ().get (RequestType.FIND_MOVIE, jsondictionary);
				if (jsonresultset.ContainsKey ("Search")) 
				{
					Lstmovies.Clear ();
					var searchresults = jsonresultset ["Search"];
					for (int i = 0; i < searchresults.Count; i++) 
					{
						if (searchresults [i].ContainsKey ("Title")) 
						 {
							Lstmovies.Add (new Movie 
								{
								Title = searchresults [i] ["Title"],
								Year = searchresults [i] ["Year"],
								IMDBID = searchresults [i] ["imdbID"],
								Type = searchresults [i] ["Type"],
								Poster = searchresults [i] ["Poster"]
								});
						 }
					}

				}
				moviesListView.MoviesList = Lstmovies;
			} 
			else 
			{
				moviesListView.AlertMessage ();
			}
				
		}
	}
}

