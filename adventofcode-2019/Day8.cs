using System;
using System.Text;

namespace AdventOfCode2019
{
    public class Day8 : Day
    {

        public override string Compute(string[] input)
        {
            var width = 25;
            var height = 6;
            var image = new DSNDecoder().Decode(input, 25, 6);
            StringBuilder builder = new StringBuilder();

            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    if (image[j * width + i] == '1')
                    {
                        builder.Append("█");

                    }
                    else builder.Append(" ");
                }

                builder.Append('\n');
            }
            return $"{builder.ToString()}";
        }
    }
}