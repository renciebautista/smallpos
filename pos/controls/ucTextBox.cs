using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pos.controls
{
    public partial class ucTextBox : TextBox
    {
        private Color objForeColor;
        private Color objHoverForeColor;

        public ucTextBox()
        {
            InitializeComponent();
        }

        public Color HoverForeColor
        {
            set { objHoverForeColor = value; }
            get { return objHoverForeColor; }
        }

        private void ucTextBox_Enter(object sender, EventArgs e)
        {
            objForeColor = this.ForeColor;
            this.ForeColor = objHoverForeColor;
        }

        private void ucTextBox_Leave(object sender, EventArgs e)
        {
            this.ForeColor = objForeColor;
        }
    }
}
