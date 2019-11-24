using NUnit.Framework;
using RWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.Tests
{
    [TestFixture]
    class ControllerSessionTests
    {
        //Requires a running VC
        [Test]
        public async void GetIOSignals_Localhost_HasValue()
        {
            //Arrange
            ControllerSession rwsCs1 = new ControllerSession("localhost");

            //Act
            var ios = await rwsCs1.RobotWareService.GetIOSignals().ConfigureAwait(false);

            //Assert
            Assert.IsNotEmpty(ios.Embedded.State);
            Assert.IsNotNull(ios.Embedded.State.First().LValue);
        }
    }
}
