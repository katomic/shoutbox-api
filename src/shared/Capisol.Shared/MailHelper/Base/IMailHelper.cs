using System;
using System.Collections.Generic;

namespace Capisol.Shared.MailHelper.Base
{
    public interface IMailHelper<EmailMessage> : IDisposable
    {
        IMailHelper<EmailMessage> Clear();

        IMailHelper<EmailMessage> AddCustomHeader(string headerKey, string headerValue);

        IMailHelper<EmailMessage> SetFromMailboxAddress(string name, string emailAddress);

        IMailHelper<EmailMessage> SetToMailboxAddress(string name, string emailAddress);

        IMailHelper<EmailMessage> AddToMailboxAddress(string name, string emailAddress);

        IMailHelper<EmailMessage> SetCcMailboxAddress(string name, string emailAddress);

        IMailHelper<EmailMessage> AddCcMailboxAddress(string name, string emailAddress);

        IMailHelper<EmailMessage> SetSubject(string subject);

        IMailHelper<EmailMessage> SetMessageBody(string message);

        IMailHelper<EmailMessage> AddAttachment(string fileName, byte[] byteArray, string contentType);

        EmailMessage SendMessage();

        List<EmailMessage> SendMessages();

        EmailMessage ForwardMessage(EmailMessage message);

        IMailHelper<EmailMessage> SendMessage(EmailMessage message);
    }
}
