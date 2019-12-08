using System;

namespace AdventOfCode2019
{
    public class DSNDecoder
    {
        public string Decode(string[] input, int width, int height)
        {
            var layersData = DSNImageParser.Parse(input, width, height);
            var image = layersData[0].ToCharArray();

            for (var l = 1; l < layersData.Length; l++)
            {
                for (var i = 0; i < layersData[l].Length; i++)
                {
                    if (image[i] == '2') image[i] = layersData[l][i];
                }
            }
            
            return new string(image);
        }
    }
}
