using Biblioteka_muzyczna.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.SqlClient;
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
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : Window
    {
        private string imagePath = "";
        private bool update;
        private Album album = new Album();





        public Input(bool edit, int albumId = -1)
        {
            InitializeComponent();
            update = edit;

            if (edit)
            {
                album.Id = albumId;
                ShowAttributes();
            }
            else
            {
                deleteButton.Visibility = Visibility.Hidden;
                album.SongList = new List<Song>();
                songListDataGrid.ItemsSource = album.SongList;
            }
        }

        private void ShowAttributes()
        {
            album = DatabaseHelper.GetAlbumDetails(album.Id);
            album.Image = ImageHelper.ConvertByteArrayToImage(album.ImageByte);

            titleTextBox.Text = album.Title;
            artistTextBox.Text = album.Artist;
            yearTextBox.Text = album.Year.ToString();
            albumImage.Source = album.Image;
            songListDataGrid.ItemsSource = album.SongList;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog loadImage = new OpenFileDialog();
            loadImage.DefaultExt = ".png";
            loadImage.Filter = "Pliki graficzne |*.png; *.jpg";

            if (loadImage.ShowDialog() == true)
            {
                Uri fileIdentifier = new Uri(loadImage.FileName);
                albumImage.Source = new BitmapImage(fileIdentifier);
                imagePath = loadImage.FileName;

                album.ImageByte = ImageHelper.ConvertImageToByteArray(new Bitmap(imagePath));
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            int year = 0;
            bool conversion = Int32.TryParse(yearTextBox.Text, out year);

            if (titleTextBox.Text == "" || artistTextBox.Text == "" || yearTextBox.Text == "" || !album.SongList.Any())
            {
                MessageBox.Show("Nie podano wystarczających informacji.");
            }

            else if (!conversion || year <= 0)
            {
                MessageBox.Show("Podany rok nie jest poprawną liczbą.");
            }

            else
            {
                album.Title = titleTextBox.Text;
                album.Artist = artistTextBox.Text;
                album.Year = year;

                if (update)
                {
                    DatabaseHelper.UpdateAlbumDetails(album);
                }
                else
                {
                    if (imagePath == "")
                    {
                        string projectFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                        string dataFolderPath = System.IO.Path.Combine(projectFolderPath, "Data");
                        imagePath = System.IO.Path.Combine(dataFolderPath, "noImage.jpg");

                        album.ImageByte = ImageHelper.ConvertImageToByteArray(new Bitmap(imagePath));
                    }

                    DatabaseHelper.SetAlbumDetails(album);
                }

                MessageBox.Show("Operacja zakończona pomyślnie.");
                this.Close();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmation = MessageBox.Show("Czy na pewno chcesz usunąć ten album?", "Usuwanie albumu", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmation==MessageBoxResult.Yes)
            {
                DatabaseHelper.DeleteAlbum(album.Id);
                MessageBox.Show("Operacja zakończona pomyślnie.");
                this.Owner.Close();
                this.Close();
            }
        }
    }
}
