using Sdf2.Core.Helper;
using Sdf2.Core.Interface;
using Sdf2.Core.Service;
using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.CORE.Helpers;

namespace Sdf2.UI
{
    public partial class MainForm : Form
    {

        private Label labelWelcome;
        private Label labelTime;
        private Button buttonSaveExpense;
        private Button buttonAddExpenseType;
        private TextBox textBoxAmount;
        private ComboBox comboBoxExpenseType;
        private DateTimePicker dateTimePickerExpenseDate;
        private DataGridView dataGridViewExpenses;
        private GroupBox groupBoxExpenseHistory;
        private GroupBox groupBoxNewExpense;
        private GroupBox groupBoxHeader;
        private Label labelExpenseType;
        private Label labelDate;
        private Label labelAmount;
        private System.Windows.Forms.Timer emailTimer, timeTimer;

        private readonly IExpenseService _expenseService;
        private readonly ExpenseTypeService _expenseTypeService;
        private readonly MailSender _mailSender;
        private readonly JsonFileLogger _logger = new JsonFileLogger();
        public MainForm(IExpenseService expenseService, ExpenseTypeService expenseTypeService)
        {
            _expenseService = expenseService;
            _expenseTypeService = expenseTypeService;
            _mailSender = new MailSender((ExpenseService)_expenseService, (ExpenseTypeService)_expenseTypeService); 
            Component();
            InitializeTimer();
            try
            {
                LoadExpenseTypes();
                LoadExpenses();
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                MessageBox.Show("Failed to load initial data. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeTimer()
        {
            timeTimer = new System.Windows.Forms.Timer();
            timeTimer.Interval = 1000; 
            timeTimer.Tick += TimeTimer_Tick;
            timeTimer.Start();

            emailTimer = new System.Windows.Forms.Timer();
            emailTimer.Interval = 60000; 
            emailTimer.Tick += EmailTimer_Tick;
            emailTimer.Start();
        }
        private void EmailTimer_Tick(object sender, EventArgs e)
        {
            
            var currentTime = DateTime.Now;
            if (currentTime.Hour == 0 && currentTime.Minute == 0) 
            {
                SendDailyEmail();
            }
        }
        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss"); 
        }

        private async void SendDailyEmail()
        {
            try
            {
                await _mailSender.SendDailyExpensesEmail();
                MessageBox.Show("Today's expenses were successfully emailed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                MessageBox.Show("Error while sending email. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExpenseTypes()
        {
            try
            {
                comboBoxExpenseType.DataSource = _expenseService.GetExpenseTypes();
                comboBoxExpenseType.DisplayMember = "Name";
                comboBoxExpenseType.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw; 
            }
        }

        private void LoadExpenses()
        {
            try
            {
                dataGridViewExpenses.DataSource = _expenseService.GetExpenses();
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw;
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxExpenseType.SelectedValue != null && decimal.TryParse(textBoxAmount.Text, out var amount))
                {
                    _expenseService.AddExpense(dateTimePickerExpenseDate.Value, (int)comboBoxExpenseType.SelectedValue, amount);
                    MessageBox.Show("Expense saved successfully.");
                    LoadExpenses();
                }
                else
                {
                    MessageBox.Show("Please enter valid expense details.");
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                MessageBox.Show("Error saving expense. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonAddExpenseType_Click(object sender, EventArgs e)
        {
            try
            {
                var addExpenseTypeForm = new AddExpenseTypeForm(_expenseTypeService);
                addExpenseTypeForm.ShowDialog();
                LoadExpenseTypes();
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                MessageBox.Show("Error adding expense type. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Component()
        {
            
            labelWelcome = new Label();
            labelTime = new Label();
            buttonSaveExpense = new Button();
            buttonAddExpenseType = new Button();
            textBoxAmount = new TextBox();
            comboBoxExpenseType = new ComboBox();
            dateTimePickerExpenseDate = new DateTimePicker();
            dataGridViewExpenses = new DataGridView();
            groupBoxExpenseHistory = new GroupBox();
            groupBoxNewExpense = new GroupBox();
            groupBoxHeader = new GroupBox();
            labelExpenseType = new Label();
            labelDate = new Label();
            labelAmount = new Label();

            
            groupBoxExpenseHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewExpenses).BeginInit();
            groupBoxNewExpense.SuspendLayout();
            groupBoxHeader.SuspendLayout();
            SuspendLayout();

            
            groupBoxHeader.Controls.Add(labelWelcome);
            groupBoxHeader.Controls.Add(labelTime);
            groupBoxHeader.Location = new Point(17, 20);
            groupBoxHeader.Name = "groupBoxHeader";
            groupBoxHeader.Size = new Size(755, 60);
            groupBoxHeader.TabStop = false;

            
            labelWelcome.AutoSize = true;
            labelWelcome.Location = new Point(18, 23);
            labelWelcome.Name = "labelWelcome";
            labelWelcome.Size = new Size(119, 20);
            labelWelcome.Text = "Xoş Gəldin Elsad";

            
            labelTime.AutoSize = true;
            labelTime.Location = new Point(688, 23);
            labelTime.Name = "labelTime";
            labelTime.Size = new Size(38, 20);
            labelTime.Text = "Saat";

            
            groupBoxNewExpense.Controls.Add(labelDate);
            groupBoxNewExpense.Controls.Add(dateTimePickerExpenseDate);
            groupBoxNewExpense.Controls.Add(labelExpenseType);
            groupBoxNewExpense.Controls.Add(comboBoxExpenseType);
            groupBoxNewExpense.Controls.Add(buttonAddExpenseType);
            groupBoxNewExpense.Controls.Add(labelAmount);
            groupBoxNewExpense.Controls.Add(textBoxAmount);
            groupBoxNewExpense.Controls.Add(buttonSaveExpense);
            groupBoxNewExpense.Location = new Point(12, 100);
            groupBoxNewExpense.Name = "groupBoxNewExpense";
            groupBoxNewExpense.Size = new Size(250, 320);
            groupBoxNewExpense.TabStop = false;
            groupBoxNewExpense.Text = "Yeni Xərcim";

            
            labelDate.AutoSize = true;
            labelDate.Location = new Point(18, 30);
            labelDate.Name = "labelDate";
            labelDate.Size = new Size(39, 20);
            labelDate.Text = "Tarix";

            
            dateTimePickerExpenseDate.Location = new Point(20, 60);
            dateTimePickerExpenseDate.Name = "dateTimePickerExpenseDate";
            dateTimePickerExpenseDate.Size = new Size(200, 27);

            
            labelExpenseType.AutoSize = true;
            labelExpenseType.Location = new Point(18, 100);
            labelExpenseType.Name = "labelExpenseType";
            labelExpenseType.Size = new Size(77, 20);
            labelExpenseType.Text = "Xərc Növü";

            
            comboBoxExpenseType.FormattingEnabled = true;
            comboBoxExpenseType.Location = new Point(20, 130);
            comboBoxExpenseType.Name = "comboBoxExpenseType";
            comboBoxExpenseType.Size = new Size(150, 28);

            
            buttonAddExpenseType.Location = new Point(180, 130);
            buttonAddExpenseType.Name = "buttonAddExpenseType";
            buttonAddExpenseType.Size = new Size(40, 30);
            buttonAddExpenseType.Text = "...";
            buttonAddExpenseType.Click += ButtonAddExpenseType_Click;
            buttonAddExpenseType.UseVisualStyleBackColor = true;

            
            labelAmount.AutoSize = true;
            labelAmount.Location = new Point(18, 170);
            labelAmount.Name = "labelAmount";
            labelAmount.Size = new Size(60, 20);
            labelAmount.Text = "Məbləğ";

            
            textBoxAmount.Location = new Point(20, 200);
            textBoxAmount.Name = "textBoxAmount";
            textBoxAmount.Size = new Size(150, 27);

            
            buttonSaveExpense.Location = new Point(20, 250);
            buttonSaveExpense.Name = "buttonSaveExpense";
            buttonSaveExpense.Size = new Size(100, 30);
            buttonSaveExpense.Text = "Yadda Saxla";
            buttonSaveExpense.Click += ButtonSave_Click;
            buttonSaveExpense.UseVisualStyleBackColor = true;

            
            groupBoxExpenseHistory.Controls.Add(dataGridViewExpenses);
            groupBoxExpenseHistory.Location = new Point(300, 100);
            groupBoxExpenseHistory.Name = "groupBoxExpenseHistory";
            groupBoxExpenseHistory.Size = new Size(470, 320);
            groupBoxExpenseHistory.TabStop = false;
            groupBoxExpenseHistory.Text = "Xərc Tarixi";

            
            dataGridViewExpenses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewExpenses.Location = new Point(20, 30);
            dataGridViewExpenses.Name = "dataGridViewExpenses";
            dataGridViewExpenses.Size = new Size(420, 270);

            
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBoxHeader);
            Controls.Add(groupBoxNewExpense);
            Controls.Add(groupBoxExpenseHistory);
            Name = "MainForm";
            Text = "MainForm";

           
            groupBoxExpenseHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewExpenses).EndInit();
            groupBoxNewExpense.ResumeLayout(false);
            groupBoxNewExpense.PerformLayout();
            groupBoxHeader.ResumeLayout(false);
            groupBoxHeader.PerformLayout();
            ResumeLayout(false);
        }
    }
}
