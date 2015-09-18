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

        public appSelectOPCServer(Sistema _system)
        {
            this._system = _system;

            this.Salir = true;
            InitializeComponent();

            this.comboBoxOPCServers.DataSource = OPCClient.GetServers();
            this.comboBoxOPCServers.SelectedIndexChanged += comboBoxOPCServers_SelectedIndexChanged;
        }

        void comboBoxOPCServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxOPCServers.SelectedIndex != -1)
            {
                if (this._system.OPCClient != null)
                {
                    this._system.OPCClient.Dispose();
                }
                this._system.OPCServerName = this.comboBoxOPCServers.SelectedItem.ToString();
                this._system.OPCClient     = new OPCClient(this._system.OPCServerName);
                this._system.OPCClient.Conectar();

                Branch branch = this._system.OPCClient.Browse();

                if (branch != null)
                {
                    TreeNode node = new TreeNode(branch.Name);
                    addNodes(node, branch);

                    this.treeViewBranch.BeginUpdate();
                    this.treeViewBranch.Nodes.Add(node);
                    this.treeViewBranch.Update();
                    this.treeViewBranch.EndUpdate();
                }
            }
        }

        private void addNodes(TreeNode t, Branch branch)
        {
            foreach (Branch b in branch.Children)
            {
                TreeNode node = new TreeNode(b.Name);
                t.Nodes.Add(node);
                addNodes(node, b);
            }
        }

        #region "BOTONES FLUJO"

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = true;
            this.Close();
        }
        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = true;
            this.Close();
        }

        #endregion
    }
}
