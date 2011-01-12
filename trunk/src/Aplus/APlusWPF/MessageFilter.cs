using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace APlusWPF
{
    class MessageFilter
    {
        /// <summary>
        /// Converts LowerCase ASCII Characters to APL Character codes.
        /// </summary>
        public static String convertLCAsciiCharToAplChar(Key key_)
        {
            string _hexValue = null;

            switch (key_)
            {
                case Key.Q:
                    _hexValue = "3F"; break;
                case Key.E:
                    _hexValue = "C5"; break;
                case Key.R:
                    _hexValue = "D2"; break;
                case Key.T:
                    _hexValue = "7E"; break;
                case Key.Y:
                    _hexValue = "D9"; break;
                case Key.U:
                    _hexValue = "D5"; break;
                case Key.I:
                    _hexValue = "C9"; break;
                case Key.O:
                    _hexValue = "CF"; break;
                case Key.P:
                    _hexValue = "2A"; break;
                case Key.Oem4:  //[
                    _hexValue = "FB"; break;
                case Key.Oem6:  //]
                    _hexValue = "FD"; break;
                case Key.S:
                    _hexValue = "D3"; break;
                case Key.D:
                    _hexValue = "C4"; break;
                case Key.F:
                    _hexValue = "5F"; break;
                case Key.J:
                    _hexValue = "CA"; break;
                case Key.K:
                    _hexValue = "27"; break;
                case Key.Z:
                    _hexValue = "DA"; break;
                case Key.X:
                    _hexValue = "D8"; break;
                case Key.V:
                    _hexValue = "D6"; break;
                case Key.B:
                    _hexValue = "C2"; break;
                case Key.N:
                    _hexValue = "CE"; break;
                case Key.M:
                    _hexValue = "7C"; break;
                case Key.Back:  //`
                    _hexValue = "FE"; break;
                case Key.D1:
                    _hexValue = "A1"; break;
                case Key.D2:
                    _hexValue = "A2"; break;
                case Key.D3:
                    _hexValue = "3C"; break;
                case Key.D4:
                    _hexValue = "A4"; break;
                case Key.D5:
                    _hexValue = "3D"; break;
                case Key.D6:
                    _hexValue = "A6"; break;
                case Key.D7:
                    _hexValue = "3E"; break;
                case Key.D8:
                    _hexValue = "A8"; break;
                case Key.D9:
                    _hexValue = "A9"; break;
                case Key.D0:
                    _hexValue = "5E"; break;
                case Key.OemMinus:
                    _hexValue = "AB"; break;
                case Key.OemPlus:
                    _hexValue = "DF"; break;
                case Key.OemSemicolon:
                    _hexValue = "DB"; break;
                case Key.OemQuotes:
                    _hexValue = "DD"; break;


            }
            return (null == _hexValue) ? null : new String(new char[] {
        (char) ushort.Parse (_hexValue, System.Globalization.NumberStyles.HexNumber) 
      });
        }
        /// <summary>
        /// Converts UpperCase ASCII Characters to APL Character codes.
        /// </summary>
        public static String convertUCAsciiCharToAplChar(Key key_)
        {
            string _hexValue = null;

            switch (key_)
            {
                case Key.Oem4:  //[
                    _hexValue = "DD"; break;
                case Key.Oem6:  //]
                    _hexValue = "DB"; break;
                case Key.G:
                    _hexValue = "E7"; break;
                case Key.H:
                    _hexValue = "E8"; break;
                case Key.C:
                    _hexValue = "E3"; break;
                case Key.B:
                    _hexValue = "E2"; break;
                case Key.N:
                    _hexValue = "EE"; break;
                case Key.Back:  //`
                    _hexValue = "7E"; break;
                case Key.D1:
                    _hexValue = "E0"; break;
                case Key.D3:
                    _hexValue = "E7"; break;
                case Key.D4:
                    _hexValue = "E8"; break;
                case Key.D5:
                    _hexValue = "F7"; break;
                case Key.D6:
                    _hexValue = "F4"; break;
                case Key.D8:
                    _hexValue = "F0"; break;
                case Key.D0:
                    _hexValue = "7E"; break;
                case Key.OemMinus:
                    _hexValue = "21"; break;
                case Key.OemPlus:
                    _hexValue = "AD"; break;
                case Key.OemComma:
                    _hexValue = "3C"; break;
                case Key.OemPeriod:
                    _hexValue = "AE"; break;
            }
            return (null == _hexValue) ? null : new String(new char[] {
        (char) ushort.Parse (_hexValue, System.Globalization.NumberStyles.HexNumber) 
      });
        }
    }
}