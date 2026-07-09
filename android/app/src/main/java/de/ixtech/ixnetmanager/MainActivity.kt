package de.ixtech.ixnetmanager

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Surface
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import de.ixtech.ixnetmanager.model.IxnetDevice
import de.ixtech.ixnetmanager.ui.screens.AddDeviceScreen
import de.ixtech.ixnetmanager.ui.screens.DeviceListScreen
import de.ixtech.ixnetmanager.ui.theme.IxnetManagerTheme

private sealed class Screen {
    data object DeviceList : Screen()
    data object AddDevice : Screen()
}

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            IxnetManagerApp()
        }
    }
}

@Composable
private fun IxnetManagerApp() {
    IxnetManagerTheme {
        Surface(modifier = Modifier.fillMaxSize()) {
            var screen by remember { mutableStateOf<Screen>(Screen.DeviceList) }
            var lastAddedDevice by remember { mutableStateOf<IxnetDevice?>(null) }

            when (val current = screen) {
                is Screen.DeviceList -> DeviceListScreen(
                    onAddDevice = { screen = Screen.AddDevice },
                    onOpenDevice = { /* device detail screen: task #31 */ }
                )
                is Screen.AddDevice -> AddDeviceScreen(
                    defaultLocalAdapter = "0.0.0.0",
                    onBack = { screen = Screen.DeviceList },
                    onDeviceAdded = {
                        lastAddedDevice = it
                        screen = Screen.DeviceList
                    }
                )
            }
        }
    }
}
