#region Copyright

// Created by Jeremy 
// 09 2013

#endregion

namespace TrekBattles.model
{
    /// <summary>
    /// Struct to repesent a targeting missile. A ship will supply the damage 
    /// the missile will do on impact. 
    /// The fleet will configure the missile target, as the fleet has the 
    /// intelligence on the changing size of the enemy fleet
    /// </summary>
    public struct TargetingMissile
    {
        private readonly int damageAmount;
        private readonly int targetShipPosition;

        /// <summary>
        /// Creates a Targeting Missile with the damage it will inflict and 
        /// its targets position in the enemy
        /// fleet. 
        /// </summary>
        /// <param name="damageAmountOfMissile">The damage that will be applied to the ship</param>
        /// <param name="targetShipPosition">The position of the target ship</param>
        public TargetingMissile(int damageAmountOfMissile, int targetShipPosition)
        {
            this.damageAmount = damageAmountOfMissile;
            this.targetShipPosition = targetShipPosition;
        }


        /// <summary>
        /// The ship position that this missile will target. Once the target 
        /// for the missile has been 
        /// set, it cannot be changed. This prevents enemy fleets from changing the target.  
        /// </summary>
        public int TargetShipPosition
        {
            get { return targetShipPosition; }
        }

        /// <summary>
        /// The damage amount that this missile will deliver
        /// </summary>
        public int DamageAmount
        {
            get { return damageAmount; }
        }
    }
}