using System;
using CreateAR.Commons.Unity.Async;
using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    public interface IStorageWorker
    {
        IAsyncToken<KvModel[]> GetAll();
        IAsyncToken<KvModel> Create(object value);
        IAsyncToken<object> Load(string key, Type type);
        IAsyncToken<Void> Save(
            string key,
            object value,
            string tags,
            int version);
        IAsyncToken<Void> Delete(string key);
    }
}