using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2019
{
    public class Day14 : TwoPartDay
    {
        List<Reaction> reactions;
        ChemicalsStore surplus;

        public string Compute(string[] input)
        {
            surplus = new ChemicalsStore();
            reactions = input.Select(i => Reaction.Parse(i)).ToList();
            return $"{CalculateOreNeeded(new Chemical { Name = "FUEL", Quantity = 1 })}";
        }

        public long CalculateOreNeeded(Chemical product)
        {
            var reaction = reactions.First(r => r.CanReverseReact(product));
            var availableProduct = surplus.Withdraw(product.Name, product.Quantity);
            var productNeeded = product.Quantity - availableProduct;
            var wishFactor = (long)Math.Ceiling((double)Math.Max(productNeeded, 0) / (double)reaction.OutputQuantity);
            var bonusProduct = (long)(reaction.OutputQuantity * wishFactor - productNeeded);

            if (!product.IsOre())
            {
                surplus.Deposit(product.Name, bonusProduct);
            }

            var requiredOre = 0L;

            foreach (var reactant in reaction.Input)
            {
                requiredOre += reactant.IsOre() ? wishFactor * reactant.Quantity : CalculateOreNeeded(reactant * wishFactor);
            }

            return requiredOre;
        }

        public string ComputePartTwo(string[] input)
        {
            surplus = new ChemicalsStore();
            reactions = input.Select(i => Reaction.Parse(i)).ToList();

            var fuel = new Chemical { Name = "FUEL", Quantity = 1 };
            var oreGoal = 1000000000000;
            var orePerFuel = CalculateOreNeeded(fuel);
            var fuelGuess = oreGoal / orePerFuel;
            var mostValidGuess = fuelGuess;
            var fuelGuessFactor = fuelGuess / 2;
            long result;

            while ((result = CalculateOreNeeded(fuel * fuelGuess)) < oreGoal)
            {
                mostValidGuess = fuelGuess;
                if (result == oreGoal)
                {
                    return $"{result}";
                }

                Console.WriteLine($"Failed with guess {fuelGuess} - {fuelGuessFactor} and result {result}/{oreGoal}");

                while (CalculateOreNeeded(fuel * (fuelGuessFactor + fuelGuess)) > oreGoal) {
                    fuelGuessFactor /= 2;
                }

                if (fuelGuessFactor == 0) fuelGuessFactor = 1;

                fuelGuess += fuelGuessFactor;
            }
            return $"{mostValidGuess}";
        }
    }

    public class ChemicalsStore
    {
        private Dictionary<string, long> chemicals = new Dictionary<string, long>();

        public void Deposit(string name, long value)
        {
            if (value > 0)
            {
                if (chemicals.ContainsKey(name))
                {
                    chemicals[name] += value;
                }
                else
                {
                    chemicals.Add(name, value);
                }
            }
        }

        public long Withdraw(string name, long value)
        {
            if (chemicals.ContainsKey(name))
            {
                var available = chemicals[name];

                if (available >= value)
                {
                    chemicals[name] -= value;
                    return value;
                }
                else if (available < value)
                {
                    chemicals[name] -= available;
                    chemicals.Remove(name);
                    return available;
                }
            }

            return 0;
        }

        public long Available(string name) => chemicals.ContainsKey(name) ? chemicals[name] : 0;
    }

    public class Reaction
    {
        public string OutputName => output.Name;
        public long OutputQuantity => output.Quantity;
        public Chemical[] Input => input;
        private Chemical[] input;
        private Chemical output;
        public Reaction(Chemical[] input, Chemical output)
        {
            this.input = input;
            this.output = output;
        }

        public bool CanReverseReact(Chemical chemical)
        {
            return output.Name.Equals(chemical.Name);
        }

        public static Reaction Parse(string data)
        {
            var membersData = data.Split("=>");
            return new Reaction(membersData[0].Split(',').Select(m => Chemical.Parse(m)).ToArray(), Chemical.Parse(membersData[1]));
        }

        public override string ToString()
        {
            return $"{string.Join(',', input.Select(i => i.ToString()))} => {output.ToString()}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Reaction;
            return input.Except(other.input).Count() == 0 && output.Equals(other.output);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return input.GetHashCode() + 13 * output.GetHashCode();
        }
    }

    public class Chemical
    {
        public string Name { get; set; }
        public long Quantity { get; set; }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Chemical;
            return (Name?.Equals(other.Name) ?? false) && Quantity == other.Quantity;
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0 + 13 * Quantity.GetHashCode();
        }

        public static Chemical Parse(string data)
        {
            var parts = data.Trim().Split(' ');
            return new Chemical { Name = parts[1], Quantity = int.Parse(parts[0]) };
        }

        public override string ToString()
        {
            return $"{Quantity} {Name}";
        }

        public bool IsOre() => "ORE".Equals(Name);

        public static Chemical operator *(Chemical chemical, long factor)
            => new Chemical { Name = chemical.Name, Quantity = chemical.Quantity * factor };
    }
}
