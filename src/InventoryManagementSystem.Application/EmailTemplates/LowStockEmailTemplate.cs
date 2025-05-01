using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.EmailTemplates
{
    public class LowStockEmailTemplate
    {
        public static string Get(List<string> lowStockAlerts)
        {
            StringBuilder htmlAlerts = new();
            foreach (var alert in lowStockAlerts)
            {
                htmlAlerts.AppendLine($@"
                    <div class='alert'>
                        {alert}
                    </div>");
            }

            return _emailTemplate.Replace("{{Alerts}}", htmlAlerts.ToString());
        }

        public static string Get(string lowStockAlert)
        {
            string Asd = $@"
                    <div class='alert'>
                        {lowStockAlert}
                    </div>"; 

            return _emailTemplate.Replace("{{Alerts}}", Asd);
        }

        private static string _emailTemplate = """
            <!DOCTYPE html>
            <html lang="en">
            <head>
              <meta charset="UTF-8">
              <style>
                body {
                  font-family: Arial, sans-serif;
                  background-color: #f6f6f6;
                  padding: 20px;
                }

                .container {
                  max-width: 600px;
                  margin: 0 auto;
                  background-color: #ffffff;
                  padding: 20px;
                  border-radius: 6px;
                  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                }

                h2 {
                  color: #333333;
                }

                .alert {
                  background-color: #fff3cd;
                  border-left: 4px solid #ffc107;
                  padding: 10px 15px;
                  margin-bottom: 10px;
                  border-radius: 4px;
                  color: #856404;
                  font-size: 14px;
                }

                .footer {
                  font-size: 12px;
                  color: #999999;
                  text-align: center;
                  margin-top: 30px;
                }
              </style>
            </head>
            <body>
              <div class="container">
                <h2>Low Stock Alerts</h2>
                <h3>This is a list of Products that need to be restocked! </h3>
               {{Alerts}}
                <div class="footer">
                  This is an automated alert from your inventory system.
                </div>
              </div>
            </body>
            </html>
          

            
            """;
    }

   
}
