using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
namespace ScintillaNet
{
	#region DropMarkerCollectEventArgs
	/// <summary>
	/// Provides data for a DropMarkerCollect event
	/// </summary>
	public class DropMarkerCollectEventArgs : CancelEventArgs
	{
		private DropMarker _dropMarker;
		/// <summary>
		/// Returns the DropMarker that was collected
		/// </summary>
		public DropMarker DropMarker
		{
			get
			{
				return _dropMarker;
			}
		}

		/// <summary>
		/// Initializes a new instance of the DropMarkerCollectEventArgs class.
		/// </summary>
		/// <param name="dropMarker"></param>
		public DropMarkerCollectEventArgs(DropMarker dropMarker)
		{
			_dropMarker = dropMarker;
		}
	} 
	#endregion

	#region CharAddedEventArgs

	/// <summary>
	/// Provides data for the CallTipClick event
	/// </summary>
	public class CallTipClickEventArgs : EventArgs
	{
		private CallTipArrow _callTipArrow;
		/// <summary>
		/// Returns the CallTipArrow that was clicked
		/// </summary>
		public CallTipArrow CallTipArrow
		{
			get
			{
				return _callTipArrow;
			}
		}

		private int _currentIndex;
		/// <summary>
		/// Gets the current index of the CallTip's overload list
		/// </summary>
		public int CurrentIndex
		{
			get
			{
				return _currentIndex;
			}
		}

		private int _newIndex;
		/// <summary>
		/// Gets/Sets the new index of the CallTip's overload list
		/// </summary>
		public int NewIndex
		{
			get
			{
				return _newIndex;
			}
			set
			{
				_newIndex = value;
			}
		}

		private OverloadList _overloadList;
		/// <summary>
		/// Returns the OverLoad list of the CallTip
		/// </summary>
		public OverloadList OverloadList
		{
			get
			{
				return _overloadList;
			}
		}

		private bool _cancel = false;
		/// <summary>
		/// Gets/Sets if the CallTip should be hidden
		/// </summary>
		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}

		private int _highlightStart;

		/// <summary>
		/// Gets/Sets the start position of the CallTip's highlighted portion of text
		/// </summary>
		public int HighlightStart
		{
			get
			{
				return _highlightStart;
			}
			set
			{
				_highlightStart = value;
			}
		}

