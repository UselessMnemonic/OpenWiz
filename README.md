# OpenWiz
![Nuget](https://img.shields.io/nuget/v/OpenWiz)

OpenWiz is a reverse-engineered implementation of the LAN API used between Wiz brand smart lights and the Wiz Android App. The project targets .NET Core, and is 100% C#.
## Acknowledgements
These projects have been every useful in confirming my understanding of how Wiz brand lights work:
https://github.com/basriram/openhab2-addons/tree/master/addons/binding/org.openhab.binding.wizlighting
https://github.com/SRGDamia1/openhab2-addons/tree/wizlighting2/bundles/org.openhab.binding.wizlighting
## Supported Devices
At the moment, only the BR30 bulbs (those in my poessesion) have been tested.
## Current Findings
### Network
The service uses UDP for transport. Port `38900` is used by the App to recieve, while `38899` is used by the light to recieve.
Broadcasts are made by clients to their broadcast address or `255.255.255.255`.
### Format
Data is exchanged between devices using JSON, seemingly encoded in UTF8.
The format for JSON objects is somewhat as follows:
```JSON
{
  "method" : "",
  "params" : {},
  "result" : {},
  "error"  : {}
}
```
Not all fields are used in every packet. For example, to request the current state of the light, one may use:
```JSON
{
  "method" : "getPilot"
}
```
The light then responds with something like the following:
```JSON
{
  "method" : "getPilot",
  "result" : {
    "mac" : "000000000000",
    "rssi" : -74,
    "state" : true,
    "sceneId" : 12,
    "speed" : 100,
    "temp" : 4200,
    "dimming" : 34
  }
}
```
All MAC addresses are unformatted, 12-digit, lowercase hex strings. All IP addresses are IPv4, in standard dot notation.
### JSON Fields
#### `method` : The method called, either by the app or the light.
* `registration` : Called by a client to search for lights on the local network.
* `pulse` : Called by a client to signal a light for updates (?)
* `firstBeat` : Called by a remote light to tell a client of its configuration (?)

* `getPilot` : Called by the client to ask a remote light of its current state.
* `setPilot` : Called by a client to change the state of a remote light.
* `syncPilot` : Called by the remote light to tell a client of its current state.

*  `getUserConfig` : Called by a client to ask a remote light of any user settings
*  `setUserConfig` : Called by a client to change a remote light's user settings
*  `getSystemConfig` : Called by a client to ask a remote light of any internal configuration
*  `setSystemConfig` : Called by a client to change a remote light's internal configuration
#### `result` and `params` : Similar fields used to capture the state variables of a light.
* `state` : Wether the remote light is on.
* `sceneId` : The Scene ID of the selected scene playing on the remote light.
* `speed` : The speed at which the selected scene is playing.
* `play` : Whether the current scene is playing.
* `r` : The Red componenet of the selected color.
* `g` : The Green component of the selected color.
* `b` : The Blue component of the selected color.
* `c` : The Cool White component of the selected color.
* `w` : The Warm White component of the selected color.
* `temp` : The selected white-light temperature, in degrees Kelvin
* `dimming` : The current brightness.
* `rssi` : The RSSI of the last transmission, always a part of the `result`.

* `phoneIp` : The IP of the client, used in the `registration` method.
* `phoneMac` : The MAC of the client, used in the `registration` method.
* `register` : Wether a registration is occuring, seemingly.

* `moduleName` : The internal name of the remote light.
* `mac` : The MAC of the remote light, used in replies to some methods.
* `typeId` : Unsure
* `homeId` : The Home ID of the target light, used in the `registration` method.
* `groupId` : The Group ID of the target light, positive if assigned.
* `roomId` : The Room ID of the target light, positive if assigned.
* `homeLock` : Unsure
* `pairingLock` : Unsure
* `fwVersion` : The firmware version of the light, as a string.
* `fadeIn` : Possibly the fade-in time during power-on
* `fadeOut` : Possibly the fade-out time during power-off
* `fadeNight` : Unsure
* `dftDim` : Unsure
* `pwmRange` : Unsure
* `drvConf` : Unsure
* `whiteRange` : The white temperature range supported by the light
* `extRange` : The white temperature range advertised to the user
* `po` : Unsure
#### `error` : Describes an error following a transmission.
* `code` : An error code, which is partially useless to us.
* `message` : A string that describes the error, sometimes useful for the client.
## How to use
If you plan to have the user input the IP of their lights, you need only use `WizHandle` and `WizSocket`.
The `WizSocket` can perform IO operations on a given `WizHandle`. Treat the `WizSocket` like any other socket,
and the `WizHandle` like a buffer with the exception that it is not modified during send operations.

If you are seeking to enable auto-discovery, you will need the user's Home ID in conjuction with `WizDiscoveryService`. This is done in good faith to avoid modifying the state of another home's lights.

The Registration operation is not required to modify a light's state, however please be curteous.
