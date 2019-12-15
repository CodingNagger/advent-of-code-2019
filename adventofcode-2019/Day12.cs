using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Day12 : TwoPartDay
    {
        const char Asteroid = '#';

        public string Compute(string[] input)
        {
            var steps = 1000;
            var moons = input.Select(Moon.Parse).ToArray();

            for (var step = 0; step < steps; step++)
            {
                moons = ApplyVelocity(ApplyGravity(moons));
            }

            var totalEnergy = moons.Sum(m => m.TotalEnergy);
            return $"{totalEnergy}";
        }

        public string ComputePartTwo(string[] input)
        {

            long[] steps = new long[] { 1, 1, 1 };
            var originalMoons = input.Select(Moon.Parse).ToArray();
            var moons = ApplyVelocity(ApplyGravity(originalMoons.Select(m => m.Copy).ToArray()));

            while ((moons[0].Position.X != originalMoons[0].Position.X || 
                moons[1].Position.X != originalMoons[1].Position.X ||
                moons[2].Position.X != originalMoons[2].Position.X  || 
                moons[3].Position.X != originalMoons[3].Position.X)
                ||
                (moons[0].Velocity.X != originalMoons[0].Velocity.X || 
                moons[1].Velocity.X != originalMoons[1].Velocity.X ||
                moons[2].Velocity.X != originalMoons[2].Velocity.X  || 
                moons[3].Velocity.X != originalMoons[3].Velocity.X))
            {
                moons = ApplyVelocity(ApplyGravity(moons));
                steps[0]++;
            }

            moons = ApplyVelocity(ApplyGravity(originalMoons.Select(m => m.Copy).ToArray()));

            while ((moons[0].Position.Y != originalMoons[0].Position.Y || 
                moons[1].Position.Y != originalMoons[1].Position.Y ||
                moons[2].Position.Y != originalMoons[2].Position.Y  || 
                moons[3].Position.Y != originalMoons[3].Position.Y)
                ||
                (moons[0].Velocity.Y != originalMoons[0].Velocity.Y || 
                moons[1].Velocity.Y != originalMoons[1].Velocity.Y ||
                moons[2].Velocity.Y != originalMoons[2].Velocity.Y  || 
                moons[3].Velocity.Y != originalMoons[3].Velocity.Y))
            {
                moons = ApplyVelocity(ApplyGravity(moons));
                steps[1]++;
            }

            moons = ApplyVelocity(ApplyGravity(originalMoons.Select(m => m.Copy).ToArray()));

           while ((moons[0].Position.Z != originalMoons[0].Position.Z || 
                moons[1].Position.Z != originalMoons[1].Position.Z ||
                moons[2].Position.Z != originalMoons[2].Position.Z  || 
                moons[3].Position.Z != originalMoons[3].Position.Z)
                ||
                (moons[0].Velocity.Z != originalMoons[0].Velocity.Z || 
                moons[1].Velocity.Z != originalMoons[1].Velocity.Z ||
                moons[2].Velocity.Z != originalMoons[2].Velocity.Z  || 
                moons[3].Velocity.Z != originalMoons[3].Velocity.Z))
            {
                moons = ApplyVelocity(ApplyGravity(moons));
                steps[2]++;
            }

            return $"{MathUtils.LCM(steps[0], MathUtils.LCM(steps[1], steps[2]))}";
        }

        public static Moon[] ApplyGravity(Moon[] moons)
        {
            for (var i = 0; i < moons.Length; i++)
            {
                for (var j = 0; j < moons.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (moons[i].Position.X > moons[j].Position.X)
                    {
                        moons[i].Velocity.X--;
                    }
                    else if (moons[i].Position.X < moons[j].Position.X)
                    {
                        moons[i].Velocity.X++;
                    }

                    if (moons[i].Position.Y > moons[j].Position.Y)
                    {
                        moons[i].Velocity.Y--;
                    }
                    else if (moons[i].Position.Y < moons[j].Position.Y)
                    {
                        moons[i].Velocity.Y++;
                    }

                    if (moons[i].Position.Z > moons[j].Position.Z)
                    {
                        moons[i].Velocity.Z--;
                    }
                    else if (moons[i].Position.Z < moons[j].Position.Z)
                    {
                        moons[i].Velocity.Z++;
                    }
                }
            }

            return moons;
        }

        public static Moon[] ApplyVelocity(Moon[] moons)
        {
            return moons.Select(m => m.ApplyVelocity()).ToArray();
        }
    }

    public class Moon
    {
        public SpaceCoordinates Position { get; set; }
        public SpaceCoordinates Velocity { get; set; }

        public Moon Copy => new Moon { Position = Position.Copy, Velocity = Velocity.Copy };

        public int PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

        public int KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

        public int TotalEnergy => PotentialEnergy * KineticEnergy;

        public static Moon Parse(string moonDetails)
        {
            var coordinates = moonDetails.Substring(1, moonDetails.Length - 2).Split(',').Select(s => int.Parse(s.Trim().Split('=')[1])).ToArray();
            return new Moon
            {
                Position = new SpaceCoordinates { X = coordinates[0], Y = coordinates[1], Z = coordinates[2] },
                Velocity = new SpaceCoordinates(),
            };
        }

        public Moon ApplyVelocity()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
            Position.Z += Velocity.Z;

            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (obj.GetHashCode() != GetHashCode())
            {
                return false;
            }

            var other = obj as Moon;

            return Position.Equals(other.Position) && Velocity.Equals(other.Velocity);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode() + 7 * Velocity.GetHashCode();
        }
    }

    public class SpaceCoordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public SpaceCoordinates Copy => new SpaceCoordinates { X = X, Y = Y, Z = Z };

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as SpaceCoordinates;

            return X == other.X && Y == other.Y && Z == other.Z;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return X + 7 * Y + 13 * Z;
        }
    }
}
