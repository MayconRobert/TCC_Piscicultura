using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site_piscicultura.Models;
using System.Web.Services;

namespace Site_piscicultura.Controllers
{
    public class BiometriaController : Controller
    {

        // GET: Biometria
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuBiometria()
        {
            return View();
        }
        public ActionResult RegistrarBiometria()
        {
            ModeloLogin ml = new ModeloLogin();
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
            ModeloTanque mt = new ModeloTanque();
            
            
            // Está listando o nome do tanque mais de uma vez, rever a query do banco de dados
            ViewBag.NomeDoTanque = new SelectList(
               new ModeloTanque().SelectNomeTanques(ml.Cpf),
               "Id",
               "Nome"
               );
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarBiometria(int Qtd_PeixesTanque, int Qtd_PeixesAmostra, int NomeDoTanque, string Peso_Medio, double Peso_Amostra, double Ganho_PesoAmostra, string Ganho_PesoMedio, string Data)
        {
            ModeloBiometria m = new ModeloBiometria();
            ModeloTanque mt = new ModeloTanque();

            m.Qtd_peixesAmostra = Qtd_PeixesAmostra;
            m.Peso_Amostra = Peso_Amostra;
            m.Peso_Medio = Peso_Medio;
            m.Ganho_PesoAmostra = Ganho_PesoAmostra;
            m.Ganho_PesoMedio = Ganho_PesoMedio;
            m.Data = Data;
            m.Id_Tanque = NomeDoTanque;
            m.Qtd_peixesTanque = m.PesquisaNumeroPeixes(m.Id_Tanque);


            TempData["Msg"] = m.RegistrarBiometria();

            return RedirectToAction("RegistrarBiometria", "Biometria"); // View e controller
        }
        

        [WebMethod]
        public JsonResult GetQuantidadePeixes(int id)
        {
            ModeloBiometria mb = new ModeloBiometria();
            return Json(mb.PesquisaNumeroPeixes(id), JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult EscolherTanque()
        {
            ModeloLogin ml = new ModeloLogin();
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
            ViewBag.NomeDoTanque = new SelectList(
               new ModeloTanque().SelectNomeTanques(ml.Cpf),
               "Id",
               "Nome"
               );
           
            return View();
        }

        [HttpPost]
        public ActionResult EscolherTanque(int NomeDoTanque)
        {
            TempData["IdTanque"] = NomeDoTanque;
            return RedirectToAction("ListaBiometria", "Biometria");
        }

        [HttpGet]
        public ActionResult ListaBiometria()
        {
            int idTanque = (int)TempData["IdTanque"];
            return View(ModeloBiometria.ListarBiometria(idTanque));
        }

        //public ActionResult EditarBiometria(int id)
        //{
       
        //    ModeloBiometria mb = ModeloBiometria.BuscaBiometria(id);
        //    if (mb == null)
        //    {
        //        //MENSAGEM DE CONFIRMAÇÃO
        //        TempData["Msg"] = "Erro ao buscar Produto!";
        //        //RETORNA PARA A VIEW 
        //        return RedirectToAction("Menu", "Home");
        //    }

        //    return View(mb);

        //}

        //[HttpPost]
        //public ActionResult EditarBiometria(int id, int Qtd_PeixesTanque, int Qtd_PeixesAmostra,  double Peso_Amostra, double Peso_Medio, double Ganho_PesoAmostra, double Ganho_PesoMedio, string Data)
        //{
        //    ModeloBiometria mb = new ModeloBiometria();
        
        //    ModeloLogin ml = new ModeloLogin();
        //    mb.Qtd_peixesTanque = Qtd_PeixesTanque;
        //    mb.Qtd_peixesAmostra = Qtd_PeixesAmostra;
        //    //mb.Id_Tanque = NomeDoTanque;
        //    mb.Peso_Amostra = Peso_Amostra;
        //    mb.Peso_Medio = Peso_Medio;
        //    mb.Ganho_PesoAmostra = Ganho_PesoAmostra;
        //    mb.Ganho_PesoMedio = Ganho_PesoMedio;
        //    mb.Data = Data;

          
        //    string res = mb.EditarBiometria(id);
        //    TempData["Msg"] = res;
        //    if (res == "Editado com Sucesso")
        //    {
        //        //return RedirectToAction("ListaEstoque" + "/" + (sec), "Estoque");
        //        return RedirectToAction("EscolherTanque", "Biometria");
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
        public ActionResult Remover(int id)
        {
            int sec;
            //sec = (int) Session["idpis"];
            ModeloBiometria mb = new ModeloBiometria();
            mb.RemoverBiometria(id);
            //return RedirectToAction("ListaEstoque"+"/"+(sec), "Estoque");
            return RedirectToAction("EscolherTanque", "Biometria");
        }
    }
}