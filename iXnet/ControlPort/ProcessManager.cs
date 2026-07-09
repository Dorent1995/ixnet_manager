using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class ProcessManager
    {
        private ControlPortClient mCtrlPort;

        private Process[] mProcesses;
        public Process[] Processes
        {
            get { return mProcesses; }
        }

        public ProcessManager(ControlPortClient ctrlPort)
        {
            mCtrlPort = ctrlPort;
        }

        public bool UpdateListing()
        {
            Process[] procs = mCtrlPort.ReadProcesses();
            if (procs == null)
            {
                mProcesses = new Process[0];
                return false;
            }
            else
            {
                mProcesses = procs;
                return true;
            }
        }

        public bool DeleteProcess(Process proc)
        {
            if (mCtrlPort.DeleteProcess(proc.Name))
            {
                UpdateListing();
                return true;
            }
            return false;
        }

        public byte[] BackupProcess(Process proc)
        {
            if (proc == null)
                throw new ArgumentNullException("proc");

            // verify specified process
            UpdateListing();
            proc= Processes.FirstOrDefault(p => p.Name == proc.Name); 
            if (proc == null)
                return null;

            byte[] processData= mCtrlPort.ReadEEProm(proc.Address, proc.ByteSize);
            if (processData == null)
                return null;

            byte[] backup = new byte[processData.Length + 100];
            // put the name in front of the byte array
            Helper.PutStringIntoAscii(backup, 0, 100, proc.Name);

            Array.Copy(processData, 0, backup, 100, processData.Length);

            return backup;
        }

        public byte[] BackupEeprom()
        {
            var props = mCtrlPort.GetProperties();
            EEPromSizeProperty eepromSizeProp = props.OfType<EEPromSizeProperty>().FirstOrDefault();

            byte[] eeprom = mCtrlPort.ReadEEProm(0, (int)eepromSizeProp.EEPromSize);
            if (eeprom == null)
                return null;

            return eeprom;
        }

        public void RestoreEeprom(byte[] backupData)
        {
            var bytesWritten = mCtrlPort.WriteEEProm(0, backupData);
            return;
        }

        public string GetProcessNameFromBackup(byte[] backupData)
        {
            return Helper.GetStringFromAscii(backupData, 0, 100);
        }

        public bool RestoreProcess(byte[] backupData)
        {
            string name = GetProcessNameFromBackup(backupData);
            byte[] processData= new byte[backupData.Length - 100];
            Array.Copy(backupData,100,processData,0,processData.Length);

            mCtrlPort.DeleteProcess(name);

            UInt32 headerPos;
            if (!mCtrlPort.ReserveProcessSpace(name, (UInt32)processData.Length, out headerPos))
                return false;

            return mCtrlPort.WriteEEProm((int)headerPos, processData) == processData.Length;
        }

    }
}
