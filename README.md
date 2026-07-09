# ix.net Manager

LED controller management tool for the iXnet Ether V2 modules. Windows desktop
application (C# WinForms, .NET Framework 4.8) plus AVR firmware in `iXnet/`.

# Features

- Discover and manage iXnet Ether V2 LED controller devices over the network
- Edit device properties (IP, netmask, gateway, LED type, brightness, bootup delay, ...)
- Send live LED control commands (flash mode, color, text, brightness)
- Firmware update / program memory flashing
  

## Added in 2.6.0

- **Balance Color Light** property (ControlPort ID `0x19`, EEPROM `0x088`) — balances the white-channel brightness against RGB.
- **Shutdown LEDs after 10 minutes** property (ControlPort ID `0x1A`, EEPROM `0x089`) — automatically turns the LEDs off after 10 min of inactivity.
- These options appear in the device property grid once the matching firmware is flashed.
- new ui + darkmode
- 
