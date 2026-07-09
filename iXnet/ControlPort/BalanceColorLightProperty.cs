using System;

namespace iXnet.ControlPort
{
    /// <summary>
    /// v2.6.0 property 0x19: Balance Color Light.
    /// When enabled, mixed colours (e.g. white = R+G+B all on) are automatically
    /// throttled so their combined brightness matches a pure base colour at the
    /// same brightness setting, preventing white from appearing far brighter than red.
    /// </summary>
    public class BalanceColorLightProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x19; }
        }

        public override string DisplayName
        {
            get { return "Balance Color Light"; }
        }

        public override string Description
        {
            get
            {
                return "When enabled, mixed colours (e.g. white) are throttled to the " +
                       "brightness of the brightest single base colour channel. " +
                       "This prevents white from appearing much brighter than red at the same setting.";
            }
        }
    }
}
