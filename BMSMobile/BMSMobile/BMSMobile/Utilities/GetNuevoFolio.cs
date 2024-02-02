using BMSMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BMSMobile.Utilities
{
    public class GetNuevoFolio
    {
        private Mensajes MostrarMsg { get; set; }
        public GetNuevoFolio()
        {
            MostrarMsg = new Mensajes();
        }

        public async Task<string> GenerarFolio(string Transaccion, string CodEstab)
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Transaccion", Transaccion);
                parametros.Add("EstabCode", CodEstab);

                var url = "http://" + General.urlWS + "/api/Entrada/GenerarFolio";
                var resp = await client.Post<NuevoFolioModel>(url, parametros, null);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo generar el folio. " + resp.Message);
                    return "";
                }
                else
                {
                    return resp.Result.Folio.Trim();
                }

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }
    }
}
