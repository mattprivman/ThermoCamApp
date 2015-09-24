using System;
using System.Collections.Generic;
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
            if (!this.checkBoxMax.Checked && !this.checkBoxMean.Checked && !this.checkBoxMin.Checked)
            {
                MessageBox.Show("Seleccione al menos una variable que desee importar Max Temp, Mean Temp o Min. Temp",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
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

            this.saveFileDialog1.FileName = "OPCVars";
            this.saveFileDialog1.Title = "Guardar archivo '*.csv' para servidor OPC";
            this.saveFileDialog1.Filter = "Archivo de valores separados por comas (*.csv)|*.csv";
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

            #region "Servidor OPC"
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
            #endregion

            this.saveFileDialog1.FileName = "TIAPortal Vars";
            this.saveFileDialog1.Title = "Guardar archivo '*.scl' para TIA PORTAL";
            this.saveFileDialog1.Filter = "Text Estructurado (*.scl)|*.scl";
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

            using (System.IO.StreamWriter w = new System.IO.StreamWriter(ruta, false))
            {
                //TIPO DE DATO
                w.WriteLine("//DATA TYPE");
                w.WriteLine("TYPE \"tempElement\"");
                w.WriteLine("VERSION : 0.1");
                w.WriteLine("   STRUCT");

                if(this.checkBoxMax.Checked)
                    w.WriteLine("       maxTemp : Int;");

                if(this.checkBoxMin.Checked)
                    w.WriteLine("       minTemp : Int;");

                if(this.checkBoxMean.Checked)
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

            #endregion

        }
    }
}
