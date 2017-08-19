using System;
using CreateAR.Commons.Unity.Async;
using NUnit.Framework;

namespace CreateAR.Commons.Unity.Storage
{
    public class DummyStorageWorker : IStorageWorker
    {
        public IAsyncToken<StorageWorkerResponse<T>> Load<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IAsyncToken<StorageWorkerResponse<T>> Save<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public IAsyncToken<T> Delete<T>(string key)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class StorageBucket_Tests
    {
        public class TestClass
        {
            public string Foo;
        }

        private readonly string _key = "this is a test key";
        private readonly int _version = 21187;
        private readonly TestClass _value = new TestClass
        {
            Foo = Guid.NewGuid().ToString()
        };

        private StorageBucket<TestClass> _bucket;

        public void Setup()
        {
            _bucket = new StorageBucket<TestClass>(
                new DummyStorageWorker(),
                _key,
                _version);
        }

        [Test]
        public void LoadValueSuccess()
        {
            var successCalled = false;
            var failureCalled = false;

            _bucket
                .Value()
                .OnSuccess(value =>
                {
                    successCalled = true;

                    Assert.Equals(_value.Foo, value.Foo);
                })
                .OnFailure(_ =>
                {
                    failureCalled = false;
                });

            Assert.IsTrue(successCalled);
            Assert.IsFalse(failureCalled);
        }
    }
}