using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    static class PropertyFactory
    {
        public static IProperty CreateProperty(int id)
        {
            switch (id)
            {
                case 0x01:
                    return new LedTypeProperty();
                case 0x02:
                    return new DigitBrightnessProperty();
                case 0x03:
                    return new LedBrightnessProperty();
                case 0x84:
                    return new FeatureProperty();
                case 0x85:
                    return new LedCountProperty();
                case 0x86:
                    return new DigitsCountProperty();
                case 0x87:
                    return new ResetStatusProperty();
                case 0x88:
                    return new StackSizeProperty();
                case 0x89:
                    return new ActiveLedCountProperty();
                case 0x0A:
                    return new ClearLedOnButtonPressedProperty();
                case 0x0B:
                    return new ClearIRCalibrationProperty();
                case 0x8B:
                    return new UpTimeProperty();
                case 0x8C:
                    return new EEPromSizeProperty();
                case 0x8D:
                    return new EEPromFreeSpaceProperty();
                case 0x0E:
                    return new ProcessEnableProperty();
                case 0x8F:
                    return new SupplyVoltageProperty();
                case 0x90:
                    return new RevisionProperty();
                case 0x40:
                    return new ProcessNextStepProperty();
                case 0x41:
                    return new ProcessBusErrorProperty();
                case 0x11:
                    return new ReportLedBusCrcProperty();
                case 0x12:
                    return new LightGridDedicatedDeviceProperty();
                case 0x13:
                    return new ExtIOModeProperty();
                case 0x94:
                    return new PowerCycleCountProperty();
                case 0x15:
                    return new LightGridModeProperty();
                case 0x16:
                    return new BootupDelayProperty();
                case 0x17:
                    return new DebugDisplayProperty();
                case 0x18:
                return new DisableIRButtonProperty();
                case 0x19:
                return new BalanceColorLightProperty();
                case 0x1A:
                return new ShutdownLedsAfter10MinProperty();
            }

            return null;
        }
    }
}
