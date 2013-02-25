# Solutions And Platforms

- **desktop** - .NET, open in Visual Studio 2012 Express for Desktop. Project platforms:
  - **net35-client** - .NET 3.5 Client Profile (runtime 2.0).
  - **net40-client** - .NET 4.0 Client Profile (runtime 4.0).
- **mono-android** - Mono for Android, open in Xamarin Studio for Windows or Mac. Project platforms:
  - **mono-android16** - Mono for Android 1.6.
  - optional: **mono40**, **mono35**.
- **mono-mac** - Mono for Mac and iOS, open in Xamarin Studio for Mac. Project platforms:
  - **mono35** - Mono for .NET 3.5 (runtime 2.0).
  - **mono40** - Mono for .NET 4.0 (runtime 4.0).
  - **mono-ios** - Mono for iOS.
  - optional **mono-android16**.
- **psm** - PlayStation Mobile. Open in PlayStation Mobile Studio. Project platforms:
  - **psm** - PlayStation Mobile.
- **web** - For web, open in Visual Studion 2012 Express for Web. Project platforms:
  - **sl4** - Silverlight 4.
  - **sl5** - Silverlight 5.
  - optional: **net35**, **net40**.
- **windows8** - Windows 8, open in Visual Studio 2012 Express for Windows 8. Project platforms:
  - **windows8** - Windows 8.
- **wp** - Windows Phone, open in Visual Studio 2012 Express for Windows Phone. Project platforms:
  - **wp71** - Windows Phone 7.1.
  - **wp8** - Windows Phone 8.0.

# API Levels

	- net35-client: 2
	- net40-client: 2
	- windows8: 1
	- sl4: 1
	- sl5: 1
	- wp71: 0
	- wp8: 1
	- mono40: 2
	- mono35: 2
	- mono-ios: 1
	- mono-android16: 1
	- psm: 1

- CityLizard: Binary, Collections, Xml, Policy
	- net35-client
	- net40-client
	- windows8
	- sl4
	- sl5
	- wp71
	- wp8
	- mono40
	- mono35
	- mono-ios
	- mono-android16
	- psm
- CityLizard.L1: Fsm
	- net35-client
	- net40-client
	- windows8
	- sl4
	- sl5
	- wp8
	- mono40
	- mono35
	- mono-ios
	- mono-android16
	- psm
- CityLizard.L2: CodeDom, Xml.Schema
	- net40-client
	- net40-client
	- mono40
	- mono35

# API Graph

- **Core** includes **Binary**, **Collections**, **Policy**, **Xml**
	- net35-client
	- net40-client
	- windows8
	- sl4
	- sl5
	- wp71
	- wp8
	- mono40
	- mono35
	- mono-ios
	- mono-android16
	- psm 
- **Fsm**. Depends on **Core**.
	- net35-client
	- net40-client
	- windows8
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
	<tr><th>windows8</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>sl5</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>sl4</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>wp8</th><td>+</td><td>+</td><td></td></tr>
	<tr><th>wp71</th><td>+</td><td></td><td></td></tr>
</table>
