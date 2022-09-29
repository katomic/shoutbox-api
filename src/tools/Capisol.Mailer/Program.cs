using Capisol.Shared.Email;

Console.WriteLine("Sending Email...");

using (var emailHelper = new MimeKitHelper())
{
    emailHelper
        .SetFromMailboxAddress("Capisol Admin", "admin@capisol.dev")
        .SetToMailboxAddress("Capisol Client", "client@capisol.dev")
        .SetSubject("Example Subject")
        .SetMessageBody("Example Message");

    emailHelper.SendMessage();
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Email Sent...");

Console.ForegroundColor = ConsoleColor.DarkGray;
Console.Write($"Exit in: ");
for (int i = 5; i > 0; i--)
{
    Console.Write($"{i}..");
    Thread.Sleep(1000);
}