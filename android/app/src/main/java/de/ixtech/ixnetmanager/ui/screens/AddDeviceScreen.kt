package de.ixtech.ixnetmanager.ui.screens

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.unit.dp
import de.ixtech.ixnetmanager.model.IxnetDevice
import de.ixtech.ixnetmanager.net.UdpDiscovery
import kotlinx.coroutines.launch
import java.net.InetAddress

/**
 * Kotlin/Compose equivalent of AddDeviceForm.cs: local adapter + target
 * (ix.net) address, "Search" runs a targeted discovery instead of the
 * default full-broadcast scan - same DiscoverDevices(localAdapter,
 * remoteAddress) call the desktop app's "Add Device..." button makes.
 */
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun AddDeviceScreen(
    defaultLocalAdapter: String,
    onBack: () -> Unit,
    onDeviceAdded: (IxnetDevice) -> Unit
) {
    var localAdapterText by remember { mutableStateOf(defaultLocalAdapter) }
    var targetText by remember { mutableStateOf("255.255.255.255") }
    var isSearching by remember { mutableStateOf(false) }
    var errorText by remember { mutableStateOf<String?>(null) }
    var foundDevice by remember { mutableStateOf<IxnetDevice?>(null) }
    val scope = rememberCoroutineScope()

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Gerät hinzufügen") },
                navigationIcon = {
                    IconButton(onClick = onBack) {
                        Icon(Icons.AutoMirrored.Filled.ArrowBack, contentDescription = "Zurück")
                    }
                }
            )
        }
    ) { padding ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(padding)
                .padding(24.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            OutlinedTextField(
                value = localAdapterText,
                onValueChange = { localAdapterText = it },
                label = { Text("Lokaler Adapter") },
                singleLine = true,
                keyboardOptions = androidx.compose.foundation.text.KeyboardOptions(keyboardType = KeyboardType.Decimal),
                modifier = Modifier.fillMaxWidth()
            )
            OutlinedTextField(
                value = targetText,
                onValueChange = { targetText = it },
                label = { Text("ix.net Adresse") },
                singleLine = true,
                keyboardOptions = androidx.compose.foundation.text.KeyboardOptions(keyboardType = KeyboardType.Decimal),
                modifier = Modifier.fillMaxWidth()
            )

            errorText?.let {
                Text(it, color = MaterialTheme.colorScheme.error, style = MaterialTheme.typography.bodyMedium)
            }

            Button(
                onClick = {
                    errorText = null
                    foundDevice = null
                    val localAdapter = runCatching { InetAddress.getByName(localAdapterText.trim()) }.getOrNull()
                    val target = runCatching { InetAddress.getByName(targetText.trim()) }.getOrNull()
                    if (target == null) {
                        errorText = "Ungültige ix.net Adresse '$targetText'"
                        return@Button
                    }
                    isSearching = true
                    scope.launch {
                        try {
                            val results = UdpDiscovery.discoverDevices(localAdapter, target)
                            if (results.isEmpty()) {
                                errorText = "Kein Gerät unter '$targetText' gefunden"
                            } else {
                                foundDevice = results.first()
                            }
                        } catch (e: Exception) {
                            errorText = e.message ?: e.toString()
                        } finally {
                            isSearching = false
                        }
                    }
                },
                enabled = !isSearching,
                modifier = Modifier.fillMaxWidth()
            ) {
                Text("Suchen")
            }

            if (isSearching) {
                CircularProgressIndicator(modifier = Modifier.padding(top = 8.dp))
            }

            foundDevice?.let { device ->
                Card(modifier = Modifier.fillMaxWidth()) {
                    Column(modifier = Modifier.padding(16.dp)) {
                        Text(device.deviceName.ifBlank { device.serialNumber }, style = MaterialTheme.typography.titleMedium)
                        Text("IP: ${device.ipAddress}", style = MaterialTheme.typography.bodyMedium)
                        Text("MAC: ${device.macAddress}", style = MaterialTheme.typography.bodyMedium)
                        Text("Firmware: ${device.firmwareVersion}", style = MaterialTheme.typography.bodyMedium)
                    }
                }
                Button(onClick = { onDeviceAdded(device) }, modifier = Modifier.fillMaxWidth()) {
                    Text("Hinzufügen")
                }
            }
        }
    }
}
