using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace ScintillaNet
{
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class Folding : TopLevelHelper
	{
		internal Folding(Scintilla scintilla) : base(scintilla) 
		{
			IsEnabled = true;
			UseCompactFolding = false;
			MarkerScheme = FoldMarkerScheme.BoxPlusMinus;
		}

		internal bool ShouldSerialize()
		{
			return ShouldSerializeFlags() ||
				ShouldSerializeIsEnabled() ||
				ShouldSerializeMarkerScheme() ||
				ShouldSerializeUseCompactFolding();
		}

		#region IsEnabled
		public bool IsEnabled
		{
			get
			{
				return Scintilla.Lexing.GetProperty("fold") == "1";
			}
			set
			{
				string s;
				if (value)
					s = "1";
				else
					s = "0";

				Scintilla.Lexing.SetProperty("fold", s);
				Scintilla.Lexing.SetProperty("fold.html", s);
			}
		}

		private bool ShouldSerializeIsEnabled()
		{
			return !IsEnabled;
		}

		private void ResetIsEnabled()
		{
			IsEnabled = true;
		}
		#endregion

		#region Flags
		private FoldFlag _flags;

		[Editor(typeof(ScintillaNet.Design.FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor)), Category("Appearance")]
		public FoldFlag Flags
		{
			get
			{
				return _flags;
			}
			set
			{
				_flags = value;
				NativeScintilla.SetFoldFlags((int)value);
			}
		}

		private bool ShouldSerializeFlags()
		{
			return (int)Flags != 0;
		}

		private void ResetFlags()
		{
			Flags = (FoldFlag)0;
		}
		#endregion

		#region UseCompactFolding
		public bool UseCompactFolding
		{
			get
			{
				return Scintilla.Lexing.GetProperty("fold.compact") == "1";
			}
			set
			{
				string val = "0";

				if (value)
					val = "1";

				Scintilla.Lexing.SetProperty("fold.compact", val);
			}
		}

		private bool ShouldSerializeUseCompactFolding()
		{
			return UseCompactFolding;
		}

		private void ResetUseCompactFolding()
		{
			UseCompactFolding = false;
		}

		#endregion	

		private FoldMarkerScheme _markerScheme;
		#region MarkerScheme
		public FoldMarkerScheme MarkerScheme
		{
			get
			{
				return _markerScheme;
			}
			set
			{
				_markerScheme = value;

				if (value == FoldMarkerScheme.Custom)
					return;

				MarkerCollection mc = Scintilla.Markers;

				mc.Folder.SetBackColorInternal(Color.Gray);
				mc.FolderEnd.SetBackColorInternal(Color.Gray);
				mc.FolderOpen.SetBackColorInternal(Color.Gray);
				mc.FolderOpenMid.SetBackColorInternal(Color.Gray);
				mc.FolderOpenMidTail.SetBackColorInternal(Color.Gray);
				mc.FolderSub.SetBackColorInternal(Color.Gray);
				mc.FolderTail.SetBackColorInternal(Color.Gray);

				mc.Folder.SetForeColorInternal(Color.White);
				mc.FolderEnd.SetForeColorInternal(Color.White);
				mc.FolderOpen.SetForeColorInternal(Color.White);
				mc.FolderOpenMid.SetForeColorInternal(Color.White);
				mc.FolderOpenMidTail.SetForeColorInternal(Color.White);
				mc.FolderSub.SetForeColorInternal(Color.White);
				mc.FolderTail.SetForeColorInternal(Color.White);

				switch (value)
				{
					case FoldMarkerScheme.Arrow:
						mc.Folder.SetSymbolInternal(MarkerSymbol.Arrow);
						mc.FolderEnd.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderOpen.SetSymbolInternal(MarkerSymbol.ArrowDown);
						mc.FolderOpenMid.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderOpenMidTail.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderSub.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderTail.SetSymbolInternal(MarkerSymbol.Empty);
						break;
					case FoldMarkerScheme.BoxPlusMinus:
						mc.Folder.SetSymbolInternal(MarkerSymbol.BoxPlus);
						mc.FolderEnd.SetSymbolInternal(MarkerSymbol.BoxPlusConnected);
						mc.FolderOpen.SetSymbolInternal(MarkerSymbol.BoxMinus);
						mc.FolderOpenMid.SetSymbolInternal(MarkerSymbol.BoxMinusConnected);
						mc.FolderOpenMidTail.SetSymbolInternal(MarkerSymbol.LCorner);
						mc.FolderSub.SetSymbolInternal(MarkerSymbol.VLine);
						mc.FolderTail.SetSymbolInternal(MarkerSymbol.LCorner);
						break;
					case FoldMarkerScheme.CirclePlusMinus:
						mc.Folder.SetSymbolInternal(MarkerSymbol.CirclePlus);
						mc.FolderEnd.SetSymbolInternal(MarkerSymbol.CirclePlusConnected);
						mc.FolderOpen.SetSymbolInternal(MarkerSymbol.CircleMinus);
						mc.FolderOpenMid.SetSymbolInternal(MarkerSymbol.CircleMinusConnected);
						mc.FolderOpenMidTail.SetSymbolInternal(MarkerSymbol.LCornerCurve);
						mc.FolderSub.SetSymbolInternal(MarkerSymbol.VLine);
						mc.FolderTail.SetSymbolInternal(MarkerSymbol.LCornerCurve);
						break;
					case FoldMarkerScheme.PlusMinus:
						mc.Folder.SetSymbolInternal(MarkerSymbol.Plus);
						mc.FolderEnd.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderOpen.SetSymbolInternal(MarkerSymbol.Minus);
						mc.FolderOpenMid.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderOpenMidTail.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderSub.SetSymbolInternal(MarkerSymbol.Empty);
						mc.FolderTail.SetSymbolInternal(MarkerSymbol.Empty);
						break;
				}
			}
		}

		private bool ShouldSerializeMarkerScheme()
		{
			return _markerScheme != FoldMarkerScheme.BoxPlusMinus;
		}

		private void ResetMarkerScheme()
		{
			MarkerScheme = FoldMarkerScheme.BoxPlusMinus;
		} 
		#endregion
	}

}

