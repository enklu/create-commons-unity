using System;
using System.Collections.Generic;
using CreateAR.Commons.Unity.Async;
using NUnit.Framework;
using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    [TestFixture]
    public class StorageService_Tests
    {
        private static readonly KvModel Old = new KvModel
        {
            key = "this is a key",
            owner = "me",
            tags = "a,b,c",
            version = 10
        };
        
        private static readonly KvModel New = new KvModel
        {
            key = "new",
            owner = "me",
            tags = "a",
            version = 0
        };
        
        public class SuccessStorageWorker : IStorageWorker
        {
            private readonly List<KvModel> _models = new List<KvModel>();

            public SuccessStorageWorker()
            {
                _models.Add(Old);
            }

            public event Action<string> OnDelete;

            public IAsyncToken<KvModel[]> GetAll()
            {
                return new AsyncToken<KvModel[]>(_models.ToArray());
            }

            public IAsyncToken<KvModel> Create(object value)
            {
                _models.Add(New);

                return new AsyncToken<KvModel>(New);
            }

            public IAsyncToken<object> Load(string key, Type type)
            {
                return new AsyncToken<object>(new TestClass());
            }

            public IAsyncToken<Void> Save(string key, object value, string tags, int version)
            {
                return new AsyncToken<Void>(Void.Instance);
            }

            public IAsyncToken<Void> Delete(string key)
            {
                OnDelete?.Invoke(key);

                return new AsyncToken<Void>(Void.Instance);
            }
        }

        private StorageService _loadedService;

        [SetUp]
        public void SetUp()
        {
            _loadedService = new StorageService(new SuccessStorageWorker());
            _loadedService.Refresh();
        }

        [Test]
        public void Refresh()
        {
            var service = new StorageService(new SuccessStorageWorker());
            var successCalled = true;

            service
                .Refresh()
                .OnSuccess(_ =>
                {
                    var all = service.All;
                    Assert.IsTrue(all.Length == 1);
                    Assert.AreSame(Old.key, all[0].Key);
                });

            Assert.IsTrue(successCalled);
        }

        [Test]
        public void RefreshError()
        {
            var service = new StorageService(new FailureStorageWorker());
            var failureCalled = true;

            service
                .Refresh()
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            Assert.IsTrue(failureCalled);
        }

        [Test]
        public void Get()
        {
            Assert.AreSame(Old.key, _loadedService.Get(Old.key).Key);
            Assert.IsNull(_loadedService.Get(New.key));
        }

        [Test]
        public void Find()
        {
            Assert.AreSame(Old.key, _loadedService.FindOne("a").Key);

            _loadedService.Create(New);

            Assert.AreSame(Old.key, _loadedService.FindOne("a").Key);
        }

        [Test]
        public void FindAll()
        {
            _loadedService.Create(New);

            var results = _loadedService.FindAll("a");
            Assert.AreEqual(2, results.Length);
        }

        [Test]
        public void Create()
        {
            var successCalled = false;

            _loadedService
                .Create(new TestClass())
                .OnSuccess(bucket =>
                {
                    successCalled = true;

                    Assert.AreEqual(New.key, bucket.Key);
                    Assert.AreEqual(New.tags, bucket.Tags);
                });

            Assert.IsTrue(successCalled);
        }

        [Test]
        public void CreateFail()
        {
            var failureCalled = false;

            new StorageService(new FailureStorageWorker())
                .Create(New)
                .OnFailure(_ =>
                {
                    failureCalled = true;
                });

            Assert.IsTrue(failureCalled);
        }
    }
}