# Setup

#### Subscribe

```
// subscribe to a message type
router.Subscribe(messageType, message => {
	...
});

// subscribe once
router.SubscribeOnce(messageType, message => {
	...
	
	// automatically unsubscribed
});

// subscribe to all
router.SubscribeAll((message, messageType) => {
	// this method will receive all messages

	...
});
```

#### Unsubscribe

```
// subscribe functions return function to unsubscribe with
var unsub = router.Subscribe(...);

...

unsub();
```

Or

```
// or use subscribe variants to pass unsubscribe function to subscriber
router.Subscribe(messageType, (message, unsub) => {
	...
	
	unsub();
});

router.SubscribeAll((message, messageType, unsub) => ...);
```

#### Consume

```
router.Subscribe(messageType, message => {
	...

	// next subscriber won't get message 
	router.Consume(message);
});
```

#### Publishing

```
// publish a message
router.Publish(messageType, message);

// publish many messages
router.Publish(messageType, messageA, messageB, messageC);
```

