using System;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sdf2.Core.Service;
using Sdf2.Services;

namespace WinFormsApp1.CORE.Helpers
{
    public class MailSender
    {
        private readonly ExpenseService _expenseService;
        private readonly ExpenseTypeService _expenseTypeService;


        public MailSender(ExpenseService expenseService, ExpenseTypeService expenseTypeService)
        {
            _expenseService = expenseService;
            _expenseTypeService = expenseTypeService;
        }

        public async Task SendDailyExpensesEmail()
        {
            try
            {
                var expenses = await _expenseService.GetAllAsync();
                if (expenses == null || !expenses.Any())
                {
                    Console.WriteLine("Gündəlik göndərmək üçün xərc yoxdur.");
                    return;
                }

                var body = new StringBuilder("Günün Xərcləri:\n-------------------------\n");

                foreach (var expense in expenses)
                {
                    var expenseType = await _expenseTypeService.GetByIdAsync(o=>o.Id == expense.ExpenseTypeId);
                    if (expenseType != null)
                    {
                        body.AppendLine($"Tarix: {expense.Date}, Növ: {expenseType.Name}, Məbləğ: {expense.Amount} AZN");
                    }
                }

                await SendEmailAsync("Gündəlik Xərclər", body.ToString());
                Console.WriteLine("Gündəlik xərclər uğurla göndərildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email göndərilərkən xəta baş verdi: {ex.Message}");
            }
        }

        private async Task SendEmailAsync(string subject, string body)
        {
            using var mail = new MailMessage("olabilirbilmem1@gmail.com", "bladebey999@gmail.com", subject, body);
            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("olabilirbilmem1@gmail.com", "gtcaxdkidocvmrlv"),
                EnableSsl = true
            };
            await smtp.SendMailAsync(mail);
        }

    }
}
