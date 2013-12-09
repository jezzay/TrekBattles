#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using TrekBattles.model;

namespace TrekBattles.Model
{
    /// <summary>
    /// A SpaceShip that can participate in a fleet of other spaceships. 
    /// </summary>
    public class SpaceShip
    {
        private const int HunderentPercent = 100;

        private readonly int hullMaxStrength;
        private readonly Random randomWeaponGenerator;
        private readonly int shieldMaxStrength;
        private readonly int shieldRegenerationRate;
        private readonly string shipClassName;
        private readonly int weaponBaseDamage;
        private readonly int weaponRandomDamage;

        private int hullCurrentStrength;
        private int shieldCurrentStrength;


        /// <summary>
        /// Commissions a new SpaceShip that is ready for active duty. 
        /// Created with the given specifications.  
        /// </summary>
        /// <param name="shipClassName">The Ship class name</param>
        /// <param name="shieldMaxStrength">The maxium shield strength of 
        /// the ship</param>
        /// <param name="hullMaxStrength">The maxium hull strength of 
        /// the ship</param>
        /// <param name="shieldRegenerationRate">The regeneration rate 
        /// for the shields</param>
        /// <param name="weaponBaseDamage">The amount of base damage 
        /// weapons will produce</param>
        /// <param name="weaponRandomDamage">The random component to 
        /// weapon damage</param>
        /// <param name="randomWeaponGenerator">Random generator that will 
        /// generate random weapon damage</param>
        public SpaceShip(string shipClassName, int shieldMaxStrength, int hullMaxStrength,
            int shieldRegenerationRate, int weaponBaseDamage, int weaponRandomDamage,
            Random randomWeaponGenerator)
        {
            this.shieldCurrentStrength = shieldMaxStrength;
            this.hullCurrentStrength = hullMaxStrength;
            this.shieldMaxStrength = shieldMaxStrength;
            this.hullMaxStrength = hullMaxStrength;
            this.shieldRegenerationRate = shieldRegenerationRate;
            this.weaponBaseDamage = weaponBaseDamage;
            this.weaponRandomDamage = weaponRandomDamage;
            this.randomWeaponGenerator = randomWeaponGenerator;
            this.shipClassName = shipClassName;
        }

        public int HullCurrentStrength
        {
            get { return hullCurrentStrength; }
            private set
            {
                if (! (value > hullMaxStrength))
                {
                    hullCurrentStrength = value;
                }
            }
        }


        public string ShipClassName
        {
            get { return shipClassName; }
        }


        public int HullMaxStrength
        {
            get { return hullMaxStrength; }
        }

        public int ShieldCurrentStrength
        {
            get { return shieldCurrentStrength; }
            private set
            {
                if (!(value > shieldMaxStrength))
                {
                    shieldCurrentStrength = value;
                }
            }
        }

        public int ShieldMaxStrength
        {
            get { return shieldMaxStrength; }
        }


        /// <summary>
        ///     Returns true if this ships shields are currently down
        /// </summary>
        /// <returns></returns>
        public bool IsShieldDown()
        {
            return ShieldCurrentStrength == 0;
        }

        /// <summary>
        /// Fires the ships weapons, returning the amount of damage that 
        /// the weapons have generated. 
        /// This is made up of the ships base weapon damage and the random generated amount.
        /// </summary>
        /// <returns>The amount of damage that the weapons have generated</returns>
        public int FireWeapons()
        {
            int random = randomWeaponGenerator.Next(weaponRandomDamage);
            return weaponBaseDamage + random;
        }

        /// <summary>
        /// Returns true if the damage percentage is between the two given damage statuses
        /// </summary>
        /// <param name="damagePercentage"></param>
        /// <param name="lowerDamageEnum"></param>
        /// <param name="higerDamageEnum"></param>
        /// <returns></returns>
        private bool BewteenDamageRange(int damagePercentage, ShipDamageEnum lowerDamageEnum,
            ShipDamageEnum higerDamageEnum)
        {
            return damagePercentage >= (int) lowerDamageEnum &&
                   damagePercentage < (int) higerDamageEnum;
        }

