using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermoVision.CustomControls
{
    public partial class NumericTextBox : UserControl
    {
        public int maxVal;
        public int minVal = 0;

        public event System.EventHandler textoCambiado;

        public string Texto                                     
        {
            get
            {
                if (this.textBox1.Text == "")
                    return "0";
                else
                return this.textBox1.Text;
            }

            set
            {
                updateText(this.textBox1, value);
            }
        }

        public NumericTextBox()                                 
        {
            InitializeComponent();
            this.textBox1.KeyDown       += textBox1_KeyDown;
            this.textBox1.TextChanged   += textBox1_TextChanged;
            this.textBox1.Click         += textBox1_Click;
        }

        void textBox1_TextChanged(object sender, EventArgs e)   
        {
            if (textBox1.Text != "")
            {
                if (int.Parse(textBox1.Text) > this.maxVal)
                    this.textBox1.Text = this.maxVal.ToString();
                if (int.Parse(textBox1.Text) < this.minVal)
                    this.textBox1.Text = this.minVal.ToString();
            }

            if (textoCambiado != null)
                textoCambiado(this, null);
        }

        void textBox1_Click(object sender, EventArgs e)         
        {
            this.textBox1.SelectAll();
        }

        void textBox1_KeyDown(object sender, KeyEventArgs e)    
        {
            if (!((e.KeyValue >= 48 && e.KeyValue <= 57) || e.KeyValue == 8 || e.KeyValue == 46))
            {
                e.SuppressKeyPress = true;
            }
        }

        private delegate void updateTextCallback(object o, string text);
        private void updateText(object o, string text)          
        {
            if (o is Control)
            {
                Control c = (Control)o;

                if (c.InvokeRequired)
                {
                    c.BeginInvoke(new updateTextCallback(updateText), o, text);
                }
                else
                {
                    try
                    {
                        c.Text = text;
                        c.Refresh();
                    }
                    catch (Exception e) { e.ToString(); }
                }
            }
        }

        public void suscribeEvent(EventHandler function)        
        {
            if (this.textoCambiado == null)
                this.textoCambiado += function;
        }
        public void unsuscribeEvent(EventHandler function)      
        {
            while (this.textoCambiado != null)
                this.textoCambiado -= function;
        }

        public void increment()
        {
            this.Texto = (int.Parse(this.Texto) + 1).ToString();
        }

        public void decrement()
        {
            this.Texto = (int.Parse(this.Texto) - 1).ToString();
        }

    }
}
