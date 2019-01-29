using Site_piscicultura.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Site_piscicultura
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        // SqlConnection con = new SqlConnection("Server=ESN509VMSSQL;Database=Piscicultura;User id=Aluno;Password=Senai1234;");

        SqlConnection con =
           new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //--CRIAR A LISTA DE TAREFAS CADASTRADOS---//
        public void ListarTarefas()
        {

            //Criar um array para guardar as informações que virão do BD
            List<ModeloTarefas> lista = new List<ModeloTarefas>();

            string tarefasJsonString = "";

            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela peixes
                SqlCommand listar = new SqlCommand("SELECT * FROM Tarefas", con);
                SqlDataReader leitor = listar.ExecuteReader();

                //Realiza a captura dos dados caso haja algo na tabela
                if (leitor.HasRows)
                {
                    //var indexOfId = leitor.GetOrdinal("ID");
                    var indexOfTitle = leitor.GetOrdinal("Title");
                    var indexOfStart = leitor.GetOrdinal("start");
                    var indexOfEnd = leitor.GetOrdinal("end");


                    while (leitor.Read())
                    {

                       //var id = (int)leitor.GetValue(indexOfId);
                        var title = leitor.GetValue(indexOfTitle).ToString();
                        var start = leitor.GetValue(indexOfStart).ToString();
                        var end = leitor.GetValue(indexOfEnd).ToString();

                        //Convert Implicity typed var to Date Time
                        DateTime RealData_Inicio = Convert.ToDateTime(start);
                        DateTime RealData_Fim = Convert.ToDateTime(end);

                        //Convert Date Time to ISO
                        String ReadyData_Inicio = RealData_Inicio.ToString("s");
                        String ReadyData_Fim = RealData_Fim.ToString("s");

                        ModeloTarefas tarefa = new ModeloTarefas(title, ReadyData_Inicio, ReadyData_Fim);

                        lista.Add(tarefa);
                    }

                    //NÃO EXCLUA ISSO
                    //NÃO FAZ SENTIDO MAS FUNCIONA
                    Context.Response.Write((new JavaScriptSerializer()).Serialize(lista));

                }
            }
            catch (Exception e)
            {
                lista = null;
            }

            if (con.State == ConnectionState.Open)
                con.Close();
            
            //return tarefasJsonString;
        }

    }
}
