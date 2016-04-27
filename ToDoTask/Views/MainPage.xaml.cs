using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToDoTask
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LocalSettingsHandler localSettingsHandler;
        public MainPage()
        {
            this.InitializeComponent();
            /*   var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
               Object value = localSettings.Values["exampleSetting"];
               if(value!=null)
               {
                   textBox.Text = localSettings.Values["exampleSetting"].ToString();

               }*/
            //  localSettings.Values["exampleSetting"] = "Hello Windows";
            localSettingsHandler = new LocalSettingsHandler();

            String text = localSettingsHandler.getFromLoadSettings("userLogin");
            if (text != null)
            {
                 user.Text = text;
            }
        }

        private async void button_add(object sender, RoutedEventArgs e)
        {
            // var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //Object value = localSettings.Values["exampleSetting"];
            //  localSettings.Values["exampleSettings"] = textBox.Text;
            //  Serialize();
            // RestConnection rest = new RestConnection();
            // rest.createClient();
            if (Title.Text != "" && Value.Text != "")
            {
                TaskManagement taskManagement = new TaskManagement();
                taskManagement.addTask(Title.Text, Value.Text);
                Debug.WriteLine("Dodaje zadanie");
            }
            else
            {
                Debug.WriteLine("Błąd, puste pola!");
                MessageDialog msgbox = new MessageDialog("Uzupełnij wszystkie pola!");

                await msgbox.ShowAsync();
            }



        }

        private void button_tasks(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(ToDoTaskList));

        }
        private void button_back(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WelcomePage));
        }
    }
}
