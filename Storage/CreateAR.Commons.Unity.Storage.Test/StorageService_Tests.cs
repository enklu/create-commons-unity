using CreateAR.Commons.Unity.Logging;
using NUnit.Framework;

namespace CreateAR.Commons.Unity.Storage
{
    [TestFixture]
    public class StorageService_Tests
    {
        public class TestObject
        {
            
        }

        public class DocumentData
        {

        }

        public class AchievementData
        {

        }

        private IStorageService _service;

        [SetUp]
        public void Setup()
        {
            
        }

        public void Usage()
        {
            // creation -- added to local manifest
            _service
                .Create(new TestObject())
                .OnSuccess(bucket => Log.Debug(this, "Created bucket {0}.", bucket));

            // refresh local manifest of all buckets
            _service
                .Manifest
                .Refresh()
                .OnSuccess(manifest => Log.Debug(this, "Manifest loaded."));
            
            // bucket search -- synchronous
            var achievements = _service.Manifest.FindOne<AchievementData>();
            var documents = _service.Manifest.FindAll<DocumentData>();

            // load -- only asynchronous if current version is different than manifest version
            achievements
                .Value()
                .OnSuccess(value =>
                {
                    //
                });

            // save -- local manifest is updated
            achievements
                .Save(new AchievementData()) // optional .Save(StorageOptions.IgnoreVersion) ignores latest version check
                .OnSuccess(value =>
                {
                    //
                });

            // delete -- local manifest is updated
            achievements
                .Delete()
                .OnSuccess(value =>
                {
                    //
                });

            // update available?
            //account.RegisterForUpdates(...);
            //account.UnRegisterForUpdates(...);
        }
    }
}