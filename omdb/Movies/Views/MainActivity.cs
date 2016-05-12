using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Collections.ObjectModel;
using MoviesDirectory.Model;
using MoviesDirectory.Adapter;
using omdb;
using Android.Views;
using System.Linq;
using MoviesDirectory.Movies.Interfaces;
using MoviesDirectory.Movies.Presenter;


namespace MoviesDirectory.Movies.Views
{
	/// <summary>
	/// Main activity.Application Startup Activity
	/// </summary>
    [Activity(Label = "Movies List", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity,IMoviesListView
    {
        #region Members

        protected ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
		private MoviesListPresenter _presenter;

        #endregion Members

		#region Controls

		protected ListView Lvmovies;
		protected DataAdapter MoviesDataAdapter;
		protected TextView txtSearch;
		protected ImageButton imgbtnMovie;

		#endregion

		#region IMoviesListView implementation

		/// <summary>
		/// Gets or sets the movies list.
		/// </summary>
		/// <value>The movies list for the listview</value>
		public ObservableCollection<Movie> MoviesList
		{
			get 
			{
				return movies;
			}

			set 
			{
				movies = value;
				MoviesDataAdapter = new DataAdapter (this, movies);
				Lvmovies.Adapter = MoviesDataAdapter;
			}
		}

		/// <summary>
		/// Gets the name of the movie.
		/// </summary>
		/// <param name="moviename">Moviename.Parameter passed from the presenter to the view</param>
		/// <param name="movieid">Movieid.Parameter passed from presenter to the view</param>
		public void GetMovieName (string moviename, string movieid)
		{
			var detailactivity = new Intent(this, typeof(DetailActivity));
			detailactivity.PutExtra("MovieName", moviename);
			StartActivity(detailactivity);
		}

		/// <summary>
		/// Alert message for the Movie Search.
		/// </summary>
		public void AlertMessage ()
		{
			AlertDialog.Builder alertdialog = new AlertDialog.Builder (this);
			alertdialog.SetTitle ("Information");
			alertdialog.SetMessage ("Please enter atleast 3 characters for the movie search.");
			alertdialog.SetPositiveButton ("OK", (senderAlert, args) => {});
			Dialog dialog = alertdialog.Create();
			dialog.Show();
		}

		#endregion

		#region Overridden Methods

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
			_presenter = new MoviesListPresenter (this);
			Lvmovies = FindViewById<ListView>(Resource.Id.listview_movies);
			txtSearch = FindViewById<TextView> (Resource.Id.txt_search);
			imgbtnMovie=FindViewById<ImageButton>(Resource.Id.image_btnmovie);

            //Presenter calls

			imgbtnMovie.Click += ((sender, e) => _presenter.GetMovieData(txtSearch.Text));

			Lvmovies.ItemClick += (sender, e) =>_presenter.GetMovieDetails(e.Position);
        }

		/// <param name="menu">The options menu as last shown or first initialized by
		///  onCreateOptionsMenu().For ascending and descending the movies based on title</param>
		/// <summary>
		/// Prepare the Screen's standard options menu to be displayed.
		/// </summary>
		/// <returns>list of optionsmenu items</returns>
		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			menu.Clear ();
			MenuInflater.Inflate(Resource.Drawable.actionmenu, menu);
			return base.OnPrepareOptionsMenu(menu);
		}

		/// <param name="item">The menu item that was selected.</param>
		/// <summary>
		/// This hook is called whenever an item in your options menu is selected.
		/// </summary>
		/// <returns>true if the item presents in the options menu</returns>
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.sorting:
				movies = new ObservableCollection<Movie> (movies.OrderBy (moviename => moviename.Title).ToList ());
				MoviesDataAdapter = new DataAdapter (this, movies);
				MoviesDataAdapter.NotifyDataSetChanged ();
				Lvmovies.Adapter = MoviesDataAdapter;
				item.SetVisible (false);
				return true;
			case Resource.Id.sortingdesc:
				movies = new ObservableCollection<Movie> (movies.OrderByDescending (moviename => moviename.Title).ToList ());
				MoviesDataAdapter = new DataAdapter (this, movies);
				MoviesDataAdapter.NotifyDataSetChanged ();
				Lvmovies.Adapter = MoviesDataAdapter;
				item.SetVisible (false);
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}
        #endregion Overridden Methods
    }
}