namespace MyCalculatorv1
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    public class MainWindow : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal TextBox tb;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            TextBox tb = this.tb;
            tb.Text = tb.Text + button.Content.ToString();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            this.tb.Text = "";
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/MyCalculatorv1;component/mainwindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void Off_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void R_Click(object sender, RoutedEventArgs e)
        {
            if (this.tb.Text.Length > 0)
            {
                this.tb.Text = this.tb.Text.Substring(0, this.tb.Text.Length - 1);
            }
        }

        private void result()
        {
            TextBox tb;
            int startIndex = 0;
            if (this.tb.Text.Contains("+"))
            {
                startIndex = this.tb.Text.IndexOf("+");
            }
            else if (this.tb.Text.Contains("-"))
            {
                startIndex = this.tb.Text.IndexOf("-");
            }
            else if (this.tb.Text.Contains("*"))
            {
                startIndex = this.tb.Text.IndexOf("*");
            }
            else if (this.tb.Text.Contains("/"))
            {
                startIndex = this.tb.Text.IndexOf("/");
            }
            string str = this.tb.Text.Substring(startIndex, 1);
            double num2 = Convert.ToDouble(this.tb.Text.Substring(0, startIndex));
            double num3 = Convert.ToDouble(this.tb.Text.Substring(startIndex + 1, (this.tb.Text.Length - startIndex) - 1));
            if (str == "+")
            {
                tb = this.tb;
                tb.Text = tb.Text + "=" + (num2 + num3);
            }
            else if (str == "-")
            {
                tb = this.tb;
                tb.Text = tb.Text + "=" + (num2 - num3);
            }
            else if (str == "*")
            {
                tb = this.tb;
                tb.Text = tb.Text + "=" + (num2 * num3);
            }
            else
            {
                tb = this.tb;
                tb.Text = tb.Text + "=" + (num2 / num3);
            }
        }

        private void Result_click(object sender, RoutedEventArgs e)
        {
            this.result();
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 2:
                    this.tb = (TextBox) target;
                    break;

                case 3:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 4:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 5:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 6:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 7:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 8:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 9:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 10:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 11:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 12:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 13:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 14:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 15:
                    ((Button) target).Click += new RoutedEventHandler(this.Result_click);
                    break;

                case 0x10:
                    ((Button) target).Click += new RoutedEventHandler(this.Button_Click_1);
                    break;

                case 0x11:
                    ((Button) target).Click += new RoutedEventHandler(this.Off_Click_1);
                    break;

                case 0x12:
                    ((Button) target).Click += new RoutedEventHandler(this.Del_Click);
                    break;

                case 0x13:
                    ((Button) target).Click += new RoutedEventHandler(this.R_Click);
                    break;

                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}

