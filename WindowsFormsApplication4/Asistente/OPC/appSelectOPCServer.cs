using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThermoVision.Models;
using OPC;

namespace WindowsFormsApplication4.Asistente.OPC
{
    public partial class appSelectOPCServer : flowControl
    {
        Sistema _system;

        public Sistema Sistema                                                      
        {
            get
            {
                return this._system;
            }
            set
            {
                this._system = value;
            }
        }

        public appSelectOPCServer(Sistema _system)                                  
        {           
            this._system = _system;

            this.Salir = true;
            InitializeComponent();

            this.comboBoxOPCServers.DataSource            = OPCClient.GetServers();
            this.comboBoxOPCServers.SelectedIndexChanged += comboBoxOPCServers_SelectedIndexChanged;

            this.treeViewBranch.AfterSelect              += treeViewBranch_AfterSelect;

            this.pictureBoxLogo.Image = System.Drawing.Bitmap.FromFile("Resources\\logo.jpg");

            this.textBoxInstrucciones.Text = "Seleccione el servidor OPC en el que va a realizar la configuración de la comunicación. \r\n\r\n" +
               "Indique también la zona de memoria a partir de la cual se van a insertar los registros que contendran las temperaturas de cada zona y de cada subzona. \r\n\r\n" +
               "A continuación seleccione las variables que desea comunicar con el PLC y genere los archivos que deberá importar en el servidor OPC y en el software de programación del PLC";

            if (this._system.OPCServerName != "")
            {
                this.comboBoxOPCServers.SelectedItem = this._system.OPCServerName;
            }

            if (this._system.Path != null && this._system.Path != "")
            {
                string BranchName = this._system.Path;

                List<string> Niveles = new List<string>();

                while (BranchName.Contains("."))
                {
                    Niveles.Add(BranchName.Substring(0, BranchName.IndexOf(".")));
                    BranchName = BranchName.Substring(BranchName.IndexOf(".") + 1);
                }

                Niveles.Add(BranchName);

                BranchName = "";

                this.treeViewBranch.Nodes[0].Expand();

                TreeNode n = this.treeViewBranch.Nodes[0];
                IEnumerable<TreeNode> node = n.Nodes.Cast<TreeNode>();
                
            }
        }

        #region "ITEMS"