		private int _highlightEnd;
		/// <summary>
		/// Gets/Sets the end position of the CallTip's highlighted portion of text
		/// </summary>
		public int HighlightEnd
		{
			get
			{
				return _highlightEnd;
			}
			set
			{
				_highlightEnd = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the CallTipClickEventArgs class.
		/// </summary>
		/// <param name="callTipArrow">CallTipArrow clicked</param>
		/// <param name="currentIndex">Current posision of the overload list</param>
		/// <param name="newIndex">New position of the overload list</param>
		/// <param name="overloadList">List of overloads to be cycled in the calltip</param>
		/// <param name="highlightStart">Start position of the highlighted text</param>
		/// <param name="highlightEnd">End position of the highlighted text</param>
		public CallTipClickEventArgs(CallTipArrow callTipArrow, int currentIndex, int newIndex, OverloadList overloadList, int highlightStart, int highlightEnd)
		{
			_callTipArrow = callTipArrow;
			_currentIndex = currentIndex;
			_newIndex = newIndex;
			_overloadList = overloadList;
			_highlightStart = highlightStart;
			_highlightEnd = highlightEnd;
		}
	}
	#endregion

	#region CharAddedEventArgs

	/// <summary>
	/// Provides data for the CharAdded event
	/// </summary>
	public class CharAddedEventArgs : EventArgs
	{
		private char _ch;
		/// <summary>
		/// Returns the character that was added
		/// </summary>
		public char Ch
		{
			get
			{
				return _ch;
			}
		}

		/// <summary>
		/// Initializes a new instance of the CharAddedEventArgs class.
		/// </summary>
		/// <param name="ch">The character that was added</param>
		public CharAddedEventArgs(char ch)
		{
			_ch = ch;
		}
	}
	#endregion

	#region FoldChangedEventArgs
	/// <summary>
	/// Provides data for the FoldChanged event
	/// </summary>
	public class FoldChangedEventArgs : ModifiedEventArgs
	{
		private int _line;
		private int _newFoldLevel;
		private int _previousFoldLevel;

		/// <summary>
		/// Gets/Sets the Line # that the fold change occured on
		/// </summary>
		public int Line
		{
			get
			{
				return _line;
			}
			set
			{
				_line = value;
			}
		}

		/// <summary>
		/// Gets the new Fold Level of the line
		/// </summary>
		public int NewFoldLevel
		{
			get
			{
				return _newFoldLevel;
			}
		}

		/// <summary>
		/// Gets the previous Fold Level of the line
		/// </summary>
		public int PreviousFoldLevel
		{
			get
			{
				return _previousFoldLevel;
			}
			
		}

		/// <summary>
		/// Initializes a new instance of the FoldChangedEventArgs class.
		/// </summary>
		/// <param name="line">Line # that the fold change occured on</param>
		/// <param name="newFoldLevel">new Fold Level of the line</param>
		/// <param name="previousFoldLevel">previous Fold Level of the line</param>
		/// <param name="modificationType">What kind of fold modification occured</param>
		public FoldChangedEventArgs(int line, int newFoldLevel, int previousFoldLevel, int modificationType) : base(modificationType)
		{
			_line				= line;
			_newFoldLevel		= newFoldLevel;
			_previousFoldLevel	= previousFoldLevel;
		}
	}
	#endregion 

	#region LinesNeedShownEventArgs
	/// <summary>
	/// Provides data for the LinesNeedShown event
	/// </summary>
	public class LinesNeedShownEventArgs : EventArgs
	{
		private int _firstLine;
		private int _lastLine;

		/// <summary>
		/// Returns the first (top) line that needs to be shown
		/// </summary>
		public int FirstLine
		{
			get { return _firstLine; }
		}

		/// <summary>
		/// Returns the last (bottom) line that needs to be shown
		/// </summary>
		public int LastLine
		{
			get { return _lastLine; }
			set { _lastLine = value; }
		}

		/// <summary>
		/// Initializes a new instance of the LinesNeedShownEventArgs class.
		/// </summary>
		/// <param name="startLine">the first (top) line that needs to be shown</param>
		/// <param name="endLine">the last (bottom) line that needs to be shown</param>
		public LinesNeedShownEventArgs(int startLine, int endLine)
		{
			_firstLine	= startLine;
			_lastLine	= endLine;
		}

	}

	#endregion

	#region MarkerChangedEventArgs
	/// <summary>
	/// Provides data for the MarkerChanged event
	/// </summary>
	public class MarkerChangedEventArgs : ModifiedEventArgs
	{
		private int _line;

		/// <summary>
		/// Returns the line number where the marker change occured
		/// </summary>
		public int Line
		{
			get
			{
				return _line;
			}
			set
			{
				_line = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the LinesNeedShownEventArgs class.
		/// </summary>
		/// <param name="line">Line number where the marker change occured</param>
		/// <param name="modificationType">What type of Scintilla modification occured</param>
		public MarkerChangedEventArgs(int line, int modificationType)
			: base(modificationType)
		{
			_line = line;
		}

	}
	#endregion

	#region ModifiedEventArgs

	
	/// <summary>
	/// Base class for modified events
	/// </summary>
	/// <remarks>
	/// ModifiedEventArgs is the base class for all events that are fired 
	///	in response to an SCN_MODIFY notification message. They all have 
	///	the Undo/Redo flags in common and I'm also including the raw 
	///	modificationType integer value for convenience purposes.
	/// </remarks>
	public abstract class ModifiedEventArgs : EventArgs
	{
		private UndoRedoFlags _undoRedoFlags;
		private int _modificationType;

		public int ModificationType
		{
			get
			{
				return _modificationType;
			}
			set
			{
				_modificationType = value;
			}
		}

		public UndoRedoFlags UndoRedoFlags
		{
			get
			{
				return _undoRedoFlags;
			}
			set
			{
				_undoRedoFlags = value;
			}
		}

		public ModifiedEventArgs(int modificationType)
		{
			_modificationType	= modificationType;
			_undoRedoFlags		= new UndoRedoFlags(modificationType);
		}
	}
	#endregion

	#region NativeScintillaEventArgs
	
	
	/// <summary>
	/// Provides data for native Scintilla Events
	/// </summary>
	/// <remarks>
	/// All events fired from the INativeScintilla Interface uses
	///	NativeScintillaEventArgs. Msg is a copy
	///	of the Notification Message sent to Scintilla's Parent WndProc
	///	and SCNotification is the SCNotification Struct pointed to by 
	///	Msg's lParam. 
	/// </remarks>
	public class NativeScintillaEventArgs : EventArgs
	{
		private Message _msg;
		private SCNotification _notification;

		/// <summary>
		/// Notification Message sent from the native Scintilla
		/// </summary>
		public Message Msg
		{
			get
			{
				return _msg;
			}
		}

		/// <summary>
		/// SCNotification structure sent from Scintilla that contains the event data
		/// </summary>
		public SCNotification SCNotification
		{
			get
			{
				return _notification;
			}
		}

		/// <summary>
		/// Initializes a new instance of the NativeScintillaEventArgs class.
		/// </summary>
		/// <param name="msg">Notification Message sent from the native Scintilla</param>
		/// <param name="notification">SCNotification structure sent from Scintilla that contains the event data</param>
		public NativeScintillaEventArgs(Message msg, SCNotification notification)
		{
			_msg			= msg;
			_notification	= notification;
		}
	}
	#endregion
		
	#region ScintillaMouseEventArgs

	/// <summary>
	/// Provides data for Scintilla mouse events
	/// </summary>
	public class ScintillaMouseEventArgs : EventArgs
	{
		private int _x;
		private int _y;
		private int _position;

		/// <summary>
		/// Returns the X (left) position of mouse in pixels
		/// </summary>
		public int X
		{
			get { return _x; }
			set { _x = value; }
		}

		/// <summary>
		/// Returns the Y (top) position of mouse in pixels
		/// </summary>
		public int Y
		{
			get { return _y; }
			set { _y = value; }
		}

		/// <summary>
		/// Returns the Document position
		/// </summary>
		public int Position
		{
			get { return _position; }
			set { _position = value; }
		}

		/// <summary>
		/// Initializes a new instance of the ScintillaMouseEventArgs class.
		/// </summary>
		/// <param name="x">X (left) position of mouse in pixels</param>
		/// <param name="y">Y (top) position of mouse in pixels</param>
		/// <param name="position"> Document position</param>
		public ScintillaMouseEventArgs(int x, int y, int position)
		{
			_x			= x;
			_y			= y;
			_position	= position;
		}
	}

	#endregion
	
	#region StyleChangedEventArgs
	
	/// <summary>
	/// Provides data for the StyleChanged event
	/// </summary>
	/// <remarks>
	/// StyleChangedEventHandler is used for the StyleChanged Event which is also used as 
	///	a more specific abstraction around the SCN_MODIFIED notification message.
	/// </remarks>
	public class StyleChangedEventArgs : ModifiedEventArgs
	{
		private int _position;
		private int _length;

		/// <summary>
		/// Returns the starting document position where the style has been changed
		/// </summary>
		public int Position
		{
			get
			{
				return _position;
			}
		}

		/// <summary>
		/// Returns how many characters have changed
		/// </summary>
		public int Length
		{
			get
			{
				return _length;
			}
		}

		internal StyleChangedEventArgs(int position, int length, int modificationType) : base(modificationType)
		{
			_position	= position;
			_length		= length;
		}
	}
	#endregion

	#region StyleNeededEventArgs

	/// <summary>
	/// Provides data for the StyleNeeded event
	/// </summary>
	public class StyleNeededEventArgs : EventArgs
	{
		private Range _range;
		/// <summary>
		/// Returns the document range that needs styling
		/// </summary>
		public Range Range
		{
			get { return _range; }
		}

		/// <summary>
		/// Initializes a new instance of the StyleNeededEventArgs class.
		/// </summary>
		/// <param name="range">the document range that needs styling</param>
		public StyleNeededEventArgs(Range range)
		{
			_range = range;
		}
	}
	


	#endregion

	#region TextModifiedEventArgs

	/// <summary>
	/// Provices data for the TextModified event
	/// </summary>
	/// <remarks>
	/// TextModifiedEventHandler is used as an abstracted subset of the
	///	SCN_MODIFIED notification message. It's used whenever the SCNotification's
	///	modificationType flags are SC_MOD_INSERTTEXT ,SC_MOD_DELETETEXT, 
	///	SC_MOD_BEFOREINSERT and SC_MOD_BEFORE_DELETE. They all use a 
	///	TextModifiedEventArgs which corresponds to a subset of the 
	///	SCNotification struct having to do with these modification types.
	/// </remarks>
	public class TextModifiedEventArgs : ModifiedEventArgs
	{
		private int _position;
		private int _length;
		private int _linesAddedCount;
		private string _text;
		private bool _isUserChange;
		private int _markerChangedLine;

		private const string STRING_FORMAT = "ModificationTypeFlags\t:{0}\r\nPosition\t\t\t:{1}\r\nLength\t\t\t\t:{2}\r\nLinesAddedCount\t\t:{3}\r\nText\t\t\t\t:{4}\r\nIsUserChange\t\t\t:{5}\r\nMarkerChangeLine\t\t:{6}";

		/// <summary>
		/// Overriden.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(STRING_FORMAT, ModificationType, _position, _length, _linesAddedCount, _text, _isUserChange, _markerChangedLine) + Environment.NewLine + UndoRedoFlags.ToString();
		}

		/// <summary>
		/// Returns true if the change was a direct result of user interaction
		/// </summary>
		public bool IsUserChange
		{
			get
			{
				return _isUserChange;
			}
		}

		/// <summary>
		/// Returns the line # of where the marker change occured (if applicable)
		/// </summary>
		public int MarkerChangedLine
		{
			get
			{
				return _markerChangedLine;
			}
		}

		/// <summary>
		/// Returns the document position where the change occured
		/// </summary>
		public int Position
		{
			get
			{
				return _position;
			}
		}


		/// <summary>
		/// Returns the length of the change occured. 
		/// </summary>
		public int Length
		{
			get
			{
				return _length;
			}
		}

		/// <summary>
		/// Returns the # of lines added or removed as a result of the change
		/// </summary>
		public int LinesAddedCount
		{
			get
			{
				return _linesAddedCount;
			}
		}

		/// <summary>
		/// The affected text of the change
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}
		}

		/// <summary>
		/// Initializes a new instance of the TextModifiedEventArgs class.
		/// </summary>
		/// <param name="position">document position where the change occured</param>
		/// <param name="length">length of the change occured</param>
		/// <param name="linesAddedCount">the # of lines added or removed as a result of the change</param>
		/// <param name="text">affected text of the change</param>
		/// <param name="isUserChange">true if the change was a direct result of user interaction</param>
		/// <param name="markerChangedLine"> the line # of where the marker change occured (if applicable)</param>

		public TextModifiedEventArgs(int modificationType, bool isUserChange, int markerChangedLine, int position, int length, int linesAddedCount, string text) : base(modificationType)
		{
			_isUserChange			= isUserChange;
			_markerChangedLine		= markerChangedLine;
			_position				= position;
			_length					= length;
			_linesAddedCount		= linesAddedCount;
			_text					= text;
		}
	}
	#endregion

