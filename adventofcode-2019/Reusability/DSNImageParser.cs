using System.Linq;

namespace AdventOfCode2019
{
    public static class DSNImageParser
    {
        public static string[] Parse(string[] input, int width, int height) {
            var rawData = input[0];
            var layerSize = width * height;
            return Enumerable.Range(0, rawData.Length / layerSize).Select(i => rawData.Substring(i * layerSize, layerSize)).ToArray();
        }
    }
}
