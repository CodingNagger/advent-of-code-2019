using System;
using Xunit;
using AdventOfCode2019;
using Moq;
using System.Linq;

namespace AdventOfCode2019.Tests
{
    public class RobotTests
    {
        [Fact]
        public void TestRun()  {
            var robot = new Robot(new Mock<IIntCodeComputer>().Object, PanelColor.Black);
            robot.HandleOutput(1);
            robot.HandleOutput(0);
            robot.HandleOutput(0);
            robot.HandleOutput(0);
            robot.HandleOutput(1);
            robot.HandleOutput(0);
            robot.HandleOutput(1);
            robot.HandleOutput(0);
            robot.HandleOutput(0);
            robot.HandleOutput(1);
            robot.HandleOutput(1);
            robot.HandleOutput(0);
            robot.HandleOutput(1);
            robot.HandleOutput(0);
            Assert.Equal(6, robot.UniquePanelsPaintedCount);
        }
    }
}
