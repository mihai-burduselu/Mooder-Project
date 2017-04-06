using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace CodeCamp2016.Player
{
    class TrackPlayer
    {
        String Path { get; set; }

        public MediaElement songPlayer = new MediaElement();

        public TrackPlayer(String path, string age)
        {
            this.Path = path;

            if( Int64.Parse(age) <= 30 )
            {
                Path = "20" + Path;
            }
            else
                if( Int64.Parse(age) <= 40 && Int64.Parse(age) > 30)
            {
                Path = "30" + Path;
            }
            else
                if( Int64.Parse(age) > 40)
            {
                Path = "40" + Path;
            }
        }

        public async Task PlayMediaElement()
        {
            await getSongPlayer();
            songPlayer.Play();
        }

        private async Task getSongPlayer()
        {
            var folder = ApplicationData.Current.LocalFolder;

            try
            {
                var file = await GetFile(folder, Path);
                var stream = await GetStream(file);

                songPlayer.SetSource(stream, "");
            }
            catch
            {

            }
        }

        private static async Task<Windows.Storage.Streams.IRandomAccessStream> GetStream(StorageFile file)
        {
            var fileAccesMode = FileAccessMode.Read;

            return await file.OpenAsync(fileAccesMode);
        }

        private static Windows.Foundation.IAsyncOperation<StorageFile> GetFile(StorageFolder folder, string title)
        {
            return folder.
                GetFileAsync(title);
        }

        private static async Task<StorageFolder> GetFolder()
        {
            var package = Package.Current.InstalledLocation.GetFolderAsync("Assets");

            return await package;
        }

    }
}
