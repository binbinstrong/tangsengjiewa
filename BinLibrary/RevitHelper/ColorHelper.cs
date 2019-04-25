using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Color = Autodesk.Revit.DB.Color;

namespace BinLibrary.RevitHelper
{
    public static class ColorHelper
    {
        /// <summary>
        /// 将十进制颜色值 转化为16进制 然后解析为RGB
        /// </summary>
        /// <param name="colorvaluestring"></param>
        /// <returns></returns>
        public static Color ParseFromInteger(string colorvaluestring)
        {
            Color result = default(Color);

            //Regex regularexp = new Regex(pattern);

            if (Convert.ToInt32(colorvaluestring) == 0)
            {
                colorvaluestring = "000000";
            }

            var colorvalue = Convert.ToInt64(colorvaluestring);

            var valuestring16 = Convert.ToString(colorvalue, 16);
            result = ParseFromHex(valuestring16);

            return result;
        }

        /// <summary>
        /// 将十六进制数值解析为RGB颜色
        /// </summary>
        /// <param name="valuestring16"></param>
        /// <returns></returns>
        public static Color ParseFromHex(string valuestring16/*6位 16进制数*/)
        {
            //MessageBox.Show(valuestring16);

            Color result = default(Color);

            int count = 6 - valuestring16.Length;
            if (count > 0)
                while (count > 0)
                {
                    valuestring16 = '0' + valuestring16;
                    count--;
                }
            else if (count < 0)
            {
                valuestring16 = valuestring16.Substring(valuestring16.Length - 6);
            }
            else;

            var Rstring = valuestring16.Substring(0, 2);
            var Gstring = valuestring16.Substring(2, 2);
            var Bstring = valuestring16.Substring(4, 2);

            var Rvalue = Convert.ToByte(Convert.ToInt32(Rstring, 16).ToString(), 10);
            var Gvalue = Convert.ToByte(Convert.ToInt32(Gstring, 16).ToString(), 10);
            var Bvalue = Convert.ToByte(Convert.ToInt32(Bstring, 16).ToString(), 10);

            result = new Color(Rvalue, Gvalue, Bvalue);

            //MessageBox.Show(Rvalue + Environment.NewLine + Gvalue + Environment.NewLine + Bvalue);
            return result;
        }

        /// <summary>
        /// 将Revit的颜色转为System.Drawing.Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color ToSysDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue);
        }

