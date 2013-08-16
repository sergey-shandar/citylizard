# Solutions, Platforms and Packages

- **CityLizard.\*.nuget** package:
	- **desktop.sln** - .NET, open in Visual Studio 2012 Express for Desktop. Project platforms:
  		- **net35-client** - .NET 3.5 Client Profile (runtime 2.0).
  		- **net40-client** - .NET 4.0 Client Profile (runtime 4.0).
	- **web.sln** - For web, open in Visual Studion 2012 Express for Web. Project platforms:
  		- **sl4** - Silverlight 4.
  		- **sl5** - Silverlight 5.
	- **netcore.sln** - Windows 8, open in Visual Studio 2012 Express for Windows 8. Project platforms:
  		- **netcore45** - Windows 8.
	- **wp.sln** - Windows Phone, open in Visual Studio 2012 Express for Windows Phone. Project platforms:
		- **sl4-wp71** - Windows Phone 7.1.
		- **wp8** - Windows Phone 8.0.
- **CityLizard.\*.zip** package:
	- **psm.sln** - PlayStation Mobile. Open in PlayStation Mobile Studio. Project platforms:
		- **psm** - PlayStation Mobile.
- **CityLizard.\*.tar** package:
	- **mono.sln** - Mono for Mac, iOS and Android, open in Xamarin Studio for Mac. Project platforms:
		- **mono-android16** - Mono for Android 1.6.
		- **mono-ios** - Mono for iOS.
		- **mono40** - Mono for .NET 3.5 (runtime 2.0), 
		- **mono35** - Mono for .NET 4.0 (runtime 4.0).

# API Levels

- **Core** includes **Binary**, **Collections**, **Policy**, **Graphics**, **Xml**
	- net35-client
	- net40-client
	- netcore45
	- sl4
	- sl5
	- sl4-wp71
	- wp8
	- mono40
	- mono35
	- mono-ios
	- mono-android16
	- psm 
- **Fsm**. Depends on **Core**.
	- net35-client
	- net40-client
	- netcore45
	- sl4
	- sl5
	- wp8
	- mono40
	- mono35
	- mono-ios
	- mono-android16
	- psm
- **Meta** includes **CodeDom**, **Xml.Schema**. Depends on **Fsm**, **Core**.
	- net35-client
	- net40-client
	- mono40
	- mono35

<table>
	<tr><th></th><th>Core</th><th>Fsm</th><th>Meta</th></tr>
	<tr><th>net40-client</th><td>+</td><td>+</td><td>+</td></tr>
	<tr><th>net35-client</th><td>+</td><td>+</td><td>+</td></tr>
	<tr><th>mono40</th><td>+</td><td>+</td><td>+</td></tr>
	<tr><th>mono35</th><td>+</td><td>+</td><td>+</td></tr>
	<tr><th>mono-android</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>mono-ios</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>psm</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>netcore45</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>sl5</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>sl4</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>wp8</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>sl4-wp71</th><td>+</td><td></td><td></td></tr>
</table>
