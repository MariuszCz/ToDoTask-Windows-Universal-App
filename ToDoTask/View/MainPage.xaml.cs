﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToDoTask.ViewModel;
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
        
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainPageViewModel();
        }
        private MainPageViewModel GetViewModel()
        {
            return DataContext as MainPageViewModel;
        }
    

        private async void button_add(object sender, RoutedEventArgs e)
        {
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