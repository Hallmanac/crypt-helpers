# crypto-helpers
A library of helpers that creates a higher level abstraction over encryption, hashing, and encoding. This includes some utilities around managing the hashing of passwords.

## Get It
```
	Install-Package Hallmanac.CryptoHelpers
```

There are two versions of this library to support the Full .NET Framework and the .NET Standard Library. They are both maintained to be perfectly aligned with one another with the exception of the missing `Rijndael` classes inside the .NET Standard Library

## Easy To Use With `CryptoHelpers` Class

There are several key helper classes within this library. All of them are either directly accessible or are more conveniently accessible through the central class named `CryptoHelpers` (with an interface contract of `ICryptoHelpers`). 

The `CryptoHelpers` class can be created using a default constructor (i.e. `new CryptoHelpers()`) or can be used via constructor injection with an IOC container like AutoFac or Unity.

#### Using Default Constructor:

```
	// Create a new instance using the default constructor
	var cryptoHelpers = new CryptoHelpers();
```

#### Using IOC container (Autofac shown as example):

```
	var builder = new ContainerBuilder();
	builder.RegisterType<CryptoHelpers>().As<ICryptoHelpers>();
        builder.RegisterType<PasswordHashingSvc>().As<IPasswordHashingSvc>();
        builder.RegisterType<SymmetricEncryptionSvc>().As<ISymmetricEncryptionSvc>();
        builder.RegisterType<RandomGenerators>().As<IRandomGenerators>();
        builder.RegisterType<HashingHelpers>().As<IHashingHelpers>();
```

The code classes and methods in the library are fairly well documented so intellisense should allow you to explore in that way.

Also, there are unit tests against most of the library right now so that is also a good example of ways to use this library.

## PasswordHashingSvc Class

The `PasswordHashingSvc` class has a constructor that can accept a Global App Salt parameter. The Global Application Salt adds an extra layer of security to the password hashing process by hashing each password with this salt first and then proceeding to the normal route of hashing a password. The globalApplicationSalt itself is intended to be kept secret (i.e. stored in something like Azure Key Vault) which means that even if someone were able to gain access to passwords they would not be able to brute force the password because, in theory, they would not have access to the global application salt.

## Feedback

There will be more documentation coming and more helper classes to arrive as well. If you have any feedback, please submit an issue so there can be discussion and a way to track back to a Pull Request. Contributions are welcome!
