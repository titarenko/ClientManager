using System;
using System.Linq.Expressions;
using BinaryStudio.ClientManager.DomainModel.Entities;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Input
{
    [TestFixture]
    class ExtensionsTests
    {
        [Test]
        public void Shoud_ReturnSourceDotSender_WhenXDotSourceDotSenderExpressionIsPassed()
        {
            Expression<Func<Inquiry, Person>> expression = x => x.Source.Sender;
            var e = expression.GetPath();
            Assert.AreEqual("Source.Sender", e);
        }

        [Test]
        public void Shoud_ReturnSource_WhenXDotSourceExpressionIsPassed()
        {
            Expression<Func<Inquiry, MailMessage>> expression = x => x.Source;
            var e = expression.GetPath();
            Assert.AreEqual("Source", e);
        }

        [Test]
        public void Shoud_ReturnSourceDotSenderDotLastName_WhenXDotSourceDotSenderDotLastName()
        {
            Expression<Func<Inquiry, string>> expression = x => x.Source.Sender.LastName;
            var e = expression.GetPath();
            Assert.AreEqual("Source.Sender.LastName", e);
        }
    }
}
