package de.ixtech.ixnetmanager.ui.theme

import android.os.Build
import androidx.compose.foundation.isSystemInDarkTheme
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.material3.dynamicDarkColorScheme
import androidx.compose.material3.dynamicLightColorScheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext

// Fallback palette (devices below Android 12 / no dynamic color): ix.net blue,
// kept close to the desktop app's accent so both apps still feel related.
private val IxnetBlue = Color(0xFF0A84FF)

private val LightColors = lightColorScheme(
    primary = IxnetBlue,
    secondary = Color(0xFF5E5CE6),
    tertiary = Color(0xFF34C759)
)

private val DarkColors = darkColorScheme(
    primary = Color(0xFF409CFF),
    secondary = Color(0xFF7D7AFF),
    tertiary = Color(0xFF30D158)
)

/**
 * Material You theme: on Android 12+ (API 31+) this pulls the user's system
 * wallpaper-derived dynamic color scheme (the current "Material 3
 * Expressive" default), matching whatever the rest of the OS looks like.
 * Older devices fall back to a fixed ix.net-blue scheme.
 */
@Composable
fun IxnetManagerTheme(
    darkTheme: Boolean = isSystemInDarkTheme(),
    dynamicColor: Boolean = true,
    content: @Composable () -> Unit
) {
    val context = LocalContext.current
    val colorScheme = when {
        dynamicColor && Build.VERSION.SDK_INT >= Build.VERSION_CODES.S -> {
            if (darkTheme) dynamicDarkColorScheme(context) else dynamicLightColorScheme(context)
        }
        darkTheme -> DarkColors
        else -> LightColors
    }

    MaterialTheme(
        colorScheme = colorScheme,
        typography = IxnetTypography,
        content = content
    )
}
