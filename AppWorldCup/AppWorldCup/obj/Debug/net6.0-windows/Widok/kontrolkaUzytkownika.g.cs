﻿#pragma checksum "..\..\..\..\Widok\kontrolkaUzytkownika.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C78461DBCB54FA2CF0018A92956E490E70C18BB1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AppWorldCup.Widok;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AppWorldCup.Widok {
    
    
    /// <summary>
    /// kontrolkaUzytkownika
    /// </summary>
    public partial class kontrolkaUzytkownika : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox poleLogin;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle kolorUzytkownika;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox colorList;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUsun;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnZapisz;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid listaUzytkownikow;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AppWorldCup;component/widok/kontrolkauzytkownika.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.poleLogin = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.kolorUzytkownika = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 3:
            this.colorList = ((System.Windows.Controls.ListBox)(target));
            
            #line 21 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
            this.colorList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.zmienKolor);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 34 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnUsun = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
            this.btnUsun.Click += new System.Windows.RoutedEventHandler(this.btnUsun_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnZapisz = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
            this.btnZapisz.Click += new System.Windows.RoutedEventHandler(this.btnZapisz_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.listaUzytkownikow = ((System.Windows.Controls.DataGrid)(target));
            
            #line 48 "..\..\..\..\Widok\kontrolkaUzytkownika.xaml"
            this.listaUzytkownikow.SelectedCellsChanged += new System.Windows.Controls.SelectedCellsChangedEventHandler(this.listaUzytkownikow_SelectedCellsChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
