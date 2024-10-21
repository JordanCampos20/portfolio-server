using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace finance_control_server.Models
{
    public class Contato
    {
        public required string ContatoId { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Assunto { get; set; }
        public required DateTime Data { get; set; }
    }
}