#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using NUnit.Framework;
using TrekBattles.Model;
using TrekBattles.model;

namespace TrekBattles.test.model
{
    [TestFixture]
    public class FleetTest
    {
        public static Random CommonRandom = new Random(6);
        private string[] shipNames = new string[] {};

        [SetUp]
        public void SetUp()
        {
            this.fleet1 = new Fleet("Federation", CreateSpaceShips(2), CommonRandom);
            this.fleet2 = new Fleet("Borg", CreateSpaceShips(3), CommonRandom);
        }

        private Fleet fleet1;
        private Fleet fleet2;

        private SpaceShip[] CreateSpaceShips(int numberOfShips)
        {
            SpaceShip[] spaceShips = new SpaceShip[numberOfShips];
            for (int i = 0; i < numberOfShips; i++)
            {
                spaceShips[i] = new SpaceShip("Test" + i, 10, 15, 4, 10, 5, CommonRandom);
            }
            return spaceShips;
        }

        [Test]
        public void TestFleetCanStartNewBattleRound()
        {
            // not much to test here really. 
            fleet1.NewBattleCommencing(fleet2.SizeofActiveFleet);
            fleet2.NewBattleCommencing(fleet1.SizeofActiveFleet);
        }

        [Test]
        public void TestMissileHitsRightTarget()
        {
            TargetingMissile missile = new TargetingMissile(15, 1);
            fleet1.NewBattleCommencing(3);
            fleet1.RecieveFireFromEnemyFleet(missile);
            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(0)); // should not be updated yet
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(2));

            // ship two should have been hit, and shields should be down
            Assert.That(fleet1.SpaceShipsInService[1].IsShieldDown());
            // ship two should not have been hit
            Assert.That(!fleet1.SpaceShipsInService[0].IsShieldDown());
        }

        [Test]
        public void TestShipDoesNotRegenerateShieldWhenHit()
        {
            fleet1 = new Fleet("Federation", CreateSpaceShips(15), CommonRandom);
            fleet1.NewBattleCommencing(3);
            TargetingMissile missile = new TargetingMissile(15, 1); // shield down
            TargetingMissile missile2 = new TargetingMissile(15, 10); // shield down
            TargetingMissile missile3 = new TargetingMissile(150, 8); // kill 
            TargetingMissile missile4 = new TargetingMissile(2, 1);

            fleet1.RecieveFireFromEnemyFleet(missile);
            fleet1.RecieveFireFromEnemyFleet(missile2);
            fleet1.RecieveFireFromEnemyFleet(missile3);
            fleet1.RecieveFireFromEnemyFleet(missile4);

            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(0)); // should not be updated yet
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(15));

            Assert.That(fleet1.SpaceShipsInService[1].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[10].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[8].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[8].IsDestroyed()); // should still be in service 

            fleet1.Ceasefire();
            // shields should not regen
            Assert.That(fleet1.SpaceShipsInService[1].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[9].IsShieldDown());
            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(1));
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(14));
        }

        [Test]
        public void TestShipRegeneratesShieldWhenNotHit()
        {
            fleet1 = new Fleet("Federation", CreateSpaceShips(15), CommonRandom);

            TargetingMissile missile = new TargetingMissile(15, 1); // shield down
            TargetingMissile missile2 = new TargetingMissile(15, 10); // shield down
            TargetingMissile missile3 = new TargetingMissile(150, 8); // kill 
            TargetingMissile missile4 = new TargetingMissile(2, 1);

            fleet1.RecieveFireFromEnemyFleet(missile);
            fleet1.RecieveFireFromEnemyFleet(missile2);
            fleet1.RecieveFireFromEnemyFleet(missile3);
            fleet1.RecieveFireFromEnemyFleet(missile4);

            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(0)); // should not be updated yet
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(15));

            Assert.That(fleet1.SpaceShipsInService[1].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[10].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[8].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[8].IsDestroyed()); // should still be in service 

            shipNames = fleet1.Ceasefire();
            // shields should not regen
            Assert.That(fleet1.SpaceShipsInService[1].IsShieldDown());
            Assert.That(fleet1.SpaceShipsInService[9].IsShieldDown());
            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(1));
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(14));
            Assert.That(shipNames[0], Is.EqualTo("Test8"));

            fleet1.NewBattleCommencing(5);

            TargetingMissile missile5 = new TargetingMissile(13, 7);
            fleet1.RecieveFireFromEnemyFleet(missile5);
            fleet1.Ceasefire();

            // should regen
            Assert.That(!fleet1.SpaceShipsInService[1].IsShieldDown());
            Assert.That(!fleet1.SpaceShipsInService[9].IsShieldDown());

            // should not regen
            Assert.That(fleet1.SpaceShipsInService[7].IsShieldDown());
            // no more losses
            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(1));
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(14));
        }

        [Test]
        public void TestTargetAndReciveDamageToFleet()
        {
            TargetingMissile missile = new TargetingMissile(1000, 0);
            fleet1.RecieveFireFromEnemyFleet(missile);
            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(0)); // should not be updated yet
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(2));
            fleet1.Ceasefire();
            Assert.That(fleet1.TotalNumberOfLosses, Is.EqualTo(1));
            Assert.That(fleet1.SizeofActiveFleet, Is.EqualTo(1));
            Assert.That(fleet1.IsFleetOperational(), Is.EqualTo(true));
        }


        [Test]
        public void TestTargetingOfEnemyShips()
        {
            fleet1.NewBattleCommencing(fleet2.SizeofActiveFleet);
            Console.WriteLine("Fleet 2 size " + fleet2.SizeofActiveFleet);
//            Console.WriteLine(CommonRandom.Next(5));
//            Console.WriteLine(CommonRandom.Next(3));

//            Console.WriteLine(CommonRandom.Next(5));
//            Console.WriteLine(CommonRandom.Next(3));


            TargetingMissile[] fleetAttackMissiles = fleet1.FireTargetingMissiles();
            Assert.That(fleetAttackMissiles.Length, Is.EqualTo(2));
            Assert.That(fleetAttackMissiles[0].DamageAmount, Is.EqualTo(14));
            Assert.That(fleetAttackMissiles[0].TargetShipPosition, Is.EqualTo(1));

            Assert.That(fleetAttackMissiles[1].DamageAmount, Is.EqualTo(14));
            Assert.That(fleetAttackMissiles[1].TargetShipPosition, Is.EqualTo(2));
            fleet2.RecieveFireFromEnemyFleet(fleetAttackMissiles);
        }

        [Test]
        public void TestBattle()
        {
            fleet1.NewBattleCommencing(fleet2.SizeofActiveFleet);
            fleet2.NewBattleCommencing(fleet1.SizeofActiveFleet);

            TargetingMissile[] fleetAttackMissiles = fleet1.FireTargetingMissiles();
            fleet2.RecieveFireFromEnemyFleet(fleetAttackMissiles);

            fleetAttackMissiles = fleet2.FireTargetingMissiles();
            fleet1.RecieveFireFromEnemyFleet(fleetAttackMissiles);

            fleet1.Ceasefire();
            fleet2.Ceasefire();
        }
    }
}