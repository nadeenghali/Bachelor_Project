﻿#pragma checksum "..\..\..\MyApp.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "14E2025AB76BF43D02DEB5AF876D74C1F80FD6D5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Samples.Kinect.DiscreteGestureBasics;
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


namespace Microsoft.Samples.Kinect.DiscreteGestureBasics {
    
    
    /// <summary>
    /// MyApp
    /// </summary>
    public partial class MyApp : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox username_txtbx;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button register_btn;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button signIn_btn;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label register_label;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label enter_label;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button notRegistered_btn;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button start_btn;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button signInStart_btn;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox gestureComboBox;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\MyApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button train_btn;
        
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
            System.Uri resourceLocater = new System.Uri("/DiscreteGestureBasics-WPF;component/myapp.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MyApp.xaml"
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
            this.username_txtbx = ((System.Windows.Controls.TextBox)(target));
            
            #line 10 "..\..\..\MyApp.xaml"
            this.username_txtbx.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.username_txtbx_TextChanged);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\MyApp.xaml"
            this.username_txtbx.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.username_txtbx_GotKeyboardFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.register_btn = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\MyApp.xaml"
            this.register_btn.Click += new System.Windows.RoutedEventHandler(this.register_btn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.signIn_btn = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\MyApp.xaml"
            this.signIn_btn.Click += new System.Windows.RoutedEventHandler(this.signIn_btn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.register_label = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.enter_label = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.notRegistered_btn = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\MyApp.xaml"
            this.notRegistered_btn.Click += new System.Windows.RoutedEventHandler(this.notRegistered_btn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.start_btn = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\MyApp.xaml"
            this.start_btn.Click += new System.Windows.RoutedEventHandler(this.start_btn_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.signInStart_btn = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\MyApp.xaml"
            this.signInStart_btn.Click += new System.Windows.RoutedEventHandler(this.signInStart_btn_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.gestureComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 18 "..\..\..\MyApp.xaml"
            this.gestureComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.gestureComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.train_btn = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\MyApp.xaml"
            this.train_btn.Click += new System.Windows.RoutedEventHandler(this.train_btn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

