using Biblioteka_muzyczna.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
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
    /// Interaction logic for AlbumList.xaml
    /// </summary>
    public partial class AlbumList : Window
    {
        private List<Album> albumList;





        public AlbumList()
        {
            InitializeComponent();

            ShowAttributes();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenAlbumDetails();
        }

        private void AlbumListDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenAlbumDetails();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowAttributes()
        {
            albumList = DatabaseHelper.GetAlbumList();
            albumListDataGrid.ItemsSource = albumList;
            sortByComboBox.SelectedIndex = 0;
            selectButton.IsEnabled = false;
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (sortByComboBox.SelectedIndex)
            {
                case 0:
                    albumList.Sort((a, b) => a.Id.CompareTo(b.Id));
                    break;

                case 1:
                    albumList.Sort((a, b) => a.Title.CompareTo(b.Title));
                    break;

                case 2:
                    albumList.Sort((a, b) => a.Artist.CompareTo(b.Artist));
                    break;

                case 3:
                    albumList.Sort((a, b) => a.Year.CompareTo(b.Year));
                    break;
            }

            albumListDataGrid.Items.Refresh();
        }

        private void OpenAlbumDetails()
        {
            if (albumListDataGrid.SelectedItem != null)
            {
                Album selected = (Album)albumListDataGrid.SelectedItem;
                AlbumDetails albumDetails = new AlbumDetails(selected.Id);
                albumDetails.Owner = this;
                albumDetails.Closed += new EventHandler(RefreshData);
                albumDetails.ShowDialog();
            }
        }

        private void RefreshData(object sender, EventArgs e)
        {
            ShowAttributes();
        }

        private void AlbumListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectButton.IsEnabled = true;
        }
    }
}
