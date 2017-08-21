using CreateAR.Commons.Unity.Async;

namespace CreateAR.Commons.Unity.Storage
{
    public interface IStorageService
    {
        StorageBucket[] All { get; }
        IAsyncToken<StorageService> Refresh();
        IAsyncToken<StorageBucket> Create<T>(T value);
        StorageBucket Get(string key);
        StorageBucket FindOne(string tags);
        StorageBucket[] FindAll(string tag);
    }
}