using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using iXnet.Bootloader;
using iXnet.ControlPort;
using iXnet.InputHub;
using iXnet.LedPort;
using System.Threading.Tasks;

namespace iXnet
{
    public class iXnetDevice : IDisposable
    {
        private IProperty[] mProperties;
        public IProperty[] Properties
        {
            get { return mProperties; }
        }
        
        private InterfaceProperties mInterface;
        public InterfaceProperties Interface
        {
            get { return mInterface; }
        }

        public Version FirmwareVersion 
        { 
            get 
            {
                if (HasBootloader)
                    return Bootloader.Properties.FirmwareVersion;
                else
                    return ControlPort.FWVersion;
            } 
        }

        public Version HardwareRevision 
        { 
            get { return new Version(); } 
        }

        public OperationMode OperationMode 
        { 
            get { return HasBootloader ? OperationMode.Bootloader : OperationMode.Normal; } 
        }

        public string SerialNumber 
        { 
            get 
            {
                if (HasBootloader)
                    return Bootloader.Properties.SerialNumber;
                else
                    return ControlPort.SerialNumber; 
            } 
        }

        public string DeviceName 
        { 
            get 
            {
                if (HasBootloader)
                    return Bootloader.DeviceName;
                else
                    return ControlPort.DeviceName; 
            } 
        }

        public IPAddress IPAddress 
        { 
            get 
            {
                if (HasBootloader)
                    return IPAddress.Any;
                else
                    return ControlPort.IPAddress; 
            } 
        }

        public MacAddress MacAddress 
        { 
            get 
            {
                if (HasBootloader)
                    return Bootloader.Address;
                else
                    return ControlPort.Address; 
            } 
        }

        public int NumDigits
        {
            get
            {
                var digitProp = GetProperty<DigitsCountProperty>();
                if (digitProp == null)
                    return 0;
                return digitProp.NumDigits;
            }
        }

        public int MaxLedCount
        {
            get
            {
                var prop = GetProperty<LedCountProperty>();
                if (prop == null)
                    return 0;

                return prop.LedCount;
            }
        }

        public int ActiveLedCount
        {
            get
            {
                var prop = GetProperty<ActiveLedCountProperty>();
                if (prop == null)
                    return MaxLedCount;

                return prop.LedCount;
            }
        }

        public Feature Features
        {
            get
            {
                if (HasBootloader)
                    return Bootloader.Properties.Features;
                else
                {
                    var prop = GetProperty<FeatureProperty>();
                    if (prop == null)
                        return Feature.None;
                    return prop.Features;
                }
            }
        }

        public TimeSpan UpTime
        {
            get
            {
                var prop = GetProperty<UpTimeProperty>();
                if (prop == null)
                    return TimeSpan.MaxValue;

                return prop.UpTime;
            }
        }


        public bool CanFlashLedModules
        {
            get
            {
                var prop = GetProperty<LedTypeProperty>();
                if (prop == null)
                    return false;
                return prop.LedMode == LedTypeProperty.LedType.RGBQuantityButton; // || prop.LedMode == LedTypeProperty.LedType.RGBQuantityButtonDbg;
            }
        }


        public bool HasBootloader { get { return Bootloader != null; } }
        public bool HasControlPort { get { return ControlPort != null; } }
        public bool HasLedPort { get { return Features.HasFlag(Feature.LedBus); } }
        public bool HasInputHub { get { return Features.HasFlag(Feature.Input); } }

        public ControlPortClient ControlPort { get; private set; }
        public BootloaderClient Bootloader { get; private set; }
        private LedPortClient mLedport;
        public LedPortClient LedPort
        {
            get
            {
                if (mLedport == null)
                    mLedport = new LedPortClient(ControlPort.LocalAdapter, IPAddress, FirmwareVersion, NumDigits, MaxLedCount);

                return mLedport;
            }
        }

        private InputHubClient mInputHub;
        public InputHubClient InputHub
        {
            get
            {
                if (mInputHub == null)
                    mInputHub = new InputHubClient(ControlPort.LocalAdapter, IPAddress);

                return mInputHub;
            }
        }

        private iXnetDevice(ControlPortClient controlPort)
        {
            ControlPort = controlPort;
        }

        private iXnetDevice(BootloaderClient bootloader)
        {
            Bootloader = bootloader;
        }

        public static void InitLibrary(IPEndPoint inputListenEP = null)
        {
            InputHubEventReceiver.Singleton.Open(inputListenEP);
        }

        public void UpdateSettings()
        {
            ReadProperties();
            ReadInterface();

            DisposeDynamicClients();
        }

        public void UpdateProperties()
        {
            ReadProperties();
        }

        private void ReadProperties()
        {
            try
            {
                if (ControlPort.CanGetProperties)
                    mProperties = ControlPort.GetProperties();
            }
            catch
            {
                mProperties = null;
            }
        }

        private void ReadInterface()
        {
            try
            {
                if (ControlPort.CanGetInterface)
                    ControlPort.GetInterface(out mInterface);
            }
            catch
            {
                mInterface = new InterfaceProperties();
            }
        }

        public static iXnetDevice[] DiscoverDevices(bool includeBootloader = false, IPAddress localAdapter = null, IPAddress broadcastTarget = null)
        {
            IEnumerable<iXnetDevice> ctrlPortDevs= null;
            var discCtrlPort = new Task(() => ctrlPortDevs = ControlPortClient.DiscoverDevices(localAdapter, broadcastTarget).Select(cPort => new iXnetDevice(cPort)));
            discCtrlPort.Start();

            IEnumerable<iXnetDevice> bootloaderDevs = Enumerable.Empty<iXnetDevice>();
            if (includeBootloader)
            {
                var discBld = new Task(() => bootloaderDevs = BootloaderClient.DiscoverDevices(localAdapter).Select(bld => new iXnetDevice(bld)));
                discBld.Start();
                discBld.Wait();
            }

            discCtrlPort.Wait();

            return ctrlPortDevs.Concat(bootloaderDevs).ToArray();
        }

        private T GetProperty<T>() where T : IProperty
        {
            if (mProperties == null)
                return default(T);
            return (T)mProperties.FirstOrDefault(p => p is T);
        }

        public void Reset()
        {
            ControlPort.Reset(false);
        }

        private void DisposeDynamicClients()
        {
            if (mLedport != null)
            {
                mLedport.Dispose();
                mLedport = null;
            }
            if (mInputHub != null)
            {
                mInputHub.Dispose();
                mInputHub = null;
            }
        }

        public void Dispose()
        {
            DisposeDynamicClients();
        }
    }
}
