using System;
using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using FizzWare.NBuilder.Dates;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Infrastructure
{
    public class DateTimeExtensionsTests
    {
        [Test, TestCaseSource("Should_ReturnStartOfWeek_WhenCalledOn_TestCaseSource")]
        public void Should_ReturnStartOfWeek_WhenCalledOn(DateTime date, DateTime startOfWeek)
        {
            date.GetStartOfWeek().Should().Be(startOfWeek);
        }

        [Test, TestCaseSource("Should_ReturnStartOfBusinessWeek_WhenCalledOn_TestCaseSource")]
        public void Should_ReturnStartOfBusinessWeek_WhenCalledOn(DateTime date, DateTime startOfWeek)
        {
            date.GetStartOfBusinessWeek().Should().Be(startOfWeek);
        }

        [Test, TestCaseSource("Should_ReturnEndOfWeek_WhenCalledOn_TestCaseSource")]
        public void Should_ReturnEndOfWeek_WhenCalledOn(DateTime date, DateTime endOfWeek)
        {
            date.GetEndOfWeek().Should().Be(endOfWeek);
        }

        [Test, TestCaseSource("Should_ReturnEndOfBusinessWeek_WhenCalledOn_TestCaseSource")]
        public void Should_ReturnEndOfBusinessWeek_WhenCalledOn(DateTime date, DateTime endOfWeek)
        {
            date.GetEndOfBusinessWeek().Should().Be(endOfWeek);
        }

        public IEnumerable<TestCaseData> Should_ReturnStartOfWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(July.The14th, July.The9th);
            yield return new TestCaseData(July.The13th, July.The9th);
            yield return new TestCaseData(July.The10th, July.The9th);
            yield return new TestCaseData(July.The9th, July.The9th);
            yield return new TestCaseData(July.The15th, July.The9th);
            yield return new TestCaseData(July.The16th, July.The16th);
            yield return new TestCaseData(July.The8th, July.The2nd);
        }

        public IEnumerable<TestCaseData> Should_ReturnStartOfBusinessWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(July.The14th, July.The9th);
            yield return new TestCaseData(July.The13th, July.The9th);
            yield return new TestCaseData(July.The10th, July.The9th);
            yield return new TestCaseData(July.The9th, July.The9th);
            yield return new TestCaseData(July.The15th, July.The9th);
            yield return new TestCaseData(July.The16th, July.The16th);
            yield return new TestCaseData(July.The8th, July.The2nd);
        }

        public IEnumerable<TestCaseData> Should_ReturnEndOfWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(July.The14th, July.The15th);
            yield return new TestCaseData(July.The13th, July.The15th);
            yield return new TestCaseData(July.The10th, July.The15th);
            yield return new TestCaseData(July.The9th, July.The15th);
            yield return new TestCaseData(July.The16th, July.The22nd);
            yield return new TestCaseData(July.The8th, July.The8th);
        }

        public IEnumerable<TestCaseData> Should_ReturnEndOfBusinessWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(July.The14th, July.The13th);
            yield return new TestCaseData(July.The13th, July.The13th);
            yield return new TestCaseData(July.The10th, July.The13th);
            yield return new TestCaseData(July.The9th, July.The13th);
            yield return new TestCaseData(July.The16th, July.The20th);
            yield return new TestCaseData(July.The8th, July.The6th);
        }
    }
}