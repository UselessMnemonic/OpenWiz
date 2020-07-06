# OpenWiz
OpenWiz is a reverse-engineered implementation of the LAN API used between Wiz brand smart lights and the Wiz Android App. The project targets .NET Core, and is 100% C#.
### Acknowledgements
These projects have been every useful in confirming my understanding of how Wiz brand lights work:
https://github.com/basriram/openhab2-addons/tree/master/addons/binding/org.openhab.binding.wizlighting
https://github.com/SRGDamia1/openhab2-addons/tree/wizlighting2/bundles/org.openhab.binding.wizlighting
### Current Findings
#### Network
The service uses UDP for transport. Port `38900` is used by the App to recieve, while `38899` is used by the light to recieve.
Broadcasts are made by clients to their broadcast address or `255.255.255.255`.
#### Format
Data exchanged is exchanged between devices using JSON, seemingly encoded in UTF8.
The format for JSON objects is somewhat follows:
```
{
  "method" : "",
  "params" : {},
  "result" : {},
  "error"  : {}
}
```
Not all fields are used in every communication. For example, to request the current state of the light, one may use:
```
{
  "method" : "getPilot"
}
```
The light then responds with something like the following:
```
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
#### JSON Fields
##### `method` : The method called, either by the app or the light.
* `syncPilot` : Called by the remote light to tell a client of its current state.
* `getPilot` : Called by the client to ask a remote light of its current state.
* `setPilot` : Called by a client to change the state of a remote light.
* `registration` : Called by a client to search for lights on the local network.
##### `result` and `params` : Similar fields used to capture the state variables of a light.
* `homeId` : The Home ID of the target light, used in the `registration` method.
* `phoneIp` : The IP of the client, used in the `registration` method.
* `phoneMac` : The MAC of the client, used in the `registration` method.
* `register` : Wether a registration is occuring, seemingly.
* `mac` : The MAC of the remote light, used in replies to some methods.
* `rssi` : The RSSI of the last transmission, always a part of the `result`.
* `state` : Wether the remote light is on.
* `sceneId` : The Scene ID of the selected scene playing on the remote light.
* `speed` : The speed at which the selected scene is playing.
* `r` : The Red componenet of the selected color.
* `g` : The Green component of the selected color.
* `b` : The Blue component of the selected color.
* `c` : The Cool White component of the selected color.
* `w` : The Warm White component of the selected color.
* `temp` : The selected white-light temperature, in degrees Kelvin
* `dimming` : The current brightness.
##### `error` : Describes an error following a transmission.
* `code` : An error code, which is partially useless to us.
* `message` : A string that describes the error, sometimes useful for the client.
