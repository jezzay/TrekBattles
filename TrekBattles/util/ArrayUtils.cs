#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using TrekBattles.Model;

namespace TrekBattles.util
{
    /// <summary>
    /// Contains utilies to work with arrays. Could be better if generics were used. 
    /// </summary>
    public class ArrayUtils
    {
        private const int InvalidNumber = -1;

        private ArrayUtils()
        {
        }

        /// <summary>
        /// Add an new Spaceship to the array
        /// </summary>
        /// <param name="spaceShips"></param>
        /// <param name="objectToAdd"></param>
        /// <returns></returns>
        public static SpaceShip[] AddToArray(SpaceShip[] spaceShips, SpaceShip objectToAdd)
        {
//            int previousLength = spaceShips.Length;
            Array.Resize(ref spaceShips, spaceShips.Length + 1);
            spaceShips[spaceShips.Length - 1] = objectToAdd;
            return spaceShips;
        }

        /// <summary>
        /// Add a new string to the array
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="stringToAdd"></param>
        /// <returns></returns>
        public static string[] AddStringToArray(string[] stringArray, string stringToAdd)
        {
            Array.Resize(ref stringArray, stringArray.Length + 1);
            stringArray[stringArray.Length - 1] = stringToAdd;
            return stringArray;
        }

        /// <summary>
        /// Add a new integer to the array
        /// </summary>
        /// <param name="arrayOfInts"></param>
        /// <param name="numberToAdd"></param>
        /// <returns></returns>
        public static int[] AddNumber(int[] arrayOfInts, int numberToAdd)
        {
            Array.Resize(ref arrayOfInts, arrayOfInts.Length + 1);
            arrayOfInts[arrayOfInts.Length - 1] = numberToAdd;
            return arrayOfInts;
        }


        /// <summary>
        /// Checks if the array has elements from the start postion through to the end postion
        /// Note, that the elements may be null, this only guarantees that elements exist 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startPostion"></param>
        /// <param name="endPostion"></param>
        /// <returns></returns>
        public static bool HasValidElementsInArrayRange(string[] array, int startPostion,
            int endPostion)
        {
            for (int i = startPostion; i <= endPostion; i++)
            {
                if (i < array.Length && ValidationUtil.ValidString(array[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if there is an element at the specified location
        /// </summary>
        /// <param name="array"></param>
        /// <param name="postion"></param>
        /// <returns></returns>
        public static bool HasElementAtArrayPostions(string[] array, int postion)
        {
            return postion < array.Length;
        }

        /// <summary>
        /// Sets the element at the spefified index to null. Once any further work on 
        /// the array that requires consistent indexs, has 
        /// been performed RemoveNullsFromArray can 
        /// be called to clean up the array. 
        /// </summary>
        /// <param name="spaceShips"></param>
        /// <param name="indexOfElement"></param>
        public static SpaceShip[] RemoveFromArray(SpaceShip[] spaceShips, int indexOfElement)
        {
            spaceShips[indexOfElement] = null;
            return spaceShips;
        }

        /// <summary>
        /// Returns true if array contains the specfied number
        /// </summary>
        /// <param name="arrayInts"></param>
        /// <param name="numberToCheck"></param>
        /// <returns></returns>
        public static bool ArrayContainsNumber(int[] arrayInts, int numberToCheck)
        {
            foreach (int number in arrayInts)
            {
                if (number == numberToCheck)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Cleans up the given array, by removing any null elements from the 
        /// given array.  
        /// </summary>
        /// <param name="arrayWithNulls"></param>
        /// <returns>New array with nulls removed</returns>
        public static SpaceShip[] RemoveNullsFromArray(SpaceShip[] arrayWithNulls)
        {
            SpaceShip[] spaceShips = {};
            if (arrayWithNulls == null)
            {
                return spaceShips;
            }
            foreach (SpaceShip spaceShip in arrayWithNulls)
            {
                if (spaceShip != null)
                {
                    spaceShips = AddToArray(spaceShips, spaceShip);
                }
            }
            return spaceShips;
        }

        /// <summary>
        /// Parses a string from the position in the array into a number. 
        /// Returns -1 if the element does not exist
        /// </summary>
        /// <param name="array"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int ParseNumberFromArray(string[] array, int position)
        {
            return HasElementAtArrayPostions(array, position)
                ? ValidationUtil.ParseStringToNumber(array[position])
                : InvalidNumber;
        }
    }
}