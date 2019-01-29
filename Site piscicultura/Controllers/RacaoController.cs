using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site_piscicultura.Models;
namespace Site_piscicultura.Controllers
{
    public class RacaoController : Controller
    {
        // GET: Racao
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuRacao()
        {
            return View();
        }

        public ActionResult CadastraRacao()
        {
            return View();
        }

        [HttpPost]

        public ActionResult CadastraRacao(string nome, string fabricante, string peso, string tamanho, string proteina, string ingredientePrincipal, string tipoRacao)
        {
            ModeloRacao m = new ModeloRacao();
            ModeloUsuario mu = new ModeloUsuario();
            m.Nome = nome;
            m.Fabricante = fabricante;
            m.Peso = peso;
            m.Tamanho = tamanho;
            m.Proteina = proteina;
            m.IngredientePrincipal = ingredientePrincipal;
            m.TipoRacao = tipoRacao;
            m.CpfUsuario = ((ModeloLogin)Session["User"]).Cpf.ToString();
            int plano = mu.PesquisaUsuario(m.CpfUsuario).Plano;
            int numeroRacoesCadastradas = m.pesquisaNumeroRacoes(m.CpfUsuario);

            if (plano == 1 && numeroRacoesCadastradas <= 2 || plano == 2 && numeroRacoesCadastradas <= 4 || plano == 3)
            {
                TempData["Msg"] = m.AdicionarRacao();
            }
            else
            {
                TempData["Msg"] = "Você não pode cadastrar uma ração";
            }



            return RedirectToAction("CadastraRacao", "Racao");
        }

        public ActionResult ListaRacao()
        {
           
            string cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
            return View(ModeloRacao.ListarRacao(cpf));
        }

        public ActionResult EditarRacao(int id)
        {
            ModeloRacao mr = ModeloRacao.BuscaRacao(id);


            if (mr == null)
            {
                //MENSAGEM DE CONFIRMAÇÃO
                TempData["Msg"] = "Erro ao buscar usuário!";
                //RETORNA PARA A VIEW 
                return RedirectToAction("Home");
            }

            return View(mr);

        }

        [HttpPost]
        public ActionResult EditarRacao(int id, string nome, string fabricante, string peso, string tamanho, string proteina, string ingredientePrincipal, string tipoRacao)
        {

            ModeloRacao mr = new ModeloRacao();
            mr.Id = id;
            mr.Nome = nome;
            mr.Fabricante = fabricante;
            mr.Peso = peso;
            mr.Tamanho = tamanho;
            mr.Proteina = proteina;
            mr.IngredientePrincipal = ingredientePrincipal;
            mr.TipoRacao = tipoRacao;

            string res = mr.EditarRacao();
            TempData["Msg"] = res;
            if (res == "Editado com Sucesso")
            {
                return RedirectToAction("ListaRacao", "Racao");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Remover(int id)
        {
            ModeloRacao mr = new ModeloRacao();
            mr.Remover(id);
            return RedirectToAction("ListaRacao", "Racao");
        }
    }
}