using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flobbster.Windows.Forms;
using System.ComponentModel;
using iXnet;
using iXnet.ControlPort;
using iXnet.LedPort;
using iXnet.InputHub;

namespace iXnetManager
{
    public class NetDevice
    {
        private iXnetDevice mDevice;
        public iXnet.iXnetDevice Device
        {
            get { return mDevice; }
        }

        private PropertyTable mPropertyTable;
        public object PropertyTable
        {
            get { return mPropertyTable; }
        }

        private IHubDevice[] mInputDevices;
        public IHubDevice[] InputDevices
        {
            get 
            {
                if (mInputDevices != null)
                    return mInputDevices;
                else
                    return new IHubDevice[0];
            }
        }

        private ProcessManager mProcessManager;
        public iXnet.ControlPort.ProcessManager ProcessManager
        {
            get 
            {
                if (mProcessManager == null)
                    mProcessManager = new ProcessManager(Device.ControlPort);

                return mProcessManager; 
            }
        }

        #region Direct Access Properties

        public OperationMode OpMode
        {
            get { return mDevice.OperationMode; }
        }

        public string SerialNumber
        {
            get { return mDevice.SerialNumber; }
        }

        public string MACAddress
        {
            get { return mDevice.MacAddress.ToString(); }
        }

        public Version FWVersion
        {
            get { return mDevice.FirmwareVersion; }
        }

        public string IPAddress
        {
            get 
            {
                if (mPropertyTable != null)
                {
                    object address= mPropertyTable["IP Address"];
                    return address == null ? "0.0.0.0" : address.ToString();
                }
                else
                {
                    return String.Format("{0}", mDevice.Interface.Address);
                }
            }
        }

        public bool HasChanges
        {
            get { return mPropertyTable != null ? mPropertyTable.HasChanges : false; }
        }

        public int LedCount
        {
            get { return mDevice.MaxLedCount; }
        }

        public string UpTime
        {
            get
            {
                TimeSpan upTime = mDevice.UpTime;
                if (upTime == TimeSpan.MaxValue)
                    return "-";

                if (upTime.TotalDays >= 1.0)
                    return String.Format("{0:f1} days", upTime.TotalDays);
                else if (upTime.TotalHours >= 1.0)
                    return String.Format("{0:f1} hours", upTime.TotalHours);
                else if (upTime.TotalMinutes >= 1.0)
                    return String.Format("{0:f0} min", upTime.TotalMinutes);
                else
                    return String.Format("{0:f0} sec", upTime.TotalSeconds);
            }
        }

        public Feature Features
        {
            get
            {
                return mDevice.Features;
            }
        }

        #endregion


        public NetDevice(iXnetDevice device)
        {
            mDevice = device;
        }


        public void UpdateFromDevice()
        {
            mDevice.UpdateSettings();

            UpdatePropertyTable();
        }


        public void UpdateInputDevices()
        {
            try
            {
                if (mDevice.HasInputHub)
                    mInputDevices = mDevice.InputHub.GetHubDevices();
                else
                    DisposeInputDevices();
            }
            catch
            {
                DisposeInputDevices();
            }
        }

        private void DisposeInputDevices()
        {
            if (mInputDevices != null)
            {
                foreach (IHubDevice dev in mInputDevices)
                {
                    dev.Dispose();
                }
                mInputDevices = null;
            }
        }

        public void Reset()
        {
            try
            {
                mDevice.Reset();
            }
            catch
            {           	
            }
        }

        public void FactoryReset()
        {
            try
            {
                if (mDevice.HasControlPort)
                    mDevice.ControlPort.FactoryReset(FactoryResetFlags.Local);
            }
            catch
            {
            	
            }
        }

        public bool Show()
        {
            if (mDevice.HasLedPort)
                return mDevice.LedPort.SetLedState(-1,LedMode.LedBlinkNormal, LedColor.Red, null);

            return false;
        }
        
        public bool ShowLedIds()
        {
            for (int i = 0; i < 400; i++)
            {
                this.mDevice.LedPort.SetLedState(i, LedMode.LedOff, LedColor.Black, i.ToString());
            }
            return true;
        }

        public bool ClearAllLeds()
        {
            if (mDevice.HasLedPort)
                return mDevice.LedPort.ClearAllLeds();

            return false;
        }

