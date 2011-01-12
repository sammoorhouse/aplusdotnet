using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace APlusWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        protected ScriptEngine _engine;
        protected MemoryStream _ms;
        protected ScriptScope _scope;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            setup.LanguageSetups.Add(AplusCore.Runtime.AplusLanguageContext.LanguageSetup);

            ScriptRuntime dlrRuntime = new ScriptRuntime(setup);
            _engine = dlrRuntime.GetEngine(@"A+");
            _ms = new MemoryStream();
            var sw = new StreamWriter(_ms);
            dlrRuntime.IO.SetErrorOutput(_ms, sw);
            dlrRuntime.IO.SetOutput(_ms, sw);

            _scope = _engine.CreateScope();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    var res = _engine.Execute(txtInput.Text, _scope);

                    txtOutput.Text += String.Format("\n{0}", res.ToString());
                }
                catch (Exception pe_)
                {
                    txtOutput.Text += String.Format("\n{0}", pe_.Message);
                }
                finally
                {
                    txtInput.Text = String.Empty;
                    scrollViewer.ScrollToEnd();
                }
            }

            if(!Keyboard.IsKeyToggled(Key.Scroll))
                return;

            Key asciiChar;
            String kaplChar;
            asciiChar = e.Key;

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                kaplChar = MessageFilter.convertUCAsciiCharToAplChar(asciiChar);
            }
            else
            {
                kaplChar = MessageFilter.convertLCAsciiCharToAplChar(asciiChar);
            }
            if (kaplChar != null)
            {
                int currentPos = txtInput.CaretIndex;
                txtInput.Text = txtInput.Text.Insert(currentPos, kaplChar);
                txtInput.CaretIndex = currentPos + 1;
                e.Handled = true;
            }
        }
    }
}
