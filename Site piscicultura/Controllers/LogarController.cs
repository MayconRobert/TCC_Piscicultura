using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site_piscicultura.Models;
using System.Net.Mail;
using System.Web.Security;

namespace Site_piscicultura.Controllers
{
    public class LogarController : Controller
    {
        // GET: Logar
        public ActionResult Index()
        {
            return View();
        }


        //LOGAR USUARIO NO SISTEMA
        
        public ActionResult Logar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(string nomeUsuario, string senha)
        {
            ModeloLogin m = new ModeloLogin();
            m.NomeUsuario = nomeUsuario;
            m.Senha = senha;
           
            if (m.metodoLogar() != null) // Se conseguiu logar 
            {
                m.Cpf = m.metodoLogar().Cpf;
                m.NivelAcesso = m.metodoLogar().NivelAcesso;
                if (m.Administrador != false)
                {
                    Session["Administrador"] = m;
                }
                else
                {
                    Session["User"] = m;
                }
                TempData["Msg"] = "Logado com sucesso";

                //SE FOR ADMINISTRADOR
                if (Session["Administrador"] != null)
                    return RedirectToAction("Menu", "Home"); // View, controller
                //SE FOR USUARIO
                if (Session["User"] != null)
                    return RedirectToAction("Menu", "Home");
            }
            else
            {
                TempData["Msg"] = "Login Inválido";

            }
            return RedirectToAction("Index", "Home");
        }


        //MÉTODO PARA DESLOGAR O USUARIO

        public ActionResult Sair()
        {
            // CASO FOR USUARIO
            Session["User"] = null;
            //CASO FOR ADMINISTRADOR
            Session["Administrador"] = null;
            return RedirectToAction("Index", "Home");

        }

        //------MÉTODO PARA RECUPERAÇÃO DE SENHA-----------

        public ActionResult RecuperarSenha()
        {
            return View();

        }

        
        [HttpPost]
        public ActionResult RecuperarSenha(String email)
        {
            ModeloLogin rec = new ModeloLogin();
            rec = rec.RecuperarSenha(email);

            rec.Email = email;

            string nome, senha;
            nome = rec.NomeUsuario;
            senha = rec.Senha;
            
            

            if (rec.SelectEmail(rec.Email))
            {
                // CASO ENCONTRAR EXECUTAR O METODO RECUPERAR

                //Configurando a mensagem
                MailMessage mail = new MailMessage();
                //Origem
                mail.From = new MailAddress("pisciculturaZerbini@gmail.com");
                //Destinatário
                mail.To.Add(rec.Email);
                //Assunto
                mail.Subject = "Recuperação de senha da Piscicultura";
                //Permitir que hiperlink no corpo do email
                mail.IsBodyHtml = true;
                //Corpo do e-mail
                mail.Body = string.Format("Esse é um e-mail automático para recuperação de senha do sistema digital Psicium Culture." + 
                    "Não é necessário responder a esta mensagem.\n"+                   
                    "Se você não for um usuário do sistema, por favor,"
                    + " ignore essa mensagem.\nUsuário: {0} \nNova Senha: {1} ",nome,senha);

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
                TempData["Msg"] = "Senha recuperada com sucesso";
                return RedirectToAction("Index", "Home");
            }
      
            // SE NÃO, APRESENTAR MENSAGEM
            else
            {
                TempData["Msg"] = "E-mail não encontrado";
            }
                
           
            return View();
        }


        public ActionResult DefinirNovaSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DefinirNovaSenha(string senha, string nomeUsuario)
        {
            ModeloLogin nva = new ModeloLogin();

            nva.Senha = senha;
            nva.NomeUsuario = nomeUsuario;

            if (senha != "")
                nva.EditarSenha(nva.Senha,nva.NomeUsuario);

            else
                TempData["Msg"] = "Porfavor, entre com uma nova senha";

            return View();

        }




    }



}
