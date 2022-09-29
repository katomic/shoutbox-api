using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using MimeKit.Text;
using Capisol.Shared.MailHelper.Base;
using Capisol.Shared.MailHelper.MimeKit.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;

namespace Capisol.Shared.Email
{
    /// <summary>
    /// Requirements:
    /// 
    /// PM> Install-Package MimeKit
    /// PM> Install-Package MailKit
    /// </summary>
    public class MimeKitHelper : IMailHelper<MimeMessage>
    {
        #region Properties
        private string _emailDirectory { get; set; }
        private IMailService _mailService { get; set; }
        private MimeEntity _messageEntity { get; set; }
        private MailboxAddress _fromMailboxAddress { get; set; }
        private List<MailboxAddress> _toMailboxAddresses { get; set; }
        private List<MailboxAddress> _ccMailboxAddresses { get; set; }
        private string _subject { get; set; }
        private string _messageBody { get; set; }
        private MimePart _messagePart { get; set; }
        private List<MimePart> _attachments { get; set; }
        private List<KeyValuePair<string, string>> _customValues { get; set; }
        private List<KeyValuePair<string, string>> _customHeaders { get; set; }
        private string _lastSavedFileName { get; set; }
        #endregion

        #region Ctor
        public MimeKitHelper(MailClientType mailType = MailClientType.SaveToDirectory, string directory = @"emails")
        {
            switch (mailType)
            {
                case MailClientType.SmtpClient: _mailService = new SmtpClient(); break;
                case MailClientType.ImapClient: _mailService = new ImapClient(); break;
                case MailClientType.Pop3Client: _mailService = new Pop3Client(); break;
                case MailClientType.SaveToDirectory: _emailDirectory = directory; break;
                default: throw new NotImplementedException("The mail service type is not supported.");
            }
        }

        public MimeKitHelper(IMailService mailService) =>
            _mailService = _mailService;
        #endregion

        #region Exposing Properties
        public IMailService GetMailService { get { return _mailService; } }
        public MailboxAddress GetFromMailboxAddress { get { return _fromMailboxAddress; } }
        public MailboxAddress GetToMailboxAddress { get { return _toMailboxAddresses?.SingleOrDefault(); } }
        public MailboxAddress GetCcMailboxAddress { get { return _ccMailboxAddresses?.SingleOrDefault(); } }
        public List<MailboxAddress> GetToMailboxAddresses { get { return _toMailboxAddresses; } }
        public List<MailboxAddress> GetCcMailboxAddresses { get { return _ccMailboxAddresses; } }
        public string GetSubject { get { return _subject; } }
        public MimePart GetMessagePart { get { return _messagePart; } }
        public MimePart GetAttachment { get { return _attachments?.SingleOrDefault(); } }
        public List<MimePart> GetAttachments { get { return _attachments; } }
        public List<KeyValuePair<string, string>> GetCustomHeaders { get { return _customHeaders; } }
        public string GetLastSavedFileName { get { return _lastSavedFileName; } }
        #endregion

        #region Service Settings
        public MimeKitHelper Connect(string host, int port, bool enableSsl = false)
        {
            _mailService?.Connect(host, port, enableSsl);
            return this;
        }

        public MimeKitHelper Authenticate(string username, string password) =>
            Authenticate(new NetworkCredential(username, password));

        public MimeKitHelper Authenticate(NetworkCredential credentials)
        {
            _mailService?.Authenticate(credentials);
            return this;
        }

        public MimeKitHelper SetSslProtocol(SslProtocols protocol)
        {
            if (_mailService != null) _mailService.SslProtocols = protocol;
            return this;
        }

        public MimeKitHelper RemoveAuthenticationMechanism(string value)
        {
            _mailService?.AuthenticationMechanisms.Remove(value);
            return this;
        }

        public byte[] ReadMimeMessageByteArray(MimeMessage message)
        {
            using (var ms = new MemoryStream())
            {
                message.WriteTo(ms);
                return ms.ToArray();
            }
        }
        #endregion

