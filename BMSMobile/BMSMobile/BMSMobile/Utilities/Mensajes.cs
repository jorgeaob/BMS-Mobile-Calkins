using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Widget;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BMSMobile.Utilities
{
    public class Mensajes
    {
        public string Message { get; set; }

        Context context = Android.App.Application.Context;

        public Mensajes()
        {
            Message = "";
        }

        public async Task ShowMessage(string msg)
        {
            Message = msg;
            await Application.Current.MainPage.DisplayAlert(General.BMS, Message, "Ok");
        }

        public async Task<Boolean> ShowQuestionMsg(string msg)
        {
            Message = msg;
            var response = await Application.Current.MainPage.DisplayAlert(General.BMS, Message, "Si", "No");
            return response;
        }

        public void MostrarToast(string msg)
        {
            Message = msg;
            Toast.MakeText(context, Message, ToastLength.Long).Show();
        }
    }
}
