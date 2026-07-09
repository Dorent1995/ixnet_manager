using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet.InputHub
{
    public interface IHubDevice : IDisposable
    {
        string Name { get; }
        IPAddress LocalAddress { get; }
        IPEndPoint EndPoint { get; }
        UniqueDevID DeviceID { get; }
        bool SupportsDebugSend { get; }
        bool IsEventDevice { get; }
        bool Acquire();
    }
}