        #region Message Settings
        #region Clear All
        public IMailHelper<MimeMessage> Clear()
        {
            _fromMailboxAddress = null;
            _toMailboxAddresses = null;
            _ccMailboxAddresses = null;
            _subject = null;
            _messageEntity = null;
            _messagePart = null;
            _attachments = null;
            _customValues = null;
            _customHeaders = null;
            return this;
        }
        #endregion

        #region Header Config
        public IMailHelper<MimeMessage> AddCustomHeader(string headerKey, string headerValue)
        {
            if (_customHeaders == null) _customHeaders = new List<KeyValuePair<string, string>>();
            _customHeaders.RemoveAll(a => a.Key == headerKey);
            _customHeaders.Add(new KeyValuePair<string, string>(headerKey, headerValue));
            return this;
        }
        #endregion

        #region From Address
        public IMailHelper<MimeMessage> SetFromMailboxAddress(string name, string emailAddress) =>
            SetFromMailboxAddress(new MailboxAddress(name, emailAddress));

        public MimeKitHelper SetFromMailboxAddress(MailboxAddress mailboxAddress)
        {
            if (!string.IsNullOrWhiteSpace(mailboxAddress.Address))
                _fromMailboxAddress = mailboxAddress;
            return this;
        }
        #endregion

        #region To Address
        public IMailHelper<MimeMessage> SetToMailboxAddress(string name, string emailAddress) =>
           SetToMailboxAddress(new MailboxAddress(name, emailAddress));

        public MimeKitHelper SetToMailboxAddress(MailboxAddress mailboxAddress)
        {
            if (_toMailboxAddresses == null) _toMailboxAddresses = new List<MailboxAddress>();
            else _toMailboxAddresses.Clear();
            if (!string.IsNullOrWhiteSpace(mailboxAddress.Address))
                _toMailboxAddresses.Add(mailboxAddress);
            return this;
        }

        public IMailHelper<MimeMessage> AddToMailboxAddress(string name, string emailAddress) =>
           AddToMailboxAddress(new MailboxAddress(name, emailAddress));

        public MimeKitHelper AddToMailboxAddress(MailboxAddress mailboxAddress)
        {
            if (_toMailboxAddresses == null) _toMailboxAddresses = new List<MailboxAddress>();
            if (!string.IsNullOrWhiteSpace(mailboxAddress.Address))
                _toMailboxAddresses.Add(mailboxAddress);
            return this;
        }
        #endregion

        #region Cc Address
        public IMailHelper<MimeMessage> SetCcMailboxAddress(string name, string emailAddress) =>
           SetCcMailboxAddress(new MailboxAddress(name, emailAddress));

        public MimeKitHelper SetCcMailboxAddress(MailboxAddress mailboxAddress)
        {
            if (_ccMailboxAddresses == null) _ccMailboxAddresses = new List<MailboxAddress>();
            else _ccMailboxAddresses.Clear();
            if (!string.IsNullOrWhiteSpace(mailboxAddress.Address))
                _ccMailboxAddresses.Add(mailboxAddress);
            return this;
        }

        public IMailHelper<MimeMessage> AddCcMailboxAddress(string name, string emailAddress) =>
           AddCcMailboxAddress(new MailboxAddress(name, emailAddress));

        public MimeKitHelper AddCcMailboxAddress(MailboxAddress mailboxAddress)
        {
            if (_ccMailboxAddresses == null) _ccMailboxAddresses = new List<MailboxAddress>();
            if (!string.IsNullOrWhiteSpace(mailboxAddress.Address))
                _ccMailboxAddresses.Add(mailboxAddress);
            return this;
        }
        #endregion

        #region Subject
        public IMailHelper<MimeMessage> SetSubject(string subject)
        {
            _subject = subject;
            return this;
        }
        #endregion

        #region Message Body
        public IMailHelper<MimeMessage> SetMessageBody(string message) =>
            SetMessageBody(message, message.Contains("<body>") ? TextFormat.Html : TextFormat.Plain);

        public MimeKitHelper SetMessageBody(string message, TextFormat textFormatType) =>
            SetMessage(new TextPart(textFormatType) { Text = message }, message);

        public MimeKitHelper SetMessage(MimePart message, string messageBody)
        {
            _messageBody = messageBody;
            _messagePart = message;
            return this;
        }

