using NUnit.Framework;
using RWS;
using RWS.Data;
using System.Threading.Tasks;

namespace RobotWebServices.Tests
{

    [TestFixture]
    class ControllerSessionTests
    {

        [Test]
        public async Task GetIOSignals_Localhost_HasValue()
        {
            //Arrange
            ControllerSession rwsCs1 = new ControllerSession(new Address("localhost:80"));

            //Act
            var ios = await rwsCs1.RobotWareService.GetIOSignalsAsync().ConfigureAwait(false);

            //Assert
            //Assert.IsNotEmpty(ios.Embedded.State);
            //Assert.IsNotNull(ios.Embedded.State.First().LValue);
        }
    }
}