using System;
using System.Collections.Generic;
using System.Text;

namespace ScintillaNet
{
	/// <summary>
	/// Used internally to signify an ignored parameter by overloads of SendMessageDirect
	/// that match the native Scintilla's Message signatures.
	/// </summary>
	public enum VOID
	{
		NULL
	}

	/// <summary>
	/// The style of visual indicator that the caret displayes.
	/// </summary>
	public enum CaretStyle
	{
		/// <summary>
		/// The caret is not displayed
		/// </summary>
		Invisible = 0,
		/// <summary>
		/// A vertical line is displayed
		/// </summary>
		Line = 1,

		/// <summary>
		/// A horizontal block is displayed that may cover the character.
		/// </summary>
		Block = 2
	}

	/// <summary>
	/// Built in lexers supported by Scintilla
	/// </summary>
	public enum Lexer
	{
		/// <summary>
		/// No lexing is performed, the Containing application must respond to StyleNeeded events
		/// </summary>
		Container = 0,
		/// <summary>
		/// No lexing is performed
		/// </summary>
		Null = 1,
		Python = 2,
		Cpp = 3,
		Hypertext = 4,
		Xml = 5,
		Perl = 6,
		Sql = 7,
		VB = 8,
		Properties = 9,
		ErrorList = 10,
		MakeFile = 11,
		Batch = 12,
		XCode = 13,
		Latex = 14,
		Lua = 15,
		Diff = 16,
		Conf = 17,
		Pascal = 18,
		Ave = 19,
		Ada = 20,
		Lisp = 21,
		Ruby = 22,
		Eiffel = 23,
		EiffelKw = 24,
		Tcl = 25,
		NnCronTab = 26,
		Bullant = 27,
		VBScript = 28,
		Asp = 29,
		Php = 30,
		Baan = 31,
		MatLab = 32,
		Scriptol = 33,
		Asm = 34,
		CppNoCase = 35,
		Fortran = 36,
		F77 = 37,
		Css = 38,
		Pov = 39,
		Lout = 40,
		EScript = 41,
		Ps = 42,
		Nsis = 43,
		Mmixal = 44,
		Clw = 45,
		ClwNoCase = 46,
		Lot = 47,
		Yaml = 48,
		Tex = 49,
		MetaPost = 50,
		PowerBasic = 51,
		Forth = 52,
		ErLang = 53,
		Octave = 54,
		MsSql = 55,
		Verilog = 56,
		Kix = 57,
		Gui4Cli = 58,
		Specman = 59,
		Au3 = 60,
		Apdl = 61,
		Bash = 62,
		Asn1 = 63,
		Vhdl = 64,
		Caml = 65,
		BlitzBasic = 66,
		PureBasic = 67,
		Haskell = 68,
		PhpScript = 69,
		Tads3 = 70,
		Rebol = 71,
		Smalltalk = 72,
		Flagship = 73,
		CSound = 74,
		FreeBasic = 75,
		InnoSetup = 76,
		Opal = 77,
		Spice = 78,
		D = 79,
		CMake = 80,
		Gap = 81,
		Plm = 82,
		Progress = 83,
		Automatic = 1000
	}

	/// <summary>
	/// Specifies the display mode of whitespace characters.
	/// </summary>
	public enum WhitespaceMode
	{
		/// <summary>
		/// The normal display mode with whitespace displayed as an empty background color.
		/// </summary>
		Invisible = 0,

		/// <summary>
		/// Whitespace characters are drawn as dots and arrows.
		/// </summary>
		VisibleAlways = 1,

		/// <summary>
		/// Whitespace used for indentation is displayed normally but after the first visible character, it is shown as dots and arrows.
		/// </summary>
		VisibleAfterIndent = 2,
	}