        public MimeKitHelper SetMessageBodyCustomValue(string key, string value)
        {
            if (_customValues == null) _customValues = new List<KeyValuePair<string, string>>();
            var keyvalue = _customValues.SingleOrDefault(a => a.Key == key);
            if (!_customValues.Any(a => a.Key == key)) _customValues.Add(keyvalue);
            return this;
        }

        public MimeKitHelper ResetMessageBodyCustomValues()
        {
            _customValues = null;
            return this;
        }

        public MimeKitHelper ApplyMessageBodyCustomValues()
        {
            if (_customValues != null && _messagePart != null)
            {
                var tempMessageBody = _messageBody;
                _customValues.ForEach(a => tempMessageBody.Replace(a.Key, a.Value));
                (_messagePart as TextPart).Text = tempMessageBody;
            }
            return this;
        }
        #endregion

        #region Attachments
        public MimeKitHelper SetAttachment(string filePath) =>
            SetAttachment(Path.GetFileName(filePath), File.OpenRead(filePath));

        public MimeKitHelper SetAttachment(string fileName, byte[] byteArray) =>
            SetAttachment(fileName, new MemoryStream(byteArray));

        public MimeKitHelper SetAttachment(string fileName, Stream stream) =>
            SetAttachment(new MimePart(MimeTypes.GetMimeType(fileName))
            {
                Content = new MimeContent(stream, ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = fileName
            });

        public MimeKitHelper SetAttachment(MimePart attachement)
        {
            if (_attachments == null) _attachments = new List<MimePart>();
            else _attachments.Clear();
            _attachments.Add(attachement);
            return this;
        }

        public MimeKitHelper AddAttachment(string filePath) =>
            AddAttachment(Path.GetFileName(filePath), File.OpenRead(filePath));

        public MimeKitHelper AddAttachment(string fileName, byte[] byteArray) =>
            AddAttachment(fileName, new MemoryStream(byteArray));

        public MimeKitHelper AddAttachment(string fileName, Stream stream) =>
            AddAttachment(new MimePart(MimeTypes.GetMimeType(fileName))
            {
                Content = new MimeContent(stream, ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = fileName
            });

        public IMailHelper<MimeMessage> AddAttachment(string fileName, byte[] byteArray, string contentType) =>
            AddAttachment(fileName, byteArray);

        public MimeKitHelper AddAttachment(MimePart attachement)
        {
            if (_attachments == null) _attachments = new List<MimePart>();
            _attachments.Add(attachement);
            return this;
        }
        #endregion
        #endregion

        #region Read Messages
        public IMailFolder InboxFolder()
        {
            if (_mailService is IMailStore mailService)
                return mailService.Inbox;
            return null;
        }

        public IList<IMailFolder> RetrieveEmailFolders()
        {
            if (_mailService is IMailStore mailService)
                return mailService.GetFolders(mailService.PersonalNamespaces.First());
            return null;
        }

        public IList<UniqueId> RetrieveEmailsFromFolder(SearchQuery query, IMailFolder folder)
        {
            if (_mailService is IMailStore mailService)
            {
                OpenFolder(folder, FolderAccess.ReadOnly);
                var emailIds = folder.Search(query);
                CloseFolder(folder);
                return emailIds;
            }
            return null;
        }

        public MimeMessage RetrieveMessageFromFolder(UniqueId id, IMailFolder folder)
        {
            if (_mailService is IMailStore mailService)
            {
                OpenFolder(folder, FolderAccess.ReadOnly);
                var message = ReadMimeMessage(id, folder);
                CloseFolder(folder);
                return message;
            }
            return null;
        }

        public void OpenFolder(IMailFolder folder, FolderAccess access) =>
            folder.Open(access);

        public MimeMessage ReadMimeMessage(UniqueId id, IMailFolder folder) =>
            folder.GetMessage(id);

        public HeaderList ReadHeaderList(UniqueId id, IMailFolder folder) =>
            folder.GetHeaders(id);

        public void CloseFolder(IMailFolder folder) =>
            folder.Close();

        public MailAttachmentType GetAttachmentType(MimeEntity attachment)
        {
            if (attachment is MessagePart) return MailAttachmentType.EmailAttachment;
            if (attachment is MimePart) return MailAttachmentType.FileAttachment;
            return MailAttachmentType.Unknown;
        }

        public byte[] ReadFileAttachment(MimeEntity attachment, out string fileName)
        {
            if (GetAttachmentType(attachment) == MailAttachmentType.FileAttachment)
                return ReadByteArrayForAttachment(attachment, out fileName);
            else throw new NotSupportedException("Attachment is not of a valid type");
        }

        public MimeMessage ReadMailAttachment(MimeEntity attachment)
        {
            if (GetAttachmentType(attachment) == MailAttachmentType.EmailAttachment)
                if (attachment is MessagePart rfc822) return rfc822.Message;
            throw new NotSupportedException("Attachment is not of a valid type");
        }

        public byte[] ReadByteArrayForAttachment(MimeEntity attachment, out string fileName)
        {
            fileName = attachment.ContentDisposition?.FileName ??
                        attachment.ContentType.Name;
            using (MemoryStream ms = new MemoryStream())
            {
                if (attachment is MessagePart rfc822) rfc822.Message.WriteTo(ms);
                else ((MimePart)attachment).Content.DecodeTo(ms);
                return ms.ToArray();
            }
        }
        #endregion

        #region Send Messages
        private void SetupMessageEntity()
        {
            var multiparts = new Multipart(MultipartTypesHelper.Mixed);
            if (_messagePart != null) multiparts.Add(_messagePart);
            _attachments?.ForEach(multiparts.Add);
            _customHeaders?.ForEach(a => multiparts.Headers.Add(a.Key, a.Value));

            _messageEntity = multiparts;
        }

        public MimeMessage SendMessage() =>
            SendMessage(true);

        public MimeMessage SendMessage(bool sendEmail = true)
        {
            SetupMessageEntity();
            var message = new MimeMessage();
            message.From.Add(_fromMailboxAddress);
            _toMailboxAddresses?.ForEach(message.To.Add);
            _ccMailboxAddresses?.ForEach(message.Cc.Add);
            message.Subject = _subject;
            message.Body = _messageEntity;
            if (sendEmail) SendMessage(message);
            return message;
        }

        public List<MimeMessage> SendMessages()
        {
            SetupMessageEntity();
            var messages = new List<MimeMessage>();
            var emails = new List<MailboxAddress>();
            _toMailboxAddresses?.ForEach(emails.Add);
            _ccMailboxAddresses?.ForEach(emails.Add);
            foreach (var email in emails)
            {
                var message = new MimeMessage();
                message.From.Add(_fromMailboxAddress);
                message.To.Add(email);
                message.Subject = _subject;
                message.Body = _messageEntity;
                SendMessage(message);
            }
            return messages;
        }

        public MimeMessage ForwardMessage(MimeMessage message)
        {
            var sendMessage = new MimeMessage(new[] { _fromMailboxAddress }, _toMailboxAddresses, $"FWD: {message.Subject}",
                new Multipart(MultipartTypesHelper.Mixed)
                {
                    message.Body
                });
            SendMessage(sendMessage);
            return sendMessage;
        }

        public IMailHelper<MimeMessage> SendMessage(MimeMessage message)
        {
            if (string.IsNullOrWhiteSpace(_emailDirectory))
                (_mailService as IMailTransport).Send(message);
            else
                _lastSavedFileName = SaveToDirectory(message);
            return this;
        }

        private string SaveToDirectory(MimeMessage message)
        {
            if (!Directory.Exists(_emailDirectory))
                Directory.CreateDirectory(_emailDirectory);

            var fileName = $"{Guid.NewGuid()}.eml";

            using (var stream = new FileStream(Path.Combine(_emailDirectory, fileName), FileMode.CreateNew))
                message.WriteTo(stream);

            return fileName;
        }
        #endregion

        #region Delete Messages
        public void DeleteMessage(UniqueId messageId, IMailFolder folder)
        {
            if (_mailService is IMailStore mailService)
            {
                OpenFolder(folder, FolderAccess.ReadWrite);
                folder.AddFlags(messageId, MessageFlags.Deleted, true);
                CloseFolder(folder);
            }
        }
        #endregion

        public void Dispose()
        {
            _mailService?.Disconnect(true);
            _mailService?.Dispose();
        }
    }
}
