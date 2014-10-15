using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pos.Maintenance
{
    public partial class frmItems : Form
    {
        DAL dal = new DAL();
        DataTable dtSource;
        public frmItems()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmAddItems additem = new frmAddItems())
            {
                additem.ShowDialog();
            }
            bindGridView();

        }

        private void frmItems_Load(object sender, EventArgs e)
        {
            
            DataTable dtDepartment = dal.GetDataTable("SELECT * FROM item_department ORDER BY dept_desc");
            DataRow row = dtDepartment.NewRow();
            row[0] = 0;
            row[1] = "ALL DEPARTMENT";
            dtDepartment.Rows.InsertAt(row, 0); //insert new to to index 0 (on top=

            cmbDepartment.DataSource = dtDepartment;
            cmbDepartment.DisplayMember = "dept_desc";
            cmbDepartment.ValueMember = "id";


            
            bindGridView();
        }

        private void bindGridView()
        {
            string selectQuery="";
            dgvItems.AutoGenerateColumns = false;
            int selectedValue = Int32.Parse( cmbDepartment.SelectedValue.ToString());
            if (selectedValue == 0)
            {
                selectQuery = "SELECT items.*,item_department.dept_desc as department FROM items JOIN item_department ON item_department.id = items.dept_id ORDER BY description";
            }
            else
            {
                selectQuery = string.Format( "SELECT items.*,item_department.dept_desc as department FROM items JOIN item_department ON item_department.id = items.dept_id where dept_id = {0} ORDER BY description",selectedValue);
            }
            dtSource = dal.GetDataTable(selectQuery);

            dtSource.DefaultView.RowFilter = string.Format("code LIKE '%{0}%' or description LIKE '%{0}%'", txtFilter.Text.Trim().Replace("'", "''"));
            dgvItems.DataSource = dtSource;
            
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            bindGridView();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbDepartment_SelectionChangeCommitted(object sender, EventArgs e)
        {
            bindGridView();
        }

    }
}