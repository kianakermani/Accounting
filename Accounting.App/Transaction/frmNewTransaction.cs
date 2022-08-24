using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class frmNewTransaction : Form
    {
        readonly UnitOfWork db = new UnitOfWork();
        public int TransactionID = 0;

        public frmNewTransaction()
        {
            InitializeComponent();
        }

        private void FrmNewTransaction_Load(object sender, EventArgs e)
        {
            dgCustomers.AutoGenerateColumns = false;
            dgCustomers.DataSource = db.CustomerRepository.GetNameCustomers();
            if(TransactionID != 0)
            {
                var account = db.AccountingRepository.GetById(TransactionID);
                txtAmount.Text = account.Amount.ToString();
                txtDescription.Text=account.Description.ToString();
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                if (account.TypeID == 1)
                {
                    rbRecieve.Checked = true;
                }
                else
                {
                    rbPay.Checked = true;
                }
                this.Text = "Edit";
                btnSave.Text = "Edit";

                
            }
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            dgCustomers.AutoGenerateColumns = false;
            dgCustomers.DataSource = db.CustomerRepository.GetNameCustomers(txtFilter.Text );
        }

        private void DgCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void TxtAmount_ValueChanged(object sender, EventArgs e)
        {
            txtAmount.Maximum = 9999999;
            txtAmount.Minimum = 1;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (txtName.Text == "")
            {
                MessageBox.Show("Please Select The Name Of The Account Party From The List!");
            }
            else if (txtAmount.Value == 0)
            {
                MessageBox.Show("Please Enter The Amount!");
            }
            else if(rbPay.Checked || rbRecieve.Checked)
            {
                DataLayer.Accounting accounting = new DataLayer.Accounting()
                {
                    Amount = txtAmount.Value.ToString(),
                    CustomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                    TypeID = (rbRecieve.Checked) ? 1 : 2,
                    DateTime = DateTime.Now,
                    Description = txtDescription.Text

                };
                if (TransactionID == 0)
                {
                    db.AccountingRepository.Insert(accounting);
                    db.Save();
                }
                else
                {
                    using (UnitOfWork db2=new UnitOfWork())
                    {
                        accounting.ID = TransactionID;
                        db2.AccountingRepository.Update(accounting);
                        db2.Save();
                    }
                }
                
                
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please Select The Type Of Transaction!");
            }
            

        }
    }
}
