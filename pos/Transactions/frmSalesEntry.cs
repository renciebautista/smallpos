using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pos.Transactions
{
    public partial class frmSalesEntry : Form
    {
        DAL dal = new DAL();
        public frmSalesEntry()
        {
            InitializeComponent();
        }

        private void salesEntry_Load(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("F");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNewSales_Click(object sender, EventArgs e)
        {
            txtSku.Focus();
        }

        private void timerDayTime_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("F");
        }

        private void txtSku_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSku.Text == "")
                {
                    using (controls.frmFilter filter = new pos.controls.frmFilter())
                    {
                        filter.DataSource = dal.GetDataTable("SELECT code,description FROM items ORDER BY description");
                        filter.SearchFor = "Items";
                        filter.FieldId = "code";
                        filter.FieldDesc = "description";
                        filter.HeaderDesc = "Description";
                        filter.ShowDialog();
                    }
                }
                else
                {
                    txtSku.Text = "";
                }

                
            }
        }



    }
}
