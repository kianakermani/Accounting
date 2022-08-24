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
    public partial class frmLogin : Form
    {
        public bool IsEdit = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if(txtUserName.Text== "")
            {
                MessageBox.Show("Please Enter Your UserName !!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(txtPassword.Text== "")
            {
                MessageBox.Show("Please Enter Your Password !!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using(UnitOfWork db= new UnitOfWork())
                {
                    if (IsEdit)
                    {
                        var login = db.LoginRepository.Get().First();
                        login.UserName = txtUserName.Text;
                        login.Password = txtPassword.Text;
                        db.LoginRepository.Update(login);
                        db.Save();
                        Application.Restart();
                    }
                    else
                    {
                        if (db.LoginRepository.Get(a => a.UserName == txtUserName.Text && a.Password == txtPassword.Text).Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("User Not Found !!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                this.Text = "Login Settings";
                btnLogin.Text="Save";
                using (UnitOfWork db = new UnitOfWork())
                {
                    var login = db.LoginRepository.Get().First();
                    txtUserName.Text=login.UserName;
                    txtPassword.Text=login.Password;
                }

            }
        }
    }
}
