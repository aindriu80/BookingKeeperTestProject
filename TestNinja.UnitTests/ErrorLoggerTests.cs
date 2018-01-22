using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ErrorLoggerTests
    {
        [Test]
        public void Log_WhenCalled_SetTheLastErrorProperty()
        {
            var logger = new ErrorLogger();
            
            logger.Log("a");
            
            Assert.That(logger.LastError, Is.EqualTo("a"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Log__InvalidError_ThrowArgumentNullException(string error)
        {
            var logger = new ErrorLogger();
            
//            logger.Log(error);
            Assert.That(() =>logger.Log(error), Throws.ArgumentNullException);
//            Assert.That(() =>logger.Log((error), Throws.Exception.TypeOf<DivideByZeroException>()) );
            
        }

        [Test]
        public void Log_ValidError_RaiseErrorLoggedEvent()
        {
            var logger = new ErrorLogger();

            var id = Guid.Empty;
            logger.ErrorLogged += (sender, args) => { id = args; };
            
            logger.Log("a");
            
            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        }

//        [Test]
//        public void OnErrorLogged_WhenCalled_RaiseEvent()
//        {
//            var logger = new ErrorLogger();
//            
//            logger.OnErrorLogged();
//            
//            Assert.That(true);
//        }
    }
}