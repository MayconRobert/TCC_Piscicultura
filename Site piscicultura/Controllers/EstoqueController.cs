using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site_piscicultura.Models;
namespace Site_piscicultura.Controllers
{
    public class EstoqueController : Controller
    {
        // GET: Estoque
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuEstoque()
        {
            return View();
        }

      


        public ActionResult CadastraEstoque()
        {

            ModeloLogin ml = new ModeloLogin();
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
            ViewBag.NomeDaPiscicultura = new SelectList
                   (
                       new ModeloTanque().SelectNomePisciculturas(ml.Cpf),
                       "Id",
                       "Nome"
                   );

            return View();
        }
        [HttpPost]
        public ActionResult CadastraEstoque(string produto, string categoria, string validade, int quantidade, int NomeDaPiscicultura)
        {
            ModeloEstoque me = new ModeloEstoque();
            ModeloLogin ml = new ModeloLogin();

            me.Produto = produto;
            me.Categoria = categoria;
            me.Validade = validade;
            me.Quantidade = quantidade;
            me.Id_Piscicultura = NomeDaPiscicultura;
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
            TempData["Msg"] = me.InserirEstoque();

            return RedirectToAction("CadastraEstoque", "Estoque");
        }



        public ActionResult EditarEstoque(int id)
        {
            ModeloEstoque me = ModeloEstoque.BuscaEstoque(id);


            if (me == null)
            {
                //MENSAGEM DE CONFIRMAÇÃO
                TempData["Msg"] = "Erro ao buscar Produto!";
                //RETORNA PARA A VIEW 
                return RedirectToAction("Menu","Home");
            }

            return View(me);

        }

        [HttpPost]
        public ActionResult EditarEstoque(int id, string produto, string categoria, string validade, int quantidade)
        {
            int sec;
            //sec = (int)Session["idpis"];
            ModeloEstoque me = new ModeloEstoque();
            ModeloLogin ml = new ModeloLogin();

            me.Produto = produto;
            me.Categoria = categoria;
            me.Validade = validade;
            me.Quantidade = quantidade;
            string res = me.EditarEstoque(id);
            TempData["Msg"] = res;
            if (res == "Editado com Sucesso")
            {
                //return RedirectToAction("ListaEstoque" + "/" + (sec), "Estoque");
                return RedirectToAction("EscolherPiscicultura", "Estoque");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Remover(int id)
        {
            int sec;
            //sec = (int) Session["idpis"];
            ModeloEstoque me = new ModeloEstoque();
            TempData["Msg"] = me.Remover(id);
            //return RedirectToAction("ListaEstoque"+"/"+(sec), "Estoque");
            return RedirectToAction("EscolherPiscicultura", "Estoque");
        }

        public ActionResult EscolherPiscicultura()
        {
            ModeloLogin ml = new ModeloLogin();
            ModeloPiscicultura mp = new ModeloPiscicultura();
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
           

            ViewBag.NomeDaPiscicultura = new SelectList(
                new ModeloTanque().SelectNomePisciculturas(ml.Cpf),
                "ID",
                "Nome"
                );

            return View();
        }

        [HttpPost]
        public ActionResult EscolherPiscicultura(int NomeDaPiscicultura)
        {
            TempData["IdPiscicultura"] = NomeDaPiscicultura;
            return RedirectToAction("ListaEstoque", "Estoque");
        }

        public ActionResult ListaEstoque()
        {
            int idPiscicultura = (int)TempData["IdPiscicultura"];
            return View(ModeloEstoque.ListarEstoque(idPiscicultura));
        }
    }
}