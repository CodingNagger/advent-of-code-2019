namespace adventofcode_2019
{
    class Day1 : Day
    {
        public override string Compute(string[] input) {
            int sum = 0;

            foreach (string moduleWeight in input) {
                sum += ComputeFuelForModuleWeight(moduleWeight);
            }

            return $"{sum}";
        }

        private int ComputeFuelForModuleWeight(string moduleWeight) {
            int totalFuel = (int.Parse(moduleWeight) / 3)-2;
            int additionalFuel = (totalFuel / 3)-2;
            
            while (additionalFuel > 0) {
                totalFuel += additionalFuel;
                additionalFuel = (additionalFuel / 3)-2;
            }

            return totalFuel;
        }
    }
}
