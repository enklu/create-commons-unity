using System;
using System.Text;
using CreateAR.Commons.Unity.Async;
using CreateAR.Commons.Unity.Http;
using Void = CreateAR.Commons.Unity.Async.Void;

namespace CreateAR.Commons.Unity.Storage
{
    /// <summary>
    /// Thin worker class that makes Http requests and does a small bit of
    /// serialization.
    /// </summary>
    public class StorageWorker : IStorageWorker
    {
        /// <summary>
        /// Root endpoint.
        /// </summary>
        private const string ENDPOINT_KVS = "/user/{userId}/kv";

        /// <summary>
        /// Dependencies.
        /// </summary>
        private readonly IHttpService _http;
        private readonly JsonSerializer _json;

        /// <inheritdoc cref="IStorageWorker"/>
        public event Action<string> OnDelete;

        /// <summary>
        /// Creates a new StorageWorker.
        /// </summary>
        /// <param name="http">IHttpService implementation.</param>
        /// <param name="json">For JSON serialization.</param>
        public StorageWorker(
            IHttpService http,
            JsonSerializer json)
        {
            _http = http;
            _json = json;
        }

        /// <inheritdoc cref="IStorageWorker"/>
        public IAsyncToken<KvModel[]> GetAll()
        {
            var token = new AsyncToken<KvModel[]>();

            _http
                .Get<GetAllKvsResponse>(_http.UrlBuilder.Url(ENDPOINT_KVS))
                .OnSuccess(response =>
                {
                    if (!response.Payload.success)
                    {
                        token.Fail(new Exception(response.Payload.error));
                        return;
                    }

                    token.Succeed(response.Payload.body);
                })
                .OnFailure(token.Fail);

            return token;
        }

        /// <inheritdoc cref="IStorageWorker"/>
        public IAsyncToken<KvModel> Create(object value)
        {
            var token = new AsyncToken<KvModel>();

            _http
                .Post<CreateKvResponse>(
                    _http.UrlBuilder.Url(ENDPOINT_KVS),
                    new CreateKvRequest
                    {
                        value = value
                    })
                .OnSuccess(response =>
                {
                    if (!response.Payload.success)
                    {
                        token.Fail(new Exception(response.Payload.error));
                        return;
                    }

                    token.Succeed(response.Payload.body);
                })
                .OnFailure(token.Fail);

            return token;
        }

        /// <inheritdoc cref="IStorageWorker"/>
        public IAsyncToken<object> Load(string key, Type type)
        {
            var token = new AsyncToken<object>();

            _http
                .Get<GetKvResponse>(_http.UrlBuilder.Url($"{ENDPOINT_KVS}/{key}"))
                .OnSuccess(response =>
                {
                    if (!response.Payload.success)
                    {
                        token.Fail(new Exception(response.Payload.error));
                        return;
                    }

                    var bytes = Encoding.UTF8.GetBytes(response.Payload.body.value);
                    object value;
                    _json.Deserialize(type, ref bytes, out value);

                    token.Succeed(value);
                })
                .OnFailure(token.Fail);

            return token;
        }

        /// <inheritdoc cref="IStorageWorker"/>
        public IAsyncToken<Void> Save(string key, object value, string tags, int version)
        {
            var token = new AsyncToken<Void>();

            _http
                .Put<UpdateKvResponse>(
                    _http.UrlBuilder.Url($"{ENDPOINT_KVS}/{key}"),
                    new UpdateKvRequest
                    {
                        value = value,
                        version = version
                    })
                .OnSuccess(response =>
                {
                    if (response.Payload.success)
                    {
                        token.Succeed(Void.Instance);
                    }
                    else
                    {
                        token.Fail(new Exception(response.Payload.error));
                    }
                })
                .OnFailure(token.Fail);

            return token;
        }

        /// <inheritdoc cref="IStorageWorker"/>
        public IAsyncToken<Void> Delete(string key)
        {
            var token = new AsyncToken<Void>();

            _http
                .Delete<CreateKvResponse>(_http.UrlBuilder.Url(ENDPOINT_KVS))
                .OnSuccess(response =>
                {
                    if (!response.Payload.success)
                    {
                        token.Fail(new Exception(response.Payload.error));
                        return;
                    }

                    OnDelete?.Invoke(key);

                    token.Succeed(Void.Instance);
                })
                .OnFailure(token.Fail);

            return token;
        }
    }
}