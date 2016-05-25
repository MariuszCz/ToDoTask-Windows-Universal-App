using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoTask.Commands;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ToDoTask.ViewModel
{

    public class MainPageViewModel : ViewModel
    {
        private ServerConnector server = new ServerConnector();
        private LocalSettingsHandler localSettingsHandler;

        private string user;

        public string User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                NotifyPropertyChanged();
            }
        }


        public string getUser()
        {
            localSettingsHandler = new LocalSettingsHandler();

            String text = localSettingsHandler.getFromLoadSettings("userLogin");
            if (text != null)
            {
                user = text;
                return user;
            }
            return null;
        }

        public async void addTask(String title, String value)
        {
            localSettingsHandler = new LocalSettingsHandler();
            String text = localSettingsHandler.getFromLoadSettings("userLogin");

            try
            {
                await server.addTask(title, value, text);
                MessageDialog msgbox = new MessageDialog("Zadanie dodano!");
                await msgbox.ShowAsync();
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Brak internetu!");
                await msgbox.ShowAsync();
            }
        }

        public MainPageViewModel()
        {
            getUser();
        }
    }
}
