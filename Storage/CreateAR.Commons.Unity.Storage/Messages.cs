namespace CreateAR.Commons.Unity.Storage
{
    /// <summary>
    /// Model of a KV across the wire.
    /// </summary>
    public class KvModel
    {
        public string key;
        public string value;
        public string owner;
        public string tags;
        public int version;
    }

    /// <summary>
    /// Base class for all responses.
    /// </summary>
    public class KvResponse
    {
        public bool success;
        public string error;
    }

    /// <summary>
    /// Response from GetAll endpoint.
    /// </summary>
    public class GetAllKvsResponse : KvResponse
    {
        public KvModel[] body;
    }

    /// <summary>
    /// Request to create a new Kv.
    /// </summary>
    public class CreateKvRequest
    {
        public object value;
    }

    /// <summary>
    /// Response from create endpoint.
    /// </summary>
    public class CreateKvResponse : KvResponse
    {
        public KvModel body;
    }

    /// <summary>
    /// Response from get endpoint.
    /// </summary>
    public class GetKvResponse : KvResponse
    {
        public KvModel body;
    }

    /// <summary>
    /// Request to update a Kv.
    /// </summary>
    public class UpdateKvRequest
    {
        public int version;
        public object value;
    }

    /// <summary>
    /// Updates a Kv.
    /// </summary>
    public class UpdateKvResponse : KvResponse
    {

    }

    /// <summary>
    /// Deletes a Kv.
    /// </summary>
    public class DeleteKvResponse : KvResponse
    {

    }
}