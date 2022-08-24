using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Customers;
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
    public partial class FrmReports : Form
    {
        public int TypeID = 0;
        public FrmReports()
        {
            InitializeComponent();
            this.dgReport.DataError += new DataGridViewDataErrorEventHandler(this.DgReport_DataError);

        }

        

        private void FrmReports_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>
                {
                    new ListCustomerViewModel()
                    {
                        CustomerID = 0,
                        FullName = " Choose Name:"
                    }
                };
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember ="CustomerID";
            }
            if(TypeID == 1)
            {
                this.Text = "Recieve Report";
            }
            else
            {
                this.Text="Payment Report";
            }
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                //DateTime? startDate;
                //DateTime? endDate;
                

                if((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID&&a.CustomerID==customerId));
                }
                else
                {
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID));
                }




                //if (txtFromDate.Text != "__/__/____")
                //{
                //    startDate = Convert.ToDateTime(txtFromDate.Text);
                //    startDate = DateConvertor.ToMiladi(startDate.Value);
                //    result = result.Where(r => r.DateTime >= startDate.Value).ToList();
                //}
                //if (txtTillDate.Text != "__/__/____")
                //{
                //    endDate = Convert.ToDateTime(txtTillDate.Text);
                //    endDate = DateConvertor.ToMiladi(endDate.Value);
                //    result = result.Where(r => r.DateTime <= endDate.Value).ToList();
                //}



                dgReport.Rows.Clear();
                foreach(var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DateTime.ToShamsi(),accounting.Description);
                }

            }
        }

        private void DgReport_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if(dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if(MessageBox.Show("Are you sure you want to delete this?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                frmNewTransaction frmNew = new frmNewTransaction
                {
                    TransactionID = id
                };
                if (frmNew.ShowDialog()== DialogResult.OK)
                {
                    Filter();
                }

            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach(DataGridViewRow item in dgReport.Rows)
            {
                dtPrint.Rows.Add(item.Cells[0].Value.ToString(),
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString()
                    );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT", dtPrint);
            stiPrint.Show();


        }
    }
}
