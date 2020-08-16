using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledybot
{
    public partial class BLInput : Form
    {

        public string input = "";

        public BLInput()
        {
            InitializeComponent();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            input = tb_FC.Text.PadLeft(12, '0');
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
