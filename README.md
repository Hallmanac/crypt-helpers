# crypto-helpers
A library of helpers that creates a higher level abstraction over encryption, hashing, and encoding. This includes some utilities around managing the hashing of passwords.

## Get It
```
	Install-Package Hallmanac.CryptoHelpers
```
_*OR*_

```
    Install-Package Hallmanac.CryptoHelpers_Net45
```

As you can see there are two versions of this library to support the Full .NET Framework and the .NET Standard Library. They are both maintained to be perfectly aligned with one another with the exception of the missing `Rijndael` classes inside the .NET Standard Library

## Description

There are several key helper areas within this library. All of them are either directly accessible or are more conveniently accessible through the central class named `CryptoHelpers` (with an interface contract of `ICryptoHelpers`). 

The `CryptoHelpers` class can be created using a default constructor (i.e. `new CryptoHelpers()`) or can be used via constructor injection with an IOC container like AutoFac or Unity.

The code classes and methods in the library are fairly well documented so intellisense should allow you to explore in that way.

Also, there are unit tests against most of the library right now so that is also a good example of ways to use this library.

## Feedback

There will be more documentation coming and more helper classes to arrive as well. If you have any feedback, please submit an issue so there can be discussion and a way to track back to a Pull Request. Contributions are welcome!
