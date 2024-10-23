using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace portfolio_server.Models
{
    public class ContatoDTO
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Assunto { get; set; }
    }
}