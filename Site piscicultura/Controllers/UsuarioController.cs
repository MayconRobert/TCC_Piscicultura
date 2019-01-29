using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site_piscicultura.Models;
using System.Text;
using System.IO;
using System.Net;
using Fluentx.Mvc;

namespace Site_piscicultura.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuUsuario()
        {
            return View();
        }


        public ActionResult CadastraUsuario()
        {



            return View();
        }

        [HttpPost]
        public ActionResult CadastraUsuario(string cmd, string hosted_button_id, int plano, string nome, string senha, string email, string estado, string cidade, string bairro, string endereco, string num_Casa, string complemento, string cpf)
        {
            ModeloUsuario m = new ModeloUsuario();            
            m.Nome = nome;
            m.Senha = senha;
            m.Email = email;
            m.Estado = estado;
            m.Cidade = cidade;
            m.Bairro = bairro;
            m.Endereco = endereco;
            m.Num_Casa = num_Casa;
            m.Complemento = complemento;
            m.Plano = plano;
            m.Atuacao = 0;
            m.Notificacao = 0;
            m.Cpf = cpf;
            m.Estatus = 0;
            //CHAMA MÉTODO DO CONTROLLER
            if (m.validaCpf().Equals("CPF Válido") && m.validaEmail())
            {
                TempData["Msg"] = m.CriarUsuario();
                //Dictionary<string, object> postData = new Dictionary<string, object>();
                //postData.Add("hosted button id", hosted_button_id);
                //postData.Add("cmd", cmd);
                //postData.Add("on0", "Tipo de Plano".Replace(' ', '+'));
                //postData.Add("currency_code", "BRL");
                //switch (plano)
                //{
                //    case 1:
                //        postData.Add("os0", "Plano Iniciante".Replace(' ', '+'));
                //        postData.Add("submit.x", 73);
                //        postData.Add("submit.y", 10);
                //        break;
                //    case 2:
                //        postData.Add("os0", "Plano Avançado".Replace(' ', '+'));
                //        postData.Add("submit.x", 73);
                //        postData.Add("submit.y", 10);
                //        break;
                //    case 3:
                //        postData.Add("os0", "Plano Premium".Replace(' ', '+'));
                //        postData.Add("submit.x", 73);
                //        postData.Add("submit.y", 10);
                //        break;
                //}

                //return this.RedirectAndPost("https://www.sandbox.paypal.com/cgi-bin/webscr", postData);

                string postData = "cmd=" + cmd + "&hosted_button_id=" + hosted_button_id;
                postData += "&on0=Tipo+de+Plano";
                switch (plano)
                {
                    case 1:
                        postData += "&os0=Plano+Iniciante";
                        break;
                    case 2:
                        postData += "&os0=Plano+Avan%C3%A7ado";
                        break;
                    case 3:
                        postData += "&os0=Plano+Premium";
                        break;
                }
                postData += "&currency_code=BRL&submit.x=73&submit.y=10#/checkout/subscription";

                return Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?" + postData);

            }
            else
            {
                TempData["Msg"] = "Dados inválidos";
                return RedirectToAction("Home", "Index");
            }


        }

        public ActionResult EditarUsuario(string cpf)
        {
            string cpfUsuario = ((ModeloLogin)Session["User"]).Cpf.ToString();
            ModeloUsuario mu = ModeloUsuario.BuscaUsuario(cpfUsuario);


            if (mu == null)
            {
                //MENSAGEM DE CONFIRMAÇÃO
                TempData["Msg"] = "Erro ao buscar usuário!";
                //RETORNA PARA A VIEW 
                return RedirectToAction("Home");
            }

            return View(mu);

        }

        [HttpPost]
        public ActionResult EditarUsuario(string nome, string email, string endereco, string estado, string cidade, string bairro, string num_Casa, string complemento)
        {

            ModeloUsuario mu = new ModeloUsuario();
            mu.Nome = nome;
            mu.Email = email;
            mu.Endereco = endereco;
            mu.Estado = estado;
            mu.Cidade = cidade;
            mu.Bairro = bairro;
            mu.Num_Casa = num_Casa;
            mu.Complemento = complemento;
            mu.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();

            string res = mu.EditarUsuario();
            TempData["Msg"] = res;
            if (res == "Editado com sucesso")
            {
                return RedirectToAction("Menu", "Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult EditarPlano()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditarPlano(int plano, string hosted_button_id, string cmd)
        {
            ModeloUsuario pl = new ModeloUsuario();

            pl.Cpf = ((ModeloLogin)Session["User"]).Cpf.ToString();
            pl.Plano = plano;

            string res = pl.EditarPlano();
            TempData["Msg"] = res;

            if (res == "Plano editado com suceso!")
            {
                string postData = "cmd=" + cmd + "&hosted_button_id=" + hosted_button_id;
                postData += "&on0=Tipo+de+Plano";
                switch (plano)
                {
                    case 1:
                        postData += "&os0=Plano+Iniciante";
                        break;
                    case 2:
                        postData += "&os0=Plano+Avan%C3%A7ado";
                        break;
                    case 3:
                        postData += "&os0=Plano+Premium";
                        break;
                }
                postData += "&currency_code=BRL&submit.x=73&submit.y=10#/checkout/subscription";



                return Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?" + postData);


            }
            else

                return View();
        }

        //---------------------------MÉTODO PARA VERIFICAR SE FOI REALIZADO O PAGAMENTO------------

        //[HttpGet]
        //public ActionResult PosttoPaypalShow()
        //{

        //    SportsStore.Models.Paypal payPal = new Paypal();
        //    payPal.cmd = "_xclick";
        //    payPal.business = ConfigurationManager.AppSettings["BusinessAccount"];
        //    bool useSendBox = Convert.ToBoolean(ConfigurationManager.AppSettings["useSendbox"]);
        //    if (useSendBox)
        //    {
        //        ViewBag.actionURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        //    }
        //    else
        //    {
        //        ViewBag.actionURL = "https://www.paypal.com/cgi-bin/webscr";
        //    }
        //    payPal.cancel_return = System.Configuration.ConfigurationManager.AppSettings["CancelUrl"];
        //    payPal.@return = ConfigurationManager.AppSettings["ReturnURL"];
        //    payPal.notify_url = ConfigurationManager.AppSettings["NotifyURL"];
        //    payPal.currency_code = ConfigurationManager.AppSettings["currencycode"];
        //    //payPal.item_Name = ProductName;
        //    payPal.item_Name = "test1";
        //    payPal.Descriptions = "tes2";
        //    payPal.amount = String.Format("{0:0.##}", Session["carttotal"]); //Convert.ToString(Session["carttotal"].ToString("0.00"));
        //    return View(payPal);
        //}
        //public ActionResult PaypalAddressAndPayment()
        //{

        //    Tbl_Order order = new Tbl_Order();
        //    var cart = ShoppingCart.GetCart(this.HttpContext);


        //    // Set up the ViewModel
        //    var viewModel = new CheckoutViewModel
        //    {
        //        CartItems = cart.GetCartItems(),
        //        CartTotal = cart.GetTotal(),
        //        Tbl_Order = order
        //    };
        //    Session["CartItems"] = viewModel.CartItems;
        //    return View(viewModel);
        //    //return View(order);
        //}
        //string GetPayPalResponse(Dictionary<string, string> formVals, bool useSandbox)
        //{

        //    string paypalUrl = useSandbox ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
        //        : "https://www.paypal.com/cgi-bin/webscr";


        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);

        //    // Set values for the request back
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";

        //    byte[] param = Request.BinaryRead(Request.ContentLength);
        //    string strRequest = Encoding.ASCII.GetString(param);

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(strRequest);

        //    foreach (string key in formVals.Keys)
        //    {
        //        sb.AppendFormat("&{0}={1}", key, formVals[key]);
        //    }
        //    strRequest += sb.ToString();
        //    req.ContentLength = strRequest.Length;

        //    //for proxy
        //    //WebProxy proxy = new WebProxy(new Uri("http://urlort#");
        //    //req.Proxy = proxy;
        //    //Send the request to PayPal and get the response
        //    string response = "";
        //    using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
        //    {

        //        streamOut.Write(strRequest);
        //        streamOut.Close();
        //        using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
        //        {
        //            response = streamIn.ReadToEnd();
        //        }
        //    }

        //    return response;
        //}

        //public ActionResult IPN()
        //{

        //    var formVals = new Dictionary<string, string>();
        //    formVals.Add("cmd", "_notify-validate");

        //    string response = GetPayPalResponse(formVals, true);

        //    if (response == "VERIFIED")
        //    {

        //        string transactionID = Request["txn_id"];
        //        string sAmountPaid = Request["mc_gross"];
        //        string orderID = Request["custom"];
        //        string pay_Status = Request["payment_status"];

        //        //_logger.Info("IPN Verified for order " + orderID);

        //        //validate the order
        //        Decimal amountPaid = 0;
        //        Decimal.TryParse(sAmountPaid, out amountPaid);

        //        //Order order = _orderService.GetOrder(new Guid(orderID));
        //        Tbl_Order order = null;
        //        //check the amount paid

        //        if (AmountPaidIsValid(order, amountPaid))
        //        {

        //            Tbl_Order add = new Tbl_Order();
        //            add.Username = User.Identity.Name;
        //            //add.FirstName = Request["first_name"];
        //            //add.LastName = Request["last_name"];
        //            //add.Email = Request["payer_email"];
        //            //add.Address = Request["address_street"];
        //            //add.City = Request["address_city"];
        //            //add.State = Request["address_state"];
        //            //add.Country = Request["address_country"];
        //            //add.PostalCode = Request["address_zip"];
        //            add.TransactionId = transactionID;
        //            add.Status = pay_Status;
        //            add.CartTotal = Convert.ToDecimal(sAmountPaid);


        //            //process it
        //            try
        //            {
        //                _OrderContext.OrderEntries.Add(add);
        //                _OrderContext.SaveChanges();
        //                //_pipeline.AcceptPalPayment(order, transactionID, amountPaid);
        //                //_logger.Info("IPN Order successfully transacted: " + orderID);
        //                //return RedirectToAction("Receipt", "Order", new { id = order.ID });
        //            }
        //            catch
        //            {
        //                //HandleProcessingError(order, x);
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            //let fail - this is the IPN so there is no viewer
        //        }
        //    }

        //}
    }
}