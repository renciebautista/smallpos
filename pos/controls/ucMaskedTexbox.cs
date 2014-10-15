using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMS.controls
{
    public partial class ucMaskedTexbox : MaskedTextBox
    {
        private Color objForeColor;
        private Color objHoverForeColor;

        public ucMaskedTexbox()
        {
            InitializeComponent();
        }

        public Color HoverForeColor
        {
            set { objHoverForeColor = value; }
            get { return objHoverForeColor; }
        }

        private void ucMaskedTexbox_Enter(object sender, EventArgs e)
        {
            objForeColor = this.ForeColor;
            this.ForeColor = objHoverForeColor;
        }

        private void ucMaskedTexbox_Leave(object sender, EventArgs e)
        {
            this.ForeColor = objForeColor;
        }
    }
}
