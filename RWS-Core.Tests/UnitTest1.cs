using RWS;
using RWS.Data;
using RWS.SubscriptionServices;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RWS_Core.Tests
{
    [TestFixture]
    public class ControllerSessionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //Requires a running VC
        [Test]
        public void GetIOSignals_Localhost_HasValue()
        {
            //Arrange
            ControllerSession rwsCs1 = new ControllerSession("localhost");

            //Act
            var ios = rwsCs1.RobotWareService.GetIOSignals().Embedded.State;

            //Assert
            Assert.IsNotEmpty(ios);
            Assert.IsNotNull(ios.First().LValue);
        }

        [Test]
        public async Task Bonjour_Test(){
            var b = await ControllerDiscovery.Discover();
            System.Console.WriteLine("Systems:");
            foreach (var system in b)
            {
                System.Console.WriteLine(string.Join(",", $"{system.DisplayName},{string.Join("-",system.IPAddresses)}"));
                System.Console.WriteLine(string.Join(",", system.Services));
            }
            
            Assert.IsNotEmpty(b);
        }
    }
}