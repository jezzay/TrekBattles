#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using TrekBattles.Model;
using TrekBattles.util;

namespace TrekBattles.model
{
    public class SpaceBattle
    {
        private readonly Fleet fleet1;
        private readonly Fleet fleet2;
        private int battleRounds;

        public SpaceBattle(Fleet fleet1, Fleet fleet2)
        {
            this.fleet1 = fleet1;
            this.fleet2 = fleet2;
        }

        /// <summary>
        /// Notifes each fleet that a new battle round is about to commence.
        /// </summary>
        private void NotifyFleetsNewBattleCommencing()
        {
            fleet1.NewBattleCommencing(fleet2.SizeofActiveFleet);
            fleet2.NewBattleCommencing(fleet1.SizeofActiveFleet);
        }

        /// <summary>
        /// Each fleet sends targeting missiles it's ships
        /// to attack the enemy fleet ships. 
        /// </summary>
        private void FleetsAttackEachOther()
        {
            TargetingMissile[] missiles = fleet1.FireTargetingMissiles();
            fleet2.RecieveFireFromEnemyFleet(missiles);

            missiles = fleet2.FireTargetingMissiles();
            fleet1.RecieveFireFromEnemyFleet(missiles);
        }

        /// <summary>
        /// Carries out the ceasefire bewtween the two fleets at the end of 
        /// the round. Any ships that have been destroyed in either fleet 
        /// will be reported. 
        /// </summary>
        private void Ceasefire()
        {
            string[] shipsDestroyed = fleet1.Ceasefire();

            if (shipsDestroyed.Length >= 1)
            {
                ConsoleUtil.WriteShipsDestroyedInRound(fleet1.FleetName, shipsDestroyed,
                    battleRounds);
            }
            shipsDestroyed = fleet2.Ceasefire();
            if (shipsDestroyed.Length >= 1)
            {
                ConsoleUtil.WriteShipsDestroyedInRound(fleet2.FleetName, shipsDestroyed,
                    battleRounds);
            }
        }

        /// <summary>
        /// Commences a battle between the two fleets. Will continue until either
        /// one fleet is destroyed or both are destroyed. 
        /// </summary>
        public void CommenceBattle()
        {
            battleRounds = 1;

            while (fleet1.IsFleetOperational() && fleet2.IsFleetOperational())
            {
                NotifyFleetsNewBattleCommencing();

                FleetsAttackEachOther();
                Ceasefire();
                ++battleRounds;
            }
        }

        /// <summary>
        /// Report on the results of the battle bewteen the two fleets
        /// </summary>
        public void ReportBattleResults()
        {
            battleRounds -= 1;
            if (!fleet1.IsFleetOperational() && !fleet2.IsFleetOperational())
            {
                ConsoleUtil.WriteDraw(battleRounds);
            }else if (fleet1.IsFleetOperational())
            {
                ConsoleUtil.WriteFleetWonBattle(fleet1, fleet2, battleRounds);
            }
            else
            {
                ConsoleUtil.WriteFleetWonBattle(fleet2, fleet1, battleRounds);
            }
        }
    }
}