        /// <summary>
        /// Returns the current damage status of the ship
        /// </summary>
        /// <returns></returns>
        public ShipDamageEnum DamageStatus()
        {
            int damagePercentage = CalulateDamagePercent();

            if (IsDestroyed() ||
                BewteenDamageRange(damagePercentage, ShipDamageEnum.VeryHeavilyDamaged,
                    ShipDamageEnum.HeavilyDamaged))
            {
                return ShipDamageEnum.VeryHeavilyDamaged;
            }
            if (BewteenDamageRange(damagePercentage, ShipDamageEnum.HeavilyDamaged,
                ShipDamageEnum.ModeratelyDamaged))
            {
                return ShipDamageEnum.HeavilyDamaged;
            }
            if (BewteenDamageRange(damagePercentage, ShipDamageEnum.ModeratelyDamaged,
                ShipDamageEnum.LightlyDamaged))
            {
                return ShipDamageEnum.ModeratelyDamaged;
            }

            return BewteenDamageRange(damagePercentage, ShipDamageEnum.LightlyDamaged,
                ShipDamageEnum.Undamaged)
                ? ShipDamageEnum.LightlyDamaged
                : ShipDamageEnum.Undamaged;
        }

        /// <summary>
        /// Calulates the damage percentage delt to the ships hull
        /// </summary>
        /// <returns>Damage percentage</returns>
        private int CalulateDamagePercent()
        {
            return HullCurrentStrength*HunderentPercent/HullMaxStrength;
        }

        /// <summary>
        /// Increases the current shield strength by the regeneration amount. The shields will 
        /// only be regerated up to the shield maxium strength rate.
        /// </summary>
        public void RegenerateShield()
        {
            if (ShieldCurrentStrength != ShieldMaxStrength)
            {
                if (shieldRegenerationRate + ShieldCurrentStrength >= ShieldMaxStrength)
                {
                    this.shieldCurrentStrength = ShieldMaxStrength;
                }
                else
                {
                    this.shieldCurrentStrength += shieldRegenerationRate;
                }
            }
        }

        /// <summary>
        /// The ships shields try to absorb the damage amount given. If the damage 
        /// amount is larger than the current shield capcity, the amount 
        /// not abosrbed by the shield will be returned in the damageNotAbsorbed paramater
        /// </summary>
        /// <param name="damageAmount">The amount of damage to inflict on the shields</param>
        /// <param name="damageNotAbsorbed">The amount of damage that was not absorbed 
        /// by the shields</param>
        private void RecieveShieldDamage(int damageAmount, out int damageNotAbsorbed)
        {
            damageNotAbsorbed = 0;
            if (damageAmount >= ShieldCurrentStrength)
            {
                // reduce shields to zero, and set the amount of damage not absorbed by the shield
                damageNotAbsorbed = damageAmount - ShieldCurrentStrength;
                ShieldCurrentStrength = 0;
            }
            else
            {
                ShieldCurrentStrength -= damageAmount;
            }
        }

        /// <summary>
        /// Deals the specified amount of damage to the ship. Damage will be applied to the
        /// ships shield first. If the ships shield is down, or the damage amount is 
        /// greater than the current shield strength, then the hull of the
        /// ship will take the remaining amount of damage.
        /// </summary>
        /// <param name="damageAmount">The amount of damage that will 
        /// be applied to this space ship</param>
        public void RecieveDamage(int damageAmount)
        {
            int damageNotAbsorbedByShields;
            RecieveShieldDamage(damageAmount, out damageNotAbsorbedByShields);
            RecieveHullDamage(damageNotAbsorbedByShields);
        }

        private void RecieveHullDamage(int damageAmount)
        {
            HullCurrentStrength -= damageAmount;
        }

        /// <summary>
        ///     Returns true if this space ship has been destroyed
        /// </summary>
        /// <returns>True if it has been destoryed, false otherwise </returns>
        public bool IsDestroyed()
        {
            return HullCurrentStrength <= 0;
        }
    }
}