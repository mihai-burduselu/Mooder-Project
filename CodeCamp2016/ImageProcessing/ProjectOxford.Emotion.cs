using System.IO;
using System.Threading.Tasks;
using CodeCamp2016.Interfaces;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Collections.Generic;

namespace CodeCamp2016.ImageProcessing
{
    public partial class ProjectOxford : IImageProcessing
    {
        public static string[] emotions = new string[100];
        public int[] positions = new int[100];

        public async Task<string> RecognizeEmotion(string imagePath)
        {
            var result = string.Empty;
            Emotion[] emotionResult = null;

            return await Task.Run(() =>
            {
                if (!File.Exists(imagePath))
                    return null;

                using (var stream = File.Open(imagePath, FileMode.Open))
                    emotionResult = emotionClient.RecognizeAsync(stream).Result;

                if(emotionResult.Length < 1)
                {
                    return "You have no emotion";
                }

                return GetHighestEmotion(emotionResult[0].Scores);
            });
        }

        private string GetHighestEmotion(Scores scores)
        {
            var points = new float[]
            {
                scores.Anger,
                scores.Contempt,
                scores.Disgust,
                scores.Fear,
                scores.Happiness,
                scores.Neutral,
                scores.Sadness,
                scores.Surprise
            };

            int pos = GetEmotionPosition(points);

            return ConvertEmotionToString(pos);
        }

        private int GetEmotionPosition(float[] points)
        {
            float max = -100;
            float aux;
            float[] copy = new float[100];

            for (int i = 0; i < points.Length; i++)
                copy[i] = points[i];

            for(int i = 0; i < copy.Length; i++)
            {
                for(int j = 0; j < copy.Length; j++)
                    if( copy[i] > copy[j] )
                    {
                        aux = copy[i];
                        copy[i] = copy[j];
                        copy[j] = aux;
                    }
            }

            for(int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < points.Length; j++)
                    if (points[i] == copy[j])
                        positions[j] = i;
            }

            return 0;
        }

        private string ConvertEmotionToString(int pos)
        {
            var dictionary = new Dictionary<int, string>();

            dictionary.Add(0, "anger");
            dictionary.Add(1, "contempt");
            dictionary.Add(2, "disgust");
            dictionary.Add(3, "fear");
            dictionary.Add(4, "happiness");
            dictionary.Add(5, "neutral");
            dictionary.Add(6, "sadness");
            dictionary.Add(7, "surprise");

            for(int i = 0; i < 4; i++)
            {
                emotions[i] = dictionary[this.positions[i]];
            }


            return "Unknown";
        }
    }
}
