using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System.Net;
using Movie = MoviesDirectory.Model.Movie;
using omdb;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

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

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
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

		/// <summary>
		/// Get the json response and bind it to the controls
		/// </summary>
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

			txtGenre.Text = GetLines(MovieDetails.Genre).ToString();
            txtYear.Text = MovieDetails.Year;
            txtIMDBVotes.Text = MovieDetails.imdbVotes;
            txtIMDBRating.Text = MovieDetails.imdbRating;
            txtMovieTitle.Text = MovieDetails.Title;
			txtActors.Text = GetLines(MovieDetails.Actors).ToString();
			txtWriters.Text = GetLines(MovieDetails.Writer).ToString();
			txtLanguage.Text = GetLines(MovieDetails.Language).ToString();
			txtDirector.Text = GetLines(MovieDetails.Director).ToString();
			txtCountry.Text = GetLines(MovieDetails.Country).ToString();
        }

		/// <summary>
		/// Gets the image bitmap from URL.
		/// </summary>
		/// <returns>The image bitmap from URL.</returns>
		/// <param name="url">URL.Image url to display the data from json response</param>
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

		public StringBuilder GetLines(string strjsonvalue)
		{
			StringBuilder appendstring = new StringBuilder ();
			List<string> strwords = strjsonvalue.Split(',').Select(p => p.Trim()).ToList();

				for (int wordscount = 0; wordscount < (strwords.Count - 1); wordscount++) 
				{
					appendstring.Append (strwords [wordscount]+ "\n");
				}
			appendstring.Append(strwords[strwords.Count - 1]);

			return appendstring;
		}
        #endregion Methods
    }
}