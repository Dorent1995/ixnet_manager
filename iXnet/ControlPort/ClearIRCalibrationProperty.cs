using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class ClearIRCalibrationProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x0B; }
        }

        public override string DisplayName
        {
            get { return "Clear IR Calibration"; }
        }

        public override string Description
        {
            get
            {
                return "Setting that property will clear the ir calibration from the eeprom, thus enforcing a calibration on next restart.";
            }
        }
    }
}