	#region UndoRedoFlags
	//	Used by TextModifiedEventArgs, StyeChangedEventArgs and FoldChangedEventArgs
	//	this provides a friendly wrapper around the SCNotification's modificationType
	//	flags having to do with Undo and Redo
	/// <summary>
	/// Contains Undo/Redo information, used by many of the events
	/// </summary>
	public struct UndoRedoFlags
	{
		/// <summary>
		/// Was this action the result of an undo action
		/// </summary>
		public bool IsUndo;
		/// <summary>
		/// Was this action the result of a redo action
		/// </summary>
		public bool IsRedo;
		/// <summary>
		/// Is this part of a multiple undo or redo
		/// </summary>
		public bool IsMultiStep;
		/// <summary>
		/// Is this the last step in an undi or redo
		/// </summary>
		public bool IsLastStep;
		/// <summary>
		/// Does this affect multiple lines
		/// </summary>
		public bool IsMultiLine;

		private const string STRING_FORMAT = "IsUndo\t\t\t\t:{0}\r\nIsRedo\t\t\t\t:{1}\r\nIsMultiStep\t\t\t:{2}\r\nIsLastStep\t\t\t:{3}\r\nIsMultiLine\t\t\t:{4}";


		/// <summary>
		/// Overriden
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(STRING_FORMAT, IsUndo, IsRedo, IsMultiStep, IsLastStep, IsMultiLine);
		}

