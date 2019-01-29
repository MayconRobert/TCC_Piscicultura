using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Site_piscicultura.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Contato()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contato(String nome, String email, String mensagem)
        {

            //Configurando a mensagem
            MailMessage mail = new MailMessage();
            //Origem
            mail.From = new MailAddress("pisciculturazerbini@gmail.com");
            //Destinatário
            mail.To.Add("pisciculturazerbini@gmail.com");
            //Assunto
            mail.Subject = "Mensagem da Homepage";
            //Permitir que hiperlink no corpo do email
            mail.IsBodyHtml = true;
            //Corpo do e-mail
            mail.Body = string.Format("Email do remetente: {0} <br />" +
                "Nome do Remetente: {1}<br /><br />" +
                "{2}", email, nome, mensagem);

            //Configurar o smtp
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            //configurou porta
            smtpServer.Port = 587;
            //Habilitou o TLS
            smtpServer.EnableSsl = true;
            //Configurou usuario e senha p/ logar
            smtpServer.Credentials = new System.Net.NetworkCredential("pisciculturazerbini@gmail.com", "pisciculturaZ");

            //Envia
            smtpServer.Send(mail);
            TempData["Msg"] = "Mensagem enviada com Sucesso";
            return RedirectToAction("Index", "Home");

        }

    }
}