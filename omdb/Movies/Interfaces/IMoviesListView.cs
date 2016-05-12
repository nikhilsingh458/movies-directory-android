using System.Collections.ObjectModel;
using MoviesDirectory.Model;

namespace MoviesDirectory.Movies.Interfaces
{
	public interface IMoviesListView
	{
		#region Properties

		ObservableCollection<Movie> MoviesList{ get; set; }

		#endregion Properties

		#region Methods

		void AlertMessage();
		void GetMovieName(string moviename,string movieid);

		#endregion Methods
	}
}

