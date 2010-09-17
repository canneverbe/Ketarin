using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ScintillaNet
{
	/// <summary>
	/// Performs OS Clipboard acess.
	/// </summary>
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class Clipboard : TopLevelHelper
	{
		internal Clipboard(Scintilla scintilla) : base(scintilla) { }

		internal bool ShouldSerialize()
		{
			return ShouldSerializeConvertEndOfLineOnPaste();
		}

		/// <summary>
		/// Copies the currently selected text to the clipboard.
		/// </summary>
		public void Copy()
		{
			NativeScintilla.Copy();
		}

		/// <summary>
		/// Copies the given text to the clipboard
		/// </summary>
		/// <param name="text">Text to be copied</param>
		public void Copy(string text)
		{
			NativeScintilla.CopyText(text.Length, text);
		}

		/// <summary>
		/// Copies the contents of a given range to the clipboard.
		/// </summary>
		/// <param name="rangeToCopy">Range of text to copy</param>
		public void Copy(Range rangeToCopy)
		{
			Copy(rangeToCopy.Start, rangeToCopy.End);
		}

		/// <summary>
		/// Copies text bounded by the given position
		/// </summary>
		/// <param name="positionStart">Start position in the document of text to copy</param>
		/// <param name="positionEnd">End position in the document of text to copy</param>
		public void Copy(int positionStart, int positionEnd)
		{
			NativeScintilla.CopyRange(positionStart, positionEnd);
		}

		/// <summary>
		/// Cuts the currently selected text to the clipboard
		/// </summary>
		public void Cut()
		{
			NativeScintilla.Cut();
		}

		/// <summary>
		/// Pastes the current content of the clipboard into the current document position
		/// </summary>
		public void Paste()
		{
			NativeScintilla.Paste();
		}

		/// <summary>
		/// Returns true if a paste operation can occur.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanPaste
		{
			get
			{
				return NativeScintilla.CanPaste();
			}
		}

		#region ConvertEndOfLineOnPaste
		/// <summary>
		/// Gets/Sets wether pasted text's end of line characters are automatically converted
		/// to match the document's.
		/// </summary>
		/// <remarks>This is the same as EndOfLine.ConvertOnPaste</remarks>
		public bool ConvertEndOfLineOnPaste
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

		private bool ShouldSerializeConvertEndOfLineOnPaste()
		{
			return !ConvertEndOfLineOnPaste;
		}

		private void ResetConvertEndOfLineOnPaste()
		{
			ConvertEndOfLineOnPaste = true;
		} 
		#endregion
	}
}