	/// <summary>
	/// Document's EndOfLine Mode
	/// </summary>
	public enum EndOfLineMode
	{
		/// <summary>
		/// Carriage Return + Line Feed (Windows Style)
		/// </summary>
		Crlf=0,
		/// <summary>
		/// Carriage Return Only (Mac Style)
		/// </summary>
		CR=1,
		/// <summary>
		/// Line Feed Only (Unix Style)
		/// </summary>
		LF=2,
	}

	/// <summary>
	/// What graphic a margin marker is displayed.
	/// </summary>
	public enum MarkerSymbol
	{
		Circle=0,
		RoundRectangle=1,
		Arrow=2,
		SmallRect=3,
		ShortArrow=4,
		Empty=5,
		ArrowDown=6,
		Minus=7,
		Plus=8,
		VLine=9,
		LCorner=10,
		TCorner=11,
		BoxPlus=12,
		BoxPlusConnected=13,
		BoxMinus=14,
		BoxMinusConnected=15,
		LCornerCurve=16,
		TCornerCurve=17,
		CirclePlus=18,
		CirclePlusConnected=19,
		CircleMinus=20,
		CircleMinusConnected=21,
		Background=22,
		Ellipsis=23,
		Arrows=24,
		PixMap=25,
		FullRectangle=26,
		Character=10000,
	}


	public enum MarkerOutline
	{
		FolderEnd=25,
		FolderOpenMid=26,
		FolderMidTail=27,
		FolderTail=28,
		FolderSub=29,
		Folder=30,
		FolderOpen=31,
	}
	public enum MarginType
	{
		Symbol=0,
		Number=1,
		Back=2,
		Fore=3,
	}

	/// <summary>
	/// Common predefined styles that are always valid with any lexer.
	/// </summary>
	public enum StylesCommon
	{
		Default=32,
		LineNumber=33,
		BraceLight=34,
		BraceBad=35,
		ControlChar=36,
		IndentGuide=37,
		CallTip=38,
		LastPredefined=39,
		Max=127,
	}

	/// <summary>
	/// The CharacterSet used by the document
	/// </summary>
	public enum CharacterSet
	{
		Ansi=0,
		Default=1,
		Baltic=186,
		Chinesebig5=136,
		EastEurope=238,
		Gb2312=134,
		Greek=161,
		Hangul=129,
		Mac=77,
		Oem=255,
		Russian=204,
		Cyrillic=1251,
		ShiftJis=128,
		Symbol=2,
		Turkish=162,
		Johab=130,
		Hebrew=177,
		Arabic=178,
		Vietnamese=163,
		Thai=222,
		CharSet885915=1000,
	}

	/// <summary>
	/// Represents casing styles
	/// </summary>
	public enum StyleCase
	{
		/// <summary>
		/// Both upper and lower case
		/// </summary>
		Mixed=0,
		/// <summary>
		/// Only upper case
		/// </summary>
		Upper=1,
		/// <summary>
		/// Only lower case
		/// </summary>
		Lower=2,
	}

	/// <summary>
	/// Style of Indicator to be displayed
	/// </summary>
	public enum IndicatorStyle
	{
		/// <summary>
		/// Underline
		/// </summary>
		Plain=0,

		/// <summary>
		/// Squigly lines (commonly used for spellcheck)
		/// </summary>
		Squiggle=1,
		/// <summary>
		/// Small t's are displayed
		/// </summary>
		TT=2,
		/// <summary>
		/// Small diagnol lines
		/// </summary>
		Diagonal=3,
		/// <summary>
		/// Strikethrough line
		/// </summary>
		Strike=4,
		/// <summary>
		/// Hidden
		/// </summary>
		Hidden=5,
		/// <summary>
		/// Displayes a bounding box around the indicated text
		/// </summary>
		Box=6,
		/// <summary>
		/// Displayes a bounding box around the indicated text with rounded corners
		/// and an translucent background color
		/// </summary>
		RoundBox=7,
	}

