using System.Collections.ObjectModel;
using MoviesDirectory.Model;
using System.Collections.Generic;
using System.Json;
using MoviesDirectory.Enums;
using MoviesDirectory.Movies.Interfaces;
using MoviesDirectory.Service;

namespace MoviesDirectory.Movies.Presenter
{
	/// <summary>
	/// Movies list presenter.
	/// </summary>
	public class MoviesListPresenter
	{

		#region Members

		protected ObservableCollection<Movie> Lstmovies = new ObservableCollection<Movie>();
	    readonly IMoviesListView _moviesListView;

		#endregion

		#region Constructor
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="view"></param>
		public MoviesListPresenter (IMoviesListView view)
		{
			_moviesListView = view;
		}

		#endregion

		#region Methods

        /// <summary>
        /// Get Movie Details based on clicked item position
        /// </summary>
        /// <param name="position"></param>
		public void GetMovieDetails(int position)
		{
			Movie selectedMovie = Lstmovies [position];
			_moviesListView.GetMovieName (selectedMovie.Title,selectedMovie.IMDBID);
		}

        /// <summary>
        /// Get Moviesdata Based on search criteria.
        /// </summary>
        /// <param name="searchtext"></param>
		public async void GetMovieData(string searchtext)
		{
			if (searchtext.Length >= 3)
			{
				var jsondictionary = new Dictionary<string, string> { { "s", searchtext } };
				JsonValue jsonresultset = await OMDBService.GetInstanse().Get(RequestType.FIND_MOVIE, jsondictionary);
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
				_moviesListView.MoviesList = Lstmovies;
			} 
			else 
			{
				_moviesListView.AlertMessage ();
			}
				
		}
		#endregion

	}
}

