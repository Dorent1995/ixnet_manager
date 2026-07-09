using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class ErrorEntry
    {
        public DateTime TimeStamp { get; protected set; }
        public Byte ErrorCode { get; protected set; }
        public String ErrorText { get; protected set; }
        public TimeSpan Age
        {
            get { return DateTime.Now - TimeStamp; }
        }

        public ErrorEntry(DateTime timeStamp, byte errorCode, string errorText)
        {
            TimeStamp = timeStamp;
            ErrorCode = errorCode;
            ErrorText = errorText;
        }

        public ErrorEntry(DateTime timeStamp, byte errorCode)
            : this(timeStamp, errorCode, String.Empty)
        { }

    }
}
