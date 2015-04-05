using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Textant.Logic.Tests.Tests
{
    /// <summary>
    /// Tests XML serialization of <see cref="OccurencyCounter{T}"/> in <see cref="OccurencyCounterTools"/>
    /// </summary>
    [TestClass]
    public class OccurencyCounterSerializationTest
    {
        [TestMethod]
        public void TestSerialization()
        {
            const string filename = @"c:\test.xml";
            var occurencyCounter = CreateSampleOccurencyCounter();
            occurencyCounter.WriteToXml(filename);
            var deserializedOccurencyCounter = OccurencyCounterTools.ReadFromXml<int>(filename);
            Assert.IsTrue(deserializedOccurencyCounter.Total == occurencyCounter.Total);
            Assert.IsTrue(deserializedOccurencyCounter.Count == occurencyCounter.Count);
            foreach (var item in occurencyCounter)
            {
                Assert.IsTrue(deserializedOccurencyCounter[item.Key] == item.Value);
            }
        }

        private OccurencyCounter<int> CreateSampleOccurencyCounter()
        {
            var counter = new OccurencyCounter<int>();
            var random = new Random();
            for (int i = 0; i < 10000; i++)
            {
                counter.Add(random.Next(128));
            }
            return counter;
        }
    }
}