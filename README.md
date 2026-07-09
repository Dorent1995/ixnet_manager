# ix.net Manager

LED controller management tool for the iXnet Ether V2 modules. Windows desktop
application (C# WinForms, .NET Framework 4.8) plus AVR firmware in `iXnet/`.

Based on Jans-ix-net-Manager, compatible with the official release. Current version: **2.6.0**.

# Features

- Discover and manage iXnet Ether V2 LED controller devices over the network
- Edit device properties (IP, netmask, gateway, LED type, brightness, bootup delay, ...)
- Send live LED control commands (flash mode, color, text, brightness)
- Firmware update / program memory flashing

## Added in 2.6.0

- **Balance Color Light** property (ControlPort ID `0x19`, EEPROM `0x088`) — balances the white-channel brightness against RGB.
- **Shutdown LEDs after 10 minutes** property (ControlPort ID `0x1A`, EEPROM `0x089`) — automatically turns the LEDs off after 10 min of inactivity.
- These options appear in the device property grid once the matching firmware is flashed.

# Building

1. Open `iXnetManager.sln` in Visual Studio (or build with MSBuild):
   ```
   MSBuild.exe iXnetManager.sln /p:Configuration=Release /p:Platform="Mixed Platforms"
   ```
2. The `ObjectListView` dependency is in `depends/` and referenced automatically.
3. Output: `iXnetManager\bin\Release\iXnetManager.exe`

# Configuration file

The configuration file is located here:
`iXnetManager\bin\Debug\iXnetManager.exe.config`

## Disable password prompt

Set the parameter `SecurityEnabled` to `False`. After that, you do not have to
enter a password each time you want to flash the LEDs.

## Set your favorite color & brightness

These change the default settings in the Led Control tab:
- Parameter `Brightness` for example `69`.
- Parameter `Color` for example `2` (= Green).
