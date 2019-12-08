using System.Linq;

namespace AdventOfCode2019
{
    public class DSNChecksumCalculator
    {
        public int Compute(string[] input, int width, int height) {
            var layersData = DSNImageParser.Parse(input, width, height);
            var layerData = FindLowerCountOf('0', layersData);

            return CountOccurences('1', layerData)*CountOccurences('2', layerData);
        }

        private string FindLowerCountOf(char c, string[] layersData)
        {
            layersData = layersData.Where(l => l.Contains(c)).ToArray();
            var cCount = CountOccurences(c, layersData[0]);
            var layerData = layersData[0];

            foreach (var l in layersData) {
                var ccc = CountOccurences(c, l);
                if (ccc < cCount) {
                    layerData = l;
                    cCount = ccc;
                }
            }

            return layerData;
        }

        private int CountOccurences(char c, string l)
        {
            return l.Count(cc => cc == c);
        }
    }
}
