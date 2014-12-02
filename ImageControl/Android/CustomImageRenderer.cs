using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Views;
using Android.Media;
using Android.Widget;
using System.Threading.Tasks;
using Android.Graphics;
using System.Diagnostics;
using Test2.Droid;
using Test2;
using System.IO;
using Android.OS;
using Java.IO;
using Java.Net;


[assembly: ExportRenderer(typeof(ImageControl), typeof(CustomImageRenderer))]

namespace Test2.Droid
{

	public class CustomImageRenderer : ViewRenderer<ImageControl,Android.Widget.FrameLayout>
	{
		ImageControl m_control;
		private Android.Widget.ImageView m_iconImageView;
		private Bitmap m_iconBitmap;

		private static object s_lock = new object();


		public CustomImageRenderer()
		{
		}


		public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{

			try
			{
				var val = base.GetDesiredSize( widthConstraint, heightConstraint );

				return val;
			}
			catch ( Exception ex )
			{
				return new SizeRequest();
			}
		}

		async protected override void OnElementChanged(ElementChangedEventArgs<ImageControl> e)
		{
			base.OnElementChanged(e);

			m_control = e.NewElement;

			if ( e.OldElement == null )
			{
				var nativeControl = new Android.Widget.FrameLayout( Context );
				nativeControl.LayoutParameters = new FrameLayout.LayoutParams( LayoutParams.MatchParent, LayoutParams.MatchParent );
				nativeControl.SetBackgroundColor( m_control.BackgroundColor.ToAndroid() );


				m_iconImageView = new Android.Widget.ImageView( Context );
				m_iconImageView.LayoutParameters = new FrameLayout.LayoutParams( LayoutParams.MatchParent, LayoutParams.MatchParent );
				nativeControl.AddView( m_iconImageView );

				m_iconBitmap = await GetBitmapAsync( m_control.ImageUrl );
				if ( m_iconBitmap != null )
				{
					m_iconImageView.SetImageBitmap( m_iconBitmap );
				}

				SetNativeControl( nativeControl );
			}
			else
			{
				if ( m_control.ImageUrl != e.OldElement.ImageUrl )
				{
					m_iconBitmap = await GetBitmapAsync( m_control.ImageUrl );
					if ( m_iconBitmap != null )
					{
						m_iconImageView.SetImageBitmap( m_iconBitmap );
					}
				}
			}
		}

		private Task<Bitmap> GetBitmapAsync(string source)
		{
			return Task.Factory.StartNew<Bitmap>( () => LoadBitmap( source ) );;
		}

		private Bitmap LoadBitmap( string source )
		{
			try
			{
				if ( m_control.UseCache )
				{
					lock ( s_lock )
					{
						int urlHash = source.GetHashCode();

						string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
						string filePath = System.IO.Path.Combine(path, string.Format( "{0}.img", urlHash ) );
						if ( System.IO.File.Exists( filePath )  )
						{
							var fileAge = DateTime.UtcNow - System.IO.File.GetCreationTimeUtc( filePath );

							if ( fileAge < m_control.CacheTTL )
							{
								using ( var fs = System.IO.File.OpenRead( filePath ) )
								{
									var bitmap = BitmapFactory.DecodeStream( fs );
									return bitmap;
								}
							}
						}


						using ( var fs = System.IO.File.Create( filePath ) )
						{
							Java.Net.URL url = new Java.Net.URL( source );

							using (URLConnection connection = url.OpenConnection())
							{
								using ( var inputStream = connection.InputStream )
								{
									inputStream.CopyTo( fs );
								}
							}


							fs.Seek( 0, SeekOrigin.Begin );

							var bitmap = BitmapFactory.DecodeStream( fs );

							return bitmap;
						}
					}
				}
				else
				{
					Java.Net.URL url = new Java.Net.URL( source );

					var bitmap = BitmapFactory.DecodeStream( url.OpenConnection().InputStream );

					return bitmap;
				}

			}
			catch ( Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( "Problem Loading Image {0} - {1}", source, ex );
				return null;	
			}

		}
	}
}

