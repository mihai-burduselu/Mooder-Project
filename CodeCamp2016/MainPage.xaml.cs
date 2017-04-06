using System;
using CodeCamp2016.ImageProcessing;
using CodeCamp2016.ImageSource;
using CodeCamp2016.Interfaces;
using CodeCamp2016.Storage;
using Windows.Media.Capture;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Collections.Generic;
using CodeCamp2016.Player;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using CodeCamp2016.Colors;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CodeCamp2016
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private SpeechSynthesizer textToSpeech;
        private MediaCapture mediaDevice;
        private readonly string PHOTO_FILE_NAME = "photo.jpg";

        private IImageProcessing imageProcessing;
        private IPhotoStorage localPhotoStorage;
        private IImageSource localCameraImageSource;

        string yourAge;

        private TrackPlayer player;

        public MainPage()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private async void InitializeApplication()
        {
            //textToSpeech = new SpeechSynthesizer();
            mediaDevice = new MediaCapture();

            imageProcessing = new ProjectOxford();
            localPhotoStorage = new LocalPhotoStorage(mediaDevice);
            localCameraImageSource = new LocalCameraImageSource(localPhotoStorage, mediaDevice);

            await localCameraImageSource.InitializeDevice();

            cameraElement.Source = mediaDevice;

            await mediaDevice.StartPreviewAsync();
        }

        private async void WhatsMyMoodButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (player != null)
                player.songPlayer.Stop();

            await localPhotoStorage.Save(PHOTO_FILE_NAME);
            var result = await imageProcessing.RecognizeEmotion(localPhotoStorage.GetLastPhotoSaved());
            List<string> listOfResults = new List<string>();

            try
            {
                await age();
            }
            catch(Exception exp)
            {
                yourAge = "20";
            }

            listOfResults.Add("Age: " + yourAge);

            for (int i = 0; i < 2; i++)
                listOfResults.Add(ProjectOxford.emotions[i]);

            emotionList.ItemsSource = listOfResults;

            string currentColor;

            Random rnd = new Random();

            int randomNum = (rnd.Next(1, 20)) % 2 + 1;

            player = new TrackPlayer(ProjectOxford.emotions[0] + randomNum.ToString() + ".mp3", yourAge);
            await player.PlayMediaElement();

        }

        private void ResetButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            mediaElementUI.Stop();

            if (player != null)
                player.songPlayer.Stop();
        }

        private async Task age()
        {
            await localPhotoStorage.Save(PHOTO_FILE_NAME);

            yourAge = await imageProcessing.GetDominantForegroundColor(localPhotoStorage.GetLastPhotoSaved());
        }

    }
}
