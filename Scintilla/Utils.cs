using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace ScintillaNet
{
	public static class Utilities
	{
		public static int ColorToRgb(Color c)
		{
			return c.R + (c.G << 8) + (c.B << 16);
		}

		public static Color RgbToColor(int color)
		{
			return Color.FromArgb(color & 0x0000ff, (color & 0x00ff00) >> 8, (color & 0xff0000) >> 16);
		}

		/// <summary>
		/// Returns an HTML #XXXXXX format for a color. Unlike the ColorTranslator class it
		/// never returns named colors.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static string ColorToHtml(Color c)
		{
			return "#" + c.R.ToString("X2", null) + c.G.ToString("X2", null) + c.B.ToString("X2", null);
		}


		public static int SignedLoWord(IntPtr loWord)
		{
			return (short)((int)(long)loWord & 0xffff);
		}

		public static int SignedHiWord(IntPtr hiWord)
		{
			return (short)(((int)(long)hiWord >> 0x10) & 0xffff);
		}

		/// <summary>
		/// Marshals an IntPtr pointing to un unmanaged byte[] to a .NET String using the given Encoding.
		/// </summary>
		/// <remarks>
		/// I'd love to have this as en extension method but ScintillaNet's probably going to be 2.0 for a long
		///	time to come. There's nothing really compelling in later versions that applies to ScintillaNet that
		///	can't be done with a 2.0 construct (extension methods, linq, etc)
		/// </remarks>
		/// <param name="encoding"></param>
		/// <param name="ptr"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string IntPtrToString(Encoding encoding, IntPtr ptr, int length)
		{
			//	null pointer = null string
			if (ptr == IntPtr.Zero)
				return null;

			//	0 length string = string.Empty
			if (length == 0)
				return string.Empty;

			byte[] buff = new byte[length];
			Marshal.Copy(ptr, buff, 0, length);

			//	We don't want to carry over the Trailing null
			if (buff[buff.Length - 1] == 0)
				length--;

			return encoding.GetString(buff, 0, length);
		}

		public static uint GetMarkerMask(IEnumerable<int> markers)
		{
			uint mask = 0;
			foreach (int i in markers)
				mask |= ((uint)1) << i;
			return mask;
		}

		public static uint GetMarkerMask(IEnumerable<Marker> markers)
		{
			uint mask = 0;
			foreach (Marker m in markers)
				mask |= m.Mask;
			return mask;
		}

		public static Keys GetKeys(char c)
		{
			switch (c)
			{
				case '/':
					return Keys.Oem2;
				case '`':
					return Keys.Oem3;
				case '[':
					return Keys.Oem4;
				case '\\':
					return Keys.Oem5;
				case ']':
					return Keys.Oem6;
				case '-':
					return (Keys)189;

			}

			return (Keys)Enum.Parse(typeof(Keys), c.ToString(), true);
		}

		public static Keys GetKeys(string s)
		{
			switch (s)
			{
				case "/":
					return Keys.Oem2;
				case "`":
					return Keys.Oem3;
				case "[":
					return Keys.Oem4;
				case "\\":
					return Keys.Oem5;
				case "]":
					return Keys.Oem6;
				case "-":
					return (Keys)189;
			}

			return (Keys)Enum.Parse(typeof(Keys), s, true);
		}

	}
}
