﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperServiceTests
    {
        private HousekeeperHelperService _service;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private DateTime _statementDate = new DateTime(2017, 1, 1);
        private Housekeeper _housekeeper;
        private string _statementFileName ;

        [SetUp]
        public void Setup()
        {
            _housekeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c"};
            
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {
                _housekeeper
            }.AsQueryable());

            _statementFileName = "fileName";
            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator
                .Setup(sg => 
                    sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statementDate)))
                .Returns(() => _statementFileName);
            
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();

            _service = new HousekeeperHelperService(
                unitOfWork.Object,
                _statementGenerator.Object,
                _emailSender.Object,
                _messageBox.Object);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            
            _service.SendStatementEmails(_statementDate);
            
            VerifyEmailSent();
        }
        
        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsNull_ShouldNotGenerateStatement()
        {
            _housekeeper.Email = null;
            
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statementDate)),
                Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_HouseKeepersEmailWhitespace_ShouldNotGenerateStatement()
        {
            _housekeeper.Email = " ";
            
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statementDate)),
                Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsEmpty_ShouldNotGenerateStatement()
        {
            _housekeeper.Email = "";
            
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statementDate)),
                Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement()
        {
            _statementGenerator
                .Setup(sg => 
                    sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, (_statementDate)))
                .Returns(_statementFileName);
            
            _service.SendStatementEmails(_statementDate);
            
            VerifyEmailSent();
        }

       

        [Test]
        public void SendStatementEmails_StatementFileNameIsNull_ShouldNotEmailTheStatement()
        {
            _statementFileName = null;
            
            _service.SendStatementEmails(_statementDate);
            
            VerifyEmailNotSent();
        }
        
        [Test]
        public void SendStatementEmails_StatementFileNameIsEmptyString_ShouldNotEmailTheStatement()
        {
            _statementFileName = "";
            
            _service.SendStatementEmails(_statementDate);
            
            VerifyEmailNotSent();
        }

        

        [Test]
        public void SendStatementEmails_StatementFileNameIsWhitespace_ShouldNotEmailTheStatement()
        {
            _statementFileName = " ";
            
            _service.SendStatementEmails(_statementDate);
            
            VerifyEmailNotSent();
        }
        
        private void VerifyEmailNotSent()
        {
            _emailSender.Verify(es => es.EmailFile(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }
        
        private void VerifyEmailSent()
        {
            _emailSender.Verify(es => es.EmailFile(
                _housekeeper.Email,
                _housekeeper.StatementEmailBody,
                _statementFileName,
                It.IsAny<string>()));
        }
        
    }
}