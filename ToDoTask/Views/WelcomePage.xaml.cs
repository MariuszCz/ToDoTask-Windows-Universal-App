﻿using Newtonsoft.Json;
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
    public sealed partial class WelcomePage : Page
    {
        private LocalSettingsHandler localSettingsHandler;
        public string Login;
        
        public WelcomePage()
        {
            this.InitializeComponent();
            localSettingsHandler = new LocalSettingsHandler();

            String text = localSettingsHandler.getFromLoadSettings("userLogin");
            if (text != null)
            {
                Username.Text = text;
            }
            //    LoadUsername();

        }

        public void LoadUsername()
        {


            // Create a simple setting

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            // Read data from a simple setting

            Object value = localSettings.Values["exampleSetting"];

            if (value == null)
            {
                localSettings.Values["exampleSetting"] = Username.Text;
            }
            else
            {
                Username.Text = value.ToString();
            }

        }

        private void button_next(object sender, RoutedEventArgs e)
        {
            String login = Username.Text;
            localSettingsHandler.saveToLoadSettings("userLogin", login);
            this.Frame.Navigate(typeof(MainPage));
          //  roamingSettings.Values[SettingName] = Login;
        }

        private void appBarButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}