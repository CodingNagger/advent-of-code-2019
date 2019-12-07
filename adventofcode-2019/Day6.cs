using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    class Day6 : Day
    {
        class Node
        {
            public string Identifier { get; set; }

            public List<Node> Children { get; set; }

            public static Node CreateNode(string identifier)
            {
                return new Node { Identifier = identifier, Children = new List<Node>() };
            }
        }

        private Node root;

        private int OrbitsCount;

        public override string Compute(string[] input)
        {
            ParseOrbits(input);
            OrbitsCount = CountDirectOrbits(root)+CountIndirectOrbits(root, 0);
            Console.WriteLine($"Common ancester: {FindCommonAncester(root, "SAN", "YOU", null).Identifier}" );
            Console.WriteLine($"Distance: {DistanceBetweenNodes("SAN", "YOU")}");
            return $"{OrbitsCount}";
        }

        void ParseOrbits(string[] input)
        {
            var laterAttempts = new List<KeyValuePair<string, string>>();

            root = Node.CreateNode("COM");

            foreach (var line in input)
            {
                var info = line.Split(")");
                if (FindNode(root, info[0]) != null) {
                    AddChild(info[0], info[1]);
                }
                else {
                    laterAttempts.Add(new KeyValuePair<string, string>(info[0], info[1]));
                }
            }

            while (laterAttempts.Count > 0) {
                var successfulAttempts = new List<KeyValuePair<string, string>>();

                for (var i = 0; i < laterAttempts.Count; i++) {
                    if (FindNode(root, laterAttempts[i].Key) != null && AddChild(laterAttempts[i].Key, laterAttempts[i].Value)) {
                        successfulAttempts.Add(laterAttempts[i]);
                    }
                }

                laterAttempts = laterAttempts.Except(successfulAttempts).ToList();
            }
        }

        bool AddChild(string parentIdentifier, string childIdentifier)
        {
            Node parent = FindNode(root, parentIdentifier);
            Node child = FindNode(root, childIdentifier);

            if (child == null)
            {
                child = Node.CreateNode(childIdentifier);
            }

            if (parent == null)
            {
                parent = Node.CreateNode(parentIdentifier);
            }

            if (root == null || (root?.Identifier?.Equals(childIdentifier) ?? false))
            {
                root = parent;
            }

            parent.Children.Add(child);

            var canFindParent = FindNode(root, parentIdentifier) != null;
            var canFindChild = FindNode(root, childIdentifier) != null;
            
            return canFindParent && canFindChild;
        }

        Node FindNode(Node start, string identifier) {
            if (start == null || string.IsNullOrWhiteSpace(identifier)) {
                return null;
            }

            if (start.Identifier.Equals(identifier)) {
                return start;
            }

            Node directChild = start.Children.Find(c => c.Identifier.Equals(identifier));

            if (directChild != null) {
                return directChild;
            }

            foreach (var child in start.Children) {
                var found = FindNode(child, identifier);
                if (found != null) {
                    return found;
                }
            }
            
            return null;
        }

        int CountDirectOrbits(Node node)
        {
            if (node == null) throw new ArgumentException("No source to count from");
            
            var count = node.Children.Count + node.Children.Select(c => CountDirectOrbits(c)).Sum();
            return count;
        }

        int CountIndirectOrbits(Node node, int depth) {
            if (node == null) throw new ArgumentException("No source to count from");
            int sum = 0;

            foreach (var child in node.Children) {
                sum += CountIndirectOrbits(child, depth + 1) + depth;
            }

            return sum;
        }

        Node FindCommonAncester(Node start, string source, string destination, Node previous) {
            if (FindNode(start, source) != null && FindNode(start, destination) != null) {
                return start.Children.Select(c => FindCommonAncester(c, source, destination, start)).FirstOrDefault();
            }
        
            return previous;
        }

        int Distance(Node start, string identifier) {
            Node cursor = start;
            int jumps = 0;
            while (FindNode(cursor, identifier) != null) {
                jumps++;
                cursor = cursor.Children.Where(c => FindNode(c, identifier) != null).FirstOrDefault();
            }

            return jumps-2;
        }

        int DistanceBetweenNodes(string start, string destination) {
            Node commonAncester = FindCommonAncester(root, start, destination, null);
            Console.WriteLine($"{Distance(commonAncester, start)} - {Distance(commonAncester, destination)}");
            return Distance(commonAncester, start) + Distance(commonAncester, destination);
        }
    }
}
