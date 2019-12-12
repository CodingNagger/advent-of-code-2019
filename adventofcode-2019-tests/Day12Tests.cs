using System;
using Xunit;

namespace AdventOfCode2019.Tests
{
    public class Day12Tests
    {
        [Theory]
        [InlineData(14, 9, 14)]
        [InlineData(6, 14, -4)]
        [InlineData(4, 4, -3)]
        public void TestMoonParse(int x, int y, int z) {
            var moonData = $"<x={x}, y={y}, z={z}>";
            var moon = Moon.Parse(moonData);
            Assert.Equal(x, moon.Position.X);
            Assert.Equal(y, moon.Position.Y);
            Assert.Equal(z, moon.Position.Z);
            Assert.Equal(0, moon.Velocity.X);
            Assert.Equal(0, moon.Velocity.Y);
            Assert.Equal(0, moon.Velocity.Z);
        }

        [Fact]
        public void TestApplyVelocity() {
            var moon = new Moon { 
                Position = new SpaceCoordinates { X = -1, Y = 0, Z = 2 },
                Velocity = new SpaceCoordinates { X = 3, Y = -1, Z = -1 },
            };

            var postVelocityMoon = moon.Copy.ApplyVelocity();

            Assert.NotEqual(moon.Position.X, postVelocityMoon.Position.X);
            Assert.NotEqual(moon.Position.Y, postVelocityMoon.Position.Y);
            Assert.NotEqual(moon.Position.Z, postVelocityMoon.Position.Z);

            Assert.Equal(2, postVelocityMoon.Position.X);
            Assert.Equal(-1, postVelocityMoon.Position.Y);
            Assert.Equal(1, postVelocityMoon.Position.Z);
        }

        [Fact]
        public void TestApplyGravity()
        {
            var moons = Day12.ApplyGravity(new Moon[] {
                Moon.Parse("<x=-1, y=  0, z= 2>"),
                Moon.Parse("<x= 2, y=-10, z=-7>"),
                Moon.Parse("<x= 4, y= -8, z= 8>"),
                Moon.Parse("<x= 3, y=  5, z=-1>"),
            });

            Assert.Equal(3, moons[0].Velocity.X);
            Assert.Equal(-1, moons[0].Velocity.Y);
            Assert.Equal(-1, moons[0].Velocity.Z);

            Assert.Equal(1, moons[1].Velocity.X);
            Assert.Equal(3, moons[1].Velocity.Y);
            Assert.Equal(3, moons[1].Velocity.Z);

            Assert.Equal(-3, moons[2].Velocity.X);
            Assert.Equal(1, moons[2].Velocity.Y);
            Assert.Equal(-3, moons[2].Velocity.Z);

            Assert.Equal(-1, moons[3].Velocity.X);
            Assert.Equal(-3, moons[3].Velocity.Y);
            Assert.Equal(1, moons[3].Velocity.Z);
        }
    }
}
