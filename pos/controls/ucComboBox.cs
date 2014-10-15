using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMS.controls
{
    public partial class ucComboBox : ComboBox
    {
        private Color objForeColor;
        private Color objHoverForeColor;

        public ucComboBox()
        {
            InitializeComponent();
        }

        public Color HoverForeColor
        {
            set { objHoverForeColor = value; }
            get { return objHoverForeColor; }
        }

        private void ucComboBox_Enter(object sender, EventArgs e)
        {
            objForeColor = this.ForeColor;
            this.ForeColor = objHoverForeColor;
        }

        private void ucComboBox_Leave(object sender, EventArgs e)
        {
            this.ForeColor = objForeColor;
        }
    }
}
