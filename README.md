# lychee
Lychee is a .NET utility library for reusable functions and components, inspired by Google Guava.

## Installation

Lychee is available as a Nuget package, and can be installed using the following:

```
Install-Package Lychee
```

## Available Components and Utilities

* **Core**
	* ```Attempt```: Basic construct for retrying an activity in the event of a failure
	* ```Precondition```: Utility functions that are useful for asserting state, variable, and any other conditional
	  circumstance that must be met prior to processing. Similar to .NET code contracts without the overhead.
	* ```Verify```: Similar to ```Precondition```, useful for post-condition checking of application state, variables,
	  and output.