        void treeViewBranch_AfterSelect(object sender, TreeViewEventArgs e)         
        {
            string path = this.treeViewBranch.SelectedNode.FullPath.Replace(@"\", ".");
            int count = this.treeViewBranch.Nodes[0].Text.Length + 1;

            if (count < path.Length)
            {
                path = path.Substring(count);
                bool startsWith_ = (this.treeViewBranch.SelectedNode.Text.StartsWith("_"));
                if (!startsWith_ && this.treeViewBranch.SelectedNode.Level > 1)
                {
                    this._system.Path = path;
                    unBoldNodes(this.treeViewBranch.Nodes[0]);
                    this.treeViewBranch.SelectedNode.NodeFont = new Font(this.treeViewBranch.Font, FontStyle.Bold);
                }

                Branch b = this._system.OPCClient.GetBranch(path);
                if (b != null)
                    fillLeafs(b);
            }
        }
        private void fillLeafs(Branch b)                                            
        {
            this.listBoxItems.BeginUpdate();
            this.listBoxItems.Items.Clear();

            foreach(Leaf l in b.Leafs)
                this.listBoxItems.Items.Add(l.Name);

            this.listBoxItems.EndUpdate();
            this.listBoxItems.Update();
        }

        void unBoldNodes(TreeNode node)                                             
        {
            foreach (TreeNode t in node.Nodes)
            {
                t.NodeFont = new Font(this.treeViewBranch.Font, FontStyle.Regular);
                unBoldNodes(t);
            }
        }

        #endregion

        #region "BRANCHES"

        void comboBoxOPCServers_SelectedIndexChanged(object sender, EventArgs e)    
        {
            if (this.comboBoxOPCServers.SelectedIndex != -1)
            {
                updateTreeViewBranches();
            }
        }
        private void updateTreeViewBranches()                                       
        {
            this.treeViewBranch.BeginUpdate();
            this.treeViewBranch.Nodes.Clear();

            if (this._system.OPCClient != null)
            {
                this._system.OPCClient.Dispose();
            }
            this._system.OPCServerName = this.comboBoxOPCServers.SelectedItem.ToString();
            this._system.OPCClient = new OPCClient(this._system.OPCServerName);
            this._system.OPCClient.Conectar();

            Branch branch = this._system.OPCClient.Browse();

            if (branch != null)
            {
                TreeNode node = new TreeNode(branch.Name);
                addNodes(node, branch);

                this.treeViewBranch.Nodes.Add(node);
            }
            this.treeViewBranch.Update();
            this.treeViewBranch.EndUpdate();
        }
        private void addNodes(TreeNode t, Branch branch)                            
        {
            foreach (Branch b in branch.Children)
            {
                int     lastIndex = b.Name.LastIndexOf(".");
                string  name      = b.Name.Substring(lastIndex + 1);

                TreeNode node = new TreeNode(name);
                t.Nodes.Add(node);
                addNodes(node, b);
            }
        }

        #endregion

        #region "BOTONES FLUJO"

        private void buttonBack_Click(object sender, EventArgs e)                   
        {
            if (this._system.OPCClient != null)
                this._system.OPCClient.Desconectar();

            this.Salir = false;
            this.Atras = true;
            this.Close();
        }
        private void buttonNext_Click(object sender, EventArgs e)                   
        {
            if(this._system.OPCClient != null)
                this._system.OPCClient.Desconectar();

            this.Salir = false;
            this.Atras = false;
            this.Close();
        }

        #endregion

        private void buttonGenerateOPCVars_Click(object sender, EventArgs e)        
        {
            if (this._system.Mode != "Rampas")
            {
                if (!this.checkBoxMax.Checked && !this.checkBoxMean.Checked && !this.checkBoxMin.Checked)
                {
                    MessageBox.Show("Seleccione al menos una variable que desee importar Max Temp, Mean Temp o Min. Temp",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

            if (!(this.treeViewBranch.SelectedNode != null && this.treeViewBranch.SelectedNode.Level > 1 && this.treeViewBranch.SelectedNode.Text.StartsWith("_") == false))
            {
                MessageBox.Show("Debe seleccionar un nodo de nivel 3",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string ruta;

            this.saveFileDialog1.FileName       = "OPCVars";
            this.saveFileDialog1.Title          = "Guardar archivo '*.csv' para servidor OPC";
            this.saveFileDialog1.Filter         = "Archivo de valores separados por comas (*.csv)|*.csv";
            this.saveFileDialog1.ShowDialog();
            this.saveFileDialog1.AddExtension   = true;
            ruta = this.saveFileDialog1.FileName;


            if (!(ruta != null && ruta != ""))
            {
                MessageBox.Show("Debe seleccionar una ruta valida",
                       "Error",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                return;

            }

            #region "Servidor OPC"

            switch (this._system.Mode)
            {
                case "Standart":
                                                                                //Generar variables para aplicación Standart para servidor OPC
                    generateStandartOPCVars(ruta);
                    break;

                case "Rampas":
                                                                                //Generar variables para aplicación de Rampas para servidor OPC
                    generateRampasOPCVars(ruta);
                    break;

                case "Tuberias":
                                                                                //Generar variables para aplicación de tuberias para servidor OPC
                    break;
            }

            #endregion

            this.saveFileDialog1.FileName = "TIAPortal Vars";
            this.saveFileDialog1.Title = "Guardar archivo '*.scl' para TIA PORTAL";
            this.saveFileDialog1.Filter = "Texto Estructurado (*.scl)|*.scl";
            this.saveFileDialog1.ShowDialog();
            this.saveFileDialog1.AddExtension = true;
            ruta = this.saveFileDialog1.FileName;


            if (!(ruta != null && ruta != ""))
            {
                MessageBox.Show("Debe seleccionar una ruta valida",
                       "Error",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                return;

            }

            #region "Siemens"

            switch (this._system.Mode)
            {
                case "Standart":
                                                                        //Generar standart scl data blocks for TIA Portal
                    generateStandartSIEMENSscl(ruta); 
                    break;

                case "Rampas":
                                                                        //Generar RAMPAS scl data blocks for TIA Portal
                    generateRampasIEMENSscl(ruta);
                    break;

                case "Tuberias":
                    
                    break;
            }

            #endregion

        }

        #region "Aplicación Standart"
        private void generateStandartOPCVars(string ruta)                           
        {
            using (System.IO.StreamWriter w = new System.IO.StreamWriter(ruta, false))
            {
                w.WriteLine("Tag Name,Address,Data Type,Respect Data Type,Client Access,Scan Rate,Scaling,Raw Low,Raw High,Scaled Low,Scaled High,Scaled Data Type,Clamp Low,Clamp High,Eng Units,Description,Negate Value");

                int index = int.Parse(this.numericTextBOffset.Texto);

                foreach (Zona z in this._system.Zonas)
                {
                    //TEMPERATURAS ZONAS
                    if (this.checkBoxMax.Checked)
                    {
                        w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + "Max" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                        index = index + 2;
                    }
                    if (this.checkBoxMin.Checked)
                    {
                        w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + "Min" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                        index = index + 2;
                    }
                    if (this.checkBoxMean.Checked)
                    {
                        w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + "Mean" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                        index = index + 2;
                    }

                    foreach (SubZona s in z.Children)
                    {
                        //TEMPERATURAS SUBZONAS
                        if (this.checkBoxMax.Checked)
                        {
                            w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + s.Nombre + "." + "Max" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                            index = index + 2;
                        }
                        if (this.checkBoxMin.Checked)
                        {
                            w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + s.Nombre + "." + "Min" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                            index = index + 2;
                        }
                        if (this.checkBoxMean.Checked)
                        {
                            w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + s.Nombre + "." + "Mean" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                            index = index + 2;
                        }

                        for (int x = 0; x < s.Columnas; x++)
                        {
                            for (int y = 0; y < s.Filas; y++)
                            {
                                if (this.checkBoxMax.Checked)
                                {
                                    w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + s.Nombre + "." + "MAX." + "[" + x + "," + y + "]" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                                    index = index + 2;
                                }
                                if (this.checkBoxMean.Checked)
                                {
                                    w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + s.Nombre + "." + "MEAN." + "[" + x + "," + y + "]" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                                    index = index + 2;
                                }
                                if (this.checkBoxMin.Checked)
                                {
                                    w.WriteLine("\"TEMPERATURES." + z.Nombre + "." + s.Nombre + "." + "MIN." + "[" + x + "," + y + "]" + "\",\"" + this.textBoxInitialMem.Text + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                                    index = index + 2;
                                }
                            }
                        }
                    }
                }
                w.Close();
                w.Dispose();

            }
        }
        private void generateStandartSIEMENSscl(string ruta)                        
        {
            using (System.IO.StreamWriter w = new System.IO.StreamWriter(ruta, false))
            {
                //TIPO DE DATO
                w.WriteLine("//DATA TYPE");
                w.WriteLine("TYPE \"tempElement\"");
                w.WriteLine("VERSION : 0.1");
                w.WriteLine("   STRUCT");

                if (this.checkBoxMax.Checked)
                    w.WriteLine("       maxTemp : Int;");

                if (this.checkBoxMin.Checked)
                    w.WriteLine("       minTemp : Int;");

                if (this.checkBoxMean.Checked)
                    w.WriteLine("       meanTemp : Int;");

                w.WriteLine("   END_STRUCT;");
                w.WriteLine("END_TYPE");

                w.WriteLine("");
                w.WriteLine("");

                //BLOQUE DE DATOS
                w.WriteLine("//DATABLOCK");
                w.WriteLine("DATA_BLOCK \"DataTemp\"");
                w.WriteLine("{ S7_Optimized_Access := 'FALSE' }");
                w.WriteLine("VERSION: 0.1");
                w.WriteLine("NON_RETAIN");
                w.WriteLine("   VAR");

                foreach (Zona z in this._system.Zonas)
                {
                    w.WriteLine("       {0} : \"tempElement\";", z.Nombre);

                    foreach (SubZona s in z.Children)
                    {
                        w.WriteLine("       {0} : \"tempElement\";", s.Nombre);
                        w.WriteLine("       {0}_Matrix : Array[0..{1}, 0..{2}] of \"tempElement\";", s.Nombre, s.Filas - 1, s.Columnas - 1);
                    }
                }
                w.WriteLine("   END_VAR");
                w.WriteLine("END_DATA_BLOCK");

                w.Close();
                w.Dispose();
            }
        }
        #endregion

        #region "Aplicación de rampas"
        private void generateRampasOPCVars(string ruta)                             
        {
            using(System.IO.StreamWriter w = new System.IO.StreamWriter(ruta, false))
            {
                w.WriteLine("Tag Name,Address,Data Type,Respect Data Type,Client Access,Scan Rate,Scaling,Raw Low,Raw High,Scaled Low,Scaled High,Scaled Data Type,Clamp Low,Clamp High,Eng Units,Description,Negate Value");

                int blockIndex = 3;

                foreach (Zona z in this._system.Zonas)
                {
                    int index = 0;

                    w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + "Cooling" + "\",\"DB" + blockIndex + ".DBX2." + index + "\",Bool,1,R/W,100,,,,,,,,,,\"\",");
                    index++;
                    w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + "Activar" + "\",\"DB" + blockIndex + ".DBX2." + index + "\",Bool,1,R/W,100,,,,,,,,,,\"\",");
                    index++;

                    index = 4;

                    //COORDENADAS DE POSICIÓN DEL CAÑON
                    w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + "X" + "\",\"DB" + blockIndex + ".DBW" + index + "\",Int,1,R/W,100,,,,,,,,,,\"\",");
                    index += 2;
                    w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + "Y" + "\",\"DB" + blockIndex + ".DBW" + index + "\",Int,1,R/W,100,,,,,,,,,,\"\",");
                    index += 2;
                    w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + "n" + "\",\"DB" + blockIndex + ".DBW" + index + "\",Int,1,R/W,100,,,,,,,,,,\"\",");
                    index += 2;

                    foreach (SubZona s in z.Children)
                    {
                        //LIMITES DE LA MATRIZ
                        w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + "Filas_" + s.Nombre + "\",\"DB" + blockIndex + ".DBW" + index + "\",Int,1,R/W,100,,,,,,,,,,\"\",");
                        index += 2;
                        w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + "Columnas_" + s.Nombre + "\",\"DB" + blockIndex + ".DBW" + index + "\",Int,1,R/W,100,,,,,,,,,,\"\",");
                        index += 2;

                        for (int x = 0; x < s.Filas; x++)
                        {
                            for (int y = 0; y < s.Columnas; y++)
                            {
                                w.WriteLine("\"RAMPAS.APAGADO." + z.Nombre + "." + s.Nombre + "." + "[" + x + "," + y + "]" + "\",\"DB" + blockIndex + ".DBW" + index + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                                index +=2;
                            } //for
                        } //for
                    } //foreach

                    blockIndex +=3;
                }

                blockIndex -= 2;

                foreach (Zona z in this._system.ZonasVaciado)
                {
                    int direccion = 0;
                    int bit       = 0;

                    blockIndex++;

                    w.WriteLine("\"RAMPAS.VACIADO." + z.Nombre + "." + "Emptying" + "\",\"DB" + blockIndex + ".DBX" + direccion + "." + bit + "\",Bool,1,R/W,100,,,,,,,,,,\"\",");
                    bit++; if (bit > 7) { direccion++; bit = 0; }
                    w.WriteLine("\"RAMPAS.VACIADO." + z.Nombre + "." + "Activar" + "\",\"DB" + blockIndex + ".DBX " + direccion + "." + bit + "\",Bool,1,R/W,100,,,,,,,,,,\"\",");
                    bit++; if (bit > 7) { direccion++; bit = 0; }

                    direccion = 2;
                    bit       = 0;

                    foreach (SubZona s in z.Children)
                    {
                        for (int x = 0; x < s.Columnas; x++)
                        {
                            for (int y = 0; y < s.Filas; y++)
                            {
                                w.WriteLine("\"RAMPAS.VACIADO." + z.Nombre + "." + s.Nombre + "." + "[" + x + "," + y + "]" + "\",\"DB" + blockIndex + ".DBX" + direccion + "." + bit + "\",Bool,1,R/W,100,,,,,,,,,,\"\",");
                                bit++; if (bit > 7) { direccion++; bit = 0; }
                            } //for
                        } //for
                    } //foreach subzona

                    blockIndex++;
                } //foreach zona

                w.Close();
            }//using
        }
        private void generateRampasIEMENSscl(string ruta)                           
        {
            using (System.IO.StreamWriter w = new System.IO.StreamWriter(ruta, false))
            {
                
                //////////////////////////////////////////////////////////////////////////////////////////////APAGADO
                foreach (Zona z in this._system.Zonas)
                {   
                    ////////////////////////////////////////////////////////// TON1_DB
                    w.WriteLine("////////////////////////////////////////////////////////// TON1_DB");
                    w.WriteLine("");
                    w.WriteLine("DATA_BLOCK \"{0}_TON_1\"", z.Nombre);
                    w.WriteLine("{OriginalPartName := 'IEC_TIMER';");
                    w.WriteLine("");
                    w.WriteLine("VersionGUID := 'b68d17d6-3fcc-4468-818a-b36d847990bb';");
                    w.WriteLine("S7_Optimized_Access := 'TRUE' }");
                    w.WriteLine("AUTHOR : Simatic");
                    w.WriteLine("FAMILY : IEC");
                    w.WriteLine("NAME : IEC_TMR");
                    w.WriteLine("VERSION : 1.0");
                    w.WriteLine("NON_RETAIN");
                    w.WriteLine("IEC_TIMER");
                    w.WriteLine("");
                    w.WriteLine("BEGIN");
                    w.WriteLine("");
                    w.WriteLine("END_DATA_BLOCK");
                    w.WriteLine("");
                    ///////////////////////////////////////////////////////// TON2_DB
                    w.WriteLine("///////////////////////////////////////////////////////// TON2_DB");
                    w.WriteLine("");
                    w.WriteLine("DATA_BLOCK \"{0}_TON_2\"", z.Nombre);
                    w.WriteLine("{OriginalPartName := 'IEC_TIMER';");
                    w.WriteLine("VersionGUID := 'b68d17d6-3fcc-4468-818a-b36d847990bb';");
                    w.WriteLine("S7_Optimized_Access := 'TRUE' }");
                    w.WriteLine("AUTHOR : Simatic");
                    w.WriteLine("FAMILY : IEC");
                    w.WriteLine("NAME : IEC_TMR");
                    w.WriteLine("VERSION : 1.0");
                    w.WriteLine("NON_RETAIN");
                    w.WriteLine("IEC_TIMER");
                    w.WriteLine("");
                    w.WriteLine("BEGIN");
                    w.WriteLine("");
                    w.WriteLine("END_DATA_BLOCK");
                    w.WriteLine("");
                    //////////////////////////////////////////////////////////// Function Block
                    w.WriteLine("//////////////////////////////////////////////////////////// Function Block");
                    w.WriteLine("");
                    w.WriteLine("FUNCTION_BLOCK \"{0}_FB\"", z.Nombre);
                    w.WriteLine("{ S7_Optimized_Access := 'FALSE' }");
                    w.WriteLine("VERSION : 0.1");
                    w.WriteLine("   VAR_INPUT ");
                    w.WriteLine("       maxTemp : Int;");
                    w.WriteLine("   END_VAR");
                    w.WriteLine("");
                    w.WriteLine("   VAR ");
                    w.WriteLine("       Cooling : Bool := false;");
                    w.WriteLine("       Activar : Bool := false;");
                    w.WriteLine("       X : Int;");
                    w.WriteLine("       Y : Int;");
                    w.WriteLine("       n : Int;");

                    int contador = 0;

                    foreach (SubZona s in z.Children)
                    {
                        w.WriteLine("       Filas_{0} : Int := {1};", contador, s.Filas - 1);
                        w.WriteLine("       Columnas_{0} : Int := {1};", contador, s.Columnas - 1);
                        w.WriteLine("       tempMatrix_{0} : Array[0..{1}, 0..{2}] of Int;", contador, s.Filas - 1, s.Columnas - 1);

                        contador++;
                    }
                    w.WriteLine("       ascender : Bool;");
                    w.WriteLine("       matrices : Int := {0};", z.Children.Count);
                    w.WriteLine("       contador : Int;");
                    w.WriteLine("   END_VAR");
                    w.WriteLine("");
                    w.WriteLine("BEGIN");
                    w.WriteLine("");
                    w.WriteLine("\"{0}_TON_1\".TON(IN := #Activar AND NOT #Cooling,", z.Nombre);
                    w.WriteLine("                 PT := t#10s);");
                    w.WriteLine("");
                    w.WriteLine("IF \"{0}_TON_1\".Q = TRUE THEN", z.Nombre);
                    w.WriteLine("   #Cooling := TRUE;");
                    w.WriteLine("   #Activar := FALSE;");
                    w.WriteLine("   #ascender := TRUE;");
                    w.WriteLine("   #contador := 0;");
                    w.WriteLine("   #X := 0;");
                    w.WriteLine("   #Y := 0;");
                    w.WriteLine("   #n := 0;");
                    w.WriteLine("END_IF;");
                    w.WriteLine("");
                    w.WriteLine("IF #Cooling = TRUE THEN");

                    contador = 0;

                    foreach (SubZona s in z.Children)
                    {
                        w.WriteLine("IF #n = {0} THEN", contador);
                        w.WriteLine("   IF #tempMatrix_{0}[#X, #Y] >= #maxTemp THEN", contador);
                        w.WriteLine("");
                        w.WriteLine("       \"{0}_TON_2\".TON(IN := TRUE,", z.Nombre);
                        w.WriteLine("                   PT := t#10s);");
                        w.WriteLine("");
                        w.WriteLine("       IF \"{0}_TON_2\".Q = TRUE THEN", z.Nombre);
                        w.WriteLine("");
                        w.WriteLine("           //SIGUIENTE COORDENADA");
                        w.WriteLine("           IF #ascender = TRUE THEN");
                        w.WriteLine("               IF #Y = #Columnas_{0} THEN", contador);
                        w.WriteLine("                   #X := #X + 1;");
                        w.WriteLine("                   #ascender := FALSE;");
                        w.WriteLine("               ELSE");
                        w.WriteLine("                   #Y := #Y + 1;");
                        w.WriteLine("               END_IF;");
                        w.WriteLine("           ELSE");
                        w.WriteLine("               IF #Y = 0 THEN");
                        w.WriteLine("                   #X := #X + 1;");
                        w.WriteLine("                   #ascender := TRUE;");
                        w.WriteLine("               ELSE");
                        w.WriteLine("                   #Y := #Y - 1;");
                        w.WriteLine("               END_IF;");
                        w.WriteLine("           END_IF;");
                        w.WriteLine("");
                        w.WriteLine("           #contador := #contador + 1;");
                        w.WriteLine("           IF #contador = ((#Filas_{0} + 1) * (#Columnas_{0} + 1)) THEN", contador);
                        w.WriteLine("               #contador := 0;");
                        w.WriteLine("               #n := #n + 1;");
                        w.WriteLine("               #X := 0;");
                        w.WriteLine("               #Y := 0;");
                        w.WriteLine("           END_IF;");
                        w.WriteLine("");
                        w.WriteLine("       \"{0}_TON_2\".TON(IN := FALSE,", z.Nombre);
                        w.WriteLine("                         PT := t#2s);");
                        w.WriteLine("       END_IF;");
                        w.WriteLine("   ELSE");
                        w.WriteLine("       //SIGUIENTE COORDENADA");
                        w.WriteLine("       IF #ascender = TRUE THEN");
                        w.WriteLine("           IF #Y = #Columnas_{0} THEN", contador);
                        w.WriteLine("               #X := #X + 1;");
                        w.WriteLine("               #ascender := FALSE;");
                        w.WriteLine("           ELSE");
                        w.WriteLine("               #Y := #Y + 1;");
                        w.WriteLine("           END_IF;");
                        w.WriteLine("       ELSE");
                        w.WriteLine("           IF #Y = 0 THEN");
                        w.WriteLine("               #X := #X + 1;");
                        w.WriteLine("               #ascender := TRUE;");
                        w.WriteLine("           ELSE");
                        w.WriteLine("               #Y := #Y - 1;");
                        w.WriteLine("           END_IF;");
                        w.WriteLine("       END_IF;");
                        w.WriteLine("");
                        w.WriteLine("       #contador := #contador + 1;");
                        w.WriteLine("       IF #contador = (#Filas_{0} + 1) * (#Columnas_{0} + 1) THEN", contador);
                        w.WriteLine("           #contador := 0;");
                        w.WriteLine("           #n := #n + 1;");
                        w.WriteLine("           #X := 0;");
                        w.WriteLine("           #Y := 0;");
                        w.WriteLine("       END_IF;");
                        w.WriteLine("");
                        w.WriteLine("   END_IF;");
                        w.WriteLine("END_IF;");

                        contador++;
                    }

                    w.WriteLine("END_IF;");
                    w.WriteLine("");

                    int acumulado = 0;

                    foreach (SubZona s in z.Children)
                        acumulado += s.Filas * s.Columnas;

                    w.WriteLine("       IF (#n = #matrices) THEN");
                    w.WriteLine("           #Cooling := FALSE;");
                    w.WriteLine("       END_IF;");
                    w.WriteLine("");
                    w.WriteLine("END_FUNCTION_BLOCK");
                    w.WriteLine("");
                    ////////////////////////////////////////////////////////// Function Block DB
                    w.WriteLine("////////////////////////////////////////////////////////// Function Block DB");
                    w.WriteLine("");
                    w.WriteLine("DATA_BLOCK \"{0}_DB\"", z.Nombre);
                    w.WriteLine("{ S7_Optimized_Access := 'FALSE' }");
                    w.WriteLine("VERSION : 0.1");
                    w.WriteLine("NON_RETAIN");
                    w.WriteLine("\"{0}_FB\"", z.Nombre);
                    w.WriteLine("");
                    w.WriteLine("BEGIN");
                    w.WriteLine("");
                    w.WriteLine("END_DATA_BLOCK");
                    w.WriteLine("");
                }

                //////////////////////////////////////////////////////////////////// VACIADO
                foreach (Zona z in this._system.ZonasVaciado)
                {
                    ////////////////////////////////////////////////////////// TON1_DB
                    w.WriteLine("////////////////////////////////////////////////////////// TON1_DB");
                    w.WriteLine("");
                    w.WriteLine("DATA_BLOCK \"{0}_TON_1\"", z.Nombre);
                    w.WriteLine("{OriginalPartName := 'IEC_TIMER';");
                    w.WriteLine("");
                    w.WriteLine("VersionGUID := 'b68d17d6-3fcc-4468-818a-b36d847990bb';");
                    w.WriteLine("S7_Optimized_Access := 'TRUE' }");
                    w.WriteLine("AUTHOR : Simatic");
                    w.WriteLine("FAMILY : IEC");
                    w.WriteLine("NAME : IEC_TMR");
                    w.WriteLine("VERSION : 1.0");
                    w.WriteLine("NON_RETAIN");
                    w.WriteLine("IEC_TIMER");
                    w.WriteLine("");
                    w.WriteLine("BEGIN");
                    w.WriteLine("");
                    w.WriteLine("END_DATA_BLOCK");
                    w.WriteLine("");

                    //////////////////////////////////////////////////////////// Function Block
                    w.WriteLine("//////////////////////////////////////////////////////////// Function Block");
                    w.WriteLine("");
                    w.WriteLine("FUNCTION_BLOCK \"{0}_FB\"", z.Nombre);
                    w.WriteLine("{ S7_Optimized_Access := 'FALSE' }");
                    w.WriteLine("VERSION : 0.1");
                    w.WriteLine("   VAR ");
                    w.WriteLine("       Emptying : Bool := false;");
                    w.WriteLine("       Activar : Bool := false;");
                    
                    foreach(SubZona s in z.Children)
                        w.WriteLine("       tempMatrix : Array[0..{0}, 0..{1}] of Bool;", s.Filas - 1, s.Columnas - 1);

                    w.WriteLine("   END_VAR");
                    w.WriteLine("");
                    w.WriteLine("BEGIN");
                    w.WriteLine("");
                    w.WriteLine("IF #Emptying = FALSE THEN");
                    w.WriteLine("   \"{0}_TON_1\".TON(IN := #Activar,", z.Nombre);
                    w.WriteLine("                   PT := t#10s);");
                    w.WriteLine("   IF \"{0}_TON_1\".Q = TRUE THEN", z.Nombre);
                    w.WriteLine("       #Emptying := TRUE;");
                    w.WriteLine("       \"{0}_TON_1\".TON(IN := FALSE,", z.Nombre);
                    w.WriteLine("                       PT := t#10s);");
                    w.WriteLine("   END_IF;");
                    w.WriteLine("ELSE");
                    w.WriteLine("   \"{0}_TON_1\".TON(IN := #Emptying,", z.Nombre);
                    w.WriteLine("                   PT := t#10s);");
                    w.WriteLine("   IF \"{0}_TON_1\".Q = TRUE THEN", z.Nombre);
                    w.WriteLine("       #Emptying := FALSE;");
                    w.WriteLine("       \"{0}_TON_1\".TON(IN := FALSE,", z.Nombre);
                    w.WriteLine("                       PT := t#10s);");
                    w.WriteLine("   END_IF;");
                    w.WriteLine("END_IF;");
                    w.WriteLine("END_FUNCTION_BLOCK");
                    w.WriteLine("");

                    ////////////////////////////////////////////////////////// Function Block DB
                    w.WriteLine("////////////////////////////////////////////////////////// Function Block DB");
                    w.WriteLine("");
                    w.WriteLine("DATA_BLOCK \"{0}_DB\"", z.Nombre);
                    w.WriteLine("{ S7_Optimized_Access := 'FALSE' }");
                    w.WriteLine("VERSION : 0.1");
                    w.WriteLine("NON_RETAIN");
                    w.WriteLine("\"{0}_FB\"", z.Nombre);
                    w.WriteLine("");
                    w.WriteLine("BEGIN");
                    w.WriteLine("");
                    w.WriteLine("END_DATA_BLOCK");
                    w.WriteLine("");
                }

                /////////////////////////////////////////////////////Main
                w.WriteLine("/////////////////////////////////////////////////////Main");
                w.WriteLine("ORGANIZATION_BLOCK \"Main\"");
                w.WriteLine("TITLE = \"Main Program Sweep (Cycle)\"");
                w.WriteLine("{ S7_Optimized_Access := 'TRUE' }");
                w.WriteLine("VERSION : 0.1");
                w.WriteLine("");
                w.WriteLine("BEGIN");
                foreach (Zona z in this._system.Zonas)
                    w.WriteLine("   \"{0}_DB\"({1});", z.Nombre, this._system.estados.tempLimiteHayQueEnfriar);
                foreach (Zona z in this._system.ZonasVaciado)
                    w.WriteLine("   \"{0}_DB\"();", z.Nombre);
                w.WriteLine("");
                w.WriteLine("END_ORGANIZATION_BLOCK");
                w.WriteLine("");

            }//using
        }
        #endregion
    }
}
