#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using TrekBattles.Model;
using TrekBattles.util;

namespace TrekBattles.io
{
    public class FleetLoader
    {
        private const int ShipClassNamePosition = 0;
        private const int ShipShieldStrengthPosition = 1;
        private const int ShipRegenerationRatePosition = 2;
        private const int ShipHullStrengthPosition = 3;
        private const int ShipWeaponBaseDamagePosition = 4;
        private const int ShipWeaponRandomDamagePosition = 5;

        private const int NumberOfPropertiesPerShip = 6;
        private const int ShipInformationStartPostion = 2;

        private readonly string fleetFileName;
        private readonly Random randomGenerator;
        private string[] fleetFileContents;

        public FleetLoader(string fleetFileName, Random randomGenerator)
        {
            this.fleetFileName = fleetFileName;
            this.randomGenerator = randomGenerator;
            this.fleetFileContents = FileUtil.ReadFileContents(fleetFileName);
        }



        /// <summary>
        /// Loads the fleet information and constructs a new fleet. Null will be 
        /// returned if any part of the fleet is invalid
        /// </summary>
        /// <returns></returns>
        public Fleet LoadFleet()
        {
            try
            {
                if (fleetFileContents == null)
                {
                    return null;
                }
                string fleetName = fleetFileContents[0];

                if (!ValidationUtil.ValidString(fleetName))
                {
                    ConsoleUtil.WriteFleetNameMissing(fleetFileName);
                    return null;
                }

                int expectedNumberOfShips = ArrayUtils.ParseNumberFromArray(fleetFileContents, 1);

                if (!ValidationUtil.ValidNumber(expectedNumberOfShips))
                {
                    ConsoleUtil.WriteInvalidNumberOfShips(fleetFileName);
                    return null;
                }

                SpaceShip[] spaceShips = CreateSpaceShips(expectedNumberOfShips);
                return spaceShips == null ? null : new Fleet(fleetName, spaceShips, randomGenerator);
            }
            catch (Exception ex)
            {
                // this is most likely an out of memory exception if to large a number is used 
                // for the number of ships in the fleet. 
                ConsoleUtil.WriteException(ex.Message, fleetFileName);
                return null;
            }
        }

        /// <summary>
        /// Creates an array of spaceships
        /// </summary>
        /// <param name="expectedNumberOfShips"></param>
        /// <returns></returns>
        private SpaceShip[] CreateSpaceShips(int expectedNumberOfShips)
        {
            SpaceShip[] spaceShips = new SpaceShip[expectedNumberOfShips];
            int shipsCreatedIndex = 0;
            int index = ShipInformationStartPostion;

            // while not created all ships
            while (shipsCreatedIndex != expectedNumberOfShips)
            {
                SpaceShip spaceShip = CreateSpaceShip(index);

                if (spaceShip == null)
                {
                    return null; // exit as soon as there is an invalid ship
                }
                spaceShips[shipsCreatedIndex] = spaceShip;
                shipsCreatedIndex++;
                index += NumberOfPropertiesPerShip;
            }

            if (ArrayUtils.HasValidElementsInArrayRange(fleetFileContents, index,
                fleetFileContents.Length))
            {
                // more content in the file than expected, reject
                ConsoleUtil.WriteMoreShipsThanExpected(fleetFileName);
                return null;
            }
            return spaceShips;
        }


        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="fileContents"></param>
        /// <param name="fleetFileName"></param>
        /// <param name="startingPosition"></param>
        /// <param name="randomGenerator"></param>
        /// <returns></returns>
        private SpaceShip CreateSpaceShip(int startingPosition)
        {
            string shipClassName = null;

            int shieldStrength = 0,
                regenerationRate = 0,
                hullStrength = 0,
                weaponBaseDamage = 0,
                weaponRandomDamage = 0;
            bool validShip = true;


            for (int index = 0; index < NumberOfPropertiesPerShip; index++)
            {
                if (!validShip)
                {
                    break;
                }

                switch (index)
                {
                    case ShipClassNamePosition:
                        DetermineShipProperty((startingPosition + index), ref validShip,
                            ref shipClassName);
                        break;
                    case ShipShieldStrengthPosition:
                        DetermineShipProperty((startingPosition + index), ref shieldStrength,
                            ref validShip, "shield strength", shipClassName);
                        break;

                    case ShipRegenerationRatePosition:
                        DetermineShipProperty((startingPosition + index), ref regenerationRate,
                            ref validShip, shieldStrength, "regen rate", shipClassName);
                        break;

                    case ShipHullStrengthPosition:
                        DetermineShipProperty((startingPosition + index), ref hullStrength,
                            ref validShip, "hull strength", shipClassName);
                        break;

                    case ShipWeaponBaseDamagePosition:
                        DetermineShipProperty((startingPosition + index), ref weaponBaseDamage,
                            ref validShip, "weapon base", shipClassName);
                        break;

                    case ShipWeaponRandomDamagePosition:
                        DetermineShipProperty((startingPosition + index), ref weaponRandomDamage,
                            ref validShip, "weapon random", shipClassName);
                        break;
                }
            }
            if (validShip)
            {
                return new SpaceShip(shipClassName, shieldStrength, hullStrength, regenerationRate,
                    weaponBaseDamage, weaponRandomDamage, randomGenerator);
            }

            return null;
        }


