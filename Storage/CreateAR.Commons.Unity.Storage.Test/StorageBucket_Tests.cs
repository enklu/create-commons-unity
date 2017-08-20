using System;
using CreateAR.Commons.Unity.Async;
using NUnit.Framework;
using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    public class ExceptionStorageWorker : IStorageWorker
    {
        public IAsyncToken<StorageWorkerResponse<T>> Load<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IAsyncToken<StorageWorkerResponse<T>> Save<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public IAsyncToken<Void> Delete<T>(string key)
        {
            throw new NotImplementedException();
        }
    }

    public class SuccessStorageWorker : IStorageWorker
    {
        private readonly Func<object> _factory;

        public SuccessStorageWorker(Func<object> factory)
        {
            _factory = factory;
        }

        public IAsyncToken<StorageWorkerResponse<T>> Load<T>(string key)
        {
            return new AsyncToken<StorageWorkerResponse<T>>(
                new StorageWorkerResponse<T>
                {
                    Value = (T) _factory(),
                    Version = 12
                });
        }

        public IAsyncToken<StorageWorkerResponse<T>> Save<T>(string key, T value)
        {
            return new AsyncToken<StorageWorkerResponse<T>>(
                new StorageWorkerResponse<T>
                {
                    Value = value
                });
        }

        public IAsyncToken<Void> Delete<T>(string key)
        {
            return new AsyncToken<Void>(Void.Instance);
        }
    }

    public class FailureStorageWorker : IStorageWorker
    {
        public IAsyncToken<StorageWorkerResponse<T>> Load<T>(string key)
        {
            return new AsyncToken<StorageWorkerResponse<T>>(
                new Exception("Could not load."));
        }

        public IAsyncToken<StorageWorkerResponse<T>> Save<T>(string key, T value)
        {
            return new AsyncToken<StorageWorkerResponse<T>>(
                new Exception("Could not save."));
        }

        public IAsyncToken<Void> Delete<T>(string key)
        {
            return new AsyncToken<Void>(
                new Exception("Could not delete."));
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
        
        [Test]
        public void LoadValueSuccess()
        {
            // Arrange
            var successCalled = false;
            var failureCalled = false;

            var bucket = new StorageBucket<TestClass>(
                new SuccessStorageWorker(() => new TestClass()), 
                _key,
                _version);

            // Act
            bucket
                .Value()
                .OnSuccess(value =>
                {
                    successCalled = true;
                })
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            // Assert
            Assert.IsTrue(successCalled);
            Assert.IsFalse(failureCalled);
        }

        [Test]
        public void SaveValueSuccess()
        {
            // Arrange
            var successCalled = false;
            var failureCalled = false;
            var foo = "{\"A\":\"This is a test\"}";

            var bucket = new StorageBucket<TestClass>(
                new SuccessStorageWorker(() => new TestClass()),
                _key,
                _version);

            // Act
            bucket
                .Save(new TestClass
                {
                    Foo = foo
                })
                .OnSuccess(value =>
                {
                    successCalled = true;
                })
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            // Assert
            Assert.IsTrue(successCalled);
            Assert.IsFalse(failureCalled);
        }

        [Test]
        public void DeleteValueSuccess()
        {
            // Arrange
            var successCalled = false;
            var failureCalled = false;

            var bucket = new StorageBucket<TestClass>(
                new SuccessStorageWorker(() => new TestClass()),
                _key,
                _version);

            // Act
            bucket
                .Delete()
                .OnSuccess(value =>
                {
                    successCalled = true;
                })
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            // Assert
            Assert.IsTrue(successCalled);
            Assert.IsFalse(failureCalled);
        }

        [Test]
        public void LoadValueFailure()
        {
            // Arrange
            var successCalled = false;
            var failureCalled = false;

            var bucket = new StorageBucket<TestClass>(
                new FailureStorageWorker(),
                _key,
                _version);

            // Act
            bucket
                .Value()
                .OnSuccess(value =>
                {
                    successCalled = true;
                })
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            // Assert
            Assert.IsFalse(successCalled);
            Assert.IsTrue(failureCalled);
        }

        [Test]
        public void SaveValueFailure()
        {
            // Arrange
            var successCalled = false;
            var failureCalled = false;
            var foo = "{\"A\":\"This is a test\"}";

            var bucket = new StorageBucket<TestClass>(
                new FailureStorageWorker(),
                _key,
                _version);

            // Act
            bucket
                .Save(new TestClass
                {
                    Foo = foo
                })
                .OnSuccess(value =>
                {
                    successCalled = true;
                })
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            // Assert
            Assert.IsFalse(successCalled);
            Assert.IsTrue(failureCalled);
        }

        [Test]
        public void DeleteValueFailure()
        {
            // Arrange
            var successCalled = false;
            var failureCalled = false;

            var bucket = new StorageBucket<TestClass>(
                new FailureStorageWorker(),
                _key,
                _version);

            // Act
            bucket
                .Delete()
                .OnSuccess(value =>
                {
                    successCalled = true;
                })
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            // Assert
            Assert.IsFalse(successCalled);
            Assert.IsTrue(failureCalled);
        }
    }
}