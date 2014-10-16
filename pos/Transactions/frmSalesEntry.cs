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
        DataTable tempTable = new DataTable();
        DAL dal = new DAL();
        public frmSalesEntry()
        {
            InitializeComponent();
        }

        private void salesEntry_Load(object sender, EventArgs e)
        {
            txtSku.Focus();
            lblDateTime.Text = DateTime.Now.ToString("F");
            setTempTable();
            txtQty.BackColor = Color.White;
            txtAmount.BackColor = Color.White;
            txtAmountDue.BackColor = Color.White;
            txtAmountDue.ForeColor = Color.Red;

            toggleButton();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (tempTable.Rows.Count > 0)
            {
                MessageBox.Show("Cannot close form, there is still a pending transaction",
                    "Alert message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.Close();
            }
            
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
            if (tempTable.Rows.Count > 0)
            {
                if (e.KeyCode == Keys.Up)
                {

                    int index = dataGridView.CurrentRow.Index;
                    if (index > 0)
                    {
                        --index;
                        dataGridView.CurrentCell = dataGridView.Rows[index].Cells[0];
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
                        dataGridView.CurrentCell = dataGridView.Rows[index].Cells[0];
                        dataGridView.Rows[index].Selected = true;
                    }

                }
            }
            

            if (e.KeyCode == Keys.Enter)
            {
                if (txtSku.Text == "")
                {
                    using (frmAddItem addItem = new frmAddItem())
                    {
                        int result;
                        addItem.DataSource = dal.GetDataTable("SELECT id, code,description,srp FROM items ORDER BY description");
                        addItem.SearchFor = "Items";
                        if (addItem.ShowDialog() == DialogResult.OK)
                        {
                            result = addItem.FilterValue;
                            if (result > 0)
                            {
                                additem(result);
                            }
                        }
                    }
                }
                else
                {
                    string query = string.Format("SELECT id, code,description,srp FROM items WHERE code = '{0}'", txtSku.Text.Trim());
                    DataTable temp = dal.GetDataTable(query);
                    if (temp.Rows.Count > 0)
                    {
                        additem(Int32.Parse( temp.Rows[0]["id"].ToString()));
                        txtSku.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Item code not found!", "Item not found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtSku.SelectAll();
                    }
                }

                
            }
        }

        private void additem(int id)
        {
            string query = string.Format("SELECT id, code,description,srp FROM items WHERE id = '{0}'",id);
            DataTable temp = dal.GetDataTable(query);
            tempTable.ImportRow(temp.Rows[0]);
            bindTable();
        }

        private void bindTable()
        {
            if (tempTable.Rows.Count > 0)
            {
                dataGridView.DataSource = tempTable;
                

                object sumObject;
                sumObject = tempTable.Compute("Sum(srp)", "");

                txtQty.Text = tempTable.Rows.Count.ToString();
                txtAmount.Text = string.Format("{0:#,##0.00}", double.Parse(sumObject.ToString()));
                txtAmountDue.Text = string.Format("{0:#,##0.00}", double.Parse(sumObject.ToString()));

                selectLastItem();
            }
            else
            {
                txtQty.Text = "";
                txtAmount.Text = "";
                txtAmountDue.Text = "";

            }
            toggleButton();


        }

        private void setTempTable()
        {
            tempTable.Columns.Add("id", typeof(String));
            tempTable.Columns.Add("code", typeof(String));
            tempTable.Columns.Add("description", typeof(String));
            tempTable.Columns.Add("srp", typeof(Decimal));

            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns[2].DefaultCellStyle.Format = "n";


        }

        private void toggleButton()
        {
            if (tempTable.Rows.Count > 0)
            {
                btnVoid.Enabled = true;
                btnDelete.Enabled = true;
                btnCash.Enabled = true;
                btnCridet.Enabled = true;
            }
            else
            {
                btnVoid.Enabled = false;
                btnDelete.Enabled = false;
                btnCash.Enabled = false;
                btnCridet.Enabled = false;
            }
        }

        private void selectLastItem()
        {
            if (tempTable.Rows.Count > 0)
            {
                dataGridView.ClearSelection();//If you want

                int nRowIndex = dataGridView.Rows.Count - 1;
                int nColumnIndex = 1;

                dataGridView.Rows[nRowIndex].Selected = true;
                dataGridView.Rows[nRowIndex].Cells[nColumnIndex].Selected = true;

                //In case if you want to scroll down as well.
                dataGridView.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this item?","Delete Item",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                int index = dataGridView.CurrentRow.Index;
                tempTable.Rows[index].Delete();
                tempTable.AcceptChanges();
                bindTable();
                txtSku.Focus();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

    }
}
