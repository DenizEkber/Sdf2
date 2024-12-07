using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sdf2.Core.Helper
{
    public class JsonFileLogger 
    {
        private readonly string logFolder = "Log";
        private readonly string logFileName = "exceptions.json";
        private readonly WwwRootController wwwRootController;

        public JsonFileLogger()
        {
            wwwRootController = new WwwRootController();
        }

        public void Write(Exception ex)
        {
            if (ex == null)
                return;

            try
            {
                string filePath = wwwRootController.WwwRoot(logFolder, logFileName);

                var log = new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Date = DateTime.Now
                };

                string jsonData = JsonConvert.SerializeObject(log, Formatting.Indented);
                File.AppendAllText(filePath, jsonData + Environment.NewLine);
            }
            catch (Exception logEx)
            {
                MessageBox.Show($"Failed to log exception: {logEx.Message}");
            }
        }
    }


}
