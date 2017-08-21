namespace CreateAR.Commons.Unity.Storage
{
    public class KvModel
    {
        public string key;
        public string value;
        public string owner;
        public string tags;
        public int version;
    }

    public class KvResponse
    {
        public bool success;
        public string error;
    }

    public class GetAllKvsResponse : KvResponse
    {
        public KvModel[] body;
    }

    public class CreateKvRequest
    {
        public object value;
    }

    public class CreateKvResponse : KvResponse
    {
        public KvModel body;
    }

    public class GetKvResponse : KvResponse
    {
        public KvModel body;
    }

    public class UpdateKvRequest
    {
        public int version;
        public object value;
    }

    public class UpdateKvResponse : KvResponse
    {

    }

    public class DeleteKvResponse : KvResponse
    {

    }
}