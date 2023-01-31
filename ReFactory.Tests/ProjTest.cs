using AudioManagementUtil;

namespace ProjectTesting
{
    [TestFixture]
    public class ProjTest {

        [SetUp]
        public void Setup()
        {
            //Run static constructor
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(AudioManager).TypeHandle);
        }

        [Test]
        public void Test()
        {
            Assert.That(AudioManager.AudioMixers, Is.Not.Empty);
            Assert.That(AudioManager.AudioMixers.Count, Is.EqualTo(Enum.GetValues(typeof(AudioMixerGroup)).Length));
        }

        [TearDown]

        public void TearDown()
        {
            
        }

    }
}
