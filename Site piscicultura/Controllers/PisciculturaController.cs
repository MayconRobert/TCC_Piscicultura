using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site_piscicultura.Models;
namespace Site_piscicultura.Controllers
{
    public class PisciculturaController : Controller
    {
        // GET: Piscicultura
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrarPiscicultura()
        {
            return View();
        }
        public ActionResult MenuPiscicultura()
        {
            return View();
        }
        [HttpPost]

        public ActionResult RegistrarPiscicultura(string nome, string local, string clima, string tamanhoLocal)
        {
            ModeloPiscicultura m = new ModeloPiscicultura();
            ModeloTanque mt = new ModeloTanque();
            //m.Id = id;
            m.Nome = nome;
            m.Qtd_Tanques = mt.PesquisaNumeroTanques(m.Cpf_usuario, m.Id);
            m.Local = local;
            m.Clima = clima;
            m.TamanhoLocal = tamanhoLocal;
            m.Cpf_usuario = ((ModeloLogin)Session["User"]).Cpf.ToString();

            ModeloUsuario mu = new ModeloUsuario();
            int plano = -1, numeroPisciculturas = 0;
            plano = mu.PesquisaUsuario(m.Cpf_usuario).Plano;
            numeroPisciculturas = m.PesquisaNumeroPisciculturas(m.Cpf_usuario);

            if (plano == 1 && numeroPisciculturas < 1 || plano == 2 && numeroPisciculturas <=2 || plano == 3)
            {
            TempData["Msg"] = m.CriarPiscicultura();
            }
            
            else
            {
                TempData["Msg"] = "Você não pode registrar uma piscicultura";
            }
            
            return RedirectToAction("RegistrarPiscicultura", "Piscicultura");
        }

        public ActionResult ListaPiscicultura()
        {
            ModeloPiscicultura m = new ModeloPiscicultura();
            m.Cpf_usuario = ((ModeloLogin)Session["User"]).Cpf.ToString();
            return View(ModeloPiscicultura.ListarPiscicultura(m.Cpf_usuario));
        }

        public ActionResult EditarPiscicultura(int id)
        {
            //UTILIZAR O OBJETO USUARIO PARA CHAMAR O MÉTODO BUSCAUSUARIO DE CONTROLLER
            ModeloPiscicultura b = ModeloPiscicultura.BuscaPiscicultura(id);
            //COMPARAR PARA VER SE ENCONTRAR UM OBJETO IGUAL
            if (b == null)
            {
                //MENSAGEM DE CONFIRMAÇÃO
                TempData["Msg"] = "Erro ao buscar piscicultura!";
                //RETORNA PARA A VIEW 
                return RedirectToAction("ListarPiscicultura");
            }
            return View(b);
        }

        //---------------------------------METODO EDITAR-----------------------------------//
        [HttpPost]
        // CONSTRUTOR COM OS ATRIBUTOS
        public ActionResult EditarPiscicultura(string nome, string local, string clima, int qtd_Tanques, string tamanhoLocal, int id)
        {
            //CRIANDO OBJETO DE USUARIO
            ModeloPiscicultura b = new ModeloPiscicultura();

            //ATRIBUINDO O VALORES 
            b.Nome = nome;
            b.Local = local;
            b.Clima = clima;
            b.Qtd_Tanques = qtd_Tanques;
            b.TamanhoLocal = tamanhoLocal;
            b.Cpf_usuario = ((ModeloLogin)Session["User"]).Cpf.ToString();
            b.Id = id;

            //CHAMA MÉTODO DO CONTROLLER
            string res = b.EditarPiscicultura();
            TempData["Msg"] = res;
            // COMPARA SE ESTA CORRETO E APRESENTA A MENSAGEM DE CONFIRMAÇÃO
            if (res == "Editado com sucesso!")
                //RETORNA PARA A VIEW 
                return RedirectToAction("ListaPiscicultura");
            else
                return View();
        }

        public ActionResult DeletarPiscicultura(int id)
        {
            ModeloPiscicultura m = new ModeloPiscicultura();
            m.Id = id;
            m.RemoverPiscicultura();
            return RedirectToAction("ListaPiscicultura");
        }
    }
}