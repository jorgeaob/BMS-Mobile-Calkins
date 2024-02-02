using BMSMobile.Models;
using BMSMobile.Utilities;
using BMSMobile.Views;
using PropertyChanged;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using BMSMobile.Services;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]

    public class LoginVM
    {
        public INavigation Navigation { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel activityModel { get; set; }
        public ColorsModel colorUser { get; set; }
        public ColorsModel colorClave { get; set; }
        
        private string _user { get; set; }
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private string _clave { get; set; }
        public string Clave 
        { 
            get { return _clave; }
            set { _clave = value; }
        }

        private LoginModel _logModel { get; set; } 
        public LoginModel LogModel
        {
            get { return _logModel; }
            set 
            { 
                if(_logModel != value)
                    _logModel = value; 
            }
        }

        private FocusTriggerAction _entryUser { get; set; }
        public FocusTriggerAction EntryUser
        {
            get { return _entryUser; }
            set { _entryUser = value; }
        }

        private FocusTriggerAction _entryClave { get; set; }
        public FocusTriggerAction EntryClave
        {
            get { return _entryClave; }
            set { _entryClave = value; }
        }

        public Command ConfigCommand { get; set; }
        public Command LoginCommand { get; set; }
        public Command CompletedUserCommand { get; set; }
        public Command CompletedClaveCommand { get; set; }

        public LoginVM(INavigation navigation)
        {
            General.urlWS = Settings.urlServidor;
            Navigation = navigation;
            MostrarMsg = new Mensajes();
            activityModel = new AIModel();
            colorUser = new ColorsModel();
            colorClave = new ColorsModel();
            _user = "";
            _clave = "";
            _logModel = new LoginModel();
            _entryUser = new FocusTriggerAction();
            _entryClave = new FocusTriggerAction();

            colorUser.MissingColor = Color.Transparent;
            colorClave.MissingColor = Color.Transparent;
            activityModel.IsBusy = false;
            _entryUser.Focused = false;
            _entryClave.Focused = false;

            ConfigCommand = new Command(OpenConfigPage);
            LoginCommand = new Command(Login);
            CompletedUserCommand = new Command(TabEntry);
            CompletedClaveCommand = new Command(Login);
        }

        private async void OpenConfigPage()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new ConexionView());
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private async void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(General.urlWS))
                {
                    await MostrarMsg.ShowMessage("Favor de ingresar o probar conexión.");
                    OpenConfigPage();
                    return;
                }
                if(string.IsNullOrEmpty(_user.Trim()) && string.IsNullOrEmpty(_clave.Trim()))
                {
                    colorUser.MissingValue();
                    colorClave.MissingValue();
                    _entryUser.Focused = true;
                    return;
                }
                if (string.IsNullOrEmpty(_user.Trim()))
                {
                    colorUser.MissingValue();
                    _entryUser.Focused = true;
                    return;
                }
                if (string.IsNullOrEmpty(_clave.Trim()))
                {
                    colorUser.CheckValue();
                    colorClave.MissingValue();
                    _entryClave.Focused = true;
                    return;
                }
                else
                {
                    activityModel.IsBusy = true;
                    colorUser.CheckValue();
                    colorClave.CheckValue();
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Usuario", _user.Trim());
                    parametros.Add("Clave", _clave.Trim());
                    string url = "http://" + General.urlWS + "/api/Login/LoginApp";
                    var resp = await client.Get<LoginModel>(url, parametros);

                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage(resp.Message);
                        _user = "";
                        _clave = "";
                        _entryUser.Focused = true;
                        _entryClave.Focused = false;
                        return;
                    }
                    if(resp.Result.status.Trim().ToUpper() == "C")
                    {
                        await MostrarMsg.ShowMessage("Su cuenta se encuentra cancelada.");
                        _user = "";
                        _clave = "";
                    }
                    else
                    {
                        _entryUser.Focused = false;
                        _entryClave.Focused = false;
                        General.userCode = resp.Result.usuario.Trim();
                        General.userName = resp.Result.nombre.Trim();
                        General.userCodEstab = resp.Result.cod_estab.Trim();
                        MostrarMsg.MostrarToast("Bienvenido: " + resp.Result.nombre.Trim());
                        await Navigation.PushModalAsync(new MenuView());
                    }
                }
            }
            catch(Exception ex)
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

        private async void TabEntry()
        {
            try
            {
                if (!string.IsNullOrEmpty(_user.Trim()))
                {
                    _entryClave.Focused = true;
                    _entryUser.Focused = false;
                }
                    
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }
    }

    public class LogoutVM
    {
        public Mensajes MostrarMsg { get; set; }
        
        public LogoutVM()
        {
            MostrarMsg = new Mensajes();
        }

        public async void LogoutSession()
        {
            try
            {
                var response = await MostrarMsg.ShowQuestionMsg("¿Desea cerrar sesión?");
                if (response == true)
                {
                    App.Current.MainPage = new NavigationPage(new LoginView())
                    {
                        BarBackgroundColor = Color.FromHex("0D47A1")
                    };

                    General.userCode = "";
                    General.userName = "";
                    General.userCodEstab = "";
                }
                else
                    return;
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
