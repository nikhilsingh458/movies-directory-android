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
				if(txtSearch.Text.Length >= 3)
					{
				var jsondictionary = new Dictionary<string, string> {{"s", txtSearch.Text}};
					JsonValue jsonresultset = await OMDBService.getInstanse().get(RequestType.FIND_MOVIE, jsondictionary);
						if (jsonresultset.ContainsKey("Search"))
						{
							Lstmovies.Clear();
							var searchresults = jsonresultset["Search"];
							for (int i = 0; i < searchresults.Count; i++)
							{
								if (searchresults[i].ContainsKey("Title"))
								{
									Lstmovies.Add(new Movie
									{
										Title = searchresults[i]["Title"],
										Year = searchresults[i]["Year"],
										imdbID = searchresults[i]["imdbID"],
										Type = searchresults[i]["Type"],
										Poster=searchresults[i]["Poster"]
									});
								}
							}
						MoviesDataAdapter = new DataAdapter(this, Lstmovies);
						Lvmovies.Adapter = MoviesDataAdapter;

						}
					}
            
			Lvmovies.ItemClick += (sender, e) =>
			{
				Movie selectedmovie = Lstmovies[e.Position];
				var detailactivity = new Intent(this, typeof(DetailActivity));
				detailactivity.PutExtra("MyData", selectedmovie.Title);
				StartActivity(detailactivity);
			};
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
            //GetMovieData();
        }

        #endregion Overridden Methods
    }
}