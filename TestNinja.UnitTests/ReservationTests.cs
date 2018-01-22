using System;

using NUnit.Framework;
using TestNinja.Fundamentals;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void CanBeCancelledby_UserIsAdmin_ReturnsTrue()
        {
            // Arramge
            var reservation = new Reservation();
            
            // Act
            var result = reservation.CanBeCancelledBy(new User {IsAdmin = true});

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void CanBeCancelledBy_SameUserCancellingTheReservation_ReturnTrue()
        {
            var user = new User();
            //Arrange
            var reservation = new Reservation { MadeBy = user};

            // Act
            var result = reservation.CanBeCancelledBy(user);

            // Assert
            Assert.IsTrue(result);
            //Assert.That(result, Is.True);
            //Assert.That(result == true);
        }

        [Test]
        public void CanBeCancelledBy_AnotherUserCancellingReservation_ReturnFalse()
        {
            //Arrange
            var reservation = new Reservation { MadeBy = new User()};

            // Act
            var result = reservation.CanBeCancelledBy(new User());

            // Act
            Assert.IsFalse(result);

        }
    }
}
