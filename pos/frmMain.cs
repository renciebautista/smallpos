using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using pos.fileMaintenance;

namespace pos
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void salesEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Transactions.frmSalesEntry salesEntry = new pos.Transactions.frmSalesEntry())
            {
                this.Hide();
                salesEntry.ShowDialog();
            }
            this.Show();
            
        }

        private void itemDepartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMaintenance itemDepartment = new frmMaintenance())
            {
                itemDepartment.FormName = "Item Department";
                itemDepartment.FieldId = "id";
                itemDepartment.DescField = "dept_desc";
                itemDepartment.DescHeader = "Department";
                itemDepartment.InsertQuery = "INSERT INTO item_department (dept_desc)VALUES (@dept_desc)";
                itemDepartment.SelectQuery = "SELECT * FROM item_department";
                itemDepartment.DeleteQuery = "DELETE FROM item_department WHERE id = @id";
                itemDepartment.UpdateQuery = "UPDATE item_department set dept_desc = @dept_desc WHERE id = @id";
                itemDepartment.CheckQuery = "SELECT * FROM item_department WHERE dept_desc = @dept_desc";
                itemDepartment.ShowDialog();
            }
            
        }

        private void itemMastefileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Maintenance.frmItems items = new pos.Maintenance.frmItems())
            {
                items.ShowDialog();
            }
        }

        private void itemCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMaintenance itemCategory = new frmMaintenance())
            {
                itemCategory.FormName = "Item Category";
                itemCategory.FieldId = "id";
                itemCategory.DescField = "cat_desc";
                itemCategory.DescHeader = "Category";
                itemCategory.InsertQuery = "INSERT INTO item_category (cat_desc)VALUES (@cat_desc)";
                itemCategory.SelectQuery = "SELECT * FROM item_category";
                itemCategory.DeleteQuery = "DELETE FROM item_category WHERE id = @id";
                itemCategory.UpdateQuery = "UPDATE item_category set dept_desc = @cat_desc WHERE id = @id";
                itemCategory.CheckQuery = "SELECT * FROM item_category WHERE cat_desc = @cat_desc";
                itemCategory.ShowDialog();
            }
        }
    }
}