	/// <summary>
	/// Controls color mode fore printing
	/// </summary>
	public enum PrintColorMode
	{
		/// <summary>
		/// Normal
		/// </summary>
		Normal=0,
		/// <summary>
		/// Inverts the colors
		/// </summary>
		InvertLight=1,
		/// <summary>
		/// Black Text on white background
		/// </summary>
		BlackOnWhite=2,

		/// <summary>
		/// Styled color text on white background
		/// </summary>
		ColorOnWhite=3,

		/// <summary>
		/// Styled color text on white background for unstyled background colors
		/// </summary>
		ColorOnWhiteDefaultBackground=4,
	}

	/// <summary>
	/// Controls find behavior for non-regular expression searches
	/// </summary>
	public enum FindOption
	{
		/// <summary>
		/// Find must match the whole word
		/// </summary>
		WholeWord=2,

		/// <summary>
		/// Find must match the case of the expression
		/// </summary>
		MatchCase=4,

		/// <summary>
		/// Only match the start of a word
		/// </summary>
		WordStart=0x00100000,

		/// <summary>
		/// Not used in ScintillaNet
		/// </summary>
		RegularExpression=0x00200000,
		
		/// <summary>
		/// Not used in ScintillaNet
		/// </summary>
		Posix=0x00400000,
	}

	public enum FoldLevel
	{
		Base=0x400,
		WhiteFlag=0x1000,
		HeaderFlag=0x2000,
		BoxHeaderFlag=0x4000,
		BoxFooterFlag=0x8000,
		Contracted=0x10000,
		Unindent=0x20000,
		NumberMask=0x0FFF,
	}

	[Flags]
	public enum FoldFlag
	{
		LineBeforeExpanded=0x0002,
		LineBeforeContracted=0x0004,
		LineAfterExpanded=0x0008,
		LineAfterContracted=0x0010,
		LevelNumbers=0x0040,
		Box=0x0001,
	}

	/// <summary>
	/// Controls how line wrapping occurs
	/// </summary>
	public enum WrapMode
	{
		/// <summary>
		/// No Wrapping
		/// </summary>
		None=0,

		/// <summary>
		/// Wraps at the nearest word
		/// </summary>
		Word=1,

		/// <summary>
		/// Wraps at the last character
		/// </summary>
		Char=2,
	}

	/// <summary>
	/// How wrap visual indicators are displayed
	/// </summary>
	[Flags]
	public enum WrapVisualFlag
	{
		/// <summary>
		/// No wrap indicators are displayed
		/// </summary>
		None=0x0000,
		/// <summary>
		/// Wrap indicators are displayed at the end of the line
		/// </summary>
		End=0x0001,
		/// <summary>
		/// Wrap indicators are displayed at the start of the line
		/// </summary>
		Start=0x0002,
	}

	[Flags]
	public enum WrapVisualLocation
	{
		Default=0x0000,
		EndByText=0x0001,
		StartByText=0x0002,
	}
	public enum LineCache
	{
		None=0,
		Caret=1,
		Page=2,
		Document=3,
	}

	/// <summary>
	/// How long lines are visually indicated
	/// </summary>
	public enum EdgeMode
	{
		/// <summary>
		/// No indication
		/// </summary>
		None=0,

		/// <summary>
		/// A vertical line is displayed
		/// </summary>
		Line=1,

		/// <summary>
		/// The background color changes
		/// </summary>
		Background=2,
	}
	public enum CursorShape
	{
		Normal=-1,
		Wait=4,
	}
	public enum CaretPolicy
	{
		Slop=0x01,
		Strict=0x04,
		Jumps=0x10,
		Even=0x08,
	}

