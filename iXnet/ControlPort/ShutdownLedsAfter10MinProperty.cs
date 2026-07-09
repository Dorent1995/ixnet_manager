using System;

namespace iXnet.ControlPort
{
    /// <summary>
    /// v2.6.0 property 0x1A: Shutdown LEDs after 10 minutes.
    /// When enabled, the device monitors LEDs that are continuously solid-on.
    /// Any LED that has been lit without interruption for more than 10 minutes
    /// is automatically switched off to prevent excessive power use or burn-in.
    /// </summary>
    public class ShutdownLedsAfter10MinProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x1A; }
        }

        public override string DisplayName
        {
            get { return "Shutdown LEDs after 10 minutes"; }
        }

        public override string Description
        {
            get
            {
                return "When enabled, LEDs that remain continuously solid-on for more than " +
                       "10 minutes are automatically switched off by the device. " +
                       "Resets whenever the LED is turned off or its flash mode changes.";
            }
        }
    }
}
