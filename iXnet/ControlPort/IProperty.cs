using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public interface IProperty
    {
        int ID { get; }
        int RawLength { get; }

        string DisplayName { get; }
        string Description { get; }
        Type ValueType { get; }
        bool IsReadOnly { get; }

        object Value { get; set; }

        void SetRawValue(byte[] data);
        byte[] GetRawValue();
    }
}
