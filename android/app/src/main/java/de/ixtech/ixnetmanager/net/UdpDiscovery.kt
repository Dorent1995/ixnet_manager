package de.ixtech.ixnetmanager.net

import de.ixtech.ixnetmanager.model.IxnetDevice
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.net.DatagramPacket
import java.net.DatagramSocket
import java.net.InetAddress
import java.net.InetSocketAddress
import java.net.NetworkInterface
import java.util.Locale

/**
 * Kotlin port of the desktop app's iXnet/UdpDiscover.cs + the discovery half
 * of iXnet/ControlPort/ControlPortClient.cs. Sends the same 2-byte discover
 * packet as a UDP broadcast on port 1999 from every local IPv4 interface,
 * and parses replies using the exact byte layout the C# client uses:
 *
 *   reply[1]      == 0x81            discover-reply command id
 *   reply[2]      = protocol version (1 or 2)
 *   reply[3..8]   = MAC address (6 bytes)
 *   reply[9..12]  = firmware version: major, minor, build, revision
 *   reply[13..32] = device name, 20 bytes ASCII, NUL-terminated
 *   reply[33..52] = serial number, 20 bytes ASCII, NUL-terminated
 *
 * NOTE: this has been translated 1:1 from the C# source but has not been
 * tested against real ix.net hardware in this environment (no device/network
 * access here) - please verify against a real device before relying on it.
 */
object UdpDiscovery {

    private const val CLIENT_PORT = 1999
    private const val REPLY_TIMEOUT_MS = 2000
    private const val MIN_REPLY_LENGTH = 53
    private const val DISCOVER_REPLY_CMD = 0x81

    private val DISCOVER_PACKET = byteArrayOf(0x00, 0x01)

    suspend fun discoverDevices(): List<IxnetDevice> = withContext(Dispatchers.IO) {
        val localAddresses = getUsableLocalIPv4Addresses()
        discoverOn(localAddresses, InetAddress.getByName("255.255.255.255"))
    }

    /**
     * Targeted discovery, used by the "Add Device" screen - mirrors
     * MainForm.DiscoverDevices(newDeviceForm.LocalAdapterIP,
     * newDeviceForm.RemoteAddress) on the desktop app: the target can be a
     * specific device's unicast IP or a broadcast address, and the local
     * adapter can be pinned to one interface instead of trying all of them.
     */
    suspend fun discoverDevices(localAdapter: InetAddress?, target: InetAddress): List<IxnetDevice> =
        withContext(Dispatchers.IO) {
            val localAddresses = if (localAdapter != null) listOf(localAdapter) else getUsableLocalIPv4Addresses()
            discoverOn(localAddresses, target)
        }

    private fun discoverOn(localAddresses: List<InetAddress>, target: InetAddress): List<IxnetDevice> {
        if (localAddresses.isEmpty()) {
            return emptyList()
        }

        val sockets = localAddresses.mapNotNull { addr ->
            try {
                DatagramSocket(0, addr).apply { broadcast = true }
            } catch (e: Exception) {
                null
            }
        }

        try {
            val broadcastTarget = InetSocketAddress(target, CLIENT_PORT)
            for (socket in sockets) {
                try {
                    socket.send(DatagramPacket(DISCOVER_PACKET, DISCOVER_PACKET.size, broadcastTarget))
                } catch (e: Exception) {
                    // one interface failing to send shouldn't stop the others
                }
            }

            val devicesByMac = LinkedHashMap<String, IxnetDevice>()
            val deadline = System.currentTimeMillis() + REPLY_TIMEOUT_MS

            for (socket in sockets) {
                socket.soTimeout = 50
            }

            while (System.currentTimeMillis() < deadline) {
                for (socket in sockets) {
                    try {
                        val buf = ByteArray(1024)
                        val packet = DatagramPacket(buf, buf.size)
                        socket.receive(packet)
                        parseDiscoverReply(packet.data, packet.length, packet.address)?.let { device ->
                            devicesByMac[device.macAddress] = device
                        }
                    } catch (timeout: java.net.SocketTimeoutException) {
                        // expected - just means no packet on this socket yet
                    } catch (e: Exception) {
                        // ignore malformed/unreadable packets, keep listening
                    }
                }
            }

            return devicesByMac.values.toList()
        } finally {
            sockets.forEach { it.close() }
        }
    }

    private fun parseDiscoverReply(data: ByteArray, length: Int, from: InetAddress): IxnetDevice? {
        if (length < MIN_REPLY_LENGTH) return null
        if ((data[1].toInt() and 0xFF) != DISCOVER_REPLY_CMD) return null

        val protoVersion = data[2].toInt() and 0xFF
        if (protoVersion != 1 && protoVersion != 2) return null

        val mac = (3..8).joinToString(":") { i -> String.format(Locale.US, "%02X", data[i]) }

        val fwVersion = "${data[9].toInt() and 0xFF}.${data[10].toInt() and 0xFF}." +
            "${data[11].toInt() and 0xFF}.${data[12].toInt() and 0xFF}"

        val deviceName = asciiZTerminated(data, 13, 20)
        val serialNumber = asciiZTerminated(data, 33, 20)

        return IxnetDevice(
            protoVersion = protoVersion,
            macAddress = mac,
            ipAddress = from.hostAddress ?: "",
            firmwareVersion = fwVersion,
            deviceName = deviceName,
            serialNumber = serialNumber
        )
    }

    private fun asciiZTerminated(data: ByteArray, offset: Int, maxLen: Int): String {
        var end = offset
        val limit = (offset + maxLen).coerceAtMost(data.size)
        while (end < limit && data[end] != 0.toByte()) end++
        return String(data, offset, end - offset, Charsets.US_ASCII)
    }

    private fun getUsableLocalIPv4Addresses(): List<InetAddress> {
        val result = mutableListOf<InetAddress>()
        try {
            val interfaces = NetworkInterface.getNetworkInterfaces() ?: return result
            for (iface in interfaces) {
                if (!iface.isUp || iface.isLoopback) continue
                for (addr in iface.inetAddresses) {
                    if (addr.isLoopbackAddress) continue
                    val bytes = addr.address
                    if (bytes.size != 4) continue // IPv4 only
                    if (bytes[0] == 169.toByte() && bytes[1] == 254.toByte()) continue // link-local
                    result.add(addr)
                }
            }
        } catch (e: Exception) {
            // fall through with whatever we already collected
        }
        return result
    }
}
