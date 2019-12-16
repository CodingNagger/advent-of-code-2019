using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2019
{
    

    class Day16 : TwoPartDay
    {
        Stopwatch stopWatch;
        int[] basePattern = new int[] { 0, 1, 0, -1 } ;

        public string Compute(string[] input) {
            var inputList = input[0];
            var offset = 0;
            var outputList = CalculateOutput(inputList);
            var phase = 1;
            
            while (phase < 100) {
                phase++;
                outputList = CalculateOutput(outputList);
            }
            return string.Join(string.Empty, outputList).Substring(offset, 8);
        }

        public string ComputePartTwo(string[] input)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var inputList = RepeatInput(input[0], 10000);
            var offset = CalculateOffset(inputList);

            inputList = inputList.Substring(offset);

            // return $"{offset} - length: {inputList.Length} - generatePattern substring: {string.Join("", GeneratePattern(inputList.Length, offset)).Substring(offset-8, 48)}";
            
            Console.WriteLine($"{DisplayUtils.DisplayValue(stopWatch)} - BeforeOutput");
            var outputList = CalculateOutputPartTwo(inputList);
            Console.WriteLine($"{DisplayUtils.DisplayValue(stopWatch)} - Generated output");
            var phase = 1;
            
            Console.WriteLine($"{DisplayUtils.DisplayValue(stopWatch)} - Calculated offset");
            // Console.WriteLine($"{DisplayUtils.DisplayValue(stopWatch)} - AppliedPhase {phase} - {inputList}");

            while (phase < 100) {
                // Console.WriteLine($"{DisplayUtils.DisplayValue(stopWatch)} - Applied phase {phase} - {outputList.Substring(outputList.Length-20), 20}");
                phase++;
                outputList = CalculateOutputPartTwo(outputList);
            }
            Console.WriteLine($"{DisplayUtils.DisplayValue(stopWatch)} - Applied phase {phase}");

            return outputList.Substring(0, 8);
        }

        public string RepeatInput(string input, int repeatCount) {
            return new StringBuilder(input.Length * repeatCount).Insert(0, input, repeatCount).ToString();
        } 

        public int CalculateOffset(string input) {
            return int.Parse(input.Substring(0, 7));
        }

        public int[] InputListToIntArray(string inputList) {
            return inputList.ToCharArray().Select(c => int.Parse($"{c}")).ToArray();
        }

        public string CalculateOutput(string inputList) {
            var numericalInput = InputListToIntArray(inputList);
            var outputList = new int[numericalInput.Length];

            for (var i = 0; i < numericalInput.Length; i++) {
                var generatedPattern = GeneratePattern(inputList.Length, i+1);
                
                var currentOutputCharSum = 0;

                for (var j = i; j < numericalInput.Length; j++) {
                    currentOutputCharSum += numericalInput[j] * generatedPattern[j];
                }

                outputList[i] = Math.Abs(currentOutputCharSum) % 10;
            }

            return string.Join("", outputList);
        }

        public string CalculateOutputPartTwo(string inputList) {
            var processableInput = InputListToIntArray(inputList).Reverse().ToArray();
            var outputList = new int[processableInput.Length];
            
            outputList[0] = processableInput[0] % 10;

            for (var i = 1; i < outputList.Length; i++) {
                outputList[i] = (outputList[i-1] + processableInput[i]) % 10;
            }

            return string.Join("",outputList.Reverse());
        }
        public int[] GeneratePattern(int size, int index) {
            var pattern = new int[size+1];
            var cursor = 0;

            for (var i = 0; i < size; i++) {
                for (var j = 0; j < index && cursor < pattern.Length; j++) {
                    pattern[cursor] = basePattern[i % basePattern.Length];
                    cursor++;
                }
            }

            return pattern.ToList().GetRange(1, size).ToArray();
        }

        
    }
}
