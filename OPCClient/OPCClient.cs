﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;

namespace OPC
{
    public class OPCClient
    {
        struct AsyncWriteStruct
        {
            public Branch   branch;
            public int      transactionID;
            public int      cancelID;
        }

        #region "Variables"

        //Servidor
        OPCServer _OPCServer;
        string    _serverName;

        bool      _Connected;

        //Estructura del servidor OPC
        OPCBrowser _OPCBrowser;
        Branch     _OPCBranch;

        int        _ClientHandle = 1;

        //Variable para los asyncWrites
        Hashtable  AsyncWrites;
        int        _transactionID = 0;

        #endregion

        #region "Propiedades"

        public string    Name                               
        {
            get
            {
                return this._serverName;
            }
        }
        private int     ClientHandle                        
        {
            get
            {
                this._ClientHandle++;
                return this._ClientHandle;
            }
        }

        #endregion

        #region "Delegados"


        #endregion

        #region "Eventos"

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public event EventHandler DataChanged;
        public event EventHandler AsyncWriteError;

        #endregion

        #region "Metodos Públicos"

        public OPCClient(string serverName)                         
        {
            AsyncWrites = new Hashtable();

            this._serverName = serverName;
            this._OPCServer  = new OPCServer();
        }

        public void Conectar()                                      
        {
            try
            {
                this._OPCServer.Connect(_serverName);
                this._OPCServer.ServerShutDown += _OPCServer_ServerShutDown;

                this._OPCBranch = new Branch(Name: _serverName);
                this._OPCBranch.DataChanged += _OPCBranch_DataChanged;

                this._OPCBrowser = this._OPCServer.CreateBrowser();

                this._Connected = true;

                if (Connected != null)
                    Connected(this, null);
            }
            catch (Exception ex)
            {
            }
        }

        public void Desconectar()                                   
        {
            if (_OPCBranch != null)
            {
                this._OPCBranch.Dispose();
                this._OPCBranch.DataChanged -= _OPCBranch_DataChanged;
            }
            if (_OPCServer != null)
            {
                this._OPCServer.Disconnect();
                this._OPCServer.ServerShutDown -= _OPCServer_ServerShutDown;
            }
            
            this._Connected = false;

            if (Disconnected != null)
                Disconnected(this, null);
        }
        public void Dispose()                                       
        {
            Desconectar();
            this.AsyncWrites.Clear();
            GC.SuppressFinalize(this._OPCServer);
        }

        public Branch Browse()                                      
        {
            if (_OPCServer != null)
            {
                if (_OPCBrowser != null && this._OPCServer.ServerState == (int) OPCServerState.OPCRunning)                   //COMPROBAR QUE EL SERVIDOR SOPORTA BROWSING
                {
                    browseBranch(_OPCBranch);
                    browseLeaf(_OPCBranch);
                }
            }

            return this._OPCBranch;
        }

