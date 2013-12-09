#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using TrekBattles.model;
using TrekBattles.util;

namespace TrekBattles.Model
{
    public class Fleet
    {
        private readonly string fleetName;
        private readonly Random randomShipTargeter;
        private int enemyFleetSize;
        private int[] shipsHitInCurrentBattle;

        public Fleet(string fleetName, SpaceShip[] spaceShips, Random randomShipTargeter)
        {
            this.fleetName = fleetName;
            SpaceShipsInService = spaceShips;
            this.randomShipTargeter = randomShipTargeter;
            this.shipsHitInCurrentBattle = new int[0];
            DestroyedSpaceShips = new SpaceShip[0];
        }

        private Random RandomShipTargeter
        {
            get { return randomShipTargeter; }
        }

        private int EnemyFleetSize
        {
            get { return enemyFleetSize; }
            set
            {
                if (value >= 1)
                {
                    enemyFleetSize = value;
                }
            }
        }

        public SpaceShip[] SpaceShipsInService { get; private set; }

        public SpaceShip[] DestroyedSpaceShips { get; private set; }

        public string FleetName
        {
            get { return fleetName; }
        }


        private int[] ShipsHitInCurrentBattle
        {
            get { return shipsHitInCurrentBattle; }
            set
            {
                if (value != null)
                {
                    shipsHitInCurrentBattle = value;
                }
            }
        }

        /// <summary>
        /// The number of ships that are active in this fleet, ie not destroyed
        /// </summary>
        public int SizeofActiveFleet
        {
            get { return SpaceShipsInService.Length; }
        }

        /// <summary>
        /// Total number of ships this fleet has lost
        /// </summary>
        public int TotalNumberOfLosses
        {
            get { return DestroyedSpaceShips.Length; }
        }


        /// <summary>
        /// Fires the fleet's targeting missiles. 
        /// </summary>
        /// <returns></returns>
        public TargetingMissile[] FireTargetingMissiles()
        {
            TargetingMissile[] attackMissiles = new TargetingMissile[SpaceShipsInService.Length];
            for (int index = 0; index < SpaceShipsInService.Length; index++)
            {
                SpaceShip spaceShip = SpaceShipsInService[index];
                int damagePayload = spaceShip.FireWeapons();
                int enemyTargetPosition = DetermineEnemyTargetPosition();
                attackMissiles[index] = new TargetingMissile(damagePayload, enemyTargetPosition);
            }
            return attackMissiles;
        }

        private int DetermineEnemyTargetPosition()
        {
            int random = RandomShipTargeter.Next(EnemyFleetSize);
            return random;
        }

        public bool IsFleetOperational()
        {
            return SpaceShipsInService.Length >= 1;
        }

        /// <summary>
        /// Recives an array of incomming missile from the enemy fleet.
        /// The missiles will deal their specified damage to the target ships 
        /// in this fleet. This Fleet will also update it's battle information on the 
        /// ships that have been hit. Will have no effect if missile targets invalid ships
        /// </summary>
        /// <param name="incommingMissiles"></param>
        public void RecieveFireFromEnemyFleet(TargetingMissile[] incommingMissiles)
        {
            foreach (TargetingMissile targetingMissile in incommingMissiles)
            {
                RecieveFireFromEnemyFleet(targetingMissile);
            }
        }

        /// <summary>
        /// Recives a incomming missile from the enemy fleet. The missile will
        /// deal it's specified damage to the target ship in this 
        /// fleet. This Fleet will also update it's battle information on the 
        /// ship that has been hit. Will have no effect if missile targets invalid ship
        /// </summary>
        /// <param name="incommingMissile"></param>
        public void RecieveFireFromEnemyFleet(TargetingMissile incommingMissile)
        {
            if (incommingMissile.TargetShipPosition > SpaceShipsInService.Length ||
                incommingMissile.TargetShipPosition < 0)
            {
                return;
            }
            SpaceShip spaceShipUnderAttack =
                SpaceShipsInService[incommingMissile.TargetShipPosition];

            spaceShipUnderAttack.RecieveDamage(incommingMissile.DamageAmount);
            UpdateBattleInformation(incommingMissile.TargetShipPosition);
        }

        /// <summary>
        /// 
        /// Records that a ship has been hit in the current battle. Will only 
        /// record that a ship has been hit, not the number of 
        /// times it has been hit. 
        /// </summary>
        /// <param name="indexOfShipHit"></param>
        private void UpdateBattleInformation(int indexOfShipHit)
        {
            if (ArrayUtils.ArrayContainsNumber(ShipsHitInCurrentBattle, indexOfShipHit))
            {
                return;
            }
            ShipsHitInCurrentBattle = ArrayUtils.AddNumber(ShipsHitInCurrentBattle, indexOfShipHit);
        }

        /// <summary>
        /// Allows a fleet to be notifed that a new battle is about to commence
        /// </summary>
        /// <param name="updatedEnemyFleetSize">The size of the enemy fleet</param>
        public void NewBattleCommencing(int updatedEnemyFleetSize)
        {
            this.EnemyFleetSize = updatedEnemyFleetSize;
            ShipsHitInCurrentBattle = new int[0];
        }

        /// <summary>
        /// A ceasefire is carried out between Fleets, giving each fleet time to 
        /// allow ships that have not been hit in the last round to regenerate. 
        /// Also allows the fleet to update it's list of ships which have
        /// been destroyed.
        /// </summary>
        public string[] Ceasefire()
        {
            RegenerateShields();
            return UpdateBattleInformation();
        }


        /// <summary>
        /// Updates the list of ships which have been destroyed, and which are still in service. 
        /// </summary>
        private string[] UpdateBattleInformation()
        {
            string[] destroyedShipNames = UpdateDestroyedShips();
            SpaceShipsInService = ArrayUtils.RemoveNullsFromArray(SpaceShipsInService);
            return destroyedShipNames;
        }

        /// <summary>
        /// Updates the list of ships destroyed and still in service. 
        /// Returns the list of ships class names which have been destroyed.  
        /// </summary>
        /// <returns></returns>
        private string[] UpdateDestroyedShips()
        {
            string[] destroyedShipNames = {};
            for (int index = 0; index < SpaceShipsInService.Length; index++)
            {
                SpaceShip spaceShip = SpaceShipsInService[index];
                if (spaceShip.IsDestroyed())
                {
                    RemoveDestroyedShipFromService(spaceShip, index);
                    destroyedShipNames = ArrayUtils.AddStringToArray(destroyedShipNames,
                        spaceShip.ShipClassName);
                }
            }
            return destroyedShipNames;
        }

        /// <summary>
        /// Each ship which is still in service and has not been hit 
        /// in the current battle regenerates it's
        /// shields. 
        /// </summary>
        private void RegenerateShields()
        {
            for (int index = 0; index < SpaceShipsInService.Length; index++)
            {
                if (HasShipBeenHitInCurrentBattle(index))
                {
                    continue;
                }
                SpaceShip spaceShip = SpaceShipsInService[index];
                spaceShip.RegenerateShield();
            }
        }

        private void RemoveDestroyedShipFromService(SpaceShip destroyedSpaceShip, 
            int spaceShipIndex)
        {
            DestroyedSpaceShips = ArrayUtils.AddToArray(DestroyedSpaceShips, destroyedSpaceShip);
            SpaceShipsInService = ArrayUtils.RemoveFromArray(SpaceShipsInService, spaceShipIndex);
        }

        private bool HasShipBeenHitInCurrentBattle(int spaceShipPosition)
        {
            return ArrayUtils.ArrayContainsNumber(ShipsHitInCurrentBattle, spaceShipPosition);
        }
    }
}