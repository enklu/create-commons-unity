using System;
using CreateAR.Commons.Unity.Async;

namespace CreateAR.Commons.Unity.Storage
{
    public sealed class StorageBucket<T>
    {
        private readonly IStorageWorker _worker;

        private int _version;
        private int _manifestVersion;

        private T _value;

        private IAsyncToken<T> _loadToken;

        public string Key { get; }

        public StorageBucket(
            IStorageWorker worker,
            string key,
            int version)
        {
            _worker = worker;
            _version = _manifestVersion = version;

            Key = key;
        }

        public IAsyncToken<T> Value()
        {
            var token = new AsyncToken<T>();

            if (_version == _manifestVersion && null != _value)
            {
                token.Succeed(_value);
            }
            else
            {
                Load()
                    .OnSuccess(token.Succeed)
                    .OnFailure(token.Fail);
            }

            return token;
        }

        public IAsyncToken<T> Save(T value)
        {
            var token = new AsyncToken<T>();

            if (_manifestVersion != _version)
            {
                token.Fail(new Exception("Cannot save without version update."));
            }
            else
            {
                _worker
                    .Save(Key, value)
                    .OnSuccess(response =>
                    {
                        _value = response.Value;
                        _version = response.Version;
                        
                        token.Succeed(_value);
                    })
                    .OnFailure(token.Fail);
            }

            return token;
        }

        public IAsyncToken<T> Delete()
        {
            var token = new AsyncToken<T>();

            if (_manifestVersion != _version)
            {
                token.Fail(new Exception("Cannot delete without version update."));
            }
            else
            {
                _worker
                    .Delete<T>(Key)
                    .OnSuccess(_ =>
                    {
                        // TODO: To think about: we're trusting the worker to
                        // TODO: tell the manifest

                        token.Succeed(_value);
                    })
                    .OnFailure(token.Fail);
            }

            return token;
        }

        internal void VersionUpdate(int manifestVersion)
        {
            _manifestVersion = manifestVersion;
        }

        private IAsyncToken<T> Load()
        {
            if (null != _loadToken)
            {
                return _loadToken;
            }

            var token = new AsyncToken<T>();

            _worker
                .Load<T>(Key)
                .OnSuccess(response =>
                {
                    _loadToken = null;

                    _version = response.Version;
                    _value = response.Value;

                    token.Succeed(_value);
                })
                .OnFailure(exception =>
                {
                    _loadToken = null;

                    token.Fail(exception);
                });

            return token;
        }
    }
}