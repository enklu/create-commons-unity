using CreateAR.Commons.Unity.Async;

namespace CreateAR.Commons.Unity.Storage
{
    /// <summary>
    /// Describes a service for manipulating storage buckets.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Retrieves all StorageBuckets.
        /// </summary>
        StorageBucket[] All { get; }

        /// <summary>
        /// Refreshes manifest of StorageBuckets.
        /// </summary>
        /// <returns></returns>
        IAsyncToken<StorageService> Refresh();

        /// <summary>
        /// Creates a new bucket.
        /// </summary>
        /// <typeparam name="T">The type of object to save.</typeparam>
        /// <param name="value">The value to save.</param>
        /// <returns></returns>
        IAsyncToken<StorageBucket> Create<T>(T value);

        /// <summary>
        /// Retrieves the StorageBucket for a particular key.
        /// </summary>
        /// <param name="key">The key of the bucket to lookup.</param>
        /// <returns></returns>
        StorageBucket Get(string key);

        /// <summary>
        /// Searches buckets for the first one with a tag that includes this
        /// tag as a substring.
        /// </summary>
        /// <param name="tag">Tag to serch for.</param>
        /// <returns></returns>
        StorageBucket FindOne(string tag);

        /// <summary>
        /// Retrieves all buckets with a matching tag.
        /// </summary>
        /// <param name="tag">The tag to lookup.</param>
        /// <returns></returns>
        StorageBucket[] FindAll(string tag);
    }
}