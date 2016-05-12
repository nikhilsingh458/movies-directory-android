using System.Collections.ObjectModel;
using Android.App;
using Android.Widget;
using MoviesDirectory.Model;
using omdb;
using Android.Graphics;
using System.Net;

namespace MoviesDirectory.Adapter
{
	/// <summary>
	/// Data adapter.Listview data adapter for custom apperance
	/// </summary>
    public class DataAdapter : BaseAdapter<MoviesDirectory.Model.Movie>
    {
        #region Members

        private readonly ObservableCollection<MoviesDirectory.Model.Movie> _lstmovies;
        private readonly Activity _context;

        #endregion

        #region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MoviesDirectory.Adapter.DataAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="lstmovies">list of movies</param>
        public DataAdapter(Activity context, ObservableCollection<MoviesDirectory.Model.Movie> lstmovies)
        {
            _lstmovies = lstmovies;
            _context = context;
        }

        #endregion

        #region implemented abstract members of BaseAdapter

		/// <param name="position">The position of the item within the adapter's data set whose row id we want.</param>
		/// <summary>
		/// Get the row id associated with the specified position in the list.
		/// </summary>
		/// <returns>To be added.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

		/// <param name="position">The position of the item within the adapter's data set of the item whose view
		///  we want.</param>
		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>
        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            ItemsHolder holder = null;
            var view = convertView;
            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.list_view_details, null);
                holder = new ItemsHolder
                {
                    Year = view.FindViewById<TextView>(Resource.Id.txtyear),
                    Title = view.FindViewById<TextView>(Resource.Id.txtmovietitle),
					ImgPoster=view.FindViewById<ImageView>(Resource.Id.imagePoster)
                };
                view.Tag = holder;
            }
            else
            {
                holder = view.Tag as ItemsHolder;
            }

            var tempitem = _lstmovies[position];

            if (holder == null) return view;

            holder.Year.Text = "Year : " + tempitem.Year;
			if(tempitem.Poster.ToUpper() != "N/A")
			{
				var imageBitmap = GetImageBitmapFromUrl (tempitem.Poster);
				holder.ImgPoster.SetImageBitmap (imageBitmap);
			}
			holder.Title.Text = tempitem.Title;

            return view;
        }
        #endregion implemented abstract members of BaseAdapter

        #region Properties

		/// <summary>
		/// How many items are in the data set represented by this Adapter.
		/// </summary>
		/// <value>To be added.</value>
        public override int Count
        {
            get
            {
                return _lstmovies.Count;
            }
        }

        #endregion

        #region Indexers

		/// <summary>
		/// Gets the <see cref="MoviesDirectory.Adapter.DataAdapter"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
        public override MoviesDirectory.Model.Movie this[int index]
        {
            get
            {
                return _lstmovies[index];
            }
        }

        #endregion

        #region Class

		/// <summary>
		/// Items holder.
		/// </summary>
        private class ItemsHolder : Java.Lang.Object
        {
            public TextView Title { get; set; }
			public ImageView ImgPoster{get;set;}
            public TextView Year { get; set; }
        }

        #endregion

        #region Methods

		/// <summary>
		/// Add the specified movielst.
		/// </summary>
		/// <param name="movielst">Movielst.</param>
        public void Add(MoviesDirectory.Model.Movie movielst)
        {
            _lstmovies.Add(movielst);
            NotifyDataSetChanged();
        }

		/// <summary>
		/// Gets the list.
		/// </summary>
		/// <returns>The list.</returns>
        public ObservableCollection<MoviesDirectory.Model.Movie> GetList()
        {
            return _lstmovies;
        }

		/// <summary>
		/// Gets the image bitmap from URL.
		/// </summary>
		/// <returns>The image bitmap from URL.</returns>
		/// <param name="url">URL.</param>
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
        #endregion
    }
}