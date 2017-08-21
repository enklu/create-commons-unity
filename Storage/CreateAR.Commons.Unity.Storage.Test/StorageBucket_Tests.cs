using NUnit.Framework;

namespace CreateAR.Commons.Unity.Storage
{
    [TestFixture]
    public class StorageBucket_Tests
    {
        private StorageBucket _bucket;

        [SetUp]
        public void SetUp()
        {
            _bucket = new StorageBucket(
                new StorageService_Tests.SuccessStorageWorker(),
                "TestKey",
                "TestTags",
                1022);
        }

        [Test]
        public void DeleteSuccess()
        {
            var called = false;
            _bucket
                .Delete()
                .OnSuccess(_ => called = true);

            Assert.IsTrue(called);
        }

        [Test]
        public void CreateSuccess()
        {
            var successCalled = false;
            _bucket
                .Delete()
                .OnSuccess(_ => successCalled = true);

            Assert.IsTrue(successCalled);
        }

        [Test]
        public void SaveSuccess()
        {
            var foo = "test1234";

            var successCalled = true;

            _bucket
                .Save(new TestClass
                {
                    Foo = foo
                })
                .OnSuccess(_ =>
                {
                    successCalled = true;
                });

            Assert.IsTrue(successCalled);
        }

        [Test]
        public void SaveTagsSuccess()
        {
            var foo = "test1234";

            var successCalled = true;

            _bucket.Tags = "newtags";
            _bucket
                .Save()
                .OnSuccess(_ =>
                {
                    successCalled = true;

                    Assert.AreEqual("newtags", _bucket.Tags);
                });

            Assert.IsTrue(successCalled);
        }
    }
}