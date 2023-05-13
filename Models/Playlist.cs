using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace AudioBeta1._0.Models
{
    public class Playlist
    {
        public ObservableCollection<Track> TracksPlayList { get; set; } = new ObservableCollection<Track>();

        public void Load(string filename)
        {
            var mpFile = TagLib.File.Create(filename);
            var bv = mpFile.Tag.Pictures[0].Data.Data;
            TimeSpan duration = mpFile.Properties.Duration;
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bv);
            image.EndInit();
            TracksPlayList.Add(new Track 
            { 
                Author = mpFile.Tag.Artists.First(), 
                Title = mpFile.Tag.Title, Path = filename, 
                Photo=image, Duration = duration
            });
        }
    }
}
