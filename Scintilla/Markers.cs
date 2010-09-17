using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace ScintillaNet
{
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class MarkerCollection : TopLevelHelper
	{
		internal MarkerCollection(Scintilla scintilla) : base(scintilla) { }

		internal bool ShouldSerialize()
		{
			return ShouldSerializeFolder() ||
				ShouldSerializeFolderEnd() ||
				ShouldSerializeFolderOpen() ||
				ShouldSerializeFolderOpenMid() ||
				ShouldSerializeFolderOpenMidTail() ||
				ShouldSerializeFolderSub() ||
				ShouldSerializeFolderTail();
		}

		public void Reset()
		{
			for (int i = 0; i < 32; i++)
				this[i].Reset();
		}

		public void AddInstanceSet(int line, uint markerMask)
		{
			NativeScintilla.MarkerAddSet(line, markerMask);
		}

		public void AddInstanceSet(Line line, uint markerMask)
		{
			AddInstanceSet(line.Number, markerMask);
		}

		public void AddInstanceSet(Line line, IEnumerable<Marker> markers)
		{
			AddInstanceSet(line, Utilities.GetMarkerMask(markers));
		}

		public void DeleteInstance(int line, int markerNumber)
		{
			NativeScintilla.MarkerDelete(line, markerNumber);
		}

		public void DeleteInstance(int line, Marker marker)
		{
			DeleteInstance(line, marker.Number);
		}

		public void DeleteAll(int marker)
		{
			NativeScintilla.MarkerDeleteAll(marker);
		}
		
		public void DeleteAll(Marker marker)
		{
			NativeScintilla.MarkerDeleteAll(marker.Number);
		}

		public void DeleteAll()
		{
			NativeScintilla.MarkerDeleteAll(-1);
		}

		public int GetMarkerMask(int line)
		{
			return NativeScintilla.MarkerGet(line);
		}

		public int GetMarkerMask(Line line)
		{
			return NativeScintilla.MarkerGet(line.Number);
		}

		public List<Marker> GetMarkers(Line line)
		{
			return GetMarkers(line.Number);
		}

		public List<Marker> GetMarkers(int line)
		{
			List<Marker> ret = new List<Marker>();
			int mask = GetMarkerMask(line);
			for (int i = 0; i < 32; i++)
				if ((mask & i) == i)
					ret.Add(new Marker(Scintilla, i));

			return ret;
		}

		#region FindNextMarker
		public Line FindNextMarker(int line, uint markerMask)
		{
			int foundLine = NativeScintilla.MarkerNext(line, markerMask);
			if (foundLine < 0)
				return null;

			return new Line(Scintilla,foundLine);
		}

		public Line FindNextMarker(Line line, uint markerMask)
		{
			return FindNextMarker(line.Number, markerMask);
		}

		public Line FindNextMarker(Line line, Marker marker)
		{
			return FindNextMarker(line.Number, (uint)marker.Number);
		}

		public Line FindNextMarker(Line line, IEnumerable<int> markers)
		{
			return FindNextMarker(line.Number, Utilities.GetMarkerMask(markers));
		}

		public Line FindNextMarker(Line line, IEnumerable<Marker> markers)
		{
			return FindNextMarker(line.Number, Utilities.GetMarkerMask(markers));
		}

		public Line FindNextMarker(int line, Marker marker)
		{
			return FindNextMarker(line, (uint)marker.Number);
		}

		public Line FindNextMarker(Marker marker)
		{
			return FindNextMarker(nextLine(), (uint)marker.Number);
		}

		public Line FindNextMarker(uint markerMask)
		{
			return FindNextMarker(nextLine(), markerMask);
		}

		public Line FindNextMarker(IEnumerable<int> markers)
		{
			return FindNextMarker(nextLine(), Utilities.GetMarkerMask(markers));
		}

		public Line FindNextMarker(IEnumerable<Marker> markers)
		{
			return FindNextMarker(nextLine(), Utilities.GetMarkerMask(markers));
		}

		public Line FindNextMarker(int line)
		{
			return FindNextMarker(line, UInt32.MaxValue);
		}


		public Line FindNextMarker(Line line)
		{
			return FindNextMarker(line.Number, UInt32.MaxValue);
		}

		public Line FindNextMarker()
		{
			unchecked
			{
				return FindNextMarker(nextLine(), UInt32.MaxValue);
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Marker this[int markerNumber]
		{
			get
			{
				return new Marker(Scintilla, markerNumber);
			}
		}

		#region FolderEnd
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Marker FolderEnd
		{
			get
			{
				return new Marker(Scintilla, Constants.SC_MARKNUM_FOLDEREND);
			}
		}

		private bool ShouldSerializeFolderEnd()
		{
			return FolderEnd.ShouldSerialize();
		}

		#endregion

		#region FolderOpenMid
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Marker FolderOpenMid
		{
			get
			{
				return new Marker(Scintilla, Constants.SC_MARKNUM_FOLDEROPENMID);
			}
		}

		private bool ShouldSerializeFolderOpenMid()
		{
			return FolderOpenMid.ShouldSerialize();
		}

		#endregion

		#region FolderOpenMidTail
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Marker FolderOpenMidTail
		{
			get
			{
				return new Marker(Scintilla, Constants.SC_MARKNUM_FOLDERMIDTAIL);
			}
		}

		private bool ShouldSerializeFolderOpenMidTail()
		{
			return FolderOpenMidTail.ShouldSerialize();
		}

		#endregion

		#region FolderTail
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Marker FolderTail
		{
			get
			{
				return new Marker(Scintilla, Constants.SC_MARKNUM_FOLDERTAIL);
			}
		}

		private bool ShouldSerializeFolderTail()
		{
			return FolderTail.ShouldSerialize();
		}

		#endregion

		#region FolderSub
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Marker FolderSub
		{
			get
			{
				return new Marker(Scintilla, Constants.SC_MARKNUM_FOLDERSUB);
			}
		}

		private bool ShouldSerializeFolderSub()
		{
			return FolderSub.ShouldSerialize();
		}

		#endregion

		#region Folder
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Marker Folder
		{
			get
			{
				return new Marker(Scintilla, Constants.SC_MARKNUM_FOLDER);
			}
		}

		private bool ShouldSerializeFolder()
		{
			return Folder.ShouldSerialize();
		}

		#endregion

		#region FolderOpen
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Marker FolderOpen
		{
			get
			{
				return new Marker(Scintilla, Constants.SC_MARKNUM_FOLDEROPEN);
			}
		}

		private bool ShouldSerializeFolderOpen()
		{
			return FolderOpen.ShouldSerialize();
		} 
		#endregion






		#endregion

		#region FindPreviousMarker
		public Line FindPreviousMarker(int line, uint markerMask)
		{
			int lineNo = NativeScintilla.MarkerPrevious(line, markerMask);
			if (lineNo < 0)
				return null;

			return new Line(Scintilla, lineNo);
		}

		public Line FindPreviousMarker(Line line, uint markerMask)
		{
			return FindPreviousMarker(line.Number, markerMask);
		}

		public Line FindPreviousMarker(Line line, Marker marker)
		{
			return FindPreviousMarker(line.Number, (uint)marker.Number);
		}

		public Line FindPreviousMarker(Line line, IEnumerable<int> markers)
		{
			return FindPreviousMarker(line.Number, Utilities.GetMarkerMask(markers));
		}

		public Line FindPreviousMarker(Line line, IEnumerable<Marker> markers)
		{
			return FindPreviousMarker(line.Number, Utilities.GetMarkerMask(markers));
		}

		public Line FindPreviousMarker(int line, Marker marker)
		{
			return FindPreviousMarker(line, (uint)marker.Number);
		}

		public Line FindPreviousMarker(Marker marker)
		{
			return FindPreviousMarker(prevLine(), (uint)marker.Number);
		}

		public Line FindPreviousMarker(uint markerMask)
		{
			return FindPreviousMarker(prevLine(), markerMask);
		}

		public Line FindPreviousMarker(int line)
		{
			return FindPreviousMarker(line, UInt32.MaxValue);
		}

		public Line FindPreviousMarker(IEnumerable<int> markers)
		{
			return FindPreviousMarker(prevLine(), Utilities.GetMarkerMask(markers));
		}

		public Line FindPreviousMarker(IEnumerable<Marker> markers)
		{
			return FindPreviousMarker(nextLine(), Utilities.GetMarkerMask(markers));
		}

		public Line FindPreviousMarker(Line line)
		{
			return FindPreviousMarker(line.Number, UInt32.MaxValue);
		}

		public Line FindPreviousMarker()
		{
			return FindPreviousMarker(prevLine(), UInt32.MaxValue);
		}
		#endregion

		private int nextLine()
		{
			return NativeScintilla.LineFromPosition(NativeScintilla.GetCurrentPos()) + 1;
		}

		private int prevLine()
		{
			return NativeScintilla.LineFromPosition(NativeScintilla.GetCurrentPos()) - 1;
		}


	}

	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class Marker : ScintillaHelperBase
	{
		internal Marker(Scintilla scintilla, int number) : base(scintilla) 
		{
			_number = number;
		}

		internal bool ShouldSerialize()
		{
			return ShouldSerializeAlpha() ||
				ShouldSerializeBackColor() ||
				ShouldSerializeForeColor() ||
				ShouldSerializeSymbol();
		}

		public void Reset()
		{
			ResetAlpha();
			ResetBackColor();
			ResetForeColor();
			ResetSymbol();
		}

		private int _number;
		public int Number
		{
			get
			{
				return _number;
			}
			set
			{
				_number = value;
			}
		}

		public uint Mask
		{
			get
			{
				uint result = ((uint)1) << Number;
				return result;
			}
		}

		#region Symbol
		public MarkerSymbol Symbol
		{
			get
			{
				if (Scintilla.PropertyBag.ContainsKey(ToString() + ".Symbol"))
					return (MarkerSymbol)Scintilla.PropertyBag[ToString() + ".Symbol"];

				return MarkerSymbol.Circle;
			}
			set
			{
				SetSymbolInternal(value);
				Scintilla.Folding.MarkerScheme = FoldMarkerScheme.Custom;
			}
		}

		internal void SetSymbolInternal(MarkerSymbol value)
		{
			Scintilla.PropertyBag[ToString() + ".Symbol"] = value;
			NativeScintilla.MarkerDefine(_number, (int)value);
		}

		private bool ShouldSerializeSymbol()
		{
			if (Scintilla.Folding.MarkerScheme == FoldMarkerScheme.Custom)
				return Symbol != MarkerSymbol.Circle;

			return false;
		}

		private void ResetSymbol()
		{
			Symbol = MarkerSymbol.Circle;
		}

		#endregion

		#region ForeColor
		public Color ForeColor
		{
			get
			{
				if (Scintilla.ColorBag.ContainsKey(ToString() + ".ForeColor"))
					return Scintilla.ColorBag[ToString() + ".ForeColor"];

				return Color.Black;
			}
			set
			{
				SetForeColorInternal(value);
				Scintilla.Folding.MarkerScheme = FoldMarkerScheme.Custom;
			}
		}

		internal void SetForeColorInternal(Color value)
		{
			Scintilla.ColorBag[ToString() + ".ForeColor"] = value;
			NativeScintilla.MarkerSetFore(_number, Utilities.ColorToRgb(value));
		}

		private bool ShouldSerializeForeColor()
		{
			if (Scintilla.Folding.MarkerScheme == FoldMarkerScheme.Custom)
				return ForeColor != Color.Black;

			return false;
		}

		private void ResetForeColor()
		{
			ForeColor = Color.Black;
		} 
		#endregion

		#region BackColor
		public Color BackColor
		{
			get
			{
				if (Scintilla.ColorBag.ContainsKey(ToString() + ".BackColor"))
					return Scintilla.ColorBag[ToString() + ".BackColor"];

				return Color.White;
			}
			set
			{
				SetBackColorInternal(value);
				Scintilla.Folding.MarkerScheme = FoldMarkerScheme.Custom;
			}
		}

		internal void SetBackColorInternal(Color value)
		{
			Scintilla.ColorBag[ToString() + ".BackColor"] = value;
			NativeScintilla.MarkerSetBack(_number, Utilities.ColorToRgb(value));
		}

		private bool ShouldSerializeBackColor()
		{
			if (Scintilla.Folding.MarkerScheme == FoldMarkerScheme.Custom)
				return BackColor != Color.White;

			return false;
		}

		private void ResetBackColor()
		{
			BackColor = Color.White;
		} 
		#endregion

		#region Alpha
		public int Alpha
		{
			get
			{

				try
				{
					if (Scintilla.PropertyBag.ContainsKey(ToString() + ".Alpha"))
						return (int)Scintilla.PropertyBag[ToString() + "Alpha"];

					return 0xff;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					return 0xff;
				}
			}
			set
			{
				Scintilla.PropertyBag[ToString() + ".Alpha"] = value;
				NativeScintilla.MarkerSetAlpha(_number, value);
			}
		}


		private bool ShouldSerializeAlpha()
		{
			return Alpha != 0xff;
		}

		private void ResetAlpha()
		{
			Alpha = 0xff;
		} 
		#endregion

		public override string ToString()
		{
			return "MarkerNumber" + _number;
		}

		public void SetImage(string xpmImage)
		{
			NativeScintilla.MarkerDefinePixmap(_number, xpmImage);
		}

		public void SetImage(Bitmap image, Color transparentColor)
		{
			NativeScintilla.MarkerDefinePixmap(_number, XpmConverter.ConvertToXPM(image, Utilities.ColorToHtml(transparentColor)));
		}

		public void SetImage(Bitmap image)
		{
			NativeScintilla.MarkerDefinePixmap(_number, XpmConverter.ConvertToXPM(image));
		}

		public MarkerInstance AddInstanceTo(int line)
		{
			return new MarkerInstance(Scintilla, this, NativeScintilla.MarkerAdd(line, _number));
		}

		public MarkerInstance AddInstanceTo(Line line)
		{
			return AddInstanceTo(line.Number);
		}

		public override bool Equals(object obj)
		{
			if (!IsSameHelperFamily(obj))
				return false;

			return ((Marker)obj).Number == this.Number;
			
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class MarkerInstance : ScintillaHelperBase
	{
		internal MarkerInstance(Scintilla scintilla, Marker marker, int handle) : base(scintilla) 
		{
			_marker = marker;
			_handle = handle;
		}

		private int _handle;
		public int Handle
		{
			get
			{
				return _handle;
			}
		}

		private Marker _marker;
		public Marker Marker
		{
			get
			{
				return _marker;
			}
		}

		public Line Line
		{
			get
			{
				int lineNo = NativeScintilla.MarkerLineFromHandle(_handle);
				if (lineNo < 0)
					return null;

				return new Line(Scintilla, lineNo);

			}
		}

		public void Delete()
		{
			NativeScintilla.MarkerDeleteHandle(_handle);
		}

		public override bool Equals(object obj)
		{
			if (!IsSameHelperFamily(obj))
				return false;

			return ((MarkerInstance)obj).Handle == this.Handle;			
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
