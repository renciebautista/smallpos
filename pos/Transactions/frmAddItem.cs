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
    public partial class frmAddItem : Form
    {
        private DataTable dtSource;
        private string strSearch;
        private string fid;
        private string fdesc;
        private string headerDesc;
        private int returnValue;

        public frmAddItem()
        {
            InitializeComponent();
        }

        public DataTable DataSource
        {
            set { dtSource = value; }
        }
        public string SearchFor
        {
            set {strSearch = value;}
        }
        public string FieldDesc
        {
            set { fdesc = value; }
        }
        public string FieldId
        {
            set { fid = value; }
        }
        public string HeaderDesc
        {
            set { headerDesc = value; }
        }
        public int FilterValue
        {
            get { return returnValue; }
        }


        private void frmAddItem_Load(object sender, EventArgs e)
        {
            Init();
            BindGridView();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.ForeColor = Color.Red;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BindGridView();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                int index = dataGridView.CurrentRow.Index;
                if (index > 0)
                {
                    --index;
                    dataGridView.CurrentCell = dataGridView.Rows[index].Cells[1];
                    dataGridView.Rows[index].Selected = true;
                }

            }
            if (e.KeyCode == Keys.Down)
            {
                int rowCount = dataGridView.Rows.Count;
                int index = dataGridView.CurrentRow.Index;
                --rowCount;
                if (index < rowCount)
                {
                    ++index;
                    dataGridView.CurrentCell = dataGridView.Rows[index].Cells[1];
                    dataGridView.Rows[index].Selected = true;
                }

            }

            if (e.KeyCode == Keys.Enter)
            {
                ReturnValue();
            }
        }

        private void frmAddItem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void frmAddItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ReturnValue();
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReturnValue();
            }
        }     

        /*methods*/
        private void Init()
        {
            this.Text = string.Format("Search {0}", strSearch);
            dataGridView.AutoGenerateColumns = false;
        }

        private void BindGridView()
        {
            dtSource.DefaultView.RowFilter = string.Format("code LIKE '%{0}%' or description LIKE '%{0}%'",txtSearch.Text.Trim().Replace("'", "''"));
            dataGridView.DataSource = dtSource;
        }

        private void ReturnValue()
        {
            if (dataGridView.Rows.Count > 0)
            {
                returnValue = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value.ToString());
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                returnValue = 0;
                this.DialogResult = DialogResult.Cancel;
            }

            
        }

        public void MoveCursor(int grid_id, DataGridView dgView)
        {
            int index = GetIndex(dgView, grid_id);
            dgView.Rows[index].Selected = true;
            try
            {
                dgView.CurrentCell = dgView.Rows[index].Cells[0];
            }
            catch
            {
                dgView.CurrentCell = dgView.Rows[index].Cells[1];
            }
            dgView.FirstDisplayedScrollingRowIndex = index;
            dgView.Update();
        }

        private int GetIndex(DataGridView dgrid, int data_id)
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in dgrid.Rows)
            {
                if (row.Cells[0].Value.ToString().Equals(data_id.ToString()))
                {
                    rowIndex = row.Index;
                    break;
                }
            }

            return rowIndex;

        }





    }
}