        /// <summary>
        /// 转换为Revit的颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToRvtColor(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B);
        }
        
        /// <summary>
        /// 翻转颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color InversColor(this Color color)
        {
            return new Color((byte)(255 - color.Red), (byte)(255 - color.Green), (byte)(255 - color.Green));
        }

        /// <summary>
        /// 判断是是否同一种颜色
        /// </summary>
        /// <param name="color"></param>
        /// <param name="color1"></param>
        /// <returns></returns>
        public static bool IsSameColor(this Color color, Color color1)
        {
            if (color.Red == color1.Red && color.Green == color1.Green && color.Blue == color1.Blue)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断颜色值相等
        /// </summary>
        /// <param name="color"></param>
        /// <param name="color1"></param>
        /// <returns></returns>
        public static bool Equal(this Color color, Color color1)
        {
            
            return color.IsSameColor(color1);
        }

    }

    /// <summary>
    /// Revit里面的颜色表（对应System.Drawing.Color的色表）
    /// </summary>
    public static class RvtColor
    {
        //
        // 摘要:
        //     获取 ARGB 值为 #FF9370DB 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumPurple
        { get { return System.Drawing.Color.MediumPurple.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF3CB371 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumSeaGreen { get { return System.Drawing.Color.MediumSeaGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF7B68EE 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumSlateBlue { get { return System.Drawing.Color.MediumSlateBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00FA9A 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumSpringGreen { get { return System.Drawing.Color.MediumSpringGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF48D1CC 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumTurquoise { get { return System.Drawing.Color.MediumTurquoise.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFC71585 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumVioletRed { get { return System.Drawing.Color.MediumVioletRed.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF191970 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MidnightBlue { get { return System.Drawing.Color.MidnightBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFBA55D3 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumOrchid { get { return System.Drawing.Color.MediumOrchid.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF5FFFA 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MintCream { get { return System.Drawing.Color.MintCream.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFE4B5 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Moccasin { get { return System.Drawing.Color.Moccasin.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFDEAD 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color NavajoWhite { get { return System.Drawing.Color.NavajoWhite.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF000080 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Navy { get { return System.Drawing.Color.Navy.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFDF5E6 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color OldLace { get { return System.Drawing.Color.OldLace.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF808000 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Olive { get { return System.Drawing.Color.Olive.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF6B8E23 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color OliveDrab { get { return System.Drawing.Color.OliveDrab.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFA500 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Orange { get { return System.Drawing.Color.Orange.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFE4E1 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MistyRose { get { return System.Drawing.Color.MistyRose.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF4500 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color OrangeRed { get { return System.Drawing.Color.OrangeRed.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF0000CD 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumBlue { get { return System.Drawing.Color.MediumBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF800000 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Maroon { get { return System.Drawing.Color.Maroon.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFADD8E6 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightBlue { get { return System.Drawing.Color.LightBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF08080 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightCoral { get { return System.Drawing.Color.LightCoral.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFAFAD2 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightGoldenrodYellow { get { return System.Drawing.Color.LightGoldenrodYellow.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF90EE90 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightGreen { get { return System.Drawing.Color.LightGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFD3D3D3 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightGray { get { return System.Drawing.Color.LightGray.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFB6C1 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightPink { get { return System.Drawing.Color.LightPink.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFA07A 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightSalmon { get { return System.Drawing.Color.LightSalmon.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF66CDAA 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color MediumAquamarine { get { return System.Drawing.Color.MediumAquamarine.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF20B2AA 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightSeaGreen { get { return System.Drawing.Color.LightSeaGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF778899 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightSlateGray { get { return System.Drawing.Color.LightSlateGray.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFB0C4DE 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightSteelBlue { get { return System.Drawing.Color.LightSteelBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFFFE0 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightYellow { get { return System.Drawing.Color.LightYellow.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00FF00 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Lime { get { return System.Drawing.Color.Lime.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF32CD32 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LimeGreen { get { return System.Drawing.Color.LimeGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFAF0E6 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Linen { get { return System.Drawing.Color.Linen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF00FF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Magenta { get { return System.Drawing.Color.Magenta.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF87CEFA 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightSkyBlue { get { return System.Drawing.Color.LightSkyBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFFACD 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LemonChiffon { get { return System.Drawing.Color.LemonChiffon.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFDA70D6 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Orchid { get { return System.Drawing.Color.Orchid.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF98FB98 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color PaleGreen { get { return System.Drawing.Color.PaleGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF6A5ACD 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SlateBlue { get { return System.Drawing.Color.SlateBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF708090 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SlateGray { get { return System.Drawing.Color.SlateGray.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFFAFA 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Snow { get { return System.Drawing.Color.Snow.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00FF7F 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SpringGreen { get { return System.Drawing.Color.SpringGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF4682B4 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SteelBlue { get { return System.Drawing.Color.SteelBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFD2B48C 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Tan { get { return System.Drawing.Color.Tan.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF008080 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Teal { get { return System.Drawing.Color.Teal.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF87CEEB 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SkyBlue { get { return System.Drawing.Color.SkyBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFD8BFD8 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Thistle { get { return System.Drawing.Color.Thistle.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF40E0D0 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Turquoise { get { return System.Drawing.Color.Turquoise.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFEE82EE 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Violet { get { return System.Drawing.Color.Violet.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF5DEB3 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Wheat { get { return System.Drawing.Color.Wheat.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFFFFF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color White { get { return System.Drawing.Color.White.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF5F5F5 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color WhiteSmoke { get { return System.Drawing.Color.WhiteSmoke.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFFF00 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Yellow { get { return System.Drawing.Color.Yellow.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF9ACD32 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color YellowGreen { get { return System.Drawing.Color.YellowGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF6347 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Tomato { get { return System.Drawing.Color.Tomato.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFEEE8AA 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color PaleGoldenrod { get { return System.Drawing.Color.PaleGoldenrod.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFC0C0C0 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Silver { get { return System.Drawing.Color.Silver.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFF5EE 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SeaShell { get { return System.Drawing.Color.SeaShell.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFAFEEEE 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color PaleTurquoise { get { return System.Drawing.Color.PaleTurquoise.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFDB7093 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color PaleVioletRed { get { return System.Drawing.Color.PaleVioletRed.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFEFD5 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color PapayaWhip { get { return System.Drawing.Color.PapayaWhip.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFDAB9 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color PeachPuff { get { return System.Drawing.Color.PeachPuff.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFCD853F 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Peru { get { return System.Drawing.Color.Peru.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFC0CB 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Pink { get { return System.Drawing.Color.Pink.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFDDA0DD 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Plum { get { return System.Drawing.Color.Plum.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFA0522D 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Sienna { get { return System.Drawing.Color.Sienna.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFB0E0E6 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color PowderBlue { get { return System.Drawing.Color.PowderBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF0000 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Red { get { return System.Drawing.Color.Red.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFBC8F8F 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color RosyBrown { get { return System.Drawing.Color.RosyBrown.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF4169E1 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color RoyalBlue { get { return System.Drawing.Color.RoyalBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF8B4513 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SaddleBrown { get { return System.Drawing.Color.SaddleBrown.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFA8072 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Salmon { get { return System.Drawing.Color.Salmon.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF4A460 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SandyBrown { get { return System.Drawing.Color.SandyBrown.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF2E8B57 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color SeaGreen { get { return System.Drawing.Color.SeaGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF800080 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Purple { get { return System.Drawing.Color.Purple.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF7CFC00 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LawnGreen { get { return System.Drawing.Color.LawnGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFE0FFFF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LightCyan { get { return System.Drawing.Color.LightCyan.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFE6E6FA 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Lavender { get { return System.Drawing.Color.Lavender.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFBDB76B 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkKhaki { get { return System.Drawing.Color.DarkKhaki.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF006400 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkGreen { get { return System.Drawing.Color.DarkGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFA9A9A9 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkGray { get { return System.Drawing.Color.DarkGray.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFB8860B 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkGoldenrod { get { return System.Drawing.Color.DarkGoldenrod.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF008B8B 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkCyan { get { return System.Drawing.Color.DarkCyan.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00008B 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkBlue { get { return System.Drawing.Color.DarkBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00FFFF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Cyan { get { return System.Drawing.Color.Cyan.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFDC143C 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Crimson { get { return System.Drawing.Color.Crimson.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFF8DC 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Cornsilk { get { return System.Drawing.Color.Cornsilk.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFF0F5 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color LavenderBlush { get { return System.Drawing.Color.LavenderBlush.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF7F50 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Coral { get { return System.Drawing.Color.Coral.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFD2691E 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Chocolate { get { return System.Drawing.Color.Chocolate.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF7FFF00 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Chartreuse { get { return System.Drawing.Color.Chartreuse.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF8B008B 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkMagenta { get { return System.Drawing.Color.DarkMagenta.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF5F9EA0 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color CadetBlue { get { return System.Drawing.Color.CadetBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFA52A2A 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Brown { get { return System.Drawing.Color.Brown.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF8A2BE2 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color BlueViolet { get { return System.Drawing.Color.BlueViolet.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF0000FF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Blue { get { return System.Drawing.Color.Blue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFEBCD 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color BlanchedAlmond { get { return System.Drawing.Color.BlanchedAlmond.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF000000 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Black { get { return System.Drawing.Color.Black.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFE4C4 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Bisque { get { return System.Drawing.Color.Bisque.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF5F5DC 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Beige { get { return System.Drawing.Color.Beige.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF0FFFF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Azure { get { return System.Drawing.Color.Azure.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF7FFFD4 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Aquamarine { get { return System.Drawing.Color.Aquamarine.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00FFFF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Aqua { get { return System.Drawing.Color.Aqua.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFAEBD7 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color AntiqueWhite { get { return System.Drawing.Color.AntiqueWhite.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF0F8FF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color AliceBlue { get { return System.Drawing.Color.AliceBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Transparent { get { return System.Drawing.Color.Transparent.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFDEB887 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color BurlyWood { get { return System.Drawing.Color.BurlyWood.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF556B2F 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkOliveGreen { get { return System.Drawing.Color.DarkOliveGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF6495ED 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color CornflowerBlue { get { return System.Drawing.Color.CornflowerBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF9932CC 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkOrchid { get { return System.Drawing.Color.DarkOrchid.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF0E68C 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Khaki { get { return System.Drawing.Color.Khaki.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFFFF0 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Ivory { get { return System.Drawing.Color.Ivory.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF8C00 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkOrange { get { return System.Drawing.Color.DarkOrange.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF4B0082 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Indigo { get { return System.Drawing.Color.Indigo.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFCD5C5C 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color IndianRed { get { return System.Drawing.Color.IndianRed.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF69B4 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color HotPink { get { return System.Drawing.Color.HotPink.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF0FFF0 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Honeydew { get { return System.Drawing.Color.Honeydew.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFADFF2F 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color GreenYellow { get { return System.Drawing.Color.GreenYellow.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF008000 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Green { get { return System.Drawing.Color.Green.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF808080 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color 结构，表示系统定义的颜色。
        public static Color Gray { get { return System.Drawing.Color.Gray.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFDAA520 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Goldenrod { get { return System.Drawing.Color.Goldenrod.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFF8F8FF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color GhostWhite { get { return System.Drawing.Color.GhostWhite.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFDCDCDC 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Gainsboro { get { return System.Drawing.Color.Gainsboro.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF00FF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Fuchsia { get { return System.Drawing.Color.Fuchsia.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFD700 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Gold { get { return System.Drawing.Color.Gold.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFFFAF0 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color FloralWhite { get { return System.Drawing.Color.FloralWhite.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF8B0000 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkRed { get { return System.Drawing.Color.DarkRed.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFE9967A 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkSalmon { get { return System.Drawing.Color.DarkSalmon.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF8FBC8F 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkSeaGreen { get { return System.Drawing.Color.DarkSeaGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF228B22 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color ForestGreen { get { return System.Drawing.Color.ForestGreen.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF2F4F4F 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkSlateGray { get { return System.Drawing.Color.DarkSlateGray.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00CED1 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkTurquoise { get { return System.Drawing.Color.DarkTurquoise.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF483D8B 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkSlateBlue { get { return System.Drawing.Color.DarkSlateBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFFF1493 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DeepPink { get { return System.Drawing.Color.DeepPink.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF00BFFF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DeepSkyBlue { get { return System.Drawing.Color.DeepSkyBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF696969 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DimGray { get { return System.Drawing.Color.DimGray.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF1E90FF 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DodgerBlue { get { return System.Drawing.Color.DodgerBlue.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FFB22222 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color Firebrick { get { return System.Drawing.Color.Firebrick.ToRvtColor(); } }
        //
        // 摘要:
        //     获取 ARGB 值为 #FF9400D3 的系统定义的颜色。
        //
        // 返回结果:
        //     一个 System.Drawing.Color，表示系统定义的颜色。
        public static Color DarkViolet { get { return System.Drawing.Color.DarkViolet.ToRvtColor(); } }

        public static Color FromArgb(byte R, byte G, byte B)
        {
            return new Color(R, G, B);
        }
    }
    

}
