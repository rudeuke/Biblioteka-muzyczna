using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka_muzyczna.Data
{
    public class Song
    {
        public int Number { get; set; }
        public string Title { get; set; }

        public Song() { }

        public Song(int number, string title)
        {
            this.Number = number;
            this.Title = title;
        }
    }
}
