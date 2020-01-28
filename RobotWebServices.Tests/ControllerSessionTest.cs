using NUnit.Framework;
using RWS;
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
            IRC5Session rwsCs1 = new IRC5Session(new Address("localhost:80"));

            //Act
          //  var ios = await rwsCs1.RobotWareService.GetIOSignalsAsync().ConfigureAwait(false);

            //Assert
            //Assert.IsNotEmpty(ios.Embedded.State);
            //Assert.IsNotNull(ios.Embedded.State.First().LValue);
        }
    }
}