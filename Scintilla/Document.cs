using System;
using System.Collections.Generic;
using System.Text;

namespace ScintillaNet
{
	/// <summary>
	/// Manages the Native Scintilla's Document features.
	/// </summary>
	/// <remarks>
	/// See Scintilla's documentation on multiple views for an understanding of Documents.
	/// Note that all ScintillaNET specific features are considered to be part of the View, not document.
	/// </remarks>
	public class DocumentHandler : TopLevelHelper
	{
		internal DocumentHandler(Scintilla scintilla) : base(scintilla) { }

		/// <summary>
		/// Gets/Sets the currently loaded Document
		/// </summary>
		public Document Current
		{
			get
			{
				return new Document(Scintilla,NativeScintilla.GetDocPointer());
			}
			set
            {
				NativeScintilla.SetDocPointer(value.Handle);
            }
		}

		/// <summary>
		/// Creates a new Document
		/// </summary>
		/// <returns></returns>
		public Document Create()
		{
			return new Document(Scintilla, NativeScintilla.CreateDocument());
		}
	}

	/// <summary>
	/// Provides an abstraction over Scintilla's Document Pointer
	/// </summary>
	public class Document : ScintillaHelperBase
	{
		private IntPtr _handle;
		/// <summary>
		/// Scintilla's internal document pointer.
		/// </summary>
		public IntPtr Handle
		{
			get
			{
				return _handle;
			}
			set
			{
				_handle = value;
			}
		}

		internal Document(Scintilla scintilla, IntPtr handle) : base(scintilla) 
		{
			_handle = handle;
		}

		/// <summary>
		/// Increases the document's reference count
		/// </summary>
		/// <remarks>No, you aren't looking at COM, move along.</remarks>
		public void AddRef()
		{
			NativeScintilla.AddRefDocument(_handle);
		}

		/// <summary>
		/// Decreases the document's reference count
		/// </summary>
		/// <remarks>
		/// When the document's reference count reaches 0 Scintilla will destroy the document
		/// </remarks>
		public void Release()
		{
			NativeScintilla.ReleaseDocument(_handle);
		}

		/// <summary>
		/// Overriden. 
		/// </summary>
		/// <param name="obj">Another Document Object</param>
		/// <returns>True if both Documents have the same Handle</returns>
		public override bool Equals(object obj)
		{
			Document d = obj as Document;

			if (_handle == IntPtr.Zero)
				return false;

			return _handle.Equals(d._handle);
		}

		/// <summary>
		/// Overriden
		/// </summary>
		/// <returns>Document Pointer's hashcode</returns>
		public override int GetHashCode()
		{
			return _handle.GetHashCode();
		}
	}
}
