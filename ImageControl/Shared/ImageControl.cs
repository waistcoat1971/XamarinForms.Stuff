using System;
using Xamarin.Forms;

namespace Test2
{

	public class ImageControl : View
	{
		public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create<ImageControl, string>( p => p.ImageUrl, default(string) );
		public string ImageUrl
		{
			get { return (string)GetValue( ImageUrlProperty ); }
			set { SetValue( ImageUrlProperty, value ); }
		}

		public static readonly BindableProperty UseCacheProperty = BindableProperty.Create<ImageControl, bool>( p => p.UseCache, true );
		public bool UseCache
		{
			get { return (bool)GetValue( UseCacheProperty ); }
			set { SetValue( UseCacheProperty, value ); }
		}


		public static readonly BindableProperty CacheTTLProperty = BindableProperty.Create<ImageControl, TimeSpan>( p => p.CacheTTL, TimeSpan.FromHours( 24 ) );
		public TimeSpan CacheTTL
		{
			get { return (TimeSpan)GetValue( CacheTTLProperty ); }
			set { SetValue( CacheTTLProperty, value ); }
		}
	}
}
