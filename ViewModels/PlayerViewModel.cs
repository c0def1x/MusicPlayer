using AudioBeta1._0.Models;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AudioBeta1._0.ViewModels
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private MediaPlayer player = new MediaPlayer();
        private void Player_MediaFailed(object sender, ExceptionEventArgs e)
        {
            MessageBox.Show("Ошибка во время открытия файла.");
        }

        public PlayerViewModel()
        {
            ChangePlayOrPause = new RelayCommand(obj =>
            {
                if (selectedTrack != null)
                {
                    var pos = player.Position;

                    if (PlayOrPauseIsCheck == true)
                    {
                        PlayOrPause = "Pause";
                        OnPropertyChanged(nameof(PlayOrPause));
                        player.Open(new Uri(selectedTrack.Path, UriKind.Relative));
                        player.Position = pos;
                        player.Play();
                    }
                    else
                    {
                        PlayOrPause = "Play";
                        OnPropertyChanged(nameof(PlayOrPause));
                        player.Pause();
                    }
                }
                else
                {
                    MessageBox.Show("Плейлист пуст!");
                }
            });
            NextTrack = new RelayCommand(obj =>
            {
                if(SelectedIndexTrack<Playlist.TracksPlayList.Count-1)
                {
                    player.Close();
                    SelectedIndexTrack++;
                    OnPropertyChanged(nameof(SelectedIndexTrack));
                    PlayOrPause = "Pause";
                    OnPropertyChanged(nameof(PlayOrPause));
                    selectedTrack = Playlist.TracksPlayList[SelectedIndexTrack];
                    OnPropertyChanged(nameof(selectedTrack));
                    player.Open(new Uri(selectedTrack.Path, UriKind.Relative));
                    player.Play();
                }
            });
            PreviousTrack = new RelayCommand(obj =>
            {
                if (SelectedIndexTrack > 0)
                {
                    player.Close();
                    SelectedIndexTrack--;
                    OnPropertyChanged(nameof(SelectedIndexTrack));
                    PlayOrPause = "Pause";
                    OnPropertyChanged(nameof(PlayOrPause));
                    selectedTrack = Playlist.TracksPlayList[SelectedIndexTrack];
                    OnPropertyChanged(nameof(selectedTrack));
                    player.Open(new Uri(selectedTrack.Path, UriKind.Relative));
                    player.Play();
                }
            });
            AddTrack = new RelayCommand(obj =>
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Audio file|*.mp3"
                };
                var result = dialog.ShowDialog();
                if (result != true)
                {
                    return;
                }
                string newFilename = Guid.NewGuid().ToString().Replace("-", "") + ".mp3";
                string pathToCopy = $"Audios/{newFilename}";
  
                try
                {
                    File.Copy(dialog.FileName, pathToCopy);
                    Playlist.Load(pathToCopy);
                }
                catch
                {
                    MessageBox.Show("Ошибка при копировании файла!");
                }
            });

            RemoveTrack = new RelayCommand(obj =>
            {
                if (selectedTrack != null)
                {
                    Playlist.TracksPlayList.Remove(selectedTrack);
                    OnPropertyChanged(nameof(selectedTrack));
                }
                else
                {
                    MessageBox.Show("Плейлист пуст!");
                }
            });
        }
        public RelayCommand RemoveTrack { get; set; }
        public RelayCommand AddTrack { get; set; }
        public RelayCommand NextTrack { get; }
        public RelayCommand PreviousTrack { get; }

        public int SelectedIndexTrack { get; set; }

        private Track selectedTrack;

        public Track SelectedTrack
        {
            get => selectedTrack;
            set 
            { 
                selectedTrack = value; 
                trackAuthor = selectedTrack.Author; 
                trackImage = selectedTrack.Photo;
                trackName = selectedTrack.Title;
                trackDuration = selectedTrack.Duration;

                OnPropertyChanged(nameof(trackAuthor));
                OnPropertyChanged(nameof(trackImage));
                OnPropertyChanged(nameof(trackName));
                OnPropertyChanged(nameof(trackDuration));
            }
        }

        public Playlist Playlist { get; set; } = new Playlist();

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string playOrPause = "Play";
        
        public string PlayOrPause
        {
            get => playOrPause;
            set 
            { 
                playOrPause = value;
                OnPropertyChanged(nameof(playOrPause));
            }
        }

        public RelayCommand ChangePlayOrPause { get; }

        private bool playOrPauseIsCheck;

        public bool PlayOrPauseIsCheck
        {
            get => playOrPauseIsCheck;
            set { playOrPauseIsCheck = value; }
        }

        private string trackName;

        public string TrackName
        {
            get => trackName;
            set { trackName = value; }
        }

        private string trackAuthor;

        public string TrackAuthor
        { 
            get => trackAuthor;
            set { trackAuthor = value; }
        }

        private BitmapImage trackImage;

        public BitmapImage TrackImage
        {
            get => trackImage;
            set { trackImage = value; }
        }

        private TimeSpan trackDuration;

        public TimeSpan TrackDuration
        {
            get { return trackDuration; }
            set { trackDuration = value; }
        }
    }
}
