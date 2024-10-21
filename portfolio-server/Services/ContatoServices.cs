using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finance_control_server.Context;
using finance_control_server.Models;
using System.Security.Claims;
using finance_control_server.Services;

public class ContatoServices
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ContatoServices(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Contato> PostContato(Contato contato)
    {
        contato.ContatoId = Guid.NewGuid().ToString();

        contato.Data = DateTime.Now;

        _context.Contatos.Add(contato);

        await _context.SaveChangesAsync();

        return contato;
    }
}
