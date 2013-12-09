#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

using System;
using System.IO;

namespace TrekBattles.util
{
    internal class FileUtil
    {
        /// <summary>
        /// Reads the contents of the given file contains fleet information. 
        /// If an exception occurs, an error will be reported and 
        /// null will be returned
        /// </summary>
        /// <param name="fleetFile">The file to load</param>
        /// <returns>An array of each line in the file</returns>
        public static string[] ReadFileContents(string fleetFile)
        {
            string[] fileContents;
            try
            {
                fileContents = File.ReadAllLines(fleetFile);
            }
            catch (IOException)
            {
                ConsoleUtil.WriteFileNotFound(fleetFile);
                return null;
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Console.WriteLine(unauthorizedAccessException.Message);
                return null;
            }
            if (fileContents != null && fileContents.Length > 0)
            {
                return fileContents;
            }
            ConsoleUtil.WriteFleetNameMissing(fleetFile);
            return null;
        }
    }
}