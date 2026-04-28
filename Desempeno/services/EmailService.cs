using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Desempeno.services;

public class EmailService
{
    public void EnviarCorreo(string destino, string asunto, string cuerpo)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Gestor Reservas", "no-reply@riwi.com"));
        message.To.Add(new MailboxAddress("Usuario", destino));
        message.Subject = asunto;
        message.Body = new TextPart("plain") { Text = cuerpo };

        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls );
        client.Authenticate("xzmangoxz@gmail.com", "bmag gtpj opfx xvtx");
        client.Send(message);
        client.Disconnect(true);
    }
}