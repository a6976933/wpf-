﻿#pragma checksum "..\..\loading_windows.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CA68D838A63BA2AFD917ED4CB60F2F4A53A43620"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using client_text_fresh;


namespace client_text_fresh {
    
    
    /// <summary>
    /// loading_windows
    /// </summary>
    public partial class loading_windows : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\loading_windows.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Ip_button;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\loading_windows.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ip_address;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/client text fresh;component/loading_windows.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\loading_windows.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Ip_button = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\loading_windows.xaml"
            this.Ip_button.Click += new System.Windows.RoutedEventHandler(this.Ip_button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ip_address = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

