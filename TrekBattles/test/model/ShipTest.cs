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
    public class ShipTest
    {
        [SetUp]
        public void SetUp()
        {
            SpaceShip = new SpaceShip("Voyager", 10, 20, 5, 5, 10, new Random());
        }

        private SpaceShip SpaceShip;


        private void ExpectSpaceShipToBe(bool shieldsDown, bool destroyed)
        {
            Assert.That(shieldsDown, Is.EqualTo(SpaceShip.IsShieldDown()),
                "Expected the shield to be " + ((shieldsDown) ? "down" : "up"));

            Assert.That(destroyed, Is.EqualTo(SpaceShip.IsDestroyed()),
                "Expected the ship to be " + ((destroyed) ? "destroyed" : "not destroyed"));
        }


        [Test]
        public void DetermineShipDamage()
        {
            SpaceShip = new SpaceShip("Test", 1, 15, 1, 1, 1, new Random());
            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.Undamaged));
            SpaceShip.RecieveDamage(2);
            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.LightlyDamaged));

            SpaceShip.RecieveDamage(1);

            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.ModeratelyDamaged));


            SpaceShip.RecieveDamage(3);
            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.HeavilyDamaged));

            SpaceShip.RecieveDamage(2);
            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.HeavilyDamaged));


            SpaceShip.RecieveDamage(2);
            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.HeavilyDamaged));


            SpaceShip.RecieveDamage(2);
            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.VeryHeavilyDamaged));

            SpaceShip.RecieveDamage(2000);
            Assert.That(SpaceShip.DamageStatus(), Is.EqualTo(ShipDamageEnum.VeryHeavilyDamaged));
        }

        [Test]
        public void TestIsShipDestroyed()
        {
            Assert.IsFalse(SpaceShip.IsDestroyed(), "Expected the spaceship to not be destroyed");

            SpaceShip.RecieveDamage(100);
            Assert.IsTrue(SpaceShip.IsDestroyed(), "Expected the spaceship to be destroyed");
        }

        [Test]
        public void TestShipCanRegenerateAfterShieldHasBeenTakenDown()
        {
            SpaceShip.RecieveDamage(19);
            Assert.IsTrue(SpaceShip.IsShieldDown());
            SpaceShip.RegenerateShield();
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(11),
                "Expected hull strength to be changed");
            SpaceShip.RegenerateShield();
            Assert.IsFalse(SpaceShip.IsShieldDown());
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(11),
                "Expected hull strength to un changed");
        }

        [Test]
        public void TestShipCanRegenerateItsShields()
        {
            SpaceShip.RecieveDamage(5);
            Assert.That(SpaceShip.ShieldCurrentStrength, Is.EqualTo(5));
            SpaceShip.RegenerateShield();
            Assert.That(SpaceShip.ShieldCurrentStrength, Is.EqualTo(10));
        }

        [Test]
        public void TestShipCanReturnWeaponDamage()
        {
            this.SpaceShip = new SpaceShip("Voyager", 10, 10, 10, 5, 30, new Random(50));
            Assert.That(SpaceShip.FireWeapons(), Is.EqualTo(30),
                "Expected the ship to return more than the base damage");
            Assert.That(SpaceShip.FireWeapons(), Is.EqualTo(19),
                "Expected the ship to return more than the base damage");
            Assert.That(SpaceShip.FireWeapons(), Is.EqualTo(26),
                "Expected the ship to return more than the base damage");
            Assert.That(SpaceShip.FireWeapons(), Is.EqualTo(11),
                "Expected the ship to return more than the base damage");
        }

        [Test]
        public void TestShipShieldStrengthDoesNotRegenerateAboveMaxShieldStrength()
        {
            SpaceShip.RecieveDamage(3);
            SpaceShip.RegenerateShield();
            SpaceShip.RegenerateShield();
            SpaceShip.RegenerateShield();
            Assert.That(SpaceShip.ShieldCurrentStrength, Is.EqualTo(SpaceShip.ShieldMaxStrength));
        }

        [Test]
        public void TestShipTakesDamageAndImpactsHull()
        {
            SpaceShip = new SpaceShip("Voyager", 20, 20, 5, 10, 10, new Random());
            SpaceShip.RecieveDamage(10);
            ExpectSpaceShipToBe(false, false);
            //shield should be at 10 now, 30 damage should destroy shield and destroy hull
            SpaceShip.RecieveDamage(30);
            ExpectSpaceShipToBe(true, true);
        }

        [Test]
        public void TestShipTakesDamageLessThanShieldStrength()
        {
            SpaceShip.RecieveDamage(2);
            ExpectSpaceShipToBe(false, false);
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(SpaceShip.HullMaxStrength));

            SpaceShip.RecieveDamage(2);
            ExpectSpaceShipToBe(false, false);
        }

        [Test]
        public void TestShipTakesDamageMoreThanShieldStrength()
        {
            SpaceShip.RecieveDamage(15);
            ExpectSpaceShipToBe(true, false);
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(15));


            SpaceShip.RecieveDamage(15);
            ExpectSpaceShipToBe(true, true);
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(0));
        }

        [Test]
        public void TestShipTakesDamageSameAmountAsShieldStrength()
        {
            SpaceShip.RecieveDamage(10);
            ExpectSpaceShipToBe(true, false);
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(SpaceShip.HullMaxStrength));

            SpaceShip.RecieveDamage(10);
            ExpectSpaceShipToBe(true, false);
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(10));

            // kill off the ship
            SpaceShip.RecieveDamage(100);
            ExpectSpaceShipToBe(true, true);
            Assert.That(SpaceShip.HullCurrentStrength, Is.EqualTo(-90));
        }
    }
}