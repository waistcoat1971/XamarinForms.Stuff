ImageControl
============

Simple control designed to display the contents of a remote URL and work round some of the
issues that Xamarin.Form's own UriImageSource is having.

Supports caching of the image to the local file system
Allows you to set a Time-To-Live for the cached item (Defaults to 24hrs)

To Use:

Include ImageControl.cs in your PCL project
Include CustomImageRenderer.cs in your Android Project (iOS to come at a future point)


In your xaml files you can host an image like so:

```
<common:ImageControl  WidthRequest="76" HeightRequest="76" ImageUrl="http://yourhost.com/yourimage.jpg" />
```

This was originally needed to work well in a ListView e.g.

```
<ListView Grid.Row="1" Grid.Column="1" ItemsSource="{Binding Images}">
	<ListView.ItemTemplate>
		<DataTemplate>
			<ViewCell>
				<ViewCell.View>
					<Grid ColumnSpacing="0" RowSpacing="0" HeightRequest="46" >
						<Grid.ColumnDefinitions>
							<ColumnDefinition Width="46" />
							<ColumnDefinition Width="*" />
						</Grid.ColumnDefinitions>
						<common:ImageControl Grid.Column="0"  WidthRequest="46" HeightRequest="46" ImageUrl="{Binding Url}" UseCache="true" BackgroundColor="Red" />
						<Label Grid.Column="1" Text="{Binding Name}" />
					</Grid>
				</ViewCell.View>
			</ViewCell>

		</DataTemplate>
	</ListView.ItemTemplate>
</ListView>
```

It is by no means currently a fully tested implementation but I hope you find it useful.

