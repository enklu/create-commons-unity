# Overview

The `CreateAR.Commons.Unity.Logging` package provides a simple interface for logging, as well as a few nice _methods_ of logging.

#### Log

The `Log` class provides a static interface for logging. This is in contrast with many `log4j` inspired loggers that require setup per class.

Usage is quite simple:

```
Log.Debug(this, "This is a {0}.", "log");
```

`Log` has a method per `LogLevel`. It requires the caller to be specified first, then a message, followed by replacements.

#### LogLevels

The `Log` class features a level filter which can be used to globally set the log level:

```
Log.Level = LogLevel.Error;
```

The above specifies that no log under `LogLevel.Error` will be logged.

#### Log Targets

The `ILogTarget` interface is where the meat of the logging system resides. This library provides a few implementations, which are described in more detail below.

These targets may be added and removed from the `Log` class:

```
Log.AddLogTarget(new DiagnosticsLogTarget(formatter));

```

Once an `ILogTarget` implementation has been added, logs will be forwarded to that target.

Most targets require an `ILogFormatter` to be passed along, which will format the logs for the specific target. `DefaultLogFormatter` is provided which will output logs with timestamp and callee.

##### DiagnosticsLogTarget

This target will forward logs to `System.Diagnostics.Debug`:

```
Log.AddLogTarget(new DiagnosticsLogTarget(formatter));
```

##### FileLogTarget

Forwards logs to a file.

```
Log.AddLogTarget(new FileLogTarget(new DefaultFormatter(), "file.txt"));
```

This log target is also an `IDisposable`, so it should be cleaned up when removed.

##### UnityLogTarget

Forwards logs to the Unity console:

```
Log.AddLogTarget(new UnityLogTarget(new DefaultFormatter()));
```
