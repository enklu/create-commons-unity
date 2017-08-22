# Overview

The Async package provides primitives for working with asynchronous code. Almost all projects within `createar-commons-unity` use the `IAsyncToken<T>` interface, so it is best to get well aquainted with it.


### Callbacks

In many codebases, callbacks are used to manage asynchronous code:

```
_controller.Foo(() => {
	...
});
```

Callbacks are very nice, but lacking in a few areas. Most notably, callbacks provide a great method for subscribing, but provide no out-of-the-box method for unsubscribing. Consider a UI class:

```
public Resource MyResource;

public void Open()
{
	_controller.Foo(Callback);
}

public void Close()
{
	MyResource.Destroy();
}

private void Callback()
{
	MyResource.Bar();
}
```

This is a simple example, but demonstrates the problem well. Here we have some asynchronous method `Foo` which is given a callback. That callback may be called at any future time. Now the problem: what if `Close` is called before `Callback`? We would have an error, because `Callback` is making an assumption on the order of thigns being called. We could add a boolean to tell us we're closed, but what we'd really like to be able to do is prevent the callback from ever being called.

### Events

C# has a nice primitive called `event`, which allows for subscription and unsubscription. This, we can get around the ealier problem:

```
public void Open()
{
	_controller.OnEvent += Callback;
	_controller.Foo();
}

public void Close()
{
	_controller.OnEvent -= Callback;
}
```

This is great because it allows us to prevent the callback from ever being called. However, it also has a problem. Consider:

```
public MyClass(Dependency dependency)
{
	dependency.OnReady += Initialize;
}
```

In the above example, we have an object that needs something to happen before it cann initialize. However, what if the `Dependency` has already called `OnReady`? This object will be waiting forever.

Thus, the need arises for a primitive that will fix these two problems.

### IAsyncToken<T>

Our answer is `IAsyncToken<T>`. This object has a few nice qualities and acts much like an asynchronous counterpart to `try/catch/finally` blocks. At a high level, this object represents an asynchronous action. This object accepts exactly one resolution. This is easiest to see by example:

```
var token = _controller.Operation();

// called if the operation was a success
token.OnSuccess(value => ...);

// called if the operation was a failure
token.OnFailure(exception => ...);

// called regardless
token.OnFinally(_ => ...);
```

In this example, internally to `Operation`, the token is being resolved with either a success or a failure. It cannot be resolved with both. Once a token has been resolved it cannot be changed.

Additionally, `Operation` may be asynchronous, but it may not be. In the case of a cache, we may want to return synchronously. For `IAsyncToken<T>`, it doesn't matter. Unlike an event, the callback will be called immediately if already resolved.

```
var token = _controller.Operation();

// Operation() is already finished!

// Callback is still called!
token.OnSuccess(value => ...);
```

For some syntactic sugar, you can also combine all of these calls:

```
_controller
	.Foo()
	.OnSuccess(value => ...)
	.OnFailure(exception => ...)
	.OnFinally(token => ...);
```

Much like an event, multiple callbacks may be added:

```
var token = _controller.OnSuccess(...);

...

token.OnSuccess(...);
```

These two callbacks will be called in order.

#### Abort

Revisiting the problem we had with callbacks, let's do this again with tokens:

```
private IAsyncToken<MyValue> _fooToken;

public void Foo()
{
	_fooToken = _controller
		.Foo()
		.OnSuccess(Callback);
}

public void Close()
{
	_fooToken.Abort();
{
```

After `Abort` is called, the token is dead. It cannot be resolved and it will call no future callbacks. In this case, if `Close` is called before the token calls `Callback`, then `Callback` will not be called at all. 














