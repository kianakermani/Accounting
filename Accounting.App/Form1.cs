using Accounting.Business;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Accounting;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomers frmCustomers = new frmCustomers();
            frmCustomers.ShowDialog();

        }

        private void BtnNewTransaction_Click(object sender, EventArgs e)
        {
            frmNewTransaction frmNewTransaction = new frmNewTransaction();
            frmNewTransaction.ShowDialog();
        }

        private void BtnReportPay_Click(object sender, EventArgs e)
        {
            FrmReports frmReports = new FrmReports
            {
                TypeID = 2
            };
            frmReports.ShowDialog();
        }

        private void BtnReportRecieve_Click(object sender, EventArgs e)
        {
            FrmReports frmReports = new FrmReports
            {
                TypeID = 1
            };
            frmReports.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            this.Hide();
            frmLogin frmLogin = new frmLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                this.Show();
                lblDate.Text = DateTime.Now.ToShamsi();
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                Report();
            }
            else
            {
                Application.Exit();
            }
        }

        void Report()
        {
            ReportViewModel report = Account.ReportFormMain();
            //lblPeyment.Text = report.Pay.ToString("#,0");
            //lblRecieve.Text=report.Recieve.ToString("#,0");
            //lblRemaining.Text=report.Remaining.ToString("#,0");
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void BtnEditLogin_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin
            {
                IsEdit = true
            };
            frmLogin.ShowDialog();
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
