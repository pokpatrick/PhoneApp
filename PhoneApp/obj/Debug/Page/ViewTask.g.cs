﻿#pragma checksum "C:\Users\VinZen\documents\visual studio 2010\Projects\PhoneApp\PhoneApp\Page\ViewTask.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "51481323FE39CE53A968E693FA9D9BCC"
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


namespace PhoneApp.Page {
    
    
    public partial class ViewTask : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Grid ContentGrid;
        
        internal System.Windows.Controls.TextBlock taskListLabel;
        
        internal System.Windows.Controls.TextBlock taskListTitle;
        
        internal System.Windows.Controls.TextBlock titleLabel;
        
        internal System.Windows.Controls.TextBlock title;
        
        internal System.Windows.Controls.TextBlock deadlineLabel;
        
        internal System.Windows.Controls.TextBlock deadlineDate;
        
        internal System.Windows.Controls.TextBlock notesLabel;
        
        internal System.Windows.Controls.TextBlock Notes;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PhoneApp;component/Page/ViewTask.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.ContentGrid = ((System.Windows.Controls.Grid)(this.FindName("ContentGrid")));
            this.taskListLabel = ((System.Windows.Controls.TextBlock)(this.FindName("taskListLabel")));
            this.taskListTitle = ((System.Windows.Controls.TextBlock)(this.FindName("taskListTitle")));
            this.titleLabel = ((System.Windows.Controls.TextBlock)(this.FindName("titleLabel")));
            this.title = ((System.Windows.Controls.TextBlock)(this.FindName("title")));
            this.deadlineLabel = ((System.Windows.Controls.TextBlock)(this.FindName("deadlineLabel")));
            this.deadlineDate = ((System.Windows.Controls.TextBlock)(this.FindName("deadlineDate")));
            this.notesLabel = ((System.Windows.Controls.TextBlock)(this.FindName("notesLabel")));
            this.Notes = ((System.Windows.Controls.TextBlock)(this.FindName("Notes")));
        }
    }
}

