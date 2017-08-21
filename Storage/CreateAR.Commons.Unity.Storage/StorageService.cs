using System;
using System.Collections.Generic;
using CreateAR.Commons.Unity.Async;

namespace CreateAR.Commons.Unity.Storage
{
    /// <summary>
    /// Service for getting, saving, and querying a user's key-value storage
    /// buckets.
    /// </summary>
    public sealed class StorageService : IStorageService
    {
        /// <summary>
        /// Object that makes calls.
        /// </summary>
        private readonly IStorageWorker _worker;

        /// <summary>
        /// Token used during refresh.
        /// </summary>
        private AsyncToken<StorageService> _refreshToken;

        /// <summary>
        /// The list of buckets this user owns.
        /// </summary>
        private readonly List<StorageBucket> _buckets = new List<StorageBucket>();

        /// <summary>
        /// Retrieves all buckets.
        /// </summary>
        public StorageBucket[] All => _buckets.ToArray();

        /// <summary>
        /// Creates a new StorageService.
        /// </summary>
        /// <param name="worker">The worker to use. Generally StorageWorker.</param>
        public StorageService(IStorageWorker worker)
        {
            _worker = worker;
            _worker.OnDelete += Worker_OnDelete;
        }

        /// <summary>
        /// Refreshes the local index.
        /// </summary>
        /// <returns></returns>
        public IAsyncToken<StorageService> Refresh()
        {
            if (null != _refreshToken)
            {
                return _refreshToken.Token();
            }

            // keep local reference
            var token = _refreshToken = new AsyncToken<StorageService>();

            _worker
                .GetAll()
                .OnSuccess(models =>
                {
                    for (int i = 0, len = models.Length; i < len; i++)
                    {
                        var model = models[i];

                        var bucket = Get(model.key);
                        if (null == bucket)
                        {
                            bucket = new StorageBucket(
                                null,
                                model.key,
                                model.tags,
                                model.version);
                            _buckets.Add(bucket);
                        }

                        bucket.VersionUpdate(model.version);
                    }
                })
                .OnFailure(exception =>
                {
                    // null out class reference first
                    _refreshToken = null;

                    token.Fail(exception);
                });

            return token;
        }

        /// <summary>
        /// Creates a new bucket.
        /// </summary>
        /// <typeparam name="T">The type of object to save.</typeparam>
        /// <param name="value">The value to save.</param>
        /// <returns></returns>
        public IAsyncToken<StorageBucket> Create<T>(T value)
        {
            var token = new AsyncToken<StorageBucket>();

            _worker
                .Create(value)
                .OnSuccess(model =>
                {
                    var bucket = new StorageBucket(
                        _worker,
                        model.key,
                        model.tags,
                        model.version);
                    _buckets.Add(bucket);

                    token.Succeed(bucket);
                })
                .OnFailure(token.Fail);

            return token;
        }

        /// <summary>
        /// Retrieves the StorageBucket for a particular key.
        /// </summary>
        /// <param name="key">The key of the bucket to lookup.</param>
        /// <returns></returns>
        public StorageBucket Get(string key)
        {
            for (int i = 0, len = _buckets.Count; i < len; i++)
            {
                var bucket = _buckets[i];
                if (bucket.Key == key)
                {
                    return bucket;
                }
            }

            return null;
        }

        /// <summary>
        /// Searches buckets for the first one with a tag that includes this
        /// tag as a substring.
        /// </summary>
        /// <param name="tag">Tag to serch for.</param>
        /// <returns></returns>
        public StorageBucket FindOne(string tag)
        {
            for (int i = 0, len = _buckets.Count; i < len; i++)
            {
                var bucket = _buckets[i];
                if (bucket.Tags.Contains(tag))
                {
                    return bucket;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves all buckets with a matching tag.
        /// </summary>
        /// <param name="tag">The tag to lookup.</param>
        /// <returns></returns>
        public StorageBucket[] FindAll(string tag)
        {
            var buckets = new List<StorageBucket>();
            for (int i = 0, len = _buckets.Count; i < len; i++)
            {
                var bucket = _buckets[i];
                if (bucket.Tags.Contains(tag))
                {
                    buckets.Add(bucket);
                }
            }

            return buckets.ToArray();
        }

        /// <summary>
        /// Called when a bucket has been deleted.
        /// </summary>
        /// <param name="key">The key of the bucket.</param>
        private void Worker_OnDelete(string key)
        {
            for (int i = 0, len = _buckets.Count; i < len; i++)
            {
                if (_buckets[i].Key == key)
                {
                    _buckets.RemoveAt(i);

                    return;
                }
            }
        }
    }
}