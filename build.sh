VERSION=$(<version.txt)
echo "Load 'mono.sln', build 'Release', exit from 'Xamarin Studio'."
open "/Applications/Xamarin Studio.app" -W
rm -f -r lib/
mkdir lib/
mkdir lib/mono40/
mkdir lib/mono35/
mkdir lib/ios/
mkdir lib/android/
cp CityLizard.Fsm/bin/Release.mono40/*.dll lib/mono40/
cp CityLizard.Fsm/bin/Release.mono35/*.dll lib/mono35/
cp CityLizard.Fsm/bin/Release.mono-ios/*.dll lib/ios/
cp CityLizard.Fsm/bin/Release.mono-android16/*.dll lib/android/
tar -c -f CityLizard.$VERSION.tar lib/
echo "done."