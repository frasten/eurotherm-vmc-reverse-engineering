using System.Collections.Generic;
using NUnit.Framework;

namespace VmcReverse.UnitTests
{
    [TestFixture]
    public class Crc16Tests
    {
        /// <summary>
        ///     Workaround in order to use complex objects (not only constant values) inside TestCases.
        ///     See: http://stackoverflow.com/a/4230328/2965109
        /// </summary>
        private static IEnumerable<TestCaseData> CalculateTestCases
        {
            get
            {
                yield return new TestCaseData(new byte[] { 0x02, 0x01, 0x01, 0x00 }, new byte[] { 0x51, 0xCC });
                yield return new TestCaseData(new byte[] { 0x02, 0x01, 0x00, 0x54, 0x00, 0x01 }, new byte[] { 0xBC, 0x29 });
                yield return new TestCaseData(new byte[] { 0x02, 0x06, 0x00, 0x00, 0x00, 0x02 }, new byte[] { 0x08, 0x38 });
                yield return new TestCaseData(new byte[] { 0x02, 0x05, 0x00, 0x03, 0xFF, 0x00 }, new byte[] { 0x7C, 0x09 });
            }
        }

        [TestCaseSource(nameof(CalculateTestCases))]
        public void Calculate_SimpleTests_ShouldMatch(byte[] message, byte[] expected)
        {
            var result = Crc16.Calculate(message);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
