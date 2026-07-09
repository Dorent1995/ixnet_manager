package de.ixtech.ixnetmanager.model

/**
 * One discovered ix.net device (ControlPort protocol, proto version 1 or 2).
 * Mirrors the fields the desktop app's device list shows; UpTime/Features
 * are filled in by a later GetProperties call (see PropertiesRepository),
 * not by discovery itself - matches iXnet/iXnetDevice.cs on the desktop side.
 */
data class IxnetDevice(
    val protoVersion: Int,
    val macAddress: String,
    val ipAddress: String,
    val firmwareVersion: String,
    val deviceName: String,
    val serialNumber: String
)
