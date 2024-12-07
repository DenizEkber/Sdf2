using Sdf2.Database.CodeFirst.Entity;
using Sdf2.Database.Interface;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sdf2.Core.Interface;
using Sdf2.Core.Service;
using Sdf2.Core.Helper;

namespace Sdf2.UI
{
    public partial class LoginForm : Form
    {
        Label labelUsername, labelPassword;
        TextBox textBoxUsername, textBoxPassword;
        Button buttonLogin, buttonCancel;

        private readonly IRepository<User> _userRepository;
        private readonly IExpenseService _expenseService;
        private readonly ExpenseTypeService _expenseTypeService;
        private readonly JsonFileLogger _logger = new JsonFileLogger();
        public LoginForm(IRepository<User> userRepository, IExpenseService expenseService, ExpenseTypeService expenseTypeService)
        {
            Component();
            _userRepository = userRepository;
            _expenseService = expenseService;
            _expenseTypeService = expenseTypeService;
        }
        private void Component()
        {
            this.labelUsername = new Label();
            this.labelPassword = new Label();
            this.textBoxUsername = new TextBox();
            this.textBoxPassword = new TextBox();
            this.buttonLogin = new Button();
            this.buttonCancel = new Button();

            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new Point(30, 30);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new Size(75, 13);
            this.labelUsername.TabIndex = 0;
            this.labelUsername.Text = "Username:";

            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new Point(30, 70);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new Size(75, 13);
            this.labelPassword.TabIndex = 1;
            this.labelPassword.Text = "Password:";

            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new Point(120, 30);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new Size(150, 20);
            this.textBoxUsername.TabIndex = 2;

            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new Point(120, 70);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new Size(150, 20);
            this.textBoxPassword.TabIndex = 3;

            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new Point(50, 110);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new Size(75, 23);
            this.buttonLogin.TabIndex = 4;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.ButtonLogin_Click);

            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new Point(150, 110);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);

            // 
            // LoginForm
            // 
            this.ClientSize = new Size(300, 180);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.buttonCancel);
            this.Name = "LoginForm";
            this.Text = "Login";
        }

        private async void ButtonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                buttonLogin.Enabled = false; 
                string hashedPassword = HashPassword(password);

                var users = await _userRepository.GetAllAsync();
                var validUser = users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hashedPassword);

                if (validUser != null)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MainForm mainForm = new MainForm(_expenseService, _expenseTypeService);
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonLogin.Enabled = true; 
            }
        }


        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }


    }
}
