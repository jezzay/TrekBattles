#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using TrekBattles.Model;
using TrekBattles.model;

namespace TrekBattles.util
{
    public static class ConsoleUtil
    {
        private static void WriteProgramFailed()
        {
            Console.WriteLine("Program failed with following error");
        }

        public static void PrintInvalidArguments()
        {
            WriteProgramFailed();
            Console.WriteLine("Invalid number of command line arguments entered");
        }

        public static void PrintInvalidSeed()
        {
            WriteProgramFailed();
            Console.WriteLine("Invalid seed value entered");
        }

        internal static void WriteFileNotFound(string fleetFile)
        {
            WriteProgramFailed();
            Console.WriteLine("{0} file not found", fleetFile);
        }

        public static void WriteShipClassNameMissing(string fleetFile)
        {
            WriteProgramFailed();
            Console.WriteLine("Ship class name missing in {0}", fleetFile);
        }

        public static void WriteFleetNameMissing(string fleetFile)
        {
            WriteProgramFailed();
            Console.WriteLine("Missing fleet name in {0}", fleetFile);
        }

        internal static void WriteInvalidNumberOfShips(string fleetFile)
        {
            WriteProgramFailed();
            Console.WriteLine("Invalid number of ships in {0}", fleetFile);
        }

        public static void WriteInvalidShieldProperty(string shipClassName, string fleetFileName,
            string invalidShipProperty)
        {
            WriteProgramFailed();
            Console.WriteLine("Invalid {0} in ship class {1} in {2}", invalidShipProperty,
                shipClassName, fleetFileName);
        }

        public static void WriteMoreShipsThanExpected(string fleetFile)
        {
            WriteProgramFailed();
            Console.WriteLine("More ships than stated in {0}", fleetFile);
        }

        public static void WriteShipsDestroyedInRound(string fleetName, string[] shipsDestroyed,
            int battleRound)
        {
            Console.WriteLine();
            Console.WriteLine("After round {0} the {1} fleet has lost", battleRound, fleetName);
            foreach (string shipName in shipsDestroyed)
            {
                Console.WriteLine("  {0} destroyed", shipName);
            }
        }

        public static void WriteDraw(int numberOfRounds)
        {
            Console.WriteLine();
            Console.WriteLine(
                "After round {0} the battle has been a draw with both sides destroyed",
                numberOfRounds);
        }

        public static void WriteFleetWonBattle(Fleet winingFleet, Fleet loosingFleet,
            int numberOfBattleRounds)
        {
            Console.WriteLine();
            Console.WriteLine("After round {0} the {1} fleet won", numberOfBattleRounds,
                winingFleet.FleetName);
            Console.WriteLine("  {0} enemy ships destroyed", loosingFleet.TotalNumberOfLosses);
            Console.WriteLine("  {0} ships lost", winingFleet.TotalNumberOfLosses);
            Console.WriteLine("  {0} ships survived", winingFleet.SizeofActiveFleet);

            foreach (SpaceShip spaceShip in winingFleet.SpaceShipsInService)
            {
                string damageDispaly = DamageStatusDisplay(spaceShip.DamageStatus());
                Console.WriteLine("    {0} - {1}", spaceShip.ShipClassName, damageDispaly);
            }
        }

        private static string DamageStatusDisplay(ShipDamageEnum damage)
        {
            switch (damage)
            {
                case ShipDamageEnum.Undamaged:
                    return "undamaged";
                case ShipDamageEnum.LightlyDamaged:
                    return "lightly damaged";
                case ShipDamageEnum.ModeratelyDamaged:
                    return "moderately damaged";
                case ShipDamageEnum.HeavilyDamaged:
                    return "heavily damaged";
                case ShipDamageEnum.VeryHeavilyDamaged:
                    return "very heavily damaged";
                default:
                    return "";
            }
        }

        public static void WriteException(string exceptionMessage, string fleetFileName)
        {
            Console.WriteLine("{0} in {1}", exceptionMessage, fleetFileName);
        }
    }
}