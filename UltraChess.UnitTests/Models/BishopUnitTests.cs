using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Models
{
    [TestClass]
    public class BishopUnitTests
    {
        [TestMethod]
        public void GetSlidingMovesTest()
        {
            // Arrange
            var sut = new Bishop();

            // Act
            var moves = sut.GetSquaresToMoveTo(0);
            //var moves = new List<int> { 9, 18, 27, 36, 45, 54, 63 };

            // Assert
            moves.ShouldBe(new List<int> { 9, 18, 27, 36, 45, 54, 63 });
        }
    }
}
