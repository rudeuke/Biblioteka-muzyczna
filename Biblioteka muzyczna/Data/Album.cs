using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Biblioteka_muzyczna.Data
{
    class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }
        public bool Favourite { get; set; }
        public bool Available { get; set; }
        public byte[] ImageByte { get; set; }
        public BitmapImage Image { get; set; }
        public List<Song> SongList { get; set; }




        public Album() { }

        public Album(int id, string title, string artist, int year)
        {
            this.Id = id;
            this.Title = title;
            this.Artist = artist;
            this.Year = year;
        }
    }
}
