using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site_piscicultura.Models;

namespace Site_piscicultura.Controllers
{
    public class TanqueController : Controller
    {
        // GET: Tanque
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTanque()
        {
            return View();
        }

        public ActionResult CadastraTanque()
        {
            ModeloLogin ml = new ModeloLogin();
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();

            ViewBag.NomeDaPiscicultura = new SelectList(
                new ModeloTanque().SelectNomePisciculturas(ml.Cpf),
                "Id",
                "Nome"
            );

            ViewBag.NomeDoPeixe = new SelectList(
                new ModeloTanque().SelectNomePeixes(),
                "Cod",
                "Especie"
                );

            ViewBag.NomeDaRacao = new SelectList(
                new ModeloTanque().SelectNomeRacoes(),
                "ID",
                "Nome"
                );


            return View();
        }

        [HttpPost]
        public ActionResult CadastraTanque(string nome, int quantidadePeixes, string tipoAgua, float volumeAgua, int NomeDaPiscicultura, int NomeDoPeixe, int NomeDaRacao)
        {
            ModeloTanque m = new ModeloTanque();
            ModeloLogin ml = new ModeloLogin();
            ModeloUsuario mu = new ModeloUsuario();
            ModeloPiscicultura mp = new ModeloPiscicultura();
            ModeloTarefas mt = new ModeloTarefas();
           
            
          
            //m.Id = id;
            m.Nome = nome;
            m.TipoAgua = tipoAgua;
            m.VolumeAgua = volumeAgua;
            m.Piscicultura_id = NomeDaPiscicultura;
            m.Peixes_cod = NomeDoPeixe;
            m.Racoes_id = NomeDaRacao;
            m.QuantidadePeixes = quantidadePeixes;
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();


            int plano = mu.PesquisaUsuario(ml.Cpf).Plano;
            int numeroTanques = m.PesquisaNumeroTanques(ml.Cpf, m.Piscicultura_id);
          

            //ViewBag.NomeDaPiscicultura = new SelectList
            //        (
            //            new ModeloTanque().SelectNomePisciculturas(ml.Cpf),
            //            "Nome",
            //            "Id",
            //            NomeDaPiscicultura // O que deve vir pré-selecionado
            //        );
            //ViewBag.NomeDaPiscicultura = NomeDaPiscicultura;

            if (plano == 1 && numeroTanques < 1 || plano == 2 && numeroTanques <=2 || plano == 3)
            {
                TempData["Msg"] = m.CriarTanque();
                mp.AdicionaNumeroTanques(ml.Cpf, m.Piscicultura_id);
                mt.CriarTarefa(nome);
            }
            else
            {
                TempData["Msg"] = "Você não pode registrar um tanque.";
            }

          

            return RedirectToAction("CadastraTanque", "Tanque");
        }

        

        public ActionResult EditarTanque(int id)
        {
            ViewBag.NomeDaRacao = new SelectList(
               new ModeloTanque().SelectNomeRacoes(),
               "ID",
               "Nome"
               );

            ModeloTanque t = ModeloTanque.BuscaTanque(id);
            if (t == null)
            {
                //MENSAGEM DE CONFIRMAÇÃO
                TempData["Msg"] = "Erro ao buscar piscicultura!";
                //RETORNA PARA A VIEW 
                return RedirectToAction("EscolherPiscicultura");
            }

            return View(t);
        }

        [HttpPost]
        public ActionResult EditarTanque(string nome, int quantidadePeixes, float volumeAgua, string tipoAgua, int id, int NomeDaRacao)
        {
            //CRIANDO OBJETO DE USUARIO
            ModeloTanque b = new ModeloTanque();

            //ATRIBUINDO O VALORES 
            b.Nome = nome;
            b.QuantidadePeixes = quantidadePeixes;
            b.VolumeAgua = volumeAgua;
            b.VolumeAgua = volumeAgua;
            b.TipoAgua = tipoAgua;
            b.Id = id;

            //CHAMA MÉTODO DO CONTROLLER
            string res = b.EditarTanque();
            TempData["Msg"] = res;
            // COMPARA SE ESTA CORRETO E APRESENTA A MENSAGEM DE CONFIRMAÇÃO
            if (res == "Editado com Sucesso")
            {
                //RETORNA PARA A VIEW 
                return RedirectToAction("EscolherPiscicultura", "Tanque");
            }
            else
            {
                return View();
            }
        }

        public ActionResult EscolherPiscicultura()
        {
            ModeloLogin ml = new ModeloLogin();
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();

            ViewBag.NomeDaPiscicultura = new SelectList(
                new ModeloTanque().SelectNomePisciculturas(ml.Cpf),
                "ID",
                "Nome"
                );

            TempData["IdPiscicultura"] = ViewBag.NomeDaPiscicultura;

            return View();
        }

        [HttpPost]
        public ActionResult EscolherPiscicultura(int NomeDaPiscicultura)
        {
            TempData["IdPiscicultura"] = NomeDaPiscicultura;
            return RedirectToAction("ListaTanque", "Tanque");
        }

        public ActionResult ListaTanque()
        {

            int idPiscicultura = (int) TempData["IdPiscicultura"];
            TempData["IdPiscicultura2"] = idPiscicultura;
            return View(ModeloTanque.ListarTanques(idPiscicultura));

        }

        public ActionResult ExcluirTanque(int id)
        {
            int sec;
            //sec = (int) Session["idpis"];
            ModeloTanque mt = new ModeloTanque();
            ModeloLogin ml = new ModeloLogin();
            ModeloPiscicultura mp = new ModeloPiscicultura();
            ml.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
            mt.Piscicultura_id = (int) TempData["IdPiscicultura2"];

            mt.RemoverTanque(id);
            int numeroTanques = mt.PesquisaNumeroTanques(ml.Cpf, mt.Piscicultura_id);
            mp.DiminuiNumeroTanques(ml.Cpf, mt.Piscicultura_id);

            return RedirectToAction("EscolherPiscicultura", "Tanque");
        }

    }
}