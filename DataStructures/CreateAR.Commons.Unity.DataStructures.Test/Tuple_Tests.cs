using System;
using NUnit.Framework;

namespace CreateAR.Commons.Unity.DataStructures.Test
{
    [TestFixture]
    public class Tuple_Tests
    {
        [Test]
        public void Create()
        {
            var a = 1;
            var b = Guid.NewGuid().ToString();

            var tuple = Tuple.Create(a, b);

            Assert.AreEqual(a, tuple.Item1);
            Assert.AreEqual(b, tuple.Item2);
        }
    }
}
