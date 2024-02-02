using BMSMobile.Models;
using BMSMobile.Utilities;
using PropertyChanged;
using BMSMobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Input;
using Xamarin.Forms;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]

    public class ConexionVM
    {
        public string cadena
        {
            get => ConexionModel.cadena;
            set { ConexionModel.cadena = value; }
        }

        public INavigation Navigation { get; set; }
        public ConexionModel connModel { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel activityModel { get; set; }
        public ColorsModel colorModel { get; set; }

        public Command ProbarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command CompletedCadenaCommand { get; set; }

        public ConexionVM(INavigation navigation)
        {
            Navigation = navigation;
            General.urlWS = Settings.urlServidor;

            connModel = new ConexionModel();
            MostrarMsg = new Mensajes();
            activityModel = new AIModel();
            colorModel = new ColorsModel();

            activityModel.IsBusy = false;
            colorModel.MissingColor = System.Drawing.Color.Transparent;

            ProbarCommand = new Command(ProbarConexion);
            GuardarCommand = new Command(GuardarConexion);
            CompletedCadenaCommand = new Command(GuardarConexion);            
        }    

        private async void GuardarConexion()
        {
            try
            {
                if (string.IsNullOrEmpty(cadena))
                {
                    colorModel.MissingValue();
                    return;
                }
                else
                {
                    colorModel.CheckValue();

                    if (cadena.StartsWith("http://"))
                    {
                        cadena = cadena.Substring("http://".Length);
                    }
                    Settings.urlServidor = cadena;
                    General.urlWS = cadena.Trim();
                    await MostrarMsg.ShowMessage("La conexión ha sido guardada.");
                }
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private async void ProbarConexion()
        {
            try
            {
                if (string.IsNullOrEmpty(General.urlWS))
                {
                    await MostrarMsg.ShowMessage("Favor de guardar la cadena de conexión");
                    return;
                }
                else
                {
                    if (cadena.StartsWith("http://"))
                    {
                        cadena = cadena.Substring("http://".Length);
                    }

                    activityModel.IsBusy = true;
                    RestClient client = new RestClient(null);

                    string url = "http://" + General.urlWS + "/api/Conexion/ProbarConexion";
                    HttpRespuesta<bool> resp = await client.Get<bool>(url);
                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage("Error de conexión, revise sus datos o intente más tarde.");
                        return;
                    }
                    else
                    {
                        await MostrarMsg.ShowMessage("La conexión se realizó con éxito.");
                    }
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
            finally
            {
                activityModel.IsBusy = false;
            }
        }


    }
}
