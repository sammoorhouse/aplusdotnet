using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Scripting.Hosting;
using AplusCore.Types;
using AplusCore.Runtime;
using AplusCore.Compiler;
using Microsoft.Win32;
using System.IO;

namespace AplusIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Switch between APL and ASCII mode
        /// </summary>
        private bool aplinput;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.aplinput = true;
            this.SetModeText();
        }

        private void ModeLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.aplinput = !this.aplinput;

            SetModeText();
        }

        private void SetModeText()
        {
            switch (this.aplinput)
            {
                case true:
                    this.ModeLabel.Content = "APL";
                    break;

                case false:
                    this.ModeLabel.Content = "ASCII";
                    break;
            }
        }

        private void codeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.aplinput && Keyboard.IsKeyDown(Key.RightShift))
            {
                String kaplChar;

                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    kaplChar = MessageFilter.convertUCAsciiCharToAplChar(e.Key);
                }
                else
                {
                    kaplChar = MessageFilter.convertLCAsciiCharToAplChar(e.Key);
                }

                if (kaplChar != null)
                {
                    int currentPos = this.codeBox.CaretIndex;
                    this.codeBox.Text = this.codeBox.Text.Insert(currentPos, kaplChar);
                    this.codeBox.CaretIndex = currentPos + 1;

                    e.Handled = true;
                }
            }
        }

        private void ExecuteLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string code = this.codeBox.Text;

            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            setup.LanguageSetups.Add(AplusCore.Runtime.AplusLanguageContext.LanguageSetup);

            setup.Options.Add(
                new KeyValuePair<string, object>(
                    "LexerMode",
                    this.aplinput ? AplusCore.Compiler.LexerMode.APL : AplusCore.Compiler.LexerMode.ASCII
                )
            );

            ScriptRuntime dlrRuntime = new ScriptRuntime(setup);

            ScriptEngine engine = dlrRuntime.GetEngine(@"A+");

            try
            {
                AType result = engine.Execute<AType>(code);
                this.ResultTextBox.Text = result.ToString();
            }
            catch (Error error)
            {
                this.ResultTextBox.Text = error.ToString();
            }
            catch (Exception ex)
            {
                this.ResultTextBox.Text = ex.Message;
            }


            if (!this.AnimatedExpander.IsExpanded)
            {
                this.AnimatedExpander.IsExpanded = true;
                ExpandOrCollapese(this.AnimatedExpander);
            }
        }

        private void AnimatedExpander_Expanded(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapese(sender as Expander);
        }

        private void AnimatedExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapese(sender as Expander);
        }



        private void ExpandOrCollapese(Expander expander)
        {
            int rowIndex = Grid.GetRow(expander);
            var row = this.MainGrid.RowDefinitions[rowIndex];

            if (expander.IsExpanded)
            {
                row.Height = this.MainGrid.RowDefinitions[rowIndex].Height;
                row.MinHeight = 100;
            }
            else
            {
                this.MainGrid.RowDefinitions[rowIndex].Height = row.Height;
                row.Height = GridLength.Auto;
                row.MinHeight = 0;
            }
        }


        private void LoadLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.DefaultExt = ".a+";
            openDialog.Filter = "A+ APL sources (.a+)|*.a+|A+ ASCII sources (.aa)|*.aa|All Files (*.*)|*.*";

            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
            {
                string filename = openDialog.FileName;
                using (StreamReader reader = new StreamReader(filename, Encoding.GetEncoding(28591)))
                {
                    this.codeBox.Text = reader.ReadToEnd();
                }
            }

        }

        private void SaveLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".a+";
            saveDialog.Filter = "A+ APL sources (.a+)|*.a+|A+ ASCII sources (.aa)|*.aa|All Files (*.*)|*.*";

            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                string filename = saveDialog.FileName;

                using (StreamWriter writer = new StreamWriter(filename, false, Encoding.GetEncoding(28591)))
                {
                    writer.Write(this.codeBox.Text);
                }
            }
        }




    }
}
