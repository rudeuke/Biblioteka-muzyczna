using Biblioteka_muzyczna.Data;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace Biblioteka_muzyczna
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NumberOfAlbumsTextBlock.Text = string.Format("Obecna liczba albumów w bibliotece: {0}", DatabaseHelper.GetNumberOfAlbums().ToString());
        }

        private void OpenAlbumListButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumList albumList = new AlbumList();
            albumList.Owner = this;
            albumList.Closed += new EventHandler(RefreshData);
            albumList.ShowDialog();
        }

        private void AddAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            Input input = new Input(false);
            input.Owner = this;
            input.Closed += new EventHandler(RefreshData);
            input.ShowDialog();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RefreshData(object sender, EventArgs e)
        {
            NumberOfAlbumsTextBlock.Text = string.Format("Obecna liczba albumów w bibliotece: {0}", DatabaseHelper.GetNumberOfAlbums().ToString());
        }
    }
}
