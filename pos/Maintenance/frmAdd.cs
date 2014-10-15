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
    public partial class frmAdd : Form
    {
        DAL dal = new DAL();
        private string insertQuery;
        private string updateQuery;
        private string checkQuery;
        private string desc;
        private string itemdesc;
        private string id_name;
        private string item_value;
        private int item_id;
        private FormMode form_mode;
        public frmAdd(FormMode mode)
        {
            InitializeComponent();
            form_mode = mode;
        }

        public enum FormMode
        {
            Add,Edit
        };

        public string FormName
        {
            set { desc = value; }
        }

        public string InsertQuery
        {
            set { insertQuery = value; }
        }

        public string UpdateQuery
        {
            set { updateQuery = value; }
        }
        public string CheckQuery
        {
            set { checkQuery = value; }
        }


        public string item
        {
            set { itemdesc = value; }
        }

        public string IdName
        {
            set { id_name = value; }
        }

        public string ItemValue
        {
            set { item_value = value; }
        }

        public int ItemId
        {
            set { item_id = value; }
        }

        private void frmAdd_Load(object sender, EventArgs e)
        {
            
            groupBox1.Text = desc;
            if (form_mode == FormMode.Add)
            {
                this.Text = string.Format("New {0}", desc);
                lblDesc.Text = string.Format("Adding new {0} description.", desc.ToLower());
                btnSave.Text = "&Save";
            }
            else
            {
                this.Text = string.Format("Update {0}", desc);
                lblDesc.Text = string.Format("Updating {0} description.", desc.ToLower());
                txtDesc.Text = item_value;
                btnSave.Text = "&Update";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

                if (txtDesc.Text == "")
                {
                    MessageBox.Show(desc + " description is required", this.Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtDesc.Focus();
                }
                else
                {
                    if (form_mode == FormMode.Add)
                    {

                        if (AddDesc() == 1)
                        {
                            MessageBox.Show(desc + " already exist.", desc + " Maintenance",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtDesc.Focus();
                            txtDesc.SelectAll();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    else
                    {
                        if (UpdateDesc() == 1)
                        {
                            MessageBox.Show(desc + " already exist.", desc + " Maintenance",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtDesc.Focus();
                            txtDesc.SelectAll();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    
                }
            
        }

        /*functions */

        private int AddDesc()
        {
            if (!CheckDesc())
            {
                object ret;
                ret = dal.ExecuteNonQuery(insertQuery, new SqlParameter("@" + itemdesc, txtDesc.Text.Trim()));
                int retVal = (int)ret;
                return retVal;
            }
            else
            {
                return 1;
            }
            
        }

        private int UpdateDesc()
        {
            if (!CheckDesc())
            {
                object ret;
                ret = dal.ExecuteNonQuery(updateQuery, new SqlParameter("@" + itemdesc, txtDesc.Text.Trim()), new SqlParameter("@" + id_name, item_id));
                int retVal = (int)ret;
                return retVal;
            }
            else
            {
                return 1;
            }
            
        }

        private bool CheckDesc()
        {
            DataTable dt = dal.GetDataTable(checkQuery, DAL.CmdType.Text, new SqlParameter("@" + itemdesc, txtDesc.Text.Trim()));
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
