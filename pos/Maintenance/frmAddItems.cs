using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace pos.Maintenance
{
    public partial class frmAddItems : Form
    {
        DAL dal = new DAL();

        public frmAddItems()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddItems_Load(object sender, EventArgs e)
        {
            DataTable dtDepartment = dal.GetDataTable("SELECT * FROM item_department ORDER BY dept_desc");
            cmbDepartment.DataSource = dtDepartment;
            cmbDepartment.DisplayMember = "dept_desc";
            cmbDepartment.ValueMember = "id";

            //DataTable dtCategory = dal.GetDataTable("SELECT * FROM item_category ORDER BY cat_desc");
            //cmbCategory.DataSource = dtCategory;
            //cmbCategory.DisplayMember = "cat_desc";
            //cmbCategory.ValueMember = "id";
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string query = "INSERT into items (code,description,dept_id,cost,srp) VALUES (@code,@description,@dept_id,@cost,@srp)";
            dal.ExecuteNonQuery(query,
                new SqlParameter("@code", txtCode.Text.Trim()),
                new SqlParameter("@description", txtDesc.Text.Trim()),
                new SqlParameter("@dept_id", cmbDepartment.SelectedValue),
                new SqlParameter("@cost", txtCost.Text.Trim()),
                new SqlParameter("@srp", txtSrp.Text.Trim()));

            this.DialogResult = DialogResult.OK;
        }
    }
}
