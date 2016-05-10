using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Json;
using MoviesDirectory.Model;
using MoviesDirectory.Adapter;
using omdb;
using Android.Views;
using System.Linq;
using Java.IO;


namespace MoviesDirectory.Activities
{
	/// <summary>
	/// Main activity.Application Startup Activity
	/// </summary>
    [Activity(Label = "Movies List", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        #region Members

        protected ObservableCollection<Movie> Lstmovies = new ObservableCollection<Movie>();
        protected ListView Lvmovies;
        protected DataAdapter MoviesDataAdapter;
		protected TextView txtSearch;
		protected ImageButton imgbtnMovie;

        #endregion Members

        #region Methods

		/// <summary>
		/// Get the json response and bind it to the listview
		/// </summary>
        public async void GetMovieData()
        {
			if (txtSearch.Text.Length >= 3) {
				var jsondictionary = new Dictionary<string, string> { { "s", txtSearch.Text } };
				JsonValue jsonresultset = await OMDBService.getInstanse ().get (RequestType.FIND_MOVIE, jsondictionary);
				SaveJSONData ();
				if (jsonresultset.ContainsKey ("Search")) {
					Lstmovies.Clear ();
					var searchresults = jsonresultset ["Search"];
					for (int i = 0; i < searchresults.Count; i++) {
						if (searchresults [i].ContainsKey ("Title")) {
							Lstmovies.Add (new Movie {
								Title = searchresults [i] ["Title"],
								Year = searchresults [i] ["Year"],
								imdbID = searchresults [i] ["imdbID"],
								Type = searchresults [i] ["Type"],
								Poster = searchresults [i] ["Poster"]
							});
						}
					}
					MoviesDataAdapter = new DataAdapter (this, Lstmovies);
					Lvmovies.Adapter = MoviesDataAdapter;

				}
			} 
			else 
			{
				AlertDialog.Builder alertdialog = new AlertDialog.Builder (this);
				alertdialog.SetTitle ("Information");
				alertdialog.SetMessage ("Please enter atleast 3 characters for the movie search.");
				alertdialog.SetPositiveButton ("OK", (senderAlert, args) => {});
				Dialog dialog = alertdialog.Create();
				dialog.Show();
			}
            
			Lvmovies.ItemClick += (sender, e) =>
			{
				Movie selectedmovie = Lstmovies[e.Position];
				var detailactivity = new Intent(this, typeof(DetailActivity));
				detailactivity.PutExtra("MyData", selectedmovie.Title);
				StartActivity(detailactivity);
			};
        }

		private void SaveJSONData()
		{
		}

        #endregion Methods

        #region Overridden Methods

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Lvmovies = FindViewById<ListView>(Resource.Id.lstview);
			txtSearch = FindViewById<TextView> (Resource.Id.txtSearch);
			imgbtnMovie=FindViewById<ImageButton>(Resource.Id.imgbtnMovie);
			imgbtnMovie.Click += ((sender, e) => {
				GetMovieData();
			});
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
			MenuInflater.Inflate(Resource.Drawable.ActionMenu, menu);
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
				Lstmovies = new ObservableCollection<Movie> (Lstmovies.OrderBy (moviename => moviename.Title).ToList ());
				MoviesDataAdapter = new DataAdapter (this, Lstmovies);
				MoviesDataAdapter.NotifyDataSetChanged ();
				Lvmovies.Adapter = MoviesDataAdapter;
				item.SetVisible (false);
				return true;
			case Resource.Id.sortingdesc:
				Lstmovies = new ObservableCollection<Movie> (Lstmovies.OrderByDescending (moviename => moviename.Title).ToList ());
				MoviesDataAdapter = new DataAdapter (this, Lstmovies);
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