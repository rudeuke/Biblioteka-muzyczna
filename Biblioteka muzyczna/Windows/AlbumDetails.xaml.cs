using Biblioteka_muzyczna.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Biblioteka_muzyczna
{
    /// <summary>
    /// Interaction logic for AlbumDetails.xaml
    /// </summary>
    public partial class AlbumDetails : Window
    {
        private Album album = new Album();





        public AlbumDetails(int albumId)
        {
            album.Id = albumId;
            InitializeComponent();
            ShowAttributes();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Input input = new Input(true, album.Id);
            input.Owner = this;
            input.Closed += new EventHandler(RefreshData);
            input.ShowDialog();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BorrowButton_Click(object sender, RoutedEventArgs e)
        {
            album.Available = !album.Available;
            DatabaseHelper.UpdateAlbumsAvailable(album.Id, album.Available);

            string availableText;
            if (album.Available)
            {
                borrowButton.Content = "Wypożycz";
                availableTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                availableText = "dostępna";
            }
            else
            {
                borrowButton.Content = "Zwróć album";
                availableTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                availableText = "niedostępna";
            }
            availableTextBlock.Text = String.Format("Płyta jest obecnie {0}.", availableText);
        }

        private void FavouriteCheckedChanged(object sender, RoutedEventArgs e)
        {
            album.Favourite = !album.Favourite;
            DatabaseHelper.UpdateAlbumsFavourite(album.Id, album.Favourite);

            favouriteCheckBox.IsChecked = album.Favourite;
        }

        private void ShowAttributes()
        {
            album = DatabaseHelper.GetAlbumDetails(album.Id);

            string availableText;

            titleTextBlock.Text = album.Title;
            artistTextBlock.Text = album.Artist;
            yearTextBlock.Text = album.Year.ToString();
            favouriteCheckBox.IsChecked = album.Favourite;


            if (album.Available)
            {
                borrowButton.Content = "Wypożycz";
                availableTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                availableText = "dostępna";
            }
            else
            {
                borrowButton.Content = "Zwróć album";
                availableTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                availableText = "niedostępna";
            }
            availableTextBlock.Text = String.Format("Płyta jest obecnie {0}.", availableText);


            if (album.ImageByte != null)
            {
                album.Image = ImageHelper.ConvertByteArrayToImage(album.ImageByte);
                albumImage.Source = album.Image;
            }


            songListDataGrid.ItemsSource = album.SongList;
        }

        private void RefreshData(object sender, EventArgs e)
        {
            ShowAttributes();
        }
    }
}
