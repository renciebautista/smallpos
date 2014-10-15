using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace pos.fileMaintenance
{
    public partial class frmMaintenance : Form
    {
        DAL dal = new DAL();
        private string maintenace_name;
        private string insertQuery;
        private string selectQuery;
        private string deleteQuery;
        private string updateQuery;
        private string checkQuery;

        private string item_id;
        private string item_desc;
        private string desc_header;
       
        private int full_accessid;

        public frmMaintenance()
        {
            InitializeComponent();
        }

        public string FormName
        {
            set { maintenace_name = value; }
        }

        public string InsertQuery
        {
            set { insertQuery = value; }
        }

        public string SelectQuery
        {
            set { selectQuery = value; }
        }

        public string DeleteQuery
        {
            set { deleteQuery = value; }
        }

        public string UpdateQuery
        {
            set { updateQuery = value; }
        }

        public string CheckQuery
        {
            set { checkQuery = value; }
        }

        public string FieldId
        {
            set { item_id = value; }
        }

        public string DescField
        {
            set { item_desc = value; }
        }

        public string DescHeader
        {
            set { desc_header = value; }
        }

        



        public int FullAccessId
        {
            set { full_accessid = value; }
        }

        private void frmMaintenance_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} Maintenance", maintenace_name);
            lblDesc.Text = string.Format("{0} Maintenance", maintenace_name);

            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns[0].DataPropertyName = item_id;
            dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns[1].HeaderText = desc_header;
            dataGridView.Columns[1].DataPropertyName = item_desc;
            dataGridView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            BindGridView();
           
        }

        private void frmMaintenance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            int result;
            DataTable dt = dal.GetDataTable(selectQuery);

            controls.frmFilter filter = new controls.frmFilter();
            filter.DataSource = dt;
            filter.SearchFor = maintenace_name;
            filter.FieldId = item_id;
            filter.FieldDesc = item_desc;
            filter.HeaderDesc = desc_header;
            if (filter.ShowDialog() == DialogResult.OK)
            {
                result = filter.FilterValue;
                BindGridView();
                filter.MoveCursor(result, dataGridView);
            }
            filter.Dispose();
            btnAdd.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmAdd add = new frmAdd(frmAdd.FormMode.Add))
            {
                add.FormName = maintenace_name;
                add.item = item_desc;
                add.InsertQuery = insertQuery;
                add.CheckQuery = checkQuery;
                DialogResult result = add.ShowDialog();
                if (result == DialogResult.OK)
                {
                    BindGridView();
                }
                
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this " + maintenace_name.ToLower() + "?", this.Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int id = Int32.Parse( dataGridView.CurrentRow.Cells["id"].Value.ToString());
                int retVal = dal.ExecuteNonQuery(deleteQuery, new SqlParameter("@id", id));
                if (retVal == 0)
                {
                    BindGridView();
                }
                else
                {
                    MessageBox.Show("Erorr deleting record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            using (frmAdd add = new frmAdd(frmAdd.FormMode.Edit))
            {
                add.FormName = maintenace_name;
                add.UpdateQuery = updateQuery;
                add.IdName = item_id;
                add.item = item_desc;
                add.CheckQuery = checkQuery;
                add.ItemValue = dataGridView.CurrentRow.Cells["desc"].Value.ToString();
                add.ItemId = Int32.Parse(dataGridView.CurrentRow.Cells["id"].Value.ToString());

                DialogResult result = add.ShowDialog();
                if (result == DialogResult.OK)
                {
                    BindGridView();
                }

            }
        }


        /* functions */

        private void BindGridView()
        {
            dataGridView.DataSource = dal.GetDataTable(selectQuery);
            ToggleAdd();
        }

        private void ToggleAdd()
        {
            if (dataGridView.Rows.Count > 0)
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnFind.Enabled = true;

            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnFind.Enabled = false;
            }
        }
    }
}
