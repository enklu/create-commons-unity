using System;
using CreateAR.Commons.Unity.Async;
using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    /// <summary>
    /// Represents a key-value storage element.
    /// </summary>
    public sealed class StorageBucket
    {
        /// <summary>
        /// Object that performs requests.
        /// </summary>
        private readonly IStorageWorker _worker;

        /// <summary>
        /// The version we know about.
        /// </summary>
        private int _version;

        /// <summary>
        /// The latest version the StorageService knows about.
        /// </summary>
        private int _manifestVersion;

        /// <summary>
        /// The loaded value.
        /// </summary>
        private object _value;
        
        /// <summary>
        /// The unique key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Tags associated with this bucket.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Creates a new bucket.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="key">Unique key.</param>
        /// <param name="tags">Associated tags.</param>
        /// <param name="version">Version of this bucket.</param>
        public StorageBucket(
            IStorageWorker worker,
            string key,
            string tags,
            int version)
        {
            _worker = worker;
            _version = _manifestVersion = version;

            Key = key;
            Tags = tags;
        }

        /// <summary>
        /// Loads the bucket's value. Subsequent Value() calls will be cached.
        /// 
        /// If StorageService::Refresh returns a version higher than the cached
        /// version, the next call to Value() will fetch the updated value.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns></returns>
        public IAsyncToken<T> Value<T>()
        {
            var token = new AsyncToken<T>();

            if (_version == _manifestVersion && null != _value)
            {
                token.Succeed((T) _value);
            }
            else
            {
                _worker
                    .Load(Key, typeof(T))
                    .OnSuccess(response =>
                    {
                        _value = (T)response;

                        token.Succeed((T)_value);
                    })
                    .OnFailure(exception => token.Fail(exception));
            }

            return token;
        }

        /// <summary>
        /// Saves the bucket, automatically updating the version.
        /// 
        /// If a call to StorageService::Refresh() found a newer version, this
        /// call will fail.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the value to.</typeparam>
        /// <param name="value">The value to save.</param>
        /// <returns></returns>
        public IAsyncToken<StorageBucket> Save<T>(T value)
        {
            var token = new AsyncToken<StorageBucket>();

            if (_manifestVersion != _version)
            {
                token.Fail(new Exception("Cannot save without version update."));
            }
            else
            {
                var newVersion = _version + 1;
                _worker
                    .Save(Key, value, Tags, newVersion)
                    .OnSuccess(_ =>
                    {
                        _value = value;
                        _version = newVersion;
                        
                        token.Succeed(this);
                    })
                    .OnFailure(token.Fail);
            }

            return token;
        }

        /// <summary>
        /// Saves current value. Useful when just updating Tags.
        /// </summary>
        /// <returns></returns>
        public IAsyncToken<StorageBucket> Save()
        {
            return Save(_value);
        }

        /// <summary>
        /// Deletes the KV. Disallows delete if latest version has not been
        /// loaded.
        /// </summary>
        /// <returns></returns>
        public IAsyncToken<Void> Delete()
        {
            var token = new AsyncToken<Void>();

            if (_manifestVersion != _version)
            {
                token.Fail(new Exception("Cannot delete without version update."));
            }
            else
            {
                _worker
                    .Delete(Key)
                    .OnSuccess(_ =>
                    {
                        // TODO: To think about: we have to trust the worker to
                        // TODO: tell the service.

                        token.Succeed(Void.Instance);
                    })
                    .OnFailure(token.Fail);
            }

            return token;
        }

        /// <summary>
        /// Called by StorageService when a later version has been found.
        /// </summary>
        /// <param name="manifestVersion"></param>
        internal void VersionUpdate(int manifestVersion)
        {
            _manifestVersion = manifestVersion;
        }
    }
}