using NUnit.Framework;
using TrekBattles.model;

namespace TrekBattles.test.model
{
    [TestFixture]
    public class TargetingMissileTest
    {
        [Test]
        public void TestMissileConfigurationCanNotBeChanged()
        {
            TargetingMissile missile = new TargetingMissile(10, 2);
            Assert.That(missile.DamageAmount, Is.EqualTo(10));
            Assert.That(missile.TargetShipPosition, Is.EqualTo(2));
        }
    }
}