	public enum SelectionMode
	{
		Stream=0,
		Rectangle=1,
		Lines=2,
	}
	public enum ModificationFlags
	{
		InsertText=0x1,
		DeleteText=0x2,
		ChangeStyle=0x4,
		ChangeFold=0x8,
		User=0x10,
		Undo=0x20,
		Redo=0x40,
		StepInUndoRedo=0x100,
		ChangeMarker=0x200,
		BeforeInsert=0x400,
		BeforeDelete=0x800,
	}
	//public enum Keys
	//{
	//    Down=300,
	//    Up=301,
	//    Left=302,
	//    Right=303,
	//    Home=304,
	//    End=305,
	//    Prior=306,
	//    Next=307,
	//    Delete=308,
	//    Insert=309,
	//    Escape=7,
	//    Back=8,
	//    Tab=9,
	//    Return=13,
	//    Add=310,
	//    Subtract=311,
	//    Divide=312,
	//}
	public enum KeyMod
	{
		Norm=0,
		Shift=1,
		Ctrl=2,
		Alt=4,
	}
	public enum Events : uint
	{
		StyleNeeded=2000,
		CharAdded=2001,
		SavePointReached=2002,
		SavePointLeft=2003,
		ModifyAttemptRO=2004,
		SCKey=2005,
		SCDoubleClick=2006,
		UpdateUI=2007,
		Modified=2008,
		MacroRecord=2009,
		MarginClick=2010,
		NeedShown=2011,
		Painted=2013,
		UserListSelection=2014,
		UriDropped=2015,
		DwellStart=2016,
		DwellEnd=2017,
		SCZoom=2018,
		HotspotClick=2019,
		HotspotDoubleClick=2020,
		CallTipClick=2021,
		AutoCSelection=2022,
	}

	public enum CopyFormat
	{
		Text,
		Rtf,
		Html
	}

	/// <summary>
	/// Represents an arrow in the CallTip
	/// </summary>
	public enum CallTipArrow
	{
		/// <summary>
		/// No arrow
		/// </summary>
		None = 0,
		/// <summary>
		/// The Up arrow
		/// </summary>
		Up = 1,
		/// <summary>
		/// The Down Arrow
		/// </summary>
		Down = 2
	}

