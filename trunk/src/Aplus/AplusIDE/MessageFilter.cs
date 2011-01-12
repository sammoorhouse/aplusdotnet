using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AplusIDE
{
    class MessageFilter
    {
        /// <summary>
        /// Converts LowerCase ASCII Characters to APL Character codes.
        /// </summary>
        public static String convertLCAsciiCharToAplChar(Key key)
        {
            ushort hexValue = 0;

            switch (key)
            {
                case Key.Q:
                    hexValue = 0x3F; break;
                case Key.E:
                    hexValue = 0xC5; break;
                case Key.R:
                    hexValue = 0xD2; break;
                case Key.T:
                    hexValue = 0x7E; break;
                case Key.Y:
                    hexValue = 0xD9; break;
                case Key.U:
                    hexValue = 0xD5; break;
                case Key.I:
                    hexValue = 0xC9; break;
                case Key.O:
                    hexValue = 0xCF; break;
                case Key.P:
                    hexValue = 0x2A; break;
                case Key.Oem4:  //[
                    hexValue = 0xFB; break;
                case Key.Oem6:  //]
                    hexValue = 0xFD; break;
                case Key.S:
                    hexValue = 0xD3; break;
                case Key.D:
                    hexValue = 0xC4; break;
                case Key.F:
                    hexValue = 0x5F; break;
                case Key.J:
                    hexValue = 0xCA; break;
                case Key.K:
                    hexValue = 0x27; break;
                case Key.Z:
                    hexValue = 0xDA; break;
                case Key.X:
                    hexValue = 0xD8; break;
                case Key.V:
                    hexValue = 0xD6; break;
                case Key.B:
                    hexValue = 0xC2; break;
                case Key.N:
                    hexValue = 0xCE; break;
                case Key.M:
                    hexValue = 0x7C; break;
                case Key.Back:  //`
                    hexValue = 0xFE; break;
                case Key.D1:
                    hexValue = 0xA1; break;
                case Key.D2:
                    hexValue = 0xA2; break;
                case Key.D3:
                    hexValue = 0x3C; break;
                case Key.D4:
                    hexValue = 0xA4; break;
                case Key.D5:
                    hexValue = 0x3D; break;
                case Key.D6:
                    hexValue = 0xA6; break;
                case Key.D7:
                    hexValue = 0x3E; break;
                case Key.D8:
                    hexValue = 0xA8; break;
                case Key.D9:
                    hexValue = 0xA9; break;
                case Key.D0:
                    hexValue = 0x5E; break;
                case Key.OemMinus:
                    hexValue = 0xAB; break;
                case Key.OemPlus:
                    hexValue = 0xDF; break;
                case Key.OemSemicolon:
                    hexValue = 0xDB; break;
                case Key.OemQuotes:
                    hexValue = 0xDD; break;


            }
            return (hexValue == 0) ? null : new String(new char[] { (char)hexValue });
        }

        /// <summary>
        /// Converts UpperCase ASCII Characters to APL Character codes.
        /// </summary>
        public static String convertUCAsciiCharToAplChar(Key key)
        {
            ushort hexValue = 0;

            switch (key)
            {
                case Key.Oem4:  //[
                    hexValue = 0xDD; break;
                case Key.Oem6:  //]
                    hexValue = 0xDB; break;
                case Key.G:
                    hexValue = 0xE7; break;
                case Key.H:
                    hexValue = 0xE8; break;
                case Key.C:
                    hexValue = 0xE3; break;
                case Key.B:
                    hexValue = 0xE2; break;
                case Key.N:
                    hexValue = 0xEE; break;
                case Key.Back:  //`
                    hexValue = 0x7E; break;
                case Key.D1:
                    hexValue = 0xE0; break;
                case Key.D3:
                    hexValue = 0xE7; break;
                case Key.D4:
                    hexValue = 0xE8; break;
                case Key.D5:
                    hexValue = 0xF7; break;
                case Key.D6:
                    hexValue = 0xF4; break;
                case Key.D8:
                    hexValue = 0xF0; break;
                case Key.D0:
                    hexValue = 0x7E; break;
                case Key.OemMinus:
                    hexValue = 0x21; break;
                case Key.OemPlus:
                    hexValue = 0xAD; break;
                case Key.OemComma:
                    hexValue = 0x3C; break;
                case Key.OemPeriod:
                    hexValue = 0xAE; break;
            }
            return (hexValue == 0) ? null : new String(new char[] { (char)hexValue });
        }
    }
}
