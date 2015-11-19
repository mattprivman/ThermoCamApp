using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC
{
    public class OPCAsyncWriteArgs
    {
        public AsyncWriteStruct operation { get; set; }
    }

    public struct AsyncWriteStruct  
    {
        public Branch branch;
        public int transactionID;
        public int cancelID;
    }
    public class OPCGroupValues     
    {
        public string             Path  { get; set; }
        public List<OPCItemValue> Items;

        public OPCGroupValues(string _path)
        {
            this.Path = _path;
            this.Items = new List<OPCItemValue>();
        }
    }
    public class OPCItemValue       
    {
        public string Item { get; set; }
        public object Value;

        public OPCItemValue(string _item, object _value)
        {
            this.Item = _item;
            this.Value = _value;
        }
    }

    public struct AsyncReadStruct
    {
        public Branch branch;
        public int transactionID;
        public int cancelID;

        public Hashtable HashtableActions;
    }
    public class OPCGroupRead      
    {
        public string Path { get; set; }
        public List<OPCItemRead> Items;

        public OPCGroupRead(string _path)
        {
            this.Path = _path;
            this.Items = new List<OPCItemRead>();
        }
    }
    public class OPCItemRead       
    {
        public string Item { get; set; }
        public Action<object> action;

        public OPCItemRead(string _item)
        {
            this.Item = _item;
        }
    }

    public class NoConnectedException : Exception
    {
        public override string Message
        {
            get
            {
                return "El servidor OPC no esta conectado";
            }
        }

        public override string ToString()
        {
            return this.Message;
        }
    }

}
