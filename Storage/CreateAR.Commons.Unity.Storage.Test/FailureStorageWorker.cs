using System;
using CreateAR.Commons.Unity.Async;
using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    public class FailureStorageWorker : IStorageWorker
    {
        public event Action<string> OnDelete;

        public IAsyncToken<KvModel[]> GetAll()
        {
            return new AsyncToken<KvModel[]>(new Exception());
        }

        public IAsyncToken<KvModel> Create(object value)
        {
            return new AsyncToken<KvModel>(new Exception());
        }

        public IAsyncToken<object> Load(string key, Type type)
        {
            return new AsyncToken<object>(new Exception());
        }

        public IAsyncToken<Void> Save(string key, object value, string tags, int version)
        {
            return new AsyncToken<Void>(Void.Instance);
        }

        public IAsyncToken<Void> Delete(string key)
        {
            return new AsyncToken<Void>(new Exception());
        }
    }
}