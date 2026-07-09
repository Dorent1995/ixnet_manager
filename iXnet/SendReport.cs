using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    public struct SendReport
    {
        public SendError Error { get; set; }
        public int Retries { get; set; }

        public static readonly SendReport Ok = new SendReport() { Error = SendError.NoError, Retries = 0 };
    }
}