        /// <summary>
        /// Determines what a interger ship propery is from the given fleet file.
        /// If the property does not exist, or is invalid, validShip will be set
        /// to false. Allows for checking the property value is less than or equal
        /// to the maxValueForProperty
        /// </summary>
        /// <param name="shipPropertyPostion">Position of property to read</param>
        /// <param name="shipProperty">Reference value of the propery</param>
        /// <param name="validShip">Flag to indicate if property is invalid</param>
        /// <param name="maxValueForProperty">Maxiumn value for property</param>
        /// <param name="shipPropertyName">Name of the ship property</param>
        /// <param name="shipClassName">Name of the ship class</param>
        private void DetermineShipProperty(int shipPropertyPostion, ref int shipProperty,
            ref bool validShip, int maxValueForProperty, string shipPropertyName,
            string shipClassName)
        {
            DetermineShipProperty(shipPropertyPostion, ref shipProperty, ref validShip,
                shipPropertyName, shipClassName);

            if (!validShip || shipProperty <= maxValueForProperty)
            {
                return;
            }
            validShip = false;
            ConsoleUtil.WriteInvalidShieldProperty(shipClassName, fleetFileName, shipPropertyName);
        }

        /// <summary>
        /// Determines what a interger ship propery is from the given fleet file.
        /// If the property does not exist, or is invalid, validShip will be set
        /// to false 
        /// </summary>
        /// <param name="shipPropertyPostion">Position of property to read</param>
        /// <param name="shipProperty">Reference value of the propery</param>
        /// <param name="validShip">Flag to indicate if property is invalid</param>
        /// <param name="shipPropertyName">Name of the ship property</param>
        /// <param name="shipClassName">Name of the ship class</param>
        private void DetermineShipProperty(int shipPropertyPostion, ref int shipProperty,
            ref bool validShip, string shipPropertyName, string shipClassName)
        {
            // get the ship property
            shipProperty = ArrayUtils.ParseNumberFromArray(fleetFileContents, shipPropertyPostion);

            // return if the ship property is valid
            if (ValidationUtil.ValidNumber(shipProperty))
            {
                return;
            }
            ConsoleUtil.WriteInvalidShieldProperty(shipClassName, fleetFileName, shipPropertyName);
            validShip = false;
        }

        /// <summary>
        /// Determines what a string ship propery is from the given fleet file.
        /// If the property does not exist, or is invalid, validShip will be set
        /// to false 
        /// </summary>
        /// <param name="shipPropertyPostion">Position of property to read</param>
        /// <param name="shipProperty">Reference value of the propery</param>
        /// <param name="validShip">Flag to indicate if property is invalid</param>
        private void DetermineShipProperty(int shipPropertyPostion, ref bool validShip,
            ref string shipProperty)
        {
            if (ArrayUtils.HasElementAtArrayPostions(fleetFileContents, shipPropertyPostion))
            {
                shipProperty = fleetFileContents[shipPropertyPostion];
            }
            if (ValidationUtil.ValidString(shipProperty))
            {
                return;
            }
            ConsoleUtil.WriteShipClassNameMissing(fleetFileName);
            validShip = false;
        }
    }
}