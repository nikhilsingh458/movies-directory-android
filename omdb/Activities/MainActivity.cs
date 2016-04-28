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
    [Activity(Label = "Movies List", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        #region Members

        protected ObservableCollection<Movie> Lstmovies = new ObservableCollection<Movie>();
        protected ListView Lvmovies;
        protected DataAdapter MoviesDataAdapter;
		protected TextView txtSearch;

        #endregion Members

        #region Methods

        public async void GetMovieData()
        {
			txtSearch.Text = "man";
			txtSearch.Enabled = false;
            var data = new Dictionary<string, string> {{"s", "man"}};
            JsonValue result = await OMDBService.getInstanse().get(RequestType.FIND_MOVIE, data);
            JsonValue resultjson = result;
            if (resultjson.ContainsKey("Search"))
            {
                Lstmovies.Clear();
                var searchresults = resultjson["Search"];
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
                Lvmovies.ItemClick += (sender, e) =>
                {
                    Movie selectedmovie = Lstmovies[e.Position];
                    var detailactivity = new Intent(this, typeof(DetailActivity));
                    detailactivity.PutExtra("MyData", selectedmovie.Title);
                    StartActivity(detailactivity);
                };
            }
        }

        #endregion Methods

        #region Overridden Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Lvmovies = FindViewById<ListView>(Resource.Id.lstview);
			txtSearch = FindViewById<TextView> (Resource.Id.txtSearch);
            GetMovieData();
        }

        #endregion Overridden Methods
    }
}