		/// <summary>
		/// Initializes a new instance of the UndoRedoFlags structure.
		/// </summary>
		/// <param name="isUndo">Was this action the result of an undo action</param>
		/// <param name="isRedo">Was this action the result of a redo action</param>
		/// <param name="isMultiStep">Is this part of a multiple undo or redo</param>
		/// <param name="isLastStep">Is this the last step in an undi or redo</param>
		/// <param name="isMultiLine">Does this affect multiple lines</param>
		public UndoRedoFlags(int modificationType)
		{
			IsLastStep		= (modificationType & Constants.SC_LASTSTEPINUNDOREDO) > 0;
			IsMultiLine		= (modificationType & Constants.SC_MULTILINEUNDOREDO) > 0;
			IsMultiStep		= (modificationType & Constants.SC_MULTISTEPUNDOREDO) > 0;
			IsRedo			= (modificationType & Constants.SC_PERFORMED_REDO) > 0;
			IsUndo			= (modificationType & Constants.SC_PERFORMED_UNDO) > 0;
		}
	}
	#endregion

	#region UriDroppedEventArgs
	/// <summary>
	/// Provides data for the UriDropped event
	/// </summary>
	public class UriDroppedEventArgs : EventArgs
	{
		//	I decided to leave it a string because I can't really
		//	be sure it is a Uri.
		private string _uriText;
		/// <summary>
		/// Text of the dropped file or uri
		/// </summary>
		public string UriText
		{
			get { return _uriText; }
			set { _uriText = value; }
		}

		/// <summary>
		/// Initializes a new instance of the UriDroppedEventArgs class.
		/// </summary>
		/// <param name="uriText">Text of the dropped file or uri</param>
		public UriDroppedEventArgs(string uriText)
		{
			_uriText = uriText;
		}
	}
	#endregion

	#region AutoCompleteAcceptedEventArgs
	/// <summary>
	/// Provides data for the AutoCompleteAccepted event
	/// </summary>
	public class AutoCompleteAcceptedEventArgs : EventArgs
	{
		private string _text;

		/// <summary>
		/// Text of the selected autocomplete entry selected
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		private int _wordStartPosition;
		/// <summary>
		/// Returns the start position of the current word in the document.
		/// </summary>
		/// <remarks>
		/// This controls how many characters of the selected autocomplete entry
		/// is actually inserted into the document
		/// </remarks>
		public int WordStartPosition
		{
			get
			{
				return _wordStartPosition;
			}
		}

		private bool _cancel = false;
		/// <summary>
		/// Gets/Sets if the autocomplete action should be cancelled
		/// </summary>
		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the AutoCompleteAcceptedEventArgs class.
		/// </summary>
		/// <param name="text">Text of the selected autocomplete entry selected</param>
		public AutoCompleteAcceptedEventArgs(string text)
		{
			_text = text;
		}

		internal AutoCompleteAcceptedEventArgs(SCNotification eventSource, Encoding encoding)
		{
			_wordStartPosition = (int)eventSource.lParam;
			_text = Utilities.IntPtrToString(encoding, eventSource.text, eventSource.length);
		}
	} 
	#endregion

	#region MarginClickEventArgs
	/// <summary>
	/// Provides data for the MarginClick event
	/// </summary>
	public class MarginClickEventArgs : EventArgs
	{
		private Keys _modifiers;
		/// <summary>
		/// Returns any Modifier keys (shift, alt, ctrl) that were in use at the
		/// time the click event occured
		/// </summary>
		public Keys Modifiers
		{
			get
			{
				return _modifiers;
			}
		}

		private int _position;
		/// <summary>
		/// Returns the Document position of the line where the click occured
		/// </summary>
		public int Position
		{
			get
			{
				return _position;
			}
		}

		private Line _line;
		/// <summary>
		/// Returns the Document line # where the click occured
		/// </summary>
		public Line Line
		{
			get
			{
				return _line;
			}
		}

		private Margin _margin;
		/// <summary>
		/// Returns the Margin where the click occured
		/// </summary>
		public Margin Margin
		{
			get
			{
				return _margin;
			}
		}

		private int _toggleMarkerNumber;
		/// <summary>
		/// Gets/Sets the marker number that should be toggled in result of the click
		/// </summary>
		public int ToggleMarkerNumber
		{
			get
			{
				return _toggleMarkerNumber;
			}
			set
			{
				_toggleMarkerNumber = value;
			}
		}

		private bool _toggleFold;
		/// <summary>
		/// Gets/Sets whether the fold at the current line should be toggled
		/// </summary>
		public bool ToggleFold
		{
			get
			{
				return _toggleFold;
			}
			set
			{
				_toggleFold = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the MarginClickEventArgs class.
		/// </summary>
		/// <param name="modifiers">any Modifier keys (shift, alt, ctrl) that were in use at the
		/// time the click event occured</param>
		/// <param name="position">Document position of the line where the click occured</param>
		/// <param name="line">Document line # where the click occured</param>
		/// <param name="margin">Margin where the click occured</param>
		/// <param name="toggleMarkerNumber"> marker number that should be toggled in result of the click</param>
		/// <param name="toggleFold">Whether the fold at the current line should be toggled</param>
		public MarginClickEventArgs(Keys modifiers, int position, Line line, Margin margin, int toggleMarkerNumber, bool toggleFold)
		{
			_modifiers = modifiers;
			_position = position;
			_line = line;
			_margin = margin;
			_toggleMarkerNumber = toggleMarkerNumber;
			_toggleFold = toggleFold;
		}
	} 
	#endregion

	#region MacroRecordEventArgs
	/// <summary>
	/// Provides data for the MacroRecorded event
	/// </summary>
	public class MacroRecordEventArgs : EventArgs
	{
		private Message _recordedMessage;
		/// <summary>
		/// Returns the recorded window message that can be sent back to the native Scintilla window
		/// </summary>
		public Message RecordedMessage
		{
			get
			{
				return _recordedMessage;
			}
		}

		/// <summary>
		/// Initializes a new instance of the MacroRecordEventArgs class.
		/// </summary>
		/// <param name="recordedMessage">the recorded window message that can be sent back to the native Scintilla window</param>
		public MacroRecordEventArgs(Message recordedMessage)
		{
			_recordedMessage = recordedMessage;
		}

		/// <summary>
		/// Initializes a new instance of the MacroRecordEventArgs class.
		/// </summary>
		/// <param name="ea">NativeScintillaEventArgs object containing the message data</param>
		public MacroRecordEventArgs(NativeScintillaEventArgs ea)
		{
			_recordedMessage = ea.Msg;
			_recordedMessage.LParam = ea.SCNotification.lParam;
			_recordedMessage.WParam = ea.SCNotification.wParam;
		}
	} 
	#endregion

	public class FileDropEventArgs : EventArgs
	{
		private string[] _fileNames;
		public string[] FileNames
		{
			get
			{
				return _fileNames;
			}
		}

		/// <summary>
		/// Initializes a new instance of the FileDropEventArgs class.
		/// </summary>
		/// <param name="fileNames"></param>
		public FileDropEventArgs(string[] fileNames)
		{
			_fileNames = fileNames;
		}
	}
}