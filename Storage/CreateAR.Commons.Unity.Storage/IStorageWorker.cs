using System;
using CreateAR.Commons.Unity.Async;
using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    public interface IStorageWorker
    {
        /// <summary>
        /// Called when Delete removes a bucket.
        /// </summary>
        event Action<string> OnDelete;

        /// <summary>
        /// Retrieves all KvModels.
        /// </summary>
        /// <returns></returns>
        IAsyncToken<KvModel[]> GetAll();

        /// <summary>
        /// Creates a new KvModel.
        /// </summary>
        /// <param name="value">The intial value.</param>
        /// <returns></returns>
        IAsyncToken<KvModel> Create(object value);

        /// <summary>
        /// Loads a KV's value.
        /// </summary>
        /// <param name="key">The Key to lookup.</param>
        /// <param name="type">The Type to deserialize the KV's value as.</param>
        /// <returns></returns>
        IAsyncToken<object> Load(string key, Type type);

        /// <summary>
        /// Saves a KV.
        /// </summary>
        /// <param name="key">The key to save under.</param>
        /// <param name="value">The value to save.</param>
        /// <param name="tags">Associated tags.</param>
        /// <param name="version">Version to save with.</param>
        /// <returns></returns>
        IAsyncToken<Void> Save(
            string key,
            object value,
            string tags,
            int version);

        /// <summary>
        /// Deletes a KV.
        /// </summary>
        /// <param name="key">The Key to lookup.</param>
        /// <returns></returns>
        IAsyncToken<Void> Delete(string key);
    }
}