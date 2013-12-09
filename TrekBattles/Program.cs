#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using TrekBattles.io;
using TrekBattles.Model;
using TrekBattles.model;
using TrekBattles.util;

namespace TrekBattles
{
    public class Program
    {
        private const int CommandLineArgumentsRequired = 3;
        private const int ThirdArgument = 2;

        private static void Main(string[] args)
        {
            Console.WriteLine("TrekBattle by Jeremy Yuille");
            
            int seed;

            if (!ValidInputArguments(args, out seed))
            {
                return;
            }
            Random randomGenerator = new Random(seed);
            FleetLoader fleetLoader = new FleetLoader(args[1], randomGenerator);
            Fleet firstFleet = fleetLoader.LoadFleet();
            Fleet secondFleet = null;
            if (firstFleet != null)
            {
                fleetLoader = new FleetLoader(args[ThirdArgument], randomGenerator);
                secondFleet = fleetLoader.LoadFleet();
            }
            if (secondFleet == null)
            {
                return;
            }
            SpaceBattle battle = new SpaceBattle(firstFleet, secondFleet);
            battle.CommenceBattle();
            battle.ReportBattleResults();
        }

        private static bool ValidInputArguments(string[] arguments, out int seed)
        {
            seed = 0;
            if (arguments == null || arguments.Length != CommandLineArgumentsRequired)
            {
                ConsoleUtil.PrintInvalidArguments();
                return false;
            }
            seed = ValidationUtil.ParseStringToNumber(arguments[0]);
            if (ValidationUtil.ValidNumber(seed))
            {
                return true;
            }
            ConsoleUtil.PrintInvalidSeed();
            return false;
        }
    }
}