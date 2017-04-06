using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCamp2016.Colors
{
    class ColorProcesser
    {
        public Dictionary<string, string> colorsEmotions;
        public static ColorProcesser Instance = new ColorProcesser();

        private ColorProcesser()
        {
            colorsEmotions.Add("anger", "3");//blue
            colorsEmotions.Add("contempt", "1");//yellow
            colorsEmotions.Add("disgust", "3");//blue
            colorsEmotions.Add("fear", "0");//green
            colorsEmotions.Add("happiness", "1");//yellow
            colorsEmotions.Add("neutral", "0");//green
            colorsEmotions.Add("sadness", "2");//orange
            colorsEmotions.Add("surprise", "1");//yellow
        }
    }
}