	//	Next = 10025
	/// <summary>
	/// List of commands that ScintillaNet can execute. These can be
	/// bound to keyboard shortcuts
	/// </summary>
	public enum BindableCommand
	{
		AcceptActiveSnippets = 10006,
		AutoCCancel = 2101,
		AutoCComplete = 2104,
		AutoCShow = 10001,
		BackTab = 2328,
		BeginUndoAction = 2078,
		CallTipCancel = 2201,
		Cancel = 2325,
		CancelActiveSnippets = 10005,
		CharLeft = 2304,
		CharLeftExtend = 2305,
		CharLeftRectExtend = 2428,
		CharRight = 2306,
		CharRightExtend = 2307,
		CharRightRectExtend = 2429,
		ChooseCaretX = 2399,
		Clear = 2180,
		ClearAll = 2004,
		ClearAllCmdKeys = 2072,
		ClearDocumentStyle = 2005,
		ClearRegisteredImages = 2408,
		Copy = 2178,
		Cut = 2177,
		DeleteBack = 2326,
		DeleteBackNotLine = 2344,
		DelLineLeft = 2395,
		DelLineRight = 2396,
		DelWordLeft = 2335,
		DelWordRight = 2336,
		DocumentEnd = 2318,
		DocumentEndExtend = 2319,
		DocumentNavigateBackward = 10018,
		DocumentNavigateForward = 10019,
		DocumentStart = 2316,
		DocumentStartExtend = 2317,
		DoSnippetCheck = 10002,
		DropMarkerCollect = 10008,
		DropMarkerDrop = 10007,
		EditToggleOvertype = 2324,
		EmptyUndoBuffer = 2175,
		EndUndoAction = 2079,
		FindNext = 10013,
		FindPrevious = 10014,
		FormFeed = 2330,
		GrabFocus = 2400,
		Home = 2312,
		HomeDisplay = 2345,
		HomeDisplayExtend = 2346,
		HomeExtend = 2313,
		HomeRectExtend = 2430,
		HomeWrap = 2349,
		HomeWrapExtend = 2450,
		IncrementalSearch = 10015,
		LineCopy = 2455,
		LineComment = 10016,
		LineCut = 2337,
		LineDelete = 2338,
		LineDown = 2300,
		LineDownExtend = 2301,
		LineDownRectExtend = 2426,
		LineDuplicate = 2404,
		LineEnd = 2314,
		LineEndDisplay = 2347,
		LineEndDisplayExtend = 2348,
		LineEndExtend = 2315,
		LineEndRectExtend = 2432,
		LineEndWrap = 2451,
		LineEndWrapExtend = 2452,
		LineScrollDown = 2342,
		LineScrollUp = 2343,
		LinesJoin = 2288,
		LineTranspose = 2339,
		LineUncomment = 10017,
		LineUp = 2302,
		LineUpExtend = 2303,
		LineUpRectExtend = 2427,
		LowerCase = 2340,
		MoveCaretInsideView = 2401,
		NewLine = 2329,
		NextSnippetRange = 10003,
		Null = 2172,
		PageDown = 2322,
		PageDownExtend = 2323,
		PageDownRectExtend = 2434,
		PageUp = 2320,
		PageUpExtend = 2321,
		PageUpRectExtend = 2433,
		ParaDown = 2413,
		ParaDownExtend = 2414,
		ParaUp = 2415,
		ParaUpExtend = 2416,
		PreviousSnippetRange = 10004,
		Print = 10009,
		PrintPreview = 10010,
		Paste = 2179,
		Redo = 2011,
		ScrollCaret = 2169,
		SearchAnchor = 2366,
		SelectAll = 2013,
		SelectionDuplicate = 2469,
		SetCharsDefault = 2444,
		SetSavePoint = 2014,
		SetZoom = 2373,
		ShowFind = 10011,
		ShowReplace = 10012,
		ShowSnippetList = 10022,
		ShowSurroundWithList = 10023,
		ShowGoTo = 10024,
		StartRecord = 3001,
		StreamComment=10021,
		StopRecord = 3002,
		StutteredPageDown = 2437,
		StutteredPageDownExtend = 2438,
		StutteredPageUp = 2435,
		StutteredPageUpExtend = 2436,
		StyleClearAll = 2050,
		StyleResetDefault = 2058,
		Tab = 2327,
		TargetFromSelection = 2287,
		ToggleCaretSticky = 2459,
		ToggleLineComment = 10020,
		Undo = 2176,
		UpperCase = 2341,
		VCHome = 2331,
		VCHomeExtend = 2332,
		VCHomeRectExtend = 2431,
		VCHomeWrap = 2453,
		VCHomeWrapExtend = 2454,
		WordLeft = 2308,
		WordLeftEnd = 2439,
		WordLeftEndExtend = 2440,
		WordLeftExtend = 2309,
		WordPartLeft = 2390,
		WordPartLeftExtend = 2391,
		WordPartRight = 2392,
		WordPartRightExtend = 2393,
		WordRight = 2310,
		WordRightEnd = 2441,
		WordRightEndExtend = 2442,
		WordRightExtend = 2311,
		ZoomIn = 2333,
		ZoomOut = 2334,
	}

	public enum FoldMarkerScheme
	{
		PlusMinus,
		BoxPlusMinus,
		CirclePlusMinus,
		Arrow,
		Custom
	}

	/// <summary>
	/// Style of smart indent
	/// </summary>
	public enum SmartIndent
	{
		/// <summary>
		/// No smart indent
		/// </summary>
		None = 0,

		/// <summary>
		/// c++ style indenting
		/// </summary>
		CPP = 1,
		/// <summary>
		/// Alternate c++ style indenting
		/// </summary>
		CPP2 = 4,
		/// <summary>
		/// Block indenting, the last indentation is retained in new lines
		/// </summary>
		Simple = 2
	}
	
}
