﻿#pragma checksum "C:\Users\VinZen\documents\visual studio 2010\Projects\PhoneApp\PhoneApp\Page\Home.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8034C603CE1F4DD1BC500469F7222779"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18010
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPControls;


namespace PhoneApp.Page {
    
    
    public partial class Home : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal Microsoft.Phone.Controls.Pivot PivotHome;
        
        internal System.Windows.Controls.Grid ContentMain;
        
        internal System.Windows.Controls.ListBox taskListBox;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal WPControls.Calendar Cal;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton Add;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PhoneApp;component/Page/Home.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PivotHome = ((Microsoft.Phone.Controls.Pivot)(this.FindName("PivotHome")));
            this.ContentMain = ((System.Windows.Controls.Grid)(this.FindName("ContentMain")));
            this.taskListBox = ((System.Windows.Controls.ListBox)(this.FindName("taskListBox")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Cal = ((WPControls.Calendar)(this.FindName("Cal")));
            this.Add = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("Add")));
        }
    }
}

