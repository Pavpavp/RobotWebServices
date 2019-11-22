using NUnit.Framework;
using RWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstrollerSessionTests
{
    [TestFixture]
    class ControllerSessionTests
    {
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
    }
}
