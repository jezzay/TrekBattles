#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using NUnit.Framework;
using TrekBattles.Model;
using TrekBattles.util;

namespace TrekBattles.test.util
{
    [TestFixture]
    public class ArrayUtilTest
    {
        private SpaceShip[] CreateSpaceShips(int amount)
        {
            SpaceShip[] spaceShips = new SpaceShip[amount];
            for (int i = 0; i < amount; i++)
            {
                spaceShips[i] = CreateSpaceShip("Test" + i);
            }
            return spaceShips;
        }

        private SpaceShip CreateSpaceShip(string name)
        {
            return new SpaceShip(name, 10, 15, 3, 5, 6, new Random(3));
        }

        [Test]
        public void TestAddNumberToArray()
        {
            int[] numbers = {1, 3, 6, 4, 2};
            numbers = ArrayUtils.AddNumber(numbers, 5);
            Assert.That(numbers.Length, Is.EqualTo(6));
            Assert.That(numbers[5], Is.EqualTo(5));
            Assert.That(ArrayUtils.ArrayContainsNumber(numbers, 5));
        }

        [Test]
        public void TestAddSpaceShipToArray()
        {
            SpaceShip[] spaceShips = CreateSpaceShips(5);
            spaceShips = ArrayUtils.AddToArray(spaceShips, CreateSpaceShip("NewSpaceShip"));
            Assert.That(spaceShips.Length, Is.EqualTo(6));
            Assert.That(spaceShips[5].ShipClassName, Is.EqualTo("NewSpaceShip"));
        }

        [Test]
        public void TestContainsNumberInArray()
        {
            int[] numbers = {1, 3, 6, 4};

            Assert.That(ArrayUtils.ArrayContainsNumber(numbers, 6));
            Assert.That(!ArrayUtils.ArrayContainsNumber(numbers, 2));
            Assert.That(ArrayUtils.ArrayContainsNumber(numbers, 4));
        }

        [Test]
        public void TestHasArrayRange()
        {
            string[] strings = new string[] {"one", "two", "three", "four"};

            Assert.True(ArrayUtils.HasValidElementsInArrayRange(strings, 0, 3));

            Assert.True(ArrayUtils.HasValidElementsInArrayRange(strings, 0, 7));
            strings = new string[] {null, null, null, "hello"};
            Assert.False(ArrayUtils.HasValidElementsInArrayRange(strings, 0, 2));
            Assert.True(ArrayUtils.HasValidElementsInArrayRange(strings, 2, 3));
        }

        [Test]
        public void TestRemoveSpaceShipFromArray()
        {
            SpaceShip[] spaceShips = CreateSpaceShips(5);
            spaceShips = ArrayUtils.RemoveFromArray(spaceShips, 2);
            Assert.That(spaceShips.Length, Is.EqualTo(5));
            Assert.That(spaceShips[2], Is.EqualTo(null));

            spaceShips = ArrayUtils.RemoveNullsFromArray(spaceShips);
            Assert.That(spaceShips.Length, Is.EqualTo(4));
        }
    }
}