package de.ixtech.ixnetmanager.ui.screens

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import de.ixtech.ixnetmanager.model.IxnetDevice
import de.ixtech.ixnetmanager.net.UdpDiscovery
import kotlinx.coroutines.launch

class DeviceListViewModel : ViewModel() {

    var devices by mutableStateOf<List<IxnetDevice>>(emptyList())
        private set

    var isDiscovering by mutableStateOf(false)
        private set

    var lastError by mutableStateOf<String?>(null)
        private set

    init {
        refresh()
    }

    fun refresh() {
        if (isDiscovering) return
        viewModelScope.launch {
            isDiscovering = true
            lastError = null
            try {
                devices = UdpDiscovery.discoverDevices()
            } catch (e: Exception) {
                lastError = e.message ?: e.toString()
            } finally {
                isDiscovering = false
            }
        }
    }
}
