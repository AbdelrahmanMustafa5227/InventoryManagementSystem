using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Helpers
{
    public class Error
    {
        public static Error Null = new Error("Null Value", HttpStatusCode.BadRequest);
        public static Error NotFound = new Error("Could not find a resource with the specified Id", HttpStatusCode.NotFound, "Resourse Not Found");
        public static Error Conflict = new Error("A resource with the same data already exists", HttpStatusCode.Conflict, "Resource Conflict");
        public static Error UnAuthorized = new Error("Invalid Credentials", HttpStatusCode.Unauthorized, "Unauthorized");
        public static Error Forbidden = new Error("Access Denied", HttpStatusCode.Forbidden, "Forbidden");

        public Error(string message = "An Error Has Ocurred While Processing your request",
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            string title = "Error")
        {
            Message = message;
            StatusCode = statusCode;
            Title = title;
        }

        public string Message { get; set; }
        public string Title { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
