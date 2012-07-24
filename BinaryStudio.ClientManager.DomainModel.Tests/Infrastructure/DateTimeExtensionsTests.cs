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
            date.WeekNumber().Should().Be(2);
        }

        public IEnumerable<TestCaseData> Should_Return1stDay_WhenCalledStartOfMonth_TestCaseSource()
        {
            //all monthes
            yield return new TestCaseData(January.The10th);
            yield return new TestCaseData(February.The1st);
            yield return new TestCaseData(March.The19th);
            yield return new TestCaseData(April.The20th);
            yield return new TestCaseData(May.The30th);
            yield return new TestCaseData(June.The11th);
            yield return new TestCaseData(July.The15th);
            yield return new TestCaseData(August.The8th);
            yield return new TestCaseData(September.The4th);
            yield return new TestCaseData(October.The6th);
            yield return new TestCaseData(November.The19th);
            yield return new TestCaseData(December.The24th);
        }

        public IEnumerable<TestCaseData> Should_ReturnLastDayOfTheMonth_WhenCalledEndOfMonth_TestCaseSource()
        {
            yield return new TestCaseData(January.The11th,January.The31st);

            //leap year
            yield return new TestCaseData(new DateTime(2012, 2, 22), new DateTime(2012, 2, 29));
            //not leap year
            yield return new TestCaseData(new DateTime(2011, 2, 23), new DateTime(2011, 2, 28));

            yield return new TestCaseData(March.The19th, March.The31st);
            yield return new TestCaseData(April.The20th, April.The30th);
            yield return new TestCaseData(May.The30th,May.The31st);
            yield return new TestCaseData(June.The11th,June.The30th);
            yield return new TestCaseData(July.The15th,July.The31st);
            yield return new TestCaseData(August.The8th,August.The31st);
            yield return new TestCaseData(September.The4th,September.The30th);
            yield return new TestCaseData(October.The6th,October.The31st);
            yield return new TestCaseData(November.The19th,November.The30th);
            yield return new TestCaseData(December.The24th,December.The31st);
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

        public IEnumerable<TestCaseData> Should_ReturnNumberOfWeek_WhenCalledOn_TestCaseSource()
        {
            yield return new TestCaseData(January.The2nd);
            yield return new TestCaseData(January.The3rd);
            yield return new TestCaseData(January.The4th);
            yield return new TestCaseData(January.The5th);
            yield return new TestCaseData(January.The6th);
            yield return new TestCaseData(January.The7th);
            yield return new TestCaseData(January.The8th);
        }
    }
}