﻿using System.Threading.Tasks;

namespace Ponto.Eletronico.Core.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string from, string subject, string body);
}
