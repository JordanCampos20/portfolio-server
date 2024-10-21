using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_control_server.ViewModels
{
    public class ResponseViewModel(string message, int status)
    {
        public string Message { get; set; } = message;
        public int Status { get; set; } = status;
    }
}