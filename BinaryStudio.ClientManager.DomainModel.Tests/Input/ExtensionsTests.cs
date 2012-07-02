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
        public void Shoud_ReturnRelatedMailsDotSender_WhenXDotRelatedMails0DotSender()
        {
            Expression<Func<Person, Person>> expression = x => x.RelatedMails[0].Sender;
            var e = expression.GetPath();
            Assert.AreEqual("RelatedMails.Sender", e);
        }

        [Test]
        public void Shoud_ReturnSourceDotSenderDotLastName_WhgenXDotSourceDotSenderDotLastName()
        {
            Expression<Func<Inquiry, string>> expression = x => x.Source.Sender.LastName;
            var e = expression.GetPath();
            Assert.AreEqual("Source.Sender.LastName", e);
        }
    }
}
