using System.Collections.ObjectModel;
using Android.App;
using Android.Widget;
using MoviesDirectory.Model;
using omdb;
using Android.Graphics;
using System.Net;

namespace MoviesDirectory.Adapter
{
    public class DataAdapter : BaseAdapter<MoviesDirectory.Model.Movie>
    {
        #region Members

        private readonly ObservableCollection<MoviesDirectory.Model.Movie> _lstmovies;
        private readonly Activity _context;

        #endregion

        #region Constructor

        public DataAdapter(Activity context, ObservableCollection<MoviesDirectory.Model.Movie> lstmovies)
        {
            _lstmovies = lstmovies;
            _context = context;
        }

        #endregion

        #region implemented abstract members of BaseAdapter

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            ItemsHolder holder = null;
            var view = convertView;
            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.ListViewDetails, null);
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

        public override int Count
        {
            get
            {
                return _lstmovies.Count;
            }
        }

        #endregion

        #region Indexers

        public override MoviesDirectory.Model.Movie this[int index]
        {
            get
            {
                return _lstmovies[index];
            }
        }

        #endregion

        #region Class

        private class ItemsHolder : Java.Lang.Object
        {
            public TextView Title { get; set; }
			public ImageView ImgPoster{get;set;}
            public TextView Year { get; set; }
        }

        #endregion

        #region Methods

        public void Add(MoviesDirectory.Model.Movie movielst)
        {
            _lstmovies.Add(movielst);
            NotifyDataSetChanged();
        }

        public ObservableCollection<MoviesDirectory.Model.Movie> GetList()
        {
            return _lstmovies;
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
        #endregion
    }
}