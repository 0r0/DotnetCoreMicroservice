﻿using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }


    public async Task<bool> SendEmail(Email email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);
        var from = new EmailAddress()
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName,
        };
        var sendGridMessage =
            MailHelper.CreateSingleEmail(from,
                new EmailAddress(email.To), email.Subject, email.Body, email.Body);
        var response = await client.SendEmailAsync(sendGridMessage);
        
        _logger.LogInformation("Email has been send");
        
        if (response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK)
            return true;
        _logger.LogError("Email sending failed.");
        return false;
    }
}