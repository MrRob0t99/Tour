﻿using System.Threading.Tasks;

namespace EleksTask
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendConfirmLetter(string token, string userId, string userEmail);
    }
}
