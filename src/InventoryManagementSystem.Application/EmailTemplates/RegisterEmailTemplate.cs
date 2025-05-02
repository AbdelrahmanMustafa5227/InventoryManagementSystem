using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.EmailTemplates
{
    [ExcludeFromCodeCoverage]
    public static class RegisterEmailTemplate
    {
        public static string Get(string userName)
        {
            return _emailTemplate
                .Replace("{{UserName}}", userName);
        }

        private static string _emailTemplate = """
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                    body { font-family: Arial, sans-serif; }
                    .content { padding: 20px; }
                    .footer { font-size: 12px; color: gray; margin-top: 20px; }
                    </style>
                </head>
                <body>
                    <div class="content">
                    <h2>Welcome, {{UserName}}!</h2>
                    <p>Thank you for registering at our site.</p>
                    </div>
                    <div class="footer">
                    &copy; 2025 MyCompany. All rights reserved.
                    </div>
                </body>
                </html>
            """;
    }
}
