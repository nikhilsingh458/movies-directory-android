using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System.Net;
using Movie = MoviesDirectory.Model.Movie;
using omdb;

namespace MoviesDirectory.Activities
{
    [Activity(Label = "Movie Detail")]
    public class DetailActivity : Activity
    {
        #region Properties

        public Movie MovieDetails { get; set; }

        #endregion Properties

        #region Controls

        protected ImageView imgPoster;
        protected TextView txtGenre;
        protected TextView txtYear;
        protected TextView txtReleased;
        protected TextView txtIMDBRating;
        protected TextView txtIMDBVotes;
        protected TextView txtMovieTitle;
        protected TextView txtActors;
        protected TextView txtWriters;
        protected TextView txtLanguage;
        protected TextView txtDirector;
        protected TextView txtCountry;

        #endregion Controls

        #region Overidden Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Details);

            imgPoster = FindViewById<ImageView>(Resource.Id.imgUrl);
            txtGenre = FindViewById<TextView>(Resource.Id.txtGenre);
            txtYear = FindViewById<TextView>(Resource.Id.txtYear);
            txtIMDBVotes = FindViewById<TextView>(Resource.Id.txtIMDBVotes);
            txtIMDBRating = FindViewById<TextView>(Resource.Id.txtIMDBRating);
            txtMovieTitle = FindViewById<TextView>(Resource.Id.txtMovieTitle);
            txtActors = FindViewById<TextView>(Resource.Id.txtActors);
            txtWriters = FindViewById<TextView>(Resource.Id.txtWriter);
            txtLanguage = FindViewById<TextView>(Resource.Id.txtLanguage);
            txtDirector = FindViewById<TextView>(Resource.Id.txtDirector);
            txtCountry = FindViewById<TextView>(Resource.Id.txtCountry);

            GetMovieData();
        }

        #endregion Overidden Methods

        #region Methods

        public async void GetMovieData()
        {
            string strMovieTitle = Intent.GetStringExtra("MyData") ?? "Data Not Available";
            OMDBService service = new OMDBService();
            var moviedetail = await service.GetMovie(strMovieTitle, "");
            MovieDetails = moviedetail;

			if (MovieDetails.Poster.ToUpper () != "N/A") {
				var imageBitmap = GetImageBitmapFromUrl (MovieDetails.Poster);
				imgPoster.SetImageBitmap (imageBitmap);
			}

            txtGenre.Text = MovieDetails.Genre;
            txtYear.Text = MovieDetails.Year;
            txtIMDBVotes.Text = MovieDetails.imdbVotes;
            txtIMDBRating.Text = MovieDetails.imdbRating;
            txtMovieTitle.Text = MovieDetails.Title;
            txtActors.Text = MovieDetails.Actors;
            txtWriters.Text = MovieDetails.Writer;
            txtLanguage.Text = MovieDetails.Language;
            txtDirector.Text = MovieDetails.Director;
            txtCountry.Text = MovieDetails.Country;
        }

		public Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        #endregion Methods
    }
}