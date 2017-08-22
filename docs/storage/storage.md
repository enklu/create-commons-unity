# Overview

The `CreateAR.Commons.Unity.Storage` package is a simple system with a nice, consistent API. From a high level, this package can create, update, and delete key-value pairs. The keys are always unique, server generated strings, and the values are always json values. These KV pairs also have a version which helps provide some protection against accidentally overwriting a stale value.

## IStorageService

The `IStorageService` interface is the entry point into the entire system. This object manages an internal manifest of all user-owned KV pairs, called `StorageBucket`s.

#### Retrieving Your Buckets

By default, the `IStorageService` knows nothing about the buckets you own. You must call `Refresh()` to get an up to date manifest of a user's buckets.

```
_storage
	.Refresh()
	.OnSuccess(...)
	.OnFailure(...);
```

Once `Refresh` has been called, the service has created a local object that represents each bucket. These can be manipulated in several ways.

#### Getting The Bucket You Want

Looking up the bucket you need can be done in a variety of ways. The first is direct lookup: since these buckets represent KV pairs, the key can be provided for a direct lookup.

```
var bucket = _storage.Get(key);
```

Additionally, the entire list of buckets may be inspected using `All`.

```
foreach (var bucket in _storage.All)
{
	...
}
```

#### Searching with Tags

Each `StorageBucket` may have tags associated with it as well. These tags provide metadata so that buckets may be manipulated easier. Synchronous methods for searching are provided.

```
var bucket = _storage.FindOne("myTag");
```

Tags need not be unique, so to get a whole set of buckets:

```
var buckets = _storage.FindAll("myTag");
```

#### Loading Buckets

Once the appropriate bucket has been located, it needs to be loaded through the `Value<T>()` method before the value can be manipulated. By default, bucket values are not loaded when `Refresh` is called. This is for performance: values may be large.

```
var bucket = _storage.FindOne("myTag");
if (null != bucket)
{
	bucket
		.Value<MyObject>()
		.OnSuccess(...)
		.OnFailure(...);
}
```

#### Caching

Note that this value will be cached, so subsequent calls to `Value<T>()` do not necessarily load the value.

`Value<T>()` will, however, reload the value if it knows there is a later version. This can happen if `StorageService::Refresh()` is called after `Value<T>()` has been called, and it returns a later version than that of the cached value. In this case, `Value<T>()` will automatically fetch the up to date value.

#### Updating Buckets

Buckets can be saved easily, through the `Save()` method:

```
var bucket = _storage.Get(key);
if (null != bucket)
{
	bucket
		.Value<MyObject>()
		.OnSuccess(value => {
			value.Foo = "Bar";
			
			// resave
			bucket.Save(value);
		})
		.OnFailure(...);
}
```

This `Save` will fail, however, if there is a later version of the value than you have. The service does this in two phases: first it compares the loaded version to the latest version is knows about (through `Refresh` calls). It does not call `Refresh` automatically. Secondly, the backend will reject the request if there is a later version in the database. This second option acts as the real enforcer, but many times the service will know locally before needing the server to tell it.


#### Creating and Destroying Buckets

A bucket may be created through the `IStorageService::Create<T>()` method.

```
_storage
	.Create(new MyDocument)
	.OnSuccess(bucket => ...)
	.OnFailure(...);
```

Destroying a bucket is just as easy:

```
_storage
	.Delete(bucket)
	.OnSuccess(_ => ...)
	.OnFailure(...);
```

# Further Reading