        public bool SetLedState(int ledID, LedMode ledMode, LedColor ledColor, string text)
        {
            try
            {
                if (mDevice.HasLedPort)
                    return mDevice.LedPort.SetLedState(ledID, ledMode, ledColor, text);

                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool ResetLedBus()
        {
            try
            {
                if (mDevice.HasLedPort)
                    return mDevice.LedPort.ResetLedBus();

                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool CalibrateResetLedBus()
        {
            try
            {
                if (mDevice.HasLedPort)
                    return mDevice.LedPort.ResetCalibrateLedBus();

                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool TriggerLedBusFwUpdate(FwUpdateMode mode)
        {
            try
            {
                if (mDevice.HasControlPort)
                    return mDevice.ControlPort.TriggerUpdate(FwComponent.LedBus, mode);

                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public ErrorEntry[] ReadErrors()
        {
            try
            {
                if (mDevice.HasControlPort)
                    return mDevice.ControlPort.ReadErrors();

                return new ErrorEntry[0];
            }
            catch (System.Exception)
            {
                return new ErrorEntry[0];
            }
        }

        public LogEntry[] ReadLogs()
        {
            try
            {
                if (mDevice.HasControlPort)
                    return mDevice.ControlPort.ReadLogs();

                return new LogEntry[0];
            }
            catch (System.Exception)
            {
                return new LogEntry[0];
            }
        }

        public void DiscardChanges()
        {
            UpdatePropertyTable();
        }

        public void SetInterfaceProperties(InterfaceProperties props)
        {
            if (mPropertyTable != null)
            {
                mPropertyTable["IP Address"] = props.Address;
                mPropertyTable["Netmask"] = props.Netmask;
                mPropertyTable["Gateway"] = props.Gateway;
            }
        }

        private void WriteProperty(IProperty prop)
        {
            try
            {
                if (mDevice.ControlPort.CanSetProperty)
                    mDevice.ControlPort.SetProperty(prop);
            }
            catch
            {
            }
        }

        private void WriteInterface(InterfaceProperties interfaceProps)
        {
            try
            {
                if (mDevice.ControlPort.CanSetInterface)
                    mDevice.ControlPort.SetInterface(interfaceProps);
            }
            catch
            {
            	
            }
        }

        private void UpdatePropertyTable()
        {
            mPropertyTable = new PropertyTable();

            mPropertyTable.Properties.Add(CreateIPSpec("IP Address","Interface",mDevice.Interface.Address));
            mPropertyTable.Properties.Add(CreateIPSpec("Netmask", "Interface", mDevice.Interface.Netmask));
            mPropertyTable.Properties.Add(CreateIPSpec("Gateway", "Interface", mDevice.Interface.Gateway));

            SetInterfaceProperties(mDevice.Interface);

            if (mDevice.Properties != null)
            {
                foreach (IProperty prop in mDevice.Properties)
                {
                    PropertySpec propSpec = new PropertySpec(prop.DisplayName, prop.ValueType, "Properties", prop.Description, prop.Value);
                    if (prop.IsReadOnly)
                        propSpec.Attributes = new Attribute[] { new ReadOnlyAttribute(true) };

                    mPropertyTable.Properties.Add(propSpec);
                    mPropertyTable[prop.DisplayName] = prop.Value;
                }
            }

            mPropertyTable.ClearHasChanges();
        }

        private PropertySpec CreateIPSpec(string name, string category, object defaultValue)
        {
            return new PropertySpec(name, typeof(System.Net.IPAddress), category, null, defaultValue, (string)null, typeof(IPAddressTypeConverter).AssemblyQualifiedName);
        }

        public void ApplyChanges()
        {
            if (!HasChanges)
                return;

            InterfaceProperties interfaceProps = mDevice.Interface;
            bool hasChanges = false;
            hasChanges = CommitProperty("IP Address", ref interfaceProps.Address) || hasChanges;
            hasChanges = CommitProperty("Netmask", ref interfaceProps.Netmask) || hasChanges;
            hasChanges = CommitProperty("Gateway", ref interfaceProps.Gateway) || hasChanges;

            if (hasChanges)
                WriteInterface(interfaceProps);

            if (mDevice.Properties != null)
            {
                foreach (IProperty prop in mDevice.Properties)
                {
                    object value = prop.Value;
                    if (CommitProperty(prop.DisplayName, ref value))
                    {
                        prop.Value = value;
                        WriteProperty(prop);
                    }
                }
            }

            UpdateFromDevice();
        }

        private bool CommitProperty<T_PROP>(string name, ref T_PROP target)
        {
            T_PROP newVal = (T_PROP)mPropertyTable[name];
            if (Object.Equals(newVal, target))
                return false;

            target = newVal;
            return true;
        }

    
    }
}
