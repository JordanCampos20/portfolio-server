using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio_server.Context;
using portfolio_server.Models;
using portfolio_server.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;

public class MailServices
{
    private readonly ApplicationDbContext _context;

    public MailServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<bool> Send(MailDTO mailDTO)
    {
        SmtpClient smtpClient = new SmtpClient
        {
            Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("EMAIL"),
                Environment.GetEnvironmentVariable("EMAIL_SENHA")),
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true
        };

        smtpClient.Send(CreateMailMessage(mailDTO));

        return Task.FromResult(true);
    }

    private MailMessage CreateMailMessage(MailDTO mailDTO)
    {
        MailMessage mailMessage = new MailMessage
        {
            Subject = mailDTO.Subject,
            Body = mailDTO.Body,
            From = new MailAddress(mailDTO.From)
        };

        foreach (var TO in mailDTO.To)
        {
            mailMessage.To.Add(TO);
        }

        foreach (var CC in mailDTO.CC)
        {
            mailMessage.CC.Add(CC);
        }

        return mailMessage;
    }
}
