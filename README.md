TrekBattles - a C# .Net Console Uni Assignment
================================
20/20 mark - September 2013

This assignment was focused around battles between between two fleets of space ships (inspired by Star Trek). Two files in a special format would be 
read in, and the result of the battle would be printed out to the console. See fleet1.txt and fleet2.txt. There were also complicated logic around 
the ordering of firing between the two fleets. 

This was my first time using C#, ended up with full marks :) 

File format
-----------

The file format from the assignment spec is as follows: 

 - The first line will contain the name of the fleet. This is a string.
 - The second line will contain the number of ships in the fleet. An integer >= 1.
 - Each ship will be listed after that. Ships are defined by 6 pieces of information, each on a separate line
1. Ship Class name. A string.
2. Shield Strength. An integer >= 1.
3. Regeneration Rate. An integer >= 1 and <= Shield Strength.
4. Hull Strength. An integer >= 1.
5. Weapon Base Damage. An integer >= 1.
6. Weapon Random Damage. An integer >= 1.
7. There are no blank lines in the file except after the last ship has been listed.

Restrictions for this assignment
--------------------------------
 - Could only use Arrays
 - No Lists or Linq
 - Could only use what was covered in class at the time the assignment was released