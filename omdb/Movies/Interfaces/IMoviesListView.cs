using System;
using System.Collections.ObjectModel;
using MoviesDirectory.Model;

namespace MoviesDirectory.Movies.Interfaces
{
	public interface IMoviesListView
	{
		ObservableCollection<Movie> MoviesList{ get; set; }
		void AlertMessage();
		void GetMovieName(string moviename,string movieid);
	}
}

