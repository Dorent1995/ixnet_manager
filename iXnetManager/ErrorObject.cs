using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iXnet.ControlPort;

namespace iXnetManager
{
    class ErrorObject
    {
        private ErrorEntry mError;
        private string mSerialNumber;

        public DateTime TimeStamp { get { return mError.TimeStamp; } }
        public string Age 
        { 
            get 
            {
                TimeSpan age = mError.Age;
                if (age == TimeSpan.MaxValue)
                    return "-";

                if (age.TotalDays >= 1.0)
                    return String.Format("{0:f1} days", age.TotalDays);
                else if (age.TotalHours >= 1.0)
                    return String.Format("{0:f1} hours", age.TotalHours);
                else if (age.TotalMinutes >= 1.0)
                    return String.Format("{0:f0} min", age.TotalMinutes);
                else
                    return String.Format("{0:f0} sec", age.TotalSeconds);
            } 
        }
        public string SerialNumber { get { return mSerialNumber; } }
        public int ErrorCode { get { return mError.ErrorCode; } }
        public string ErrorText { get { return mError.ErrorText; } }

        public ErrorObject(ErrorEntry error, string serialNumber)
        {
            mError = error;
            mSerialNumber = serialNumber;
        }
    }
}
