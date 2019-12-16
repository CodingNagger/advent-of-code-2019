using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2019
{
    public class Day14 : TwoPartDay
    {
        public string Compute(string[] input)
        {
            var reactions = input.Select(i => Reaction.Parse(surplus, i));
            var needs = reactions.First(r => r.OutputName.Equals("FUEL")).Input.ToList();

            while (needs.)
        }

        private List<Chemical> Merge(List<Chemical> inputs)
        {
            var map = new Dictionary<string, int>();

            foreach (var i in inputs)
            {
                if (map.ContainsKey(i.Name))
                {
                    map[i.Name] += i.Quantity;
                }
                else
                {
                    map.Add(i.Name, i.Quantity);
                }
            }

            return map.Select(m => new Chemical { Name = m.Key, Quantity = m.Value }).ToList();
        }

        public string ComputePartTwo(string[] input)
        {
            return "nope";
        }
    }

    public class ChemicalsStore
    {
        private Dictionary<string, int> chemicals = new Dictionary<string, int>();

        public void Deposit(string name, int value)
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

        public int Withdraw(string name, int value)
        {
            if (chemicals.ContainsKey(name))
            {
                var available = chemicals[name];

                if (available >= value) {
                    chemicals[name] -= value;
                    return value;
                }
                else if (available < value) {
                    chemicals[name] -= available;
                    chemicals.Remove(name);
                    return available;
                }
            }

            return 0;
        }
    }

    public class Reaction
    {
        public string OutputName => output.Name;
        public Chemical[] Input => input;
        public ChemicalsStore surplus;
        private Chemical[] input;
        private Chemical output;

        public Reaction SoullessCopy => new Reaction(new ChemicalsStore(), input, output);

        public Reaction(Chemical[] input, Chemical output)
        {
            this.surplus = surplus;
            this.input = input;
            this.output = output;
        }

        public bool CanReverseReact(Chemical chemical)
        {
            return output.Name.Equals(chemical.Name);
        }

        public Chemical[] ReverseReact(Chemical chemical)
        {
            if (CanReverseReact(chemical))
            {
                var result = new List<Chemical>();
                foreach (var i in input) {
                    result.Add( new Chemical { Name = i.Name, Quantity = ConvertQuantity(chemical, i) } );
                }
                Console.WriteLine($"Reverse reaction from {chemical} to {string.Join("; ", result)}");
                return result.ToArray();
            }

            return new Chemical[0];
        }

        private int ConvertQuantity(Chemical chemical, Chemical i)
        {
            var wishedQuantity = (int) Math.Ceiling((double) chemical.Quantity *  i.Quantity/(double)output.Quantity);
            var producedQuantity = 0;

            while (producedQuantity < wishedQuantity)
            {
                producedQuantity += i.Quantity;
            }

            return producedQuantity;
        }

        public static Reaction Parse(ChemicalsStore surplus, string data)
        {
            var membersData = data.Split("=>");
            return new Reaction(surplus, membersData[0].Split(',').Select(m => Chemical.Parse(m)).ToArray(), Chemical.Parse(membersData[1]));
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
        public int Quantity { get; set; }

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
    }
}
