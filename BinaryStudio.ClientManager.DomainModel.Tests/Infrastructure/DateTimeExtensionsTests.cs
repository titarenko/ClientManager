using System;
using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.ClientManager.DomainModel.Tests.Infrastructure
{
    public class DateTimeExtensionsTests
    {
        [Test, TestCaseSource("Should_Return1stDay_WhenCalledStartOfMonth_TestCaseSource")]
        public void Should_Return1stDay_WhenCalledStartOfMonth(DateTime date)
        {
            date.GetStartOfMonth().Day.Should().Be(1);
        }

        [Test, TestCaseSource("Should_ReturnLastDayOfTheMonth_WhenCalledEndOfMonth_TestCaseSource")]
        public void Should_ReturnLastDayOfTheMonth_WhenCalledEndOfMonth(DateTime date, DateTime lastDay)
        {
            date.GetEndOfMonth().Should().Be(lastDay);
        }

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

        [Test, TestCaseSource("Should_ReturnNumberOfWeek_WhenCalledOn_TestCaseSource")]
        public void Should_ReturnNumberOfWeek_WhenCalledOn(DateTime date)
        {
            date.GetWeekNumber().Should().Be(2);
        }

        [Test, TestCaseSource("Should_DefineWeekend_WhenCalledOn_TestCaseSource")]
        public void Should_DefineWeekend_WhenCalledOn(DateTime date, bool isWeekend)
        {
            date.IsWeekend().Should().Be(isWeekend);
        }

        public IEnumerable<TestCaseData> Should_Return1stDay_WhenCalledStartOfMonth_TestCaseSource()
        {
            //all months
            yield return new TestCaseData(new DateTime(2012, 1, 10));
            yield return new TestCaseData(new DateTime(2012, 2, 1));
            yield return new TestCaseData(new DateTime(2012, 3, 19));
            yield return new TestCaseData(new DateTime(2012, 4, 20));
            yield return new TestCaseData(new DateTime(2012, 5, 30));
            yield return new TestCaseData(new DateTime(2012, 6, 11));
            yield return new TestCaseData(new DateTime(2012, 7, 15));
            yield return new TestCaseData(new DateTime(2012, 8, 8));
            yield return new TestCaseData(new DateTime(2012, 9, 4));
            yield return new TestCaseData(new DateTime(2012, 10, 6));
            yield return new TestCaseData(new DateTime(2012, 12, 19));
            yield return new TestCaseData(new DateTime(2012, 12, 24));
        }

        public IEnumerable<TestCaseData> Should_ReturnLastDayOfTheMonth_WhenCalledEndOfMonth_TestCaseSource()
        {
            yield return new TestCaseData(new DateTime(2012, 1, 11),new DateTime(2012, 1, 31));

            //leap year
            yield return new TestCaseData(new DateTime(2012, 2, 22), new DateTime(2012, 2, 29));
            //not leap year
            yield return new TestCaseData(new DateTime(2011, 2, 23), new DateTime(2011, 2, 28));

            yield return new TestCaseData(new DateTime(2012, 3, 19), new DateTime(2012, 3, 31));
            yield return new TestCaseData(new DateTime(2012, 4, 20), new DateTime(2012, 4, 30));
            yield return new TestCaseData(new DateTime(2012, 5, 30),new DateTime(2012, 5, 31));
            yield return new TestCaseData(new DateTime(2012, 6, 11),new DateTime(2012, 6, 30));
            yield return new TestCaseData(new DateTime(2012, 7, 15),new DateTime(2012, 7, 31));
            yield return new TestCaseData(new DateTime(2012, 8, 8),new DateTime(2012, 8, 31));
            yield return new TestCaseData(new DateTime(2012, 9, 4),new DateTime(2012, 9, 30));
            yield return new TestCaseData(new DateTime(2012, 10, 6),new DateTime(2012, 10, 31));
            yield return new TestCaseData(new DateTime(2012, 11, 19),new DateTime(2012, 11, 30));
            yield return new TestCaseData(new DateTime(2012, 12, 24),new DateTime(2012, 12, 31));
        }

        public IEnumerable<TestCaseData> Should_ReturnStartOfWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(new DateTime(2012, 7, 14), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 13), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 10), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 9), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 15), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 16), new DateTime(2012, 7, 16));
            yield return new TestCaseData(new DateTime(2012, 7, 8), new DateTime(2012, 7, 2));
        }

        public IEnumerable<TestCaseData> Should_ReturnStartOfBusinessWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(new DateTime(2012, 7, 14), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 13), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 10), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 9), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 15), new DateTime(2012, 7, 9));
            yield return new TestCaseData(new DateTime(2012, 7, 16), new DateTime(2012, 7, 16));
            yield return new TestCaseData(new DateTime(2012, 7, 8), new DateTime(2012, 7, 2));
        }

        public IEnumerable<TestCaseData> Should_ReturnEndOfWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(new DateTime(2012, 7, 14), new DateTime(2012, 7, 15));
            yield return new TestCaseData(new DateTime(2012, 7, 13), new DateTime(2012, 7, 15));
            yield return new TestCaseData(new DateTime(2012, 7, 10), new DateTime(2012, 7, 15));
            yield return new TestCaseData(new DateTime(2012, 7, 9), new DateTime(2012, 7, 15));
            yield return new TestCaseData(new DateTime(2012, 7, 16), new DateTime(2012, 7, 22));
            yield return new TestCaseData(new DateTime(2012, 7, 8), new DateTime(2012, 7, 8));
        }

        public IEnumerable<TestCaseData> Should_ReturnEndOfBusinessWeek_WhenCalledOn_TestCaseSource()
        {
            // week is 9, 10, ..., 13, 14, 15 (Mon, Tue, ..., Fri, Sat, Sun) of July
            yield return new TestCaseData(new DateTime(2012, 7, 14), new DateTime(2012, 7, 13));
            yield return new TestCaseData(new DateTime(2012, 7, 13), new DateTime(2012, 7, 13));
            yield return new TestCaseData(new DateTime(2012, 7, 10), new DateTime(2012, 7, 13));
            yield return new TestCaseData(new DateTime(2012, 7, 9), new DateTime(2012, 7, 13));
            yield return new TestCaseData(new DateTime(2012, 7, 16), new DateTime(2012, 7, 20));
            yield return new TestCaseData(new DateTime(2012, 7, 8), new DateTime(2012, 7, 6));
        }

        public IEnumerable<TestCaseData> Should_ReturnNumberOfWeek_WhenCalledOn_TestCaseSource()
        {
            yield return new TestCaseData(new DateTime(2012, 1, 2));
            yield return new TestCaseData(new DateTime(2012, 1, 3));
            yield return new TestCaseData(new DateTime(2012, 1, 4));
            yield return new TestCaseData(new DateTime(2012, 1, 5));
            yield return new TestCaseData(new DateTime(2012, 1, 6));
            yield return new TestCaseData(new DateTime(2012, 1, 7));
            yield return new TestCaseData(new DateTime(2012, 1, 8));
        }

        public IEnumerable<TestCaseData> Should_DefineWeekend_WhenCalledOn_TestCaseSource()
        {
            yield return new TestCaseData(new DateTime(2012, 7, 23), false);
            yield return new TestCaseData(new DateTime(2012, 7, 24), false);
            yield return new TestCaseData(new DateTime(2012, 7, 25), false);
            yield return new TestCaseData(new DateTime(2012, 7, 26), false);
            yield return new TestCaseData(new DateTime(2012, 7, 27), false);
            yield return new TestCaseData(new DateTime(2012, 7, 28), true);
            yield return new TestCaseData(new DateTime(2012, 7, 29), true);
            yield return new TestCaseData(new DateTime(2012, 7, 31), false);
            yield return new TestCaseData(new DateTime(2012, 8, 3, 12, 0, 0), false);
        }
    }
}