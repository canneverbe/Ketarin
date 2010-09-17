using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ScintillaNet
{
	/// <summary>
	/// Manages End of line settings for the Scintilla Control
	/// </summary>
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class EndOfLine : TopLevelHelper
	{
		internal EndOfLine(Scintilla scintilla) : base(scintilla) { }

		internal bool ShouldSerialize()
		{
			return ShouldSerializeIsVisible() || ShouldSerializeMode() || ShouldSerializeConvertOnPaste();
		}

		#region ConvertOnPaste
		/// <summary>
		/// Gets/Sets whether pasted content's line endings should be changed to match
		/// the current document's settings.
		/// </summary>
		public bool ConvertOnPaste
		{
			get
			{
				return NativeScintilla.GetPasteConvertEndings();
			}
			set
			{
				NativeScintilla.SetPasteConvertEndings(value);
			}
		}

		private bool ShouldSerializeConvertOnPaste()
		{
			return !ConvertOnPaste;
		}

		private void ResetConvertOnPaste()
		{
			ConvertOnPaste = true;
		} 
		#endregion

		#region Mode
		/// <summary>
		/// Gets/Sets the <see cref="EndOfLineMode"/> for the document. Default is CrLf.
		/// </summary>
		public EndOfLineMode Mode
		{
			get
			{
				return (EndOfLineMode)NativeScintilla.GetEolMode();
			}
			set
			{
				NativeScintilla.SetEolMode((int)value);
			}
		}

		private bool ShouldSerializeMode()
		{
			//	Yeah I'm assuming Windows, if this does ever make it to another platform 
			//	a check should be made to make it platform specific
			return Mode != EndOfLineMode.Crlf;
		}

		private void ResetMode()
		{
			Mode = EndOfLineMode.Crlf;
		} 
		#endregion

		#region IsVisible
		/// <summary>
		/// Gets/Sets if End of line markers are visible in the Scintilla control.
		/// </summary>
		public bool IsVisible
		{
			get
			{
				return NativeScintilla.GetViewEol();
			}
			set
			{
				NativeScintilla.SetViewEol(value);
			}
		}

		private bool ShouldSerializeIsVisible()
		{
			return IsVisible;
		}

		private void ResetIsVisible()
		{
			IsVisible = false;
		} 
		#endregion

		public string EolString
		{
			get
			{
				switch (Mode)
				{
					case EndOfLineMode.CR:
						return "\r";
					case EndOfLineMode.LF:
						return "\n";
					case EndOfLineMode.Crlf:
						return "\r\n";
				}
				return "";
			}
		}

		/// <summary>
		/// Converts all lines in the document to the given mode.
		/// </summary>
		/// <param name="toMode">The EndOfLineMode to convert all lines to </param>
		public void ConvertAllLines(EndOfLineMode toMode)
		{
			NativeScintilla.ConvertEols((int)toMode);
		}
	}
}