        public bool WritteSync(string group, string item, object value) 
        {
            Branch b = GetBranch(group);

            if(b == null)
                throw new Exception("No se ha podido encontrar la rama especificada");
            
            try
            {
                if (this._OPCServer.ServerState == (int)OPCServerState.OPCRunning)
                {
                    if (b.Leafs.Exists(x => x.Name == item))
                    {
                        OPCItem i = (b.Leafs.Where(x => x.Name == item).First()).OPCItem;
                        i.Write(value);

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Fallo en la operación de escritura síncrona de la variable " + item + " en la ruta " + group + ".");
            }

            return false;
        }
        public void WriteAsync(String group, string item, object value) 
        {
            Branch b = GetBranch(group);

            if (b == null)
                throw new Exception("No se ha podido encontrar la rama especificada");

            if(b.Leafs.Exists((x) => x.Name == item))
            {
                Leaf l = b.Leafs.Where((x) => x.Name == item).First();

                Array serverHandles = Array.CreateInstance(typeof(int),
                    new int[1] { 1 },
                    new int[1] { 1 });

                serverHandles.SetValue(l.ServerHandle, 1);

                Array values = Array.CreateInstance(typeof(object),
                    new int[1] { 1 },
                    new int[1] { 1 });

                values.SetValue(value, 1);

                Array errors = Array.CreateInstance(typeof(int),
                    new int[1] { 1 },
                    new int[1] { 1 });

                AsyncWriteStruct a = new AsyncWriteStruct(){
                    branch = b,
                    transactionID = this._transactionID,
                };
                
                try
                {
                    this.AsyncWrites.Add(a.transactionID, a);
                    b.OPCGroup.AsyncWrite(1, ref serverHandles, ref values, out errors, a.transactionID, out a.cancelID);
                    this._transactionID++;
                }
                catch (Exception e)
                {
                    if (this.AsyncWrites.Contains(a.transactionID))
                        this.AsyncWrites.Remove(a.transactionID);

                    throw new Exception("Fallo en la operación de escritura asíncrona de la variable " + item + " en la ruta " + group + ".");
                }

            }
        }
        public void WriteAsync(OPCGroupValues gv)                       
        {
            Branch g = GetBranch(gv.Path);

            if (g == null)
                throw new Exception("No se ha podido encontrar la rama especificada");

            int count = 0; //Coincidencias
            #region "Buscar cuantos elementos coincidentes hay"

            for (int i = 0; i < gv.Items.Count; i++ )
            {
                if (g.Leafs.Exists(x => x.Name == gv.Items[i].Item))
                {
                    count++;
                }
            }
            #endregion

            #region "Crear arrays"
            Array serverHandles = Array.CreateInstance(typeof(int), 
                new int[1] { count }, 
                new int[1] { 1 });

            Array values = Array.CreateInstance(typeof(object),
                new int[1] { count },
                new int[1] { 1 });
            Array errors = Array.CreateInstance(typeof(int),
                new int[1] { count },
                new int[1] { 1 }); ;
            #endregion

            #region "Rellenar Arays"
            int index = 1;
            for (int i = 0; i < gv.Items.Count; i++)
            {
                if(g.Leafs.Exists(x => x.Name == gv.Items[i].Item))
                {
                    serverHandles.SetValue(
                        (g.Leafs.Where(x => x.Name == gv.Items[i].Item).First()).ServerHandle,
                        index);
                    values.SetValue(gv.Items[i].Value, index);
                    index++;
                }
            }
            #endregion

            #region "Estructura para controlar la operación asincrona"
            AsyncWriteStruct a = new AsyncWriteStruct()
                {
                    branch = g,
                    transactionID = this._transactionID,
                };

            g.OPCGroup.AsyncWriteComplete += OPCGroup_AsyncWriteComplete;
            #endregion

            try
            {
                //VBHelpers.Helpers.WritteAsync(g.OPCGroup);
                this.AsyncWrites.Add(a.transactionID, a);
                g.OPCGroup.AsyncWrite(count, ref serverHandles, ref values, out errors, a.transactionID, out a.cancelID);
                this._transactionID++;
            }
            catch (Exception e)
            {
                if (this.AsyncWrites.Contains(a.transactionID))
                    this.AsyncWrites.Remove(a.transactionID);

                throw new Exception("Fallo en la operación de escritura asíncrona con el grupo de variables " + gv.Path + ".");

            }
        }

        public object readSync(string path, string Item)            
        {
            object response;
            object Quality;
            object TimeStamp;

            Branch b = GetBranch(path);

            if (b == null)
                throw new Exception("No se ha podido encontrar la rama especificada");

            if (b.Leafs.Exists(x => x.Name == Item))
            {
                OPCItem i = b.Leafs.Where(x => x.Name == Item).First().OPCItem;

                i.Read((short)OPCDataSource.OPCDevice, out response, out Quality, out TimeStamp);

                if (response  != null)
                    return response;
            }

            throw new Exception("Fallo en la operación de lectura sincrona de la variable "+ Item + " con ruta " + path + ".");
        }

        public bool SuscribeGroup (string Group)                    
        {
            Branch b = GetBranch(Group);

            if (b != null)
            {
                b.Suscribe(true);
                return true;
            }

            return false;
        }
        public Branch GetBranch   (string Group)                    
        {
            if (this._OPCBrowser != null)
            {
                try
                {
                    string BranchName = Group;
                    Branch b = this._OPCBranch;

                    List<string> Niveles = new List<string>();

                    while (BranchName.Contains("."))
                    {
                        Niveles.Add(BranchName.Substring(0, BranchName.IndexOf(".")));
                        BranchName = BranchName.Substring(BranchName.IndexOf(".") + 1);
                    }

                    Niveles.Add(BranchName);

                    BranchName = "";

                    foreach (string s in Niveles)
                    {
                        BranchName = BranchName + s;

                        if (b.Children.Exists(x => x.Name == BranchName))
                        {
                            b = b.Children.Where(x => x.Name == BranchName).First();
                        }
                        else
                        {
                            return null;
                        }

                        BranchName = BranchName + ".";
                    }

                    return b;
                }
                catch (Exception ex)
                {
                    Conectar();
                    Browse();
                }
            }
            Browse();
            return null;
        }

        #endregion

        #region "Metodos privados"

        private void browseBranch (Branch Parent)                       
        {
            _OPCBrowser.ShowBranches();

            if (_OPCBrowser.Count > 0)
            {
                for (int i = 1; i <= _OPCBrowser.Count; i++)
                {
                    try
                    {
                        _OPCBrowser.MoveDown(_OPCBrowser.Item(i));

                        //string BranchName = _OPCBrowser.CurrentPosition;

                        //while(BranchName.Contains("."))
                        //{
                        //    BranchName = BranchName.Substring(_OPCBrowser.CurrentPosition.IndexOf(".") + 1);
                        //}

                        Branch b = new Branch(Name: _OPCBrowser.CurrentPosition, Server: this._OPCServer, Parent: Parent);

                        browseBranch(b);
                        browseLeaf(b);

                        _OPCBrowser.MoveUp();
                        _OPCBrowser.ShowBranches();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }
        }
        private void browseLeaf   (Branch Parent)                       
        {
            try
            {
                _OPCBrowser.ShowLeafs();

                if (_OPCBrowser.Count > 0)
                {
                    for (int i = 1; i <= _OPCBrowser.Count; i++)
                    {
                        Leaf l = new Leaf(Parent: Parent, Name: _OPCBrowser.Item(i), ClientHandle: this.ClientHandle, Browser: this._OPCBrowser);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OPCGroup_AsyncWriteComplete(int TransactionID, int NumItems, ref Array ClientHandles, ref Array Errors)
        {
            try
            {
                if (this.AsyncWrites.Contains(TransactionID))
                {
                    AsyncWriteStruct a = (AsyncWriteStruct)this.AsyncWrites[TransactionID];

                    for (int i = Errors.GetLowerBound(0); i <= Errors.GetUpperBound(0); i++)
                    {
                        if (Errors.GetValue(i) is int)
                        {
                            if ((int)Errors.GetValue(i) != 0)
                            {
                                Console.WriteLine(Errors.GetValue(i).ToString());
                                if (this.AsyncWriteError != null)
                                {
                                    this.AsyncWriteError(this, null);
                                }
                            }
                        }
                    }

                    this.AsyncWrites.Remove(TransactionID);
                    a.branch.OPCGroup.AsyncWriteComplete -= OPCGroup_AsyncWriteComplete;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void _OPCBranch_DataChanged(object sender, EventArgs e) 
        {
            //this._OPCBranch.Dispose();

            //this._OPCBranch = new Branch(Name: _serverName);
            //Browse();
        }
        private void _OPCServer_ServerShutDown(string Reason)           
        {
            this.Desconectar();
        }

        #endregion

        #region "Metodos Estáticos"

        public static List<string> GetServers()                 
        {
            List<string> serversList = new List<string>();

            OPCServer OPCServer = new OPCServer();

            Object servers =  OPCServer.GetOPCServers();

            foreach(string s in (Array)servers)
                    serversList.Add(s);

            return serversList;
        }

        #endregion
    }

    public class Branch
    {
        #region "Variables"

        private string          _Name;

        private bool            _HasOPCGroup;
        private OPCGroup        _OPCGroup;

        private bool            _HasParent;
        private Branch          _Parent;

        private bool            _HasChildren;
        private List<Branch>    _Children;

        private bool             _HasLeafs;
        private List<Leaf>      _Leafs;

        #endregion

        #region "Propiedades"

        public string           Name                            
        {
            get
            {
                return this._Name;
            }
        }

        public bool             HasOPCGroup                     
        {
            get
            {
                return this._HasOPCGroup;
            }
        }
        public OPCGroup         OPCGroup                        
        {
            get
            {
                return this._OPCGroup;
            }
        }

        public bool             HasParent                       
        {
            get
            {
                return this._HasParent;
            }
        }
        public Branch           Parent                          
        {
            get
            {
                return this._Parent;
            }
        }

        public bool             HasChildren                     
        {
            get
            {
                return this._HasChildren;
            }
        }
        public List<Branch>     Children                        
        {
            get
            {
                return this._Children;
            }
        }

        public bool HasLeafs
        {
            get
            {
                return this._HasLeafs;
            }
        }
        public List<Leaf>       Leafs                           
        {
            get
            {
                return this._Leafs;
            }
        }

        #endregion

        #region "Eventos"

        public event EventHandler DataChanged;

        #endregion

        #region "Metodos públicos"

        public Branch(String Name, OPCServer Server = null, Branch Parent = null) 
        {
            this._Name      = Name;
            this._Children  = new List<Branch>();
            this._Leafs     = new List<Leaf>();

            if (Parent != null)
            {
                this._HasParent     = true;
                this._Parent = Parent;
                Parent.addChildren(this);
            }
            else
            {
                this._HasParent     = false;
                this._Parent        = null;
            }

            if (Server != null)
            {
                this._OPCGroup = Server.OPCGroups.Add(this._Name);
                this._OPCGroup.UpdateRate   = 500;              //ms
                this._OPCGroup.IsActive     = true;             //Grupo activo
                this._OPCGroup.DataChange += _OPCGroup_DataChange;
                this._OPCGroup.IsSubscribed = true;
                this._HasOPCGroup           = true;
            }
            else
            {
                this._HasOPCGroup           = false;
                this._OPCGroup              = null;
            }

            this._HasChildren               = false;
            this._HasLeafs                  = false;
        }

        public void Branch_DataChanged(object sender, EventArgs e)
        {
            if (DataChanged != null)
                DataChanged(this, null);
        }

        public void Dispose()                                   
        {
            if(_HasOPCGroup)
                this._OPCGroup.DataChange -= _OPCGroup_DataChange;

            if (this._HasChildren)
            {
                foreach (Branch b in this._Children)
                    b.Dispose();

                this._Children.Clear();
                this._Children = null;
                this._HasChildren = false;
            }

            if (this._HasLeafs)
            {
                this._Leafs.Clear();
                this._Leafs = null;
                this._HasLeafs = false;
            }
        }

        public void Suscribe(bool status)
        {
            if (this._HasOPCGroup)
            {
                this._OPCGroup.DataChange += _OPCGroup_DataChange;
                this._OPCGroup.IsSubscribed = true;
            }
        }
        public void addChildren(Branch branch)                  
        {
            if (_HasChildren == false)
            {
                this._HasChildren    = true;
            }

            this._Children.Add(branch);
        }
        public void addLeaf(Leaf l)                             
        {
            if (this._HasLeafs == false)
                this._HasLeafs = true;

            this._Leafs.Add(l);
        }

        public Branch FindBranch(string Name)                   
        {
            foreach (Branch b in this._Children)
            {
                if (b.Name == Name)
                    return b;
                else
                    b.FindBranch(Name);
            }

            return null;
        }

        #endregion

        #region "Métodos privados"

        void _OPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            if (DataChanged != null)
                DataChanged(this, null);
        }

        #endregion
    }

    public class Leaf
    {
        #region "Variables"

        private string           _Name;
        private OPCItem          _OPCItem;
        private int              _ClientHandle;
        private int              _serverHandle;

        private Branch          _Parent;

        #endregion

        #region "Propiedades"

        public string           Name                    
        {
            get
            {
                return _Name;
            }
        }
        public OPCItem          OPCItem                 
        {
            get
            {
                return _OPCItem;
            }
        }
        public int              ClientHandle            
        {
            get
            {
                return this._ClientHandle;
            }
        }
        public int ServerHandle
        {
            get
            {
                return this._serverHandle;
            }
        }

        #endregion

        #region "Metodos públicos"

        public Leaf(Branch Parent, string Name, int ClientHandle, OPCBrowser Browser)
        {
            this._Parent            = Parent;
            this._Name              = Name;
            this._ClientHandle      = ClientHandle;

            this._Parent.addLeaf(this);

            this._OPCItem           = this._Parent.OPCGroup.OPCItems.AddItem(Browser.GetItemID(this._Name), this._ClientHandle);
            this._serverHandle      = this.OPCItem.ServerHandle;
            this._OPCItem.IsActive  = true;
        }

        #endregion
    }
}
