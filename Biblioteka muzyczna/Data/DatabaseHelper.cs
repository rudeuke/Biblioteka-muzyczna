using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biblioteka_muzyczna.Data
{
    static class DatabaseHelper
    {
        private static SQLiteConnection Connection = new SQLiteConnection(string.Format("Data source={0}", System.IO.Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "Data/Database.db")));



        public static void Connect()
        {
            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd połączenia z bazą danych.");
                throw new Exception(e.Message);
            }
        }

        public static void Disonnect()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd zamknięcia bazy danych.");
                throw new Exception(e.Message);
            }
        }

        public static int GetNumberOfAlbums()
        {
            Connect();

            int count;
            SQLiteCommand selectCommand = new SQLiteCommand("select count(id_album) from albums;", Connection);

            try
            {
                count = Convert.ToInt32(selectCommand.ExecuteScalar());
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                throw new Exception(e.Message);
            }
            finally
            {
                Disonnect();
            }

            return count;
        }

        public static List<Album> GetAlbumList()
        {
            Connect();

            List<Album> albumList = new List<Album>();
            SQLiteCommand selectCommand = new SQLiteCommand("select id_album, title, artist, year from albums;", Connection);

            try
            {
                SQLiteDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    albumList.Add(new Album(Convert.ToInt32(reader["id_album"]), Convert.ToString(reader["title"]), Convert.ToString(reader["artist"]), Convert.ToInt32(reader["year"])));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                throw new Exception(e.Message);
            }
            finally
            {
                Disonnect();
            }

            return albumList;
        }

        public static void UpdateAlbumsAvailable(int albumId, bool available)
        {
            Connect();

            SQLiteCommand updateCommand = new SQLiteCommand("update albums set available=@0 where id_album=@1;", Connection);

            SQLiteParameter parameter = new SQLiteParameter("@0", System.Data.DbType.Boolean);
            parameter.Value = available;
            updateCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@1", System.Data.DbType.Int32);
            parameter.Value = albumId;
            updateCommand.Parameters.Add(parameter);

            try
            {
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                throw new Exception(e.Message);
            }
            finally
            {
                Disonnect();
            }
        }

        public static void UpdateAlbumsFavourite(int albumId, bool favourite)
        {
            Connect();

            SQLiteCommand updateCommand = new SQLiteCommand("update albums set favourite=@0 where id_album=@1;", Connection);

            SQLiteParameter parameter = new SQLiteParameter("@0", System.Data.DbType.Boolean);
            parameter.Value = favourite;
            updateCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@1", System.Data.DbType.Int32);
            parameter.Value = albumId;
            updateCommand.Parameters.Add(parameter);

            try
            {
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                throw new Exception(e.Message);
            }
            finally
            {
                Disonnect();
            }
        }

        public static Album GetAlbumDetails(int albumId)
        {
            Connect();

            Album album = new Album();
            album.Id = albumId;
            SQLiteCommand selectCommand = new SQLiteCommand("select title, artist, year, favourite, available, image from albums where id_album=@0;", Connection);

            SQLiteParameter parameter = new SQLiteParameter("@0", System.Data.DbType.Int32);
            parameter.Value = albumId;
            selectCommand.Parameters.Add(parameter);

            try
            {
                SQLiteDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    album.Title = Convert.ToString(reader["title"]);
                    album.Artist = Convert.ToString(reader["artist"]);
                    album.Year = Convert.ToInt32(reader["year"]);
                    album.Favourite = Convert.ToBoolean(reader["favourite"]);
                    album.Available = Convert.ToBoolean(reader["available"]);
                    album.ImageByte = (System.Byte[])reader["image"];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                Disonnect();
                throw new Exception(e.Message);
            }



            album.SongList = new List<Song>();
            selectCommand = new SQLiteCommand("select number, title from songs where id_album=@0;", Connection);
            selectCommand.Parameters.Add(parameter);

            try
            {
                SQLiteDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    album.SongList.Add(new Song(Convert.ToInt32(reader["number"]), Convert.ToString(reader["title"])));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                throw new Exception(e.Message);
            }
            finally
            {
                Disonnect();
            }



            return album;
        }

        public static void SetAlbumDetails(Album album)
        {
            Connect();

            SQLiteCommand insertCommand = new SQLiteCommand("insert into albums(title, artist, year, image) values(@0,@1,@2,@3);", Connection);

            SQLiteParameter parameter = new SQLiteParameter("@0", System.Data.DbType.String);
            parameter.Value = album.Title;
            insertCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@1", System.Data.DbType.String);
            parameter.Value = album.Artist;
            insertCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@2", System.Data.DbType.Int32);
            parameter.Value = album.Year;
            insertCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@3", System.Data.DbType.Binary);
            parameter.Value = album.ImageByte;
            insertCommand.Parameters.Add(parameter);

            try
            {
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                Disonnect();
                throw new Exception(e.Message);
            }





            album.Id = (int)Connection.LastInsertRowId;

            foreach (var song in album.SongList)
            {
                insertCommand = new SQLiteCommand("insert into songs(id_album, number, title) values(@0,@1,@2);", Connection);

                parameter = new SQLiteParameter("@0", System.Data.DbType.Int32);
                parameter.Value = album.Id;
                insertCommand.Parameters.Add(parameter);

                parameter = new SQLiteParameter("@1", System.Data.DbType.Int32);
                parameter.Value = song.Number;
                insertCommand.Parameters.Add(parameter);

                parameter = new SQLiteParameter("@2", System.Data.DbType.String);
                parameter.Value = song.Title;
                insertCommand.Parameters.Add(parameter);

                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wystąpił błąd.");
                    throw new Exception(e.Message);
                }
            }

            Disonnect();
        }

        public static void UpdateAlbumDetails(Album album)
        {
            Connect();

            SQLiteCommand updateCommand = new SQLiteCommand("update albums set title=@0, artist=@1, year=@2, image=@3 where id_album=@4;", Connection);

            SQLiteParameter parameter = new SQLiteParameter("@0", System.Data.DbType.String);
            parameter.Value = album.Title;
            updateCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@1", System.Data.DbType.String);
            parameter.Value = album.Artist;
            updateCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@2", System.Data.DbType.Int32);
            parameter.Value = album.Year;
            updateCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@3", System.Data.DbType.Binary);
            parameter.Value = album.ImageByte;
            updateCommand.Parameters.Add(parameter);

            parameter = new SQLiteParameter("@4", System.Data.DbType.Int32);
            parameter.Value = album.Id;
            updateCommand.Parameters.Add(parameter);

            try
            {
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                Disonnect();
                throw new Exception(e.Message);
            }





            SQLiteCommand deleteCommand = new SQLiteCommand("delete from songs where id_album=@0;", Connection);

            parameter = new SQLiteParameter("@0", System.Data.DbType.Int32);
            parameter.Value = album.Id;
            deleteCommand.Parameters.Add(parameter);

            try
            {
                deleteCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                Disonnect();
                throw new Exception(e.Message);
            }





            foreach (var song in album.SongList)
            {
                SQLiteCommand insertCommand = new SQLiteCommand("insert into songs(id_album, number, title) values(@0,@1,@2);", Connection);

                parameter = new SQLiteParameter("@0", System.Data.DbType.Int32);
                parameter.Value = album.Id;
                insertCommand.Parameters.Add(parameter);

                parameter = new SQLiteParameter("@1", System.Data.DbType.Int32);
                parameter.Value = song.Number;
                insertCommand.Parameters.Add(parameter);

                parameter = new SQLiteParameter("@2", System.Data.DbType.String);
                parameter.Value = song.Title;
                insertCommand.Parameters.Add(parameter);

                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wystąpił błąd.");
                    throw new Exception(e.Message);
                }
            }

            Disonnect();
        }

        public static void DeleteAlbum(int albumId)
        {
            Connect();

            SQLiteCommand deleteCommand = new SQLiteCommand("delete from songs where id_album=@0;", Connection);

            SQLiteParameter parameter = new SQLiteParameter("@0", System.Data.DbType.Int32);
            parameter.Value = albumId;
            deleteCommand.Parameters.Add(parameter);

            try
            {
                deleteCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                Disonnect();
                throw new Exception(e.Message);
            }





            deleteCommand = new SQLiteCommand("delete from albums where id_album=@0;", Connection);

            deleteCommand.Parameters.Add(parameter);

            try
            {
                deleteCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Wystąpił błąd.");
                throw new Exception(e.Message);
            }
            finally
            {
                Disonnect();
            }
        }
    }
}
