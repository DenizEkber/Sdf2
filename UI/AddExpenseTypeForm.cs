using Sdf2.Core.Helper;
using Sdf2.Core.Service;
using Sdf2.Database.CodeFirst.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sdf2.UI
{
    partial class AddExpenseTypeForm : Form
    {
        private readonly ExpenseTypeService _expenseTypeService;
        private readonly JsonFileLogger _logger = new JsonFileLogger();

        private TextBox textBoxName;
        private Button buttonSave;

        public AddExpenseTypeForm(ExpenseTypeService expenseTypeService)
        {
            _expenseTypeService = expenseTypeService;

            Component();
        }

        private void Component()
        {
            textBoxName = new TextBox { Location = new Point(20, 20), Width = 200 };
            buttonSave = new Button { Text = "Yadda Saxla", Location = new Point(20, 60) };
            buttonSave.Click += ButtonSave_Click;

            Controls.Add(textBoxName);
            Controls.Add(buttonSave);

            Text = "Xərc Növü Əlavə Et";
            ClientSize = new Size(250, 120);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
        }

        private async void ButtonSave_Click(object sender, EventArgs e)
        {
            string expenseTypeName = textBoxName.Text.Trim();

            if (string.IsNullOrWhiteSpace(expenseTypeName))
            {
                MessageBox.Show("Xərc adı boş ola bilməz.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                buttonSave.Enabled = false;

                
                var existingTypes = await _expenseTypeService.GetAllAsync();
                if (existingTypes.Any(et => et.Name.Equals(expenseTypeName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Bu xərc növü artıq mövcuddur.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var expenseType = new ExpenseType { Name = expenseTypeName };
                await _expenseTypeService.AddAsync(expenseType);

                MessageBox.Show("Xərc növü əlavə edildi.", "Uğur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                MessageBox.Show($"Xərc növü əlavə olunarkən xəta baş verdi: {ex.Message}", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonSave.Enabled = true; 
            }
        }
    }
}
