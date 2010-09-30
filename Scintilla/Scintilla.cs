#region Using Directives

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing.Design;

using ScintillaNet.Configuration;

#endregion Using Directives


namespace ScintillaNet
{
	/// <summary>
	/// Represents a Scintilla text editor control.
	/// </summary>
	[Designer(typeof(ScintillaDesigner)), Docking(DockingBehavior.Ask)]
	[DefaultBindingProperty("Text"), DefaultProperty("Text"), DefaultEvent("DocumentChanged")]
	public partial class Scintilla : System.Windows.Forms.Control, INativeScintilla, ISupportInitialize
	{
		#region Constants

		public const string DefaultDllName = "SciLexer.dll";

		#endregion Constants

		#region Fields

		private static readonly int _modifiedState = BitVector32.CreateMask();
		private static readonly int _acceptsReturnState = BitVector32.CreateMask(_modifiedState);
		private static readonly int _acceptsTabState = BitVector32.CreateMask(_acceptsReturnState);
		private BitVector32 _state;
		private Whitespace _whitespace;

		#endregion Fields

		#region Property Bags
		private Dictionary<string, Color> _colorBag = new Dictionary<string, Color>();
		internal Dictionary<string, Color> ColorBag { get { return _colorBag; } }

		private Hashtable _propertyBag = new Hashtable();
		internal Hashtable PropertyBag { get { return _propertyBag; } }

		#endregion

		#region Constructor / Dispose

		public Scintilla()
		{
			this._state = new BitVector32(0);
			this._state[_acceptsReturnState] = true;
			this._state[_acceptsTabState] = true;

			_ns = (INativeScintilla)this;

			_textChangedTimer = new Timer();
			_textChangedTimer.Interval = 1;
			_textChangedTimer.Tick += new EventHandler(this.textChangedTimer_Tick);

			_caption = GetType().FullName;

			//	Set up default encoding to UTF-8 which is the Scintilla's best supported.
			//	.NET strings are UTF-16 but should be able to convert without any problems
			this.Encoding = Encoding.UTF8;

			//	Ensure all style values have at least defaults
			_ns.StyleClearAll();

			_caret = new CaretInfo(this);
			_lines = new LinesCollection(this);
			_selection = new Selection(this);
			_indicators = new IndicatorCollection(this);
			_snippets = new SnippetManager(this);
			_margins = new MarginCollection(this);
			_scrolling = new Scrolling(this);
			_whitespace = new Whitespace(this);
			_endOfLine = new EndOfLine(this);
			_clipboard = new Clipboard(this);
			_undoRedo = new UndoRedo(this);
			_dropMarkers = new DropMarkers(this);
			_hotspotStyle = new HotspotStyle(this);
			_callTip = new CallTip(this);
			_styles = new StyleCollection(this);
			_indentation = new Indentation(this);
			_markers = new MarkerCollection(this);
			_autoComplete = new AutoComplete(this);
			_documentHandler = new DocumentHandler(this);
			_lineWrap = new LineWrap(this);
			_lexing = new Lexing(this);
			_longLines = new LongLines(this);
			_commands = new Commands(this);
			_folding = new Folding(this);
			_configurationManager = new ConfigurationManager(this);
			_printing = new Printing(this);
			_findReplace = new FindReplace(this);
			_documentNavigation = new DocumentNavigation(this);
			_goto = new GoTo(this);


			_helpers.AddRange(new TopLevelHelper[] 
			{ 
				_caret, 
				_lines, 
				_selection,
				_indicators, 
				_snippets,
				_margins,
				_scrolling,
				_whitespace,
				_endOfLine,
				_clipboard,
				_undoRedo,
				_dropMarkers,
				_hotspotStyle,
				_styles,
				_indentation,
				_markers,
				_autoComplete,
				_documentHandler,
				_lineWrap,
				_lexing,
				_longLines,
				_commands,
				_folding,
				_configurationManager,
				_printing,
				_findReplace,
				_documentNavigation,
				_goto
			});


			//	Changing the Default values from Scintilla's default Black on White
			//	to platform defaults for Edits
			BackColor = SystemColors.Window;
			ForeColor = SystemColors.WindowText;
		}

		/// <summary>
		/// Overriden. See <see cref="Control.Dispose"/>.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			foreach (ScintillaHelperBase heler in _helpers)
			{
				heler.Dispose();
			}

			if (disposing && IsHandleCreated)
			{
				//	wi11811 2008-07-28 Chris Rickard
				//	Since we eat the destroy message in WndProc
				//	we have to manually let Scintilla know to
				//	clean up its resources.
				Message destroyMessage = new Message();
				destroyMessage.Msg = NativeMethods.WM_DESTROY;
				destroyMessage.HWnd = Handle;
				base.DefWndProc(ref destroyMessage);
			}

			base.Dispose(disposing);
		}
		#endregion

		#region Protected Control Overrides



		/// <summary>
		/// Overriden. See <see cref="Control.WndProc"/>.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			//	Hmm... this if/else construct is getting bigger and bigger every revision.
			//	Maybe I should be using switch like a *real* WndProc :)
		
			//	wi11811 2008-07-28 Chris Rickard
			//	If we get a destroy message we make this window a message-only window so that it doesn't actually
			//	get destroyed, causing Scintilla to wipe out all its settings associated with this window handle.
			//	We do send a WM_DESTROY message to Scintilla in the Dispose() method so that it does clean up its 
			//	resources when this control is actually done with. Credit (blame :) goes to tom103 for figuring
			//	this one out.
			if (m.Msg == NativeMethods.WM_DESTROY)
			{
				if (this.IsHandleCreated)
				{
					NativeMethods.SetParent(this.Handle, NativeMethods.HWND_MESSAGE);
					return;
				}
			}
			else if ((int)m.Msg == NativeMethods.WM_PAINT)
			{
				//	I tried toggling the ControlStyles.UserPaint flag and sending the message
				//	to both base.WndProc and DefWndProc in order to get the best of both worlds,
				//	Scintilla Paints as normal and .NET fires the Paint Event with the proper
				//	clipping regions and such. This didn't work too well, I kept getting weird
				//	phantom paints, or sometimes the .NET paint events would seem to get painted
				//	over by Scintilla. This technique I use below seems to work perfectly.

				base.WndProc(ref m);

				if (_isCustomPaintingEnabled)
				{
					RECT r;
					if (!NativeMethods.GetUpdateRect(Handle, out r, false))
						r = ClientRectangle;

					Graphics g = CreateGraphics();
					g.SetClip(r);

					OnPaint(new PaintEventArgs(CreateGraphics(), r));
				}
				return;
			}
			else if ((m.Msg) == NativeMethods.WM_DROPFILES)
			{
				handleFileDrop(m.WParam);
				return;
			}
            else if ((m.Msg) == NativeMethods.WM_SETCURSOR)
            {
                base.DefWndProc(ref m);
                return;
            }
			else if (m.Msg == NativeMethods.WM_GETTEXT)
			{
				m.WParam = (IntPtr)(Caption.Length + 1);
				Marshal.Copy(Caption.ToCharArray(), 0, m.LParam, Caption.Length);
				m.Result = (IntPtr)Caption.Length;
				return;
			}
			else if (m.Msg == NativeMethods.WM_GETTEXTLENGTH)
			{
				m.Result = (IntPtr)Caption.Length;
				return;
			}
			else if ((m.Msg ^ NativeMethods.WM_REFLECT) == NativeMethods.WM_NOTIFY)
			{
				//	OK Turns out the undocumented reflected message is a standard thing to do in MFC.
				//	But it isn't defined within the Win32 Platform SDK which is why I couldn't find 
				//	anything about it at the time.
				ReflectNotify(ref m);
				return;
			}
			else if ((int)m.Msg >= 10000)
			{
				_commands.Execute((BindableCommand)m.Msg);
				return;
			}

			if (m.Msg == NativeMethods.WM_HSCROLL || m.Msg == NativeMethods.WM_VSCROLL)
			{
				FireScroll(ref m);

				//	FireOnScroll calls WndProc so no need to call it again
				return;
			}

			base.WndProc(ref m);
			return;
		}

		private void ReflectNotify(ref Message m)
		{
			SCNotification scn = (SCNotification)Marshal.PtrToStructure(m.LParam, typeof(SCNotification));
			NativeScintillaEventArgs nsea = new NativeScintillaEventArgs(m, scn);

			switch (scn.nmhdr.code)
			{
				case Constants.SCN_AUTOCSELECTION:
					FireAutoCSelection(nsea);
					break;

				case Constants.SCN_CALLTIPCLICK:
					FireCallTipClick(nsea);
					break;

				case Constants.SCN_CHARADDED:
					FireCharAdded(nsea);
					break;

				case Constants.SCEN_CHANGE:
					FireChange(nsea);
					break;

				case Constants.SCN_DOUBLECLICK:
					FireDoubleClick(nsea);
					break;

				case Constants.SCN_DWELLEND:
					FireDwellEnd(nsea);
					break;

				case Constants.SCN_DWELLSTART:
					FireDwellStart(nsea);
					break;

				case Constants.SCN_HOTSPOTCLICK:
					FireHotSpotClick(nsea);
					break;

				case Constants.SCN_HOTSPOTDOUBLECLICK:
					FireHotSpotDoubleclick(nsea);
					break;

				case Constants.SCN_INDICATORCLICK:
					FireIndicatorClick(nsea);
					break;

				case Constants.SCN_INDICATORRELEASE:
					FireIndicatorRelease(nsea);
					break;

				case Constants.SCN_KEY:
					FireKey(nsea);
					break;

				case Constants.SCN_MACRORECORD:
					FireMacroRecord(nsea);
					break;

				case Constants.SCN_MARGINCLICK:
					FireMarginClick(nsea);
					break;

				case Constants.SCN_MODIFIED:
					FireModified(nsea);
					break;

				case Constants.SCN_MODIFYATTEMPTRO:
					FireModifyAttemptRO(nsea);
					break;

				case Constants.SCN_NEEDSHOWN:
					FireNeedShown(nsea);
					break;

				case Constants.SCN_PAINTED:
					FirePainted(nsea);
					break;

				case Constants.SCN_SAVEPOINTLEFT:
					FireSavePointLeft(nsea);
					break;

				case Constants.SCN_SAVEPOINTREACHED:
					FireSavePointReached(nsea);
					break;

				case Constants.SCN_STYLENEEDED:
					FireStyleNeeded(nsea);
					break;

				case Constants.SCN_UPDATEUI:
					FireUpdateUI(nsea);
					break;

				case Constants.SCN_URIDROPPED:
					FireUriDropped(nsea);
					break;

				case Constants.SCN_USERLISTSELECTION:
					FireUserListSelection(nsea);
					break;

				case Constants.SCN_ZOOM:
					FireZoom(nsea);
					break;

			}
		}
		private static bool _sciLexerLoaded = false;

		/// <summary>
		/// Overriden. See <see cref="Control.CreateParams"/>.
		/// </summary>
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				//	Otherwise Scintilla won't paint. When UserPaint is set to
				//	true the base Class (Control) eats the WM_PAINT message.
				//	Of course when this set to false we can't use the Paint
				//	events. This is why I'm relying on the Paint notification
				//	sent from scintilla to paint the Marker Arrows.
				SetStyle(ControlStyles.UserPaint, false);

				//	Registers the Scintilla Window Class
				//	I'm relying on the fact that a version specific renamed
				//	SciLexer exists either in the Current Dir or a global path
				//	(See LoadLibrary Windows API Search Rules)

				//	{wi15726} 2008-07-28 Chris Rickard 
				//	As milang pointed out there were some improvements to be made
				//	to this section of code. Now LoadLibrary is only called once
				//	per process (well, appdomain) and a better exception is thrown
				//	if it can't be loaded.
				//	Lastly I took out the whole concept of using an alternate name
				//	for SciLexer.dll. This is a breaking change but I don't think
				//	ANYONE has ever used this feature. If people complain I'll put
				//	it back but it completely avoids the weird behavoir described 
				//	in {wi15726}.
				//	Exception handling by jacobslusser
				if (!_sciLexerLoaded)
				{
					if (NativeMethods.LoadLibrary(DefaultDllName) == IntPtr.Zero)
					{
						int errorCode = Marshal.GetLastWin32Error();
						if (errorCode == NativeMethods.ERROR_MOD_NOT_FOUND)
						{
							// Couldn't find the SciLexer library. Provider a friendlier error message.
							string message = String.Format(
								CultureInfo.CurrentCulture,
								@"The Scintilla library could not be found. Please place the library " +
								@"in a searchable path such as the application or '{0}' directory.",
								Environment.SystemDirectory);

							throw new FileNotFoundException(message, new Win32Exception(errorCode));
						}

						throw new Win32Exception(errorCode);
					}
					else
					{
						_sciLexerLoaded = true;
					}
				}

				//	Tell Windows Forms to create a Scintilla
				//	derived Window Class for this control
				CreateParams cp = base.CreateParams;
				cp.ClassName = "Scintilla";

				return cp;
			}
		}


		/// <summary>
		/// Overriden. See <see cref="Control.IsInputKey"/>.
		/// </summary>
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Shift) != Keys.None)
				keyData ^= Keys.Shift;

			switch (keyData)
			{
				case Keys.Tab:
					return _state[_acceptsTabState];
				case Keys.Enter:
					return _state[_acceptsReturnState];
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
				case Keys.F:

					return true;
			}

			return base.IsInputKey(keyData);
		}

		/// <summary>
		/// Overriden. See <see cref="Control.OnKeyPress"/>.
		/// </summary>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (_supressControlCharacters && (int)e.KeyChar < 32)
				e.Handled = true;

			if (_snippets.IsEnabled && _snippets.IsOneKeySelectionEmbedEnabled && _selection.Length > 0)
			{
				Snippet s;
				if (_snippets.List.TryGetValue(e.KeyChar.ToString(), out s))
				{
					if (s.IsSurroundsWith)
					{
						_snippets.InsertSnippet(s);
						e.Handled = true;
					}
				}
			}

			base.OnKeyPress(e);
		}

		/// <summary>
		/// Overriden. See <see cref="Control.OnKeyDown"/>.
		/// </summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
				e.SuppressKeyPress = _commands.ProcessKey(e);
		}

		internal void FireKeyDown(KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		/// <summary>
		/// Overriden. See <see cref="Control.ProcessKeyMessage"/>.
		/// </summary>
		protected override bool ProcessKeyMessage(ref Message m)
		{
			//	For some reason IsInputKey isn't working for
			//	Key.Enter. This seems to make it work as expected
			if ((int)m.WParam == (int)Keys.Enter && !AcceptsReturn)
			{
				return true;
			}
			else
			{
				return base.ProcessKeyMessage(ref m);
			}
		}

		/// <summary>
		/// Overriden. See <see cref="Control.DefaultSize"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 100);
			}
		}

        /// <summary>Gets or sets the default cursor for the control.</summary>
        /// <returns>An object of type <see cref="T:System.Windows.Forms.Cursor"></see> representing the current default cursor.</returns>
        protected override Cursor DefaultCursor
        {
            get
            {
                return Cursors.IBeam;
            }
        }
		/// <summary>
		/// Overriden. See <see cref="Control.OnLostFocus"/>.
		/// </summary>
		protected override void OnLostFocus(EventArgs e)
		{
			if (Selection.HideSelection)
				_ns.HideSelection(true);

			_ns.SetSelBack(Selection.BackColorUnfocused != Color.Transparent, Utilities.ColorToRgb(Selection.BackColorUnfocused));
			_ns.SetSelFore(Selection.ForeColorUnfocused != Color.Transparent, Utilities.ColorToRgb(Selection.ForeColorUnfocused));

			base.OnLostFocus(e);
		}

		/// <summary>
		/// Overriden. See <see cref="Control.OnGotFocus"/>.
		/// </summary>
		protected override void OnGotFocus(EventArgs e)
		{
			if (!Selection.Hidden)
				_ns.HideSelection(false);

			_ns.SetSelBack(Selection.BackColor != Color.Transparent, Utilities.ColorToRgb(Selection.BackColor));
			_ns.SetSelFore(Selection.ForeColor != Color.Transparent, Utilities.ColorToRgb(Selection.ForeColor));

			base.OnGotFocus(e);
		}

		/// <summary>
		/// Provides the support for code block selection
		/// </summary>
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);

			if (_isBraceMatching)
			{
				int position = CurrentPos - 1,
					   bracePosStart = -1,
					   bracePosEnd = -1;

				char character = (char)CharAt(position);

				switch (character)
				{
					case '{':
					case '(':
					case '[':
						if (!this.PositionIsOnComment(position))
						{
							bracePosStart = position;
							bracePosEnd = _ns.BraceMatch(position, 0) + 1;
							_selection.Start = bracePosStart;
							_selection.End = bracePosEnd;
						}
						break;
				}
			}
		}

		/// <summary>
		/// Overriden. See <see cref="Control.OnCreateControl"/>.
		/// </summary>
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			OnLoad(EventArgs.Empty);
		}

		/// <summary>
		/// Overriden. See <see cref="Control.OnPaint"/>.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			paintRanges(e.Graphics);


		}
		#endregion

		#region Public Properties

		#region AcceptsReturn

		/// <summary>
		/// Gets or sets a value indicating whether pressing ENTER creates a new line of text in the
		/// control or activates the default button for the form.
		/// </summary>
		/// <returns><c>true</c> if the ENTER key creates a new line of text; <c>false</c> if the ENTER key activates
		/// the default button for the form. The default is <c>false</c>.</returns>
		[DefaultValue(true), Category("Behavior")]
		[Description("Indicates if return characters are accepted as text input.")]
		public bool AcceptsReturn
		{
			get { return _state[_acceptsReturnState]; }
			set { _state[_acceptsReturnState] = value; }
		}

		#endregion

		#region AcceptsTab

		/// <summary>
		/// Gets or sets a value indicating whether pressing the TAB key types a TAB character in the control
		/// instead of moving the focus to the next control in the tab order.
		/// </summary>
		/// <returns><c>true</c> if users can enter tabs using the TAB key; <c>false</c> if pressing the TAB key
		/// moves the focus. The default is <c>false</c>.</returns>
		[DefaultValue(true), Category("Behavior")]
		[Description("Indicates if tab characters are accepted as text input.")]
		public bool AcceptsTab
		{
			get { return _state[_acceptsTabState]; }
			set { _state[_acceptsTabState] = value; }
		}

		#endregion

		#region AllowDrop
		private bool _allowDrop;
		/// <summary>
		/// Gets or sets if .NET Drag and Drop operations are supported.
		/// </summary>
		public override bool AllowDrop
		{
			get
			{
				return _allowDrop;
			}
			set
			{
				NativeMethods.DragAcceptFiles(Handle, value);
				_allowDrop = value;
			}
		}
		#endregion

		#region AutoComplete
		private AutoComplete _autoComplete;
		/// <summary>
		/// Controls autocompletion behavior.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public AutoComplete AutoComplete
		{
			get
			{
				return _autoComplete;
			}
		}

		private bool ShouldSerializeAutoComplete()
		{
			return _autoComplete.ShouldSerialize();
		}
		#endregion

		#region BackColor

		/// <summary>
		/// Overriden. See <see cref="Control.BackColor"/>.
		/// </summary>
		[DefaultValue(typeof(Color), "Window")]
		public override Color BackColor
		{
			get
			{
				if (_colorBag.ContainsKey("BackColor"))
					return _colorBag["BackColor"];

				return SystemColors.Window;
			}

			set
			{
				Color currentColor = BackColor;

				if (value == SystemColors.Window)
					_colorBag.Remove("BackColor");
				else
					_colorBag["BackColor"] = value;



				if (_useBackColor)
				{
					for (int i = 0; i < 128; i++)
						if (i != (int)StylesCommon.LineNumber)
							Styles[i].BackColor = value;
				}



			}
		}

		#endregion

		#region BackgroundImage

		/// <summary>
		/// This property is not relevant for this class.
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>
		/// This property is not relevant for this class.
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		#endregion BackgroundImage

		#region CallTip
		private CallTip _callTip;
		/// <summary>
		/// Manages CallTip (Visual Studio-like code Tooltip) behaviors
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public CallTip CallTip
		{
			get
			{
				return _callTip;
			}
			set
			{
				_callTip = value;
			}
		}

		private bool ShouldSerializeCallTip()
		{
			return _callTip.ShouldSerialize();
		}
		#endregion

		#region Caption

		private string _caption;
		/// <summary>
		/// Gets/Sets the Win32 Window Caption. Defaults to Type's FullName
		/// </summary>
		[Category("Behavior")]
		[Description("Win32 Window Caption")]
		public string Caption
		{
			get { return _caption; }
			set
			{
				if (_caption != value)
				{
					_caption = value;

					//	Triggers a new WM_GETTEXT query
					base.Text = value;
				}

			}
		}

		private void ResetCaption()
		{
			Caption = GetType().FullName;
		}

		private bool ShouldSerializeCaption()
		{
			return Caption != GetType().FullName;
		}
		#endregion


		#region Caret
		private CaretInfo _caret;

		/// <summary>
		/// Controls Caret Behavior
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
		public CaretInfo Caret
		{
			get
			{
				return _caret;
			}
		}

		private bool ShouldSerializeCaret()
		{
			return _caret.ShouldSerialize();
		}

		#endregion

		#region Clipboard
		private Clipboard _clipboard;
		/// <summary>
		/// Controls Clipboard behavior.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public Clipboard Clipboard
		{
			get
			{
				return _clipboard;
			}
		}

		private bool ShouldSerializeClipboard()
		{
			return _clipboard.ShouldSerialize();
		}
		#endregion

		#region CurrentPos

		/// <summary>
		/// Gets or sets the character index of the current caret position.
		/// </summary>
		/// <returns>The character index of the current caret position.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentPos
		{
			get
			{
				return NativeInterface.GetCurrentPos();
			}
			set
			{
				NativeInterface.GotoPos(value);
			}
		}
		#endregion

		#region Commands
		private Commands _commands;
		/// <summary>
		/// Controls behavior of keyboard bound commands.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public Commands Commands
		{
			get
			{
				return _commands;
			}
			set
			{
				_commands = value;
			}
		}

		private bool ShouldSerializeCommands()
		{
			return _commands.ShouldSerialize();
		}
		#endregion

		#region ConfigurationManager
		private Configuration.ConfigurationManager _configurationManager;
		/// <summary>
		/// Controls behavior of loading/managing ScintillaNET configurations.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public Configuration.ConfigurationManager ConfigurationManager
		{
			get
			{
				return _configurationManager;
			}
			set
			{
				_configurationManager = value;
			}
		}

		private bool ShouldSerializeConfigurationManager()
		{
			return _configurationManager.ShouldSerialize();
		}
		#endregion

		#region DocumentHandler
		private DocumentHandler _documentHandler;
		/// <summary>
		/// Controls behavior of Documents
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DocumentHandler DocumentHandler
		{
			get
			{
				return _documentHandler;
			}
			set
			{
				_documentHandler = value;
			}
		}
		#endregion

		#region DocumentNavigation
		private DocumentNavigation _documentNavigation;
		/// <summary>
		/// Controls behavior of automatic document navigation
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public DocumentNavigation DocumentNavigation
		{
			get
			{
				return _documentNavigation;
			}
			set
			{
				_documentNavigation = value;
			}
		}

		private bool ShouldSerializeDocumentNavigation()
		{
			return _documentNavigation.ShouldSerialize();
		}

		#endregion

		#region DropMarkers
		private DropMarkers _dropMarkers;
		/// <summary>
		/// Controls behavior of Drop Markers
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public DropMarkers DropMarkers
		{
			get
			{
				return _dropMarkers;
			}
		}

		private bool ShouldSerializeDropMarkers()
		{
			return _dropMarkers.ShouldSerialize();
		}
		#endregion

		#region EndOfLine
		private EndOfLine _endOfLine;

		/// <summary>
		/// Controls End Of Line Behavior
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public EndOfLine EndOfLine
		{
			get
			{
				return _endOfLine;
			}
			set
			{
				_endOfLine = value;
			}
		}

		private bool ShouldSerializeEndOfLine()
		{
			return _endOfLine.ShouldSerialize();
		}

		#endregion

		#region Encoding

		//	List of Scintilla Supported encodings
		internal static readonly IList<Encoding> ValidCodePages = new Encoding[]
		{
			Encoding.ASCII,
			Encoding.UTF8,
			Encoding.Unicode,			//	UTF-16
			Encoding.GetEncoding(932),	//	shift_jis - Japanese (Shift-JIS) 
			Encoding.GetEncoding(936),	//	gb2312 - Chinese Simplified (GB2312)
			Encoding.GetEncoding(949),	//	ks_c_5601-1987  - Korean
			Encoding.GetEncoding(950),	//	big5 - Chinese Traditional (Big5) 
			Encoding.GetEncoding(1361)	//	Johab - Korean (Johab)
		};

		private Encoding _encoding;
		/// <summary>
		/// Controls Encoding behavior
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Encoding Encoding
		{
			get
			{
				return _encoding;
			}
			set
			{
				//	EncoderFallbackException isn't really the correct exception but 
				//	I'm being lazy and you get the point
				if (!ValidCodePages.Contains(value))
					throw new EncoderFallbackException("Scintilla only supports the following Encodings: " + ValidCodePages.ToString());

				_encoding = value;
				_ns.SetCodePage(_encoding.CodePage);
			}
		}
		#endregion

		#region FindReplace
		private FindReplace _findReplace;
		[Category("Behavior")]
		public FindReplace FindReplace
		{
			get
			{
				return _findReplace;
			}
			set
			{
				_findReplace = value;
			}
		}
		private bool ShouldSerializeFindReplace()
		{
			return _findReplace.ShouldSerialize();
		}

		#endregion

		#region Folding
		private Folding _folding;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public Folding Folding
		{
			get
			{
				return _folding;
			}
			set
			{
				_folding = value;
			}
		}

		private bool ShouldSerializeFolding()
		{
			return _folding.ShouldSerialize();
		}
		#endregion

		#region Font
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				if (value == null && Parent != null)
					value = Parent.Font;
				else if (value == null)
					value = Control.DefaultFont;

				Font currentFont = base.Font;

				if (_useFont)
				{
					for (int i = 0; i < 32; i++)
						if (i != (int)StylesCommon.CallTip)
							Styles[i].Font = value;
				}

				if (!base.Font.Equals(value))
					base.Font = value;
			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			//	We're doing this becuase when the Ambient font
			//	changes this method is called but the property
			//	setter for font isn't and we need to set the
			//	Default Style on Scintilla
			Font = Font;
			base.OnFontChanged(e);
		}

		public override void ResetFont()
		{
			if (Parent != null)
				Font = Parent.Font;
			else
				Font = Control.DefaultFont;
		}

		private bool ShouldSerializeFont()
		{
			return Parent == null || Font != Parent.Font;
		}
		#endregion

		#region ForeColor

		/// <summary>
		/// Overriden. See <see cref="Control.ForeColor"/>.
		/// </summary>
		[DefaultValue(typeof(Color), "WindowText")]
		public override Color ForeColor
		{
			get
			{
				if (_colorBag.ContainsKey("ForeColor"))
					return _colorBag["ForeColor"];

				return base.ForeColor;
			}
			set
			{
				Color currentForeColor = ForeColor;

				if (value == SystemColors.WindowText)
					_colorBag.Remove("ForeColor");
				else
					_colorBag["ForeColor"] = value;

				if (_useForeColor)
				{
					for (int i = 0; i < 32; i++)
						if (i != (int)StylesCommon.LineNumber)
							Styles[i].ForeColor = value;
				}

				base.ForeColor = value;
			}
		}


		protected override void OnForeColorChanged(EventArgs e)
		{
			ForeColor = base.ForeColor;
			base.OnForeColorChanged(e);
		}

		#endregion

		#region GoTo
		private GoTo _goto;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GoTo GoTo
		{
			get
			{
				return _goto;
			}
			set
			{
				_goto = value;
			}
		}
		#endregion

		#region HotspotStyle
		private HotspotStyle _hotspotStyle;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
		public HotspotStyle HotspotStyle
		{
			get
			{
				return _hotspotStyle;
			}
			set
			{
				_hotspotStyle = value;
			}
		}

		private bool ShouldSerializeHotspotStyle()
		{
			return _hotspotStyle.ShouldSerialize();
		}
		#endregion

		#region Indicators
		private IndicatorCollection _indicators;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IndicatorCollection Indicators
		{
			get { return _indicators; }
		}
		#endregion

		#region Indentation
		private Indentation _indentation;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public Indentation Indentation
		{
			get
			{
				return _indentation;
			}
			set
			{
				_indentation = value;
			}
		}

		private bool ShouldSerializeIndentation()
		{
			return _indentation.ShouldSerialize();
		}
		#endregion

		#region IsBraceMatching
		/// <summary>
		/// Enables the brace matching from current position.
		/// </summary>
		private bool _isBraceMatching = false;
		[DefaultValue(false), Category("Behavior")]
		public bool IsBraceMatching
		{
			get { return _isBraceMatching; }
			set { _isBraceMatching = value; }
		}



		/// <summary>
		/// Custom way to find the matching brace when BraceMatch() does not work
		/// </summary>
		internal int SafeBraceMatch(int position)
		{
			int match = this.CharAt(position);
			int toMatch = 0;
			int length = TextLength;
			int ch;
			int sub = 0;
			Lexer lexer = _lexing.Lexer;
			_lexing.Colorize(0, -1);
			bool comment = PositionIsOnComment(position, lexer);
			switch (match)
			{
				case '{':
					toMatch = '}';
					goto down;
				case '(':
					toMatch = ')';
					goto down;
				case '[':
					toMatch = ']';
					goto down;
				case '}':
					toMatch = '{';
					goto up;
				case ')':
					toMatch = '(';
					goto up;
				case ']':
					toMatch = '[';
					goto up;
			}
			return -1;
		// search up
		up:
			while (position >= 0)
			{
				position--;
				ch = CharAt(position);
				if (ch == match)
				{
					if (comment == PositionIsOnComment(position, lexer)) sub++;
				}
				else if (ch == toMatch && comment == PositionIsOnComment(position, lexer))
				{
					sub--;
					if (sub < 0) return position;
				}
			}
			return -1;
		// search down
		down:
			while (position < length)
			{
				position++;
				ch = CharAt(position);
				if (ch == match)
				{
					if (comment == PositionIsOnComment(position, lexer)) sub++;
				}
				else if (ch == toMatch && comment == PositionIsOnComment(position, lexer))
				{
					sub--;
					if (sub < 0) return position;
				}
			}
			return -1;
		}
		#endregion

		#region IsCustomPaintingEnabled
		private bool _isCustomPaintingEnabled = true;
		[DefaultValue(true), Category("Behavior")]
		public bool IsCustomPaintingEnabled
		{
			get
			{
				return _isCustomPaintingEnabled;
			}
			set
			{
				_isCustomPaintingEnabled = value;
			}
		}

		#endregion

		#region IsReadOnly
		[DefaultValue(false), Category("Behavior")]
		public bool IsReadOnly
		{
			get
			{
				return _ns.GetReadOnly();

			}
			set
			{
				_ns.SetReadOnly(value);
			}
		}
		#endregion

		#region Lexing
		private Lexing _lexing;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public Lexing Lexing
		{
			get
			{
				return _lexing;
			}
			set
			{
				_lexing = value;
			}
		}

		private bool ShouldSerializeLexing()
		{
			return _lexing.ShouldSerialize();
		}
		#endregion

		#region Lines
		private LinesCollection _lines;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LinesCollection Lines
		{
			get
			{

				return _lines;
			}
		}

		#endregion

		#region LineWrap
		private LineWrap _lineWrap;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public LineWrap LineWrap
		{
			get
			{
				return _lineWrap;
			}
			set
			{
				_lineWrap = value;
			}
		}

		private bool ShouldSerializeLineWrap()
		{
			return LineWrap.ShouldSerialize();
		}

		#endregion

		#region LongLines
		private LongLines _longLines;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public LongLines LongLines
		{
			get
			{
				return _longLines;
			}
			set
			{
				_longLines = value;
			}
		}

		private bool ShouldSerializeLongLines()
		{
			return _longLines.ShouldSerialize();
		}
		#endregion

		#region ManagedRanges
		private List<ManagedRange> _managedRanges = new List<ManagedRange>();
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<ManagedRange> ManagedRanges
		{
			get { return _managedRanges; }
		}
		#endregion

		#region Margins
		private MarginCollection _margins;
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
		public MarginCollection Margins
		{
			get
			{
				return _margins;
			}
		}

		private bool ShouldSerializeMargins()
		{
			return _margins.ShouldSerialize();
		}

		private void ResetMargins()
		{
			_margins.Reset();
		}
		#endregion

		#region Markers
		private MarkerCollection _markers;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public MarkerCollection Markers
		{
			get
			{
				return _markers;
			}
			set
			{
				_markers = value;
			}
		}

		private bool ShouldSerializeMarkers()
		{
			return _markers.ShouldSerialize();
		}
		#endregion

		#region MatchBraces
		private bool _matchBraces = true;
		[DefaultValue(true), Category("Behavior")]
		public bool MatchBraces
		{
			get
			{
				return _matchBraces;
			}
			set
			{
				_matchBraces = value;

				//	Clear any active Brace matching that may exist
				if (!value)
					_ns.BraceHighlight(-1, -1);
			}
		}

		#endregion

		#region Modified

		/// <summary>
		/// Gets or sets a value that indicates that the control has been modified by the user since
		/// the control was created or its contents were last set.
		/// </summary>
		/// <returns><c>true</c> if the control's contents have been modified; otherwise, <c>false</c>.
		/// The default is <c>false</c>.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Modified
		{
			get { return _state[_modifiedState]; }
			set
			{
				if (_state[_modifiedState] != value)
				{
					// Update the local (and native) state
					_state[_modifiedState] = value;
					if (!value)
						_ns.SetSavePoint();

					OnModifiedChanged(EventArgs.Empty);
				}
			}
		}

		#endregion

		#region MouseDownCaptures
		[DefaultValue(true), Category("Behavior")]
		public bool MouseDownCaptures
		{
			get
			{
				return NativeInterface.GetMouseDownCaptures();
			}
			set
			{
				NativeInterface.SetMouseDownCaptures(value);
			}
		}
		#endregion

		#region Native Interface
		private INativeScintilla _ns;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public INativeScintilla NativeInterface
		{
			get
			{
				return this as INativeScintilla;
			}
		}
		#endregion

		#region OverType

		[DefaultValue(false), Category("Behavior")]
		public bool OverType
		{
			get
			{
				return _ns.GetOvertype();
			}
			set
			{
				_ns.SetOvertype(value);
			}
		}
		#endregion

		#region Printing
		private Printing _printing;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Layout")]
		public Printing Printing
		{
			get
			{
				return _printing;
			}
			set
			{
				_printing = value;
			}
		}
		private bool ShouldSerializePrinting()
		{
			return _printing.ShouldSerialize();
		}
		#endregion

		#region RawText
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		unsafe public byte[] RawText
		{
			get
			{
				int length = NativeInterface.GetTextLength() + 1;

				//	May as well avoid all the crap below if we know what the outcome
				//	is going to be :)
				if (length == 1)
					return new byte[] { 0 };

				//  Allocate a buffer the size of the string + 1 for 
				//  the NULL terminator. Scintilla always sets this
				//  regardless of the encoding
				byte[] buffer = new byte[length];

				//  Get a direct pointer to the the head of the buffer
				//  to pass to the message along with the wParam. 
				//  Scintilla will fill the buffer with string data.
				fixed (byte* bp = buffer)
				{
					_ns.SendMessageDirect(Constants.SCI_GETTEXT, (IntPtr)length, (IntPtr)bp);
					return buffer;
				}
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					_ns.ClearAll();
				}
				else
				{
					//	This byte[] HAS to be NULL terminated or who knows how big 
					//	of an overrun we'll have on our hands
					if (value[value.Length - 1] != 0)
					{
						//	I hate to have to do this becuase it can be very inefficient.
						//	It can probably be done much better by the client app
						Array.Resize<byte>(ref value, value.Length + 1);
						value[value.Length - 1] = 0;
					}
					fixed (byte* bp = value)
						_ns.SendMessageDirect(Constants.SCI_SETTEXT, IntPtr.Zero, (IntPtr)bp);
				}
			}
		}
		#endregion

		#region Scrolling
		private Scrolling _scrolling;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Layout")]
		public Scrolling Scrolling
		{
			get
			{
				return _scrolling;
			}
			set
			{
				_scrolling = value;
			}
		}

		private bool ShouldSerializeScrolling()
		{
			return _scrolling.ShouldSerialize();
		}

		#endregion

		#region Selection
		private Selection _selection;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
		public Selection Selection
		{
			get
			{
				return _selection;
			}
		}

		private bool ShouldSerializeSelection()
		{
			return _selection.ShouldSerialize();
		}

		#endregion

		#region SearchFlags
		private SearchFlags _searchFlags = SearchFlags.Empty;
		[
		DefaultValue(SearchFlags.Empty), Category("Behavior"),
		Editor(typeof(Design.FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))
		]
		public SearchFlags SearchFlags
		{
			get
			{
				return _searchFlags;
			}
			set
			{
				_searchFlags = value;
			}
		}
		#endregion

		#region Snippets
		private SnippetManager _snippets;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public SnippetManager Snippets
		{
			get
			{
				return _snippets;
			}
		}

		private bool ShouldSerializeSnippets()
		{
			return _snippets.ShouldSerialize();
		}

		#endregion

		#region Styles
		private StyleCollection _styles;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
		public StyleCollection Styles
		{
			get
			{
				return _styles;
			}
			set
			{
				_styles = value;
			}
		}

		private bool ShouldSerializeStyles()
		{
			return _styles.ShouldSerialize();
		}
		#endregion

		#region SupressControlCharacters
		private bool _supressControlCharacters = true;

		/// <summary>
		/// Gets or sets a value indicating whether characters not considered alphanumeric (ASCII values 0 through 31)
		/// are prevented as text input.
		/// </summary>
		/// <returns><c>true</c> to prevent control characters as input; otherwise, <c>false</c>.
		/// The default is <c>true</c>.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SupressControlCharacters
		{
			get
			{
				return _supressControlCharacters;
			}
			set
			{
				_supressControlCharacters = value;
			}
		}
		#endregion

		#region Text

		/// <summary>
		/// Gets or sets the current text in the <see cref="Scintilla" /> control.
		/// </summary>
		/// <returns>The text displayed in the control.</returns>
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(UITypeEditor))]
		public override string Text
		{
			get
			{
				string s;
				_ns.GetText(_ns.GetLength() + 1, out s);
				return s;
			}

			set
			{
				if (string.IsNullOrEmpty(value))
					_ns.ClearAll();
				else
					_ns.SetText(value);
			}
		}
		#endregion

		#region TextLength

		/// <summary>
		/// Gets the length of text in the control.
		/// </summary>
		/// <returns>The number of characters contained in the text of the control.</returns>
		[Browsable(false)]
		public int TextLength
		{
			get
			{
				return NativeInterface.GetTextLength();
			}
		}
		#endregion

		#region UndoRedo
		private UndoRedo _undoRedo;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
		public UndoRedo UndoRedo
		{
			get
			{
				return _undoRedo;
			}
		}

		public bool ShouldSerializeUndoRedo()
		{
			return _undoRedo.ShouldSerialize();
		}
		#endregion

		#region UseForeColor
		private bool _useForeColor = false;
		[Category("Appearance"), DefaultValue(false)]
		public bool UseForeColor
		{
			get
			{
				return _useForeColor;
			}
			set
			{
				_useForeColor = value;

				if (value)
					ForeColor = ForeColor;
			}
		}

		#endregion

		#region UseFont
		private bool _useFont = false;
		[Category("Appearance"), DefaultValue(false)]
		public bool UseFont
		{
			get
			{
				return _useFont;
			}
			set
			{
				_useFont = value;

				if (value)
					Font = Font;
			}
		}
		#endregion

		#region UseBackColor
		private bool _useBackColor = false;
		[Category("Appearance"), DefaultValue(false)]
		public bool UseBackColor
		{
			get
			{
				return _useBackColor;
			}
			set
			{
				_useBackColor = value;
				if (value)
					BackColor = BackColor;
			}
		}
		#endregion

		#region UseWaitCursor
		public new bool UseWaitCursor
		{
			get
			{
				return base.UseWaitCursor;
			}
			set
			{
				base.UseWaitCursor = value;

				if (value)
					NativeInterface.SetCursor(Constants.SC_CURSORWAIT);
				else
					NativeInterface.SetCursor(Constants.SC_CURSORNORMAL);
			}
		}
		#endregion

		#region Whitespace

		/// <summary>
		/// Gets the <see cref="Whitespace"/> display mode and style behavior associated with the <see cref="Scintilla"/> control.
		/// </summary>
		/// <returns>A <see cref="Whitespace"/> object that represents whitespace display mode and style behavior in a <see cref="Scintilla"/> control.</returns>
		[Category("Appearance"), Description("The display mode and style of whitespace characters.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Whitespace Whitespace
		{
			get { return _whitespace; }
		}

		#endregion Whitespace

		#region Zoom

		/// <summary>
		/// Gets or sets the current zoom level of the <see cref="Scintilla" /> control.
		/// </summary>
		/// <returns>The factor by which the contents of the control is zoomed.</returns>
		[DefaultValue(0), Category("Appearance")]
		[Description("Defines the current scaling factor of the text display; 0 is normal viewing.")]
		public int Zoom
		{
			get
			{
				return _ns.GetZoom();
			}
			set
			{
				_ns.SetZoom(value);
			}
		}
		#endregion

		#endregion

		#region Private Methods

		private void handleFileDrop(IntPtr hDrop)
		{
			StringBuilder buffer = null;
			uint nfiles = NativeMethods.DragQueryFile(hDrop, 0xffffffff, buffer, 0);
			List<string> files = new List<string>();
			for (uint i = 0; i < nfiles; i++)
			{
				buffer = new StringBuilder(512);

				NativeMethods.DragQueryFile(hDrop, i, buffer, 512);
				files.Add(buffer.ToString());
			}
			NativeMethods.DragFinish(hDrop);

			OnFileDrop(new FileDropEventArgs(files.ToArray()));
		}


		private List<ManagedRange> managedRangesInRange(int firstPos, int lastPos)
		{
			//	TODO: look into optimizing this so that it isn't a linear
			//	search. This is fine for a few markers per document but
			//	can be greatly improved if there are a large # of markers
			List<ManagedRange> ret = new List<ManagedRange>();
			foreach (ManagedRange mr in _managedRanges)
				if (mr.Start >= firstPos && mr.Start <= lastPos)
					ret.Add(mr);

			return ret;
		}


		private void paintRanges(Graphics g)
		{
			//	First we want to get the range (in positions) of what
			//	will be painted so that we know which markers to paint
			int firstLine = _ns.GetFirstVisibleLine();
			int lastLine = firstLine + _ns.LinesOnScreen();
			int firstPos = _ns.PositionFromLine(firstLine);
			int lastPos = _ns.PositionFromLine(lastLine + 1) - 1;

			//	If the lastLine was outside the defined document range it will
			//	contain -1, defualt it to the last doc position
			if (lastPos < 0)
				lastPos = _ns.GetLength();

			List<ManagedRange> mrs = managedRangesInRange(firstPos, lastPos);


			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			foreach (ManagedRange mr in mrs)
			{
				mr.Paint(g);
			}
		}

		#endregion

		#region Events
		private static readonly object _loadEventKey = new object();
		private static readonly object _textInsertedEventKey = new object();
		private static readonly object _textDeletedEventKey = new object();
		private static readonly object _beforeTextInsertEventKey = new object();
		private static readonly object _beforeTextDeleteEventKey = new object();
		private static readonly object _documentChangeEventKey = new object();
		private static readonly object _foldChangedEventKey = new object();
		private static readonly object _markerChangedEventKey = new object();
		private static readonly object _styleNeededEventKey = new object();
		private static readonly object _charAddedEventKey = new object();
		private static readonly object _modifiedChangedEventKey = new object();
		private static readonly object _readOnlyModifyAttemptEventKey = new object();
		private static readonly object _selectionChangedEventKey = new object();
		private static readonly object _linesNeedShownEventKey = new object();
		private static readonly object _uriDroppedEventKey = new object();
		private static readonly object _dwellStartEventKey = new object();
		private static readonly object _dwellEndEventKey = new object();
		private static readonly object _zoomChangedEventKey = new object();
		private static readonly object _hotspotClickedEventKey = new object();
		private static readonly object _hotspotDoubleClickedEventKey = new object();
		private static readonly object _dropMarkerCollectEventKey = new object();
		private static readonly object _callTipClickEventKey = new object();
		private static readonly object _autoCompleteAcceptedEventKey = new object();
		private static readonly object _marginClickEventKey = new object();
		private static readonly object _indicatorClickEventKey = new object();
		private static readonly object _scrollEventKey = new object();
		private static readonly object _macroRecordEventKey = new object();
		private static readonly object _userListEventKey = new object();
		private static readonly object _fileDropEventKey = new object();
		private static readonly object _textChangedKey = new object();


		#region Load

		/// <summary>
		/// Occurs when the control is first loaded.
		/// </summary>
		[Category("Behavior"), Description("Occurs when the control is first loaded.")]
		public event EventHandler Load
		{
			add { Events.AddHandler(_loadEventKey, value); }
			remove { Events.RemoveHandler(_loadEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="Load"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnLoad(EventArgs e)
		{
			EventHandler handler = Events[_loadEventKey] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region TextChanged
		/// <summary>
		/// Occurs when the text or styling of the document changes or is about to change.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the text has changed.")]
		public new event EventHandler<EventArgs> TextChanged
		{
			add { Events.AddHandler(_textChangedKey, value); }
			remove { Events.RemoveHandler(_textChangedKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="TextChanged"/> event.
		/// </summary>
		/// <param name="e">Empty</param>
		protected virtual void OnTextChanged()
		{
			EventHandler<EventArgs> handler = Events[_textChangedKey] as EventHandler<EventArgs>;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}
		#endregion

		#region DocumentChange

		/// <summary>
		/// Occurs when the text or styling of the document changes or is about to change.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the text or styling of the document changes or is about to change.")]
		public event EventHandler<NativeScintillaEventArgs> DocumentChange
		{
			add { Events.AddHandler(_documentChangeEventKey, value); }
			remove { Events.RemoveHandler(_documentChangeEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="DocumentChange"/> event.
		/// </summary>
		/// <param name="e">An <see cref="NativeScintillaEventArgs"/> that contains the event data.</param>
		protected virtual void OnDocumentChange(NativeScintillaEventArgs e)
		{
			EventHandler<NativeScintillaEventArgs> handler = Events[_documentChangeEventKey] as EventHandler<NativeScintillaEventArgs>;
			if (handler != null)
				handler(this, e);
		}
		#endregion

		#region CallTipClick

		/// <summary>
		/// Occurs when a user clicks on a call tip.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a user clicks on a call tip.")]
		public event EventHandler<CallTipClickEventArgs> CallTipClick
		{
			add { Events.AddHandler(_callTipClickEventKey, value); }
			remove { Events.RemoveHandler(_callTipClickEventKey, value); }
		}

		internal void FireCallTipClick(int arrow)
		{
			CallTipArrow a = (CallTipArrow)arrow;
			OverloadList ol = CallTip.OverloadList;
			CallTipClickEventArgs e;

			if (ol == null)
			{
				e = new CallTipClickEventArgs(a, -1, -1, null, CallTip.HighlightStart, CallTip.HighlightEnd);
			}
			else
			{
				int newIndex = ol.CurrentIndex;

				if (a == CallTipArrow.Down)
				{
					if (ol.CurrentIndex == ol.Count - 1)
						newIndex = 0;
					else
						newIndex++;
				}
				else if (a == CallTipArrow.Up)
				{
					if (ol.CurrentIndex == 0)
						newIndex = ol.Count - 1;
					else
						newIndex--;
				}

				e = new CallTipClickEventArgs(a, ol.CurrentIndex, newIndex, ol, CallTip.HighlightStart, CallTip.HighlightEnd);
			}

			OnCallTipClick(e);

			if (e.Cancel)
			{
				CallTip.Cancel();
			}
			else
			{
				if (ol != null)
				{
					//	We allow them to alse replace the list entirely or just
					//	manipulate the New Index
					CallTip.OverloadList = e.OverloadList;
					CallTip.OverloadList.CurrentIndex = e.NewIndex;
					CallTip.ShowOverloadInternal();
				}

				CallTip.HighlightStart = e.HighlightStart;
				CallTip.HighlightEnd = e.HighlightEnd;
			}
		}


		/// <summary>
		/// Raises the <see cref="CallTipClick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="CallTipClickEventArgs"/> that contains the event data.</param>
		protected virtual void OnCallTipClick(CallTipClickEventArgs e)
		{
			EventHandler<CallTipClickEventArgs> handler = Events[_callTipClickEventKey] as EventHandler<CallTipClickEventArgs>;
			if (handler != null)
				handler(this, e);
		}
		#endregion

		#region AutoCompleteAccepted

		/// <summary>
		/// Occurs when the user makes a selection from the auto-complete list.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the user makes a selection from the auto-complete list.")]
		public event EventHandler<AutoCompleteAcceptedEventArgs> AutoCompleteAccepted
		{
			add { Events.AddHandler(_autoCompleteAcceptedEventKey, value); }
			remove { Events.RemoveHandler(_autoCompleteAcceptedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="AutoCompleteAccepted"/> event.
		/// </summary>
		/// <param name="e">An <see cref="AutoCompleteAcceptedEventArgs"/> that contains the event data.</param>
		protected virtual void OnAutoCompleteAccepted(AutoCompleteAcceptedEventArgs e)
		{
			EventHandler<AutoCompleteAcceptedEventArgs> handler = Events[_autoCompleteAcceptedEventKey] as EventHandler<AutoCompleteAcceptedEventArgs>;
			if (handler != null)
				handler(this, e);

			if (e.Cancel)
				AutoComplete.Cancel();
		}

		#endregion

		#region TextInserted

		/// <summary>
		/// Occurs when text has been inserted into the document.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when text has been inserted into the document.")]
		public event EventHandler<TextModifiedEventArgs> TextInserted
		{
			add { Events.AddHandler(_textInsertedEventKey, value); }
			remove { Events.RemoveHandler(_textInsertedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="TextInserted"/> event.
		/// </summary>
		/// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
		protected virtual void OnTextInserted(TextModifiedEventArgs e)
		{
			EventHandler<TextModifiedEventArgs> handler = Events[_textInsertedEventKey] as EventHandler<TextModifiedEventArgs>;
			if (handler != null)
				handler(this, e);
		}
		#endregion

		#region TextDeleted

		/// <summary>
		/// Occurs when text has been removed from the document.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when text has been removed from the document.")]
		public event EventHandler<TextModifiedEventArgs> TextDeleted
		{
			add { Events.AddHandler(_textDeletedEventKey, value); }
			remove { Events.RemoveHandler(_textDeletedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="TextDeleted"/> event.
		/// </summary>
		/// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
		protected virtual void OnTextDeleted(TextModifiedEventArgs e)
		{
			EventHandler<TextModifiedEventArgs> handler = Events[_textDeletedEventKey] as EventHandler<TextModifiedEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region BeforeTextInsert

		/// <summary>
		/// Occurs when text is about to be inserted into the document.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when text is about to be inserted into the document.")]
		public event EventHandler<TextModifiedEventArgs> BeforeTextInsert
		{
			add { Events.AddHandler(_beforeTextInsertEventKey, value); }
			remove { Events.RemoveHandler(_beforeTextInsertEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="BeforeTextInsert"/> event.
		/// </summary>
		/// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
		protected virtual void OnBeforeTextInsert(TextModifiedEventArgs e)
		{
			List<ManagedRange> offsetRanges = new List<ManagedRange>();
			foreach (ManagedRange mr in _managedRanges)
			{
				if (mr.Start == e.Position && mr.PendingDeletion)
				{
					mr.PendingDeletion = false;
					ManagedRange lmr = mr;
					BeginInvoke(new MethodInvoker(delegate() { lmr.Change(e.Position, e.Position + e.Length); }));
				}

				//	If the Range is a single point we treat it slightly 
				//	differently than a spanned range
				if (mr.IsPoint)
				{
					//	Unlike a spanned range, if the insertion point of
					//	the new text == the start of the range (and thus
					//	the end as well) we offset the entire point.
					if (mr.Start >= e.Position)
						mr.Change(mr.Start + e.Length, mr.End + e.Length);
					else if (mr.End >= e.Position)
						mr.Change(mr.Start, mr.End + e.Length);
				}
				else
				{
					//	We offset a spanned range entirely only if the
					//	start occurs after the insertion point of the new
					//	text.
					if (mr.Start > e.Position)
						mr.Change(mr.Start + e.Length, mr.End + e.Length);
					else if (mr.End >= e.Position)
					{
						//	However it the start of the range == the insertion
						//	point of the new text instead of offestting the 
						//	range we expand it.
						mr.Change(mr.Start, mr.End + e.Length);
					}
				}

			}

			EventHandler<TextModifiedEventArgs> handler = Events[_beforeTextInsertEventKey] as EventHandler<TextModifiedEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region BeforeTextDelete

		/// <summary>
		/// Occurs when text is about to be removed from the document.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when text is about to be removed from the document.")]
		public event EventHandler<TextModifiedEventArgs> BeforeTextDelete
		{
			add { Events.AddHandler(_beforeTextDeleteEventKey, value); }
			remove { Events.RemoveHandler(_beforeTextDeleteEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="BeforeTextDelete"/> event.
		/// </summary>
		/// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
		protected virtual void OnBeforeTextDelete(TextModifiedEventArgs e)
		{
			int firstPos = e.Position;
			int lastPos = firstPos + e.Length;

			List<ManagedRange> deletedRanges = new List<ManagedRange>();
			foreach (ManagedRange mr in _managedRanges)
			{

				//	These ranges lie within the deleted range so
				//	the ranges themselves need to be deleted
				if (mr.Start >= firstPos && mr.End <= lastPos)
				{

					//	If the entire range is being delete and NOT a superset of the range,
					//	don't delete it, only collapse it.
					if (!mr.IsPoint && e.Position == mr.Start && (e.Position + e.Length == mr.End))
					{
						mr.Change(mr.Start, mr.Start);
					}
					else
					{
						//	Notify the virtual Range that it needs to cleanup
						mr.Change(-1, -1);

						//	Mark for deletion after this foreach:
						deletedRanges.Add(mr);

					}
				}
				else if (mr.Start >= lastPos)
				{
					//	These ranges are merely offset by the deleted range
					mr.Change(mr.Start - e.Length, mr.End - e.Length);
				}
				else if (mr.Start >= firstPos && mr.Start <= lastPos)
				{
					//	The left side of the managed range is getting
					//	cut off
					mr.Change(firstPos, mr.End - e.Length);
				}
				else if (mr.Start < firstPos && mr.End >= firstPos && mr.End >= lastPos)
				{
					mr.Change(mr.Start, mr.End - e.Length);
				}
				else if (mr.Start < firstPos && mr.End >= firstPos && mr.End < lastPos)
				{
					mr.Change(mr.Start, firstPos);
				}

			}

			foreach (ManagedRange mr in deletedRanges)
				mr.Dispose();

			EventHandler<TextModifiedEventArgs> handler = Events[_beforeTextDeleteEventKey] as EventHandler<TextModifiedEventArgs>;
			if (handler != null)
				handler(this, e);
		}
		#endregion

		#region FoldChanged

		/// <summary>
		/// Occurs when a folding change has occurred.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a folding change has occurred.")]
		public event EventHandler<FoldChangedEventArgs> FoldChanged
		{
			add { Events.AddHandler(_foldChangedEventKey, value); }
			remove { Events.RemoveHandler(_foldChangedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="FoldChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="FoldChangedEventArgs"/> that contains the event data.</param>
		protected virtual void OnFoldChanged(FoldChangedEventArgs e)
		{
			EventHandler<FoldChangedEventArgs> handler = Events[_foldChangedEventKey] as EventHandler<FoldChangedEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region MarkerChanged

		/// <summary>
		/// Occurs when one or more markers has changed in a line of text.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when one or more markers has changed in a line of text.")]
		public event EventHandler<MarkerChangedEventArgs> MarkerChanged
		{
			add { Events.AddHandler(_markerChangedEventKey, value); }
			remove { Events.RemoveHandler(_markerChangedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="MarkerChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="MarkerChangedEventArgs"/> that contains the event data.</param>
		protected virtual void OnMarkerChanged(MarkerChangedEventArgs e)
		{
			EventHandler<MarkerChangedEventArgs> handler = Events[_markerChangedEventKey] as EventHandler<MarkerChangedEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region IndicatorClick

		/// <summary>
		/// Occurs when the a clicks or releases the mouse on text that has an indicator.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the a clicks or releases the mouse on text that has an indicator.")]
		public event EventHandler<ScintillaMouseEventArgs> IndicatorClick
		{
			add { Events.AddHandler(_indicatorClickEventKey, value); }
			remove { Events.RemoveHandler(_indicatorClickEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="IndicatorClick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
		protected virtual void OnIndicatorClick(ScintillaMouseEventArgs e)
		{
			EventHandler<ScintillaMouseEventArgs> handler = Events[_indicatorClickEventKey] as EventHandler<ScintillaMouseEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region MarginClick

		/// <summary>
		/// Occurs when the mouse was clicked inside a margin that was marked as sensitive.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the mouse was clicked inside a margin that was marked as sensitive.")]
		public event EventHandler<MarginClickEventArgs> MarginClick
		{
			add { Events.AddHandler(_marginClickEventKey, value); }
			remove { Events.RemoveHandler(_marginClickEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="MarginClick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="MarginClickEventArgs"/> that contains the event data.</param>
		protected virtual void OnMarginClick(MarginClickEventArgs e)
		{
			EventHandler<MarginClickEventArgs> handler = Events[_marginClickEventKey] as EventHandler<MarginClickEventArgs>;
			if (handler != null)
				handler(this, e);

			if (e.ToggleMarkerNumber >= 0)
			{
				int mask = (int)Math.Pow(2, e.ToggleMarkerNumber);
				if ((e.Line.GetMarkerMask() & mask) == mask)
					e.Line.DeleteMarker(e.ToggleMarkerNumber);
				else
					e.Line.AddMarker(e.ToggleMarkerNumber);
			}

			if (e.ToggleFold)
				e.Line.ToggleFoldExpanded();
		}

		internal void FireMarginClick(SCNotification n)
		{
			Margin m = Margins[n.margin];
			Keys k = Keys.None;

			if ((n.modifiers & (int)KeyMod.Alt) == (int)KeyMod.Alt)
				k |= Keys.Alt;

			if ((n.modifiers & (int)KeyMod.Ctrl) == (int)KeyMod.Ctrl)
				k |= Keys.Control;

			if ((n.modifiers & (int)KeyMod.Shift) == (int)KeyMod.Shift)
				k |= Keys.Shift;

			OnMarginClick(new MarginClickEventArgs(k, n.position, Lines.FromPosition(n.position), m, m.AutoToggleMarkerNumber, m.IsFoldMargin));
		}

		#endregion

		#region StyleNeeded

		/// <summary>
		/// Occurs when the control is about to display or print text that requires styling.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the control is about to display or print text that requires styling.")]
		public event EventHandler<StyleNeededEventArgs> StyleNeeded
		{
			add { Events.AddHandler(_styleNeededEventKey, value); }
			remove { Events.RemoveHandler(_styleNeededEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="StyleNeeded"/> event.
		/// </summary>
		/// <param name="e">An <see cref="StyleNeededEventArgs"/> that contains the event data.</param>
		protected virtual void OnStyleNeeded(StyleNeededEventArgs e)
		{
			EventHandler<StyleNeededEventArgs> handler = Events[_styleNeededEventKey] as EventHandler<StyleNeededEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region CharAdded

		/// <summary>
		/// Occurs when the user types an ordinary text character (as opposed to a command character) into the text.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the user types a text character.")]
		public event EventHandler<CharAddedEventArgs> CharAdded
		{
			add { Events.AddHandler(_charAddedEventKey, value); }
			remove { Events.RemoveHandler(_charAddedEventKey, value); }
		}


		/// <summary>
		/// Raises the <see cref="CharAdded"/> event.
		/// </summary>
		/// <param name="e">An <see cref="CharAddedEventArgs"/> that contains the event data.</param>
		protected virtual void OnCharAdded(CharAddedEventArgs e)
		{
			EventHandler<CharAddedEventArgs> handler = Events[_charAddedEventKey] as EventHandler<CharAddedEventArgs>;
			if (handler != null)
				handler(this, e);

			if (_indentation.SmartIndentType != SmartIndent.None)
				_indentation.CheckSmartIndent(e.Ch);
		}
		#endregion

		#region ModifiedChanged

		/// <summary>
		/// Occurs when the value of the <see cref="Modified"> property has changed.
		/// </summary>
		[Category("Property Changed"), Description("Occurs when the value of the Modified property changes.")]
		public event EventHandler ModifiedChanged
		{
			add { Events.AddHandler(_modifiedChangedEventKey, value); }
			remove { Events.RemoveHandler(_modifiedChangedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="ModifiedChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnModifiedChanged(EventArgs e)
		{
			EventHandler handler = Events[_modifiedChangedEventKey] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion ModifiedChanged

		#region ReadOnlyModifyAttempt

		/// <summary>
		/// Occurs when a user tries to modify text when in read-only mode.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a user tries to modifiy text when in read-only mode.")]
		public event EventHandler ReadOnlyModifyAttempt
		{
			add { Events.AddHandler(_readOnlyModifyAttemptEventKey, value); }
			remove { Events.RemoveHandler(_readOnlyModifyAttemptEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="ReadOnlyModifyAttempt"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnReadOnlyModifyAttempt(EventArgs e)
		{
			EventHandler handler = Events[_readOnlyModifyAttemptEventKey] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region SelectionChanged

		/// <summary>
		/// Occurs when the selection has changed.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the selection has changed.")]
		public event EventHandler SelectionChanged
		{
			add { Events.AddHandler(_selectionChangedEventKey, value); }
			remove { Events.RemoveHandler(_selectionChangedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="SelectionChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnSelectionChanged(EventArgs e)
		{	//this is being fired in tandem with the cursor blink...
			EventHandler handler = Events[_selectionChangedEventKey] as EventHandler;
			if (handler != null)
				handler(this, e);

			if (_isBraceMatching && (_selection.Length == 0))
			{
				int position = CurrentPos - 1,
					bracePosStart = -1,
					bracePosEnd = -1;

				char character = (char)CharAt(position);

				switch (character)
				{
					case '{':
					case '}':
					case '(':
					case ')':
					case '[':
					case ']':
						if (!this.PositionIsOnComment(position))
						{
							bracePosStart = position;
							bracePosEnd = _ns.BraceMatch(position, 0);
						}
						break;
					default:
						position = CurrentPos;
						character = (char)CharAt(position); //this is not being used anywhere... --Cory
						break;
				}

				_ns.BraceHighlight(bracePosStart, bracePosEnd);
			}
		}

		#endregion

		#region LinesNeedShown

		/// <summary>
		/// Occurs when a range of lines that is currently invisible should be made visible.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a range of lines that is currently invisible should be made visible.")]
		public event EventHandler<LinesNeedShownEventArgs> LinesNeedShown
		{
			add { Events.AddHandler(_linesNeedShownEventKey, value); }
			remove { Events.RemoveHandler(_linesNeedShownEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="LinesNeedShown"/> event.
		/// </summary>
		/// <param name="e">An <see cref="LinesNeedShownEventArgs"/> that contains the event data.</param>
		protected virtual void OnLinesNeedShown(LinesNeedShownEventArgs e)
		{
			EventHandler<LinesNeedShownEventArgs> handler = Events[_linesNeedShownEventKey] as EventHandler<LinesNeedShownEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region UriDropped

		//[Category("Scintilla")]
		//public event EventHandler<UriDroppedEventArgs> UriDropped
		//{
		//    add { Events.AddHandler(_uriDroppedEventKey, value); }
		//    remove { Events.RemoveHandler(_uriDroppedEventKey, value); }
		//}

		//protected virtual void OnUriDropped(UriDroppedEventArgs e)
		//{
		//    EventHandler<UriDroppedEventArgs> handler = Events[_uriDroppedEventKey] as EventHandler<UriDroppedEventArgs>;
		//    if (handler != null)
		//        handler(this, e);
		//}

		#endregion

		#region DwellStart

		/// <summary>
		/// Occurs when the user hovers the mouse (dwells) in one position for the dwell period.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the user hovers the mouse (dwells) in one position for the dwell period.")]
		public event EventHandler<ScintillaMouseEventArgs> DwellStart
		{
			add { Events.AddHandler(_dwellStartEventKey, value); }
			remove { Events.RemoveHandler(_dwellStartEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="DwellStart"/> event.
		/// </summary>
		/// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
		protected virtual void OnDwellStart(ScintillaMouseEventArgs e)
		{
			EventHandler<ScintillaMouseEventArgs> handler = Events[_dwellStartEventKey] as EventHandler<ScintillaMouseEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region DwellEnd

		/// <summary>
		/// Occurs when a user actions such as a mouse move or key press ends a dwell (hover) activity.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a dwell (hover) activity has ended.")]
		public event EventHandler<ScintillaMouseEventArgs> DwellEnd
		{
			add { Events.AddHandler(_dwellEndEventKey, value); }
			remove { Events.RemoveHandler(_dwellEndEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="DwellEnd"/> event.
		/// </summary>
		/// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
		protected virtual void OnDwellEnd(ScintillaMouseEventArgs e)
		{
			EventHandler<ScintillaMouseEventArgs> handler = Events[_dwellEndEventKey] as EventHandler<ScintillaMouseEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region ZoomChanged

		/// <summary>
		/// Occurs when the user zooms the display using the keyboard or the <see cref="Zoom"/> property is set.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when the user zooms the display using the keyboard or the Zoom property is set.")]
		public event EventHandler ZoomChanged
		{
			add { Events.AddHandler(_zoomChangedEventKey, value); }
			remove { Events.RemoveHandler(_zoomChangedEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="ZoomChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnZoomChanged(EventArgs e)
		{
			EventHandler handler = Events[_zoomChangedEventKey] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region HotspotClick

		/// <summary>
		/// Occurs when a user clicks on text that is in a style with the hotspot attribute set.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a user clicks on text with the hotspot style.")]
		public event EventHandler<ScintillaMouseEventArgs> HotspotClick
		{
			add { Events.AddHandler(_hotSpotClickEventKey, value); }
			remove { Events.RemoveHandler(_hotSpotClickEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="HotspotClick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
		protected virtual void OnHotspotClick(ScintillaMouseEventArgs e)
		{
			EventHandler<ScintillaMouseEventArgs> handler = Events[_hotSpotClickEventKey] as EventHandler<ScintillaMouseEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region HotspotDoubleClick

		/// <summary>
		/// Occurs when a user double-clicks on text that is in a style with the hotspot attribute set.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a user double-clicks on text with the hotspot style.")]
		public event EventHandler<ScintillaMouseEventArgs> HotspotDoubleClick
		{
			add { Events.AddHandler(_hotSpotDoubleClickEventKey, value); }
			remove { Events.RemoveHandler(_hotSpotDoubleClickEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="HotspotDoubleClick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
		protected virtual void OnHotspotDoubleClick(ScintillaMouseEventArgs e)
		{
			EventHandler<ScintillaMouseEventArgs> handler = Events[_hotSpotDoubleClickEventKey] as EventHandler<ScintillaMouseEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region DropMarkerCollect

		/// <summary>
		/// Occurs when a <see cref="DropMarker"/> is about to be collected.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a DropMarker is about to be collected.")]
		public event EventHandler<DropMarkerCollectEventArgs> DropMarkerCollect
		{
			add { Events.AddHandler(_dropMarkerCollectEventKey, value); }
			remove { Events.RemoveHandler(_dropMarkerCollectEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="DropMarkerCollect"/> event.
		/// </summary>
		/// <param name="e">An <see cref="DropMarkerCollectEventArgs"/> that contains the event data.</param>
		protected internal virtual void OnDropMarkerCollect(DropMarkerCollectEventArgs e)
		{
			EventHandler<DropMarkerCollectEventArgs> handler = Events[_dropMarkerCollectEventKey] as EventHandler<DropMarkerCollectEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region Scroll

		/// <summary>
		/// Occurs when the control is scrolled.
		/// </summary>
		[Category("Action"), Description("Occurs when the control is scrolled.")]
		public event EventHandler<ScrollEventArgs> Scroll
		{
			add { Events.AddHandler(_scrollEventKey, value); }
			remove { Events.RemoveHandler(_scrollEventKey, value); }
		}

		internal void FireScroll(ref Message m)
		{
			ScrollOrientation so = ScrollOrientation.VerticalScroll;
			int oldScroll = 0, newScroll = 0;
			ScrollEventType set = (ScrollEventType)(Utilities.SignedLoWord(m.WParam));
			if (m.Msg == NativeMethods.WM_HSCROLL)
			{
				so = ScrollOrientation.HorizontalScroll;
				oldScroll = _ns.GetXOffset();

				//	Let Scintilla Handle the scroll Message to actually perform scrolling
				base.WndProc(ref m);
				newScroll = _ns.GetXOffset();
			}
			else
			{
				so = ScrollOrientation.VerticalScroll;
				oldScroll = _ns.GetFirstVisibleLine();
				base.WndProc(ref m);
				newScroll = _ns.GetFirstVisibleLine();
			}

			OnScroll(new ScrollEventArgs(set, oldScroll, newScroll, so));
		}

		/// <summary>
		/// Raises the <see cref="Scroll"/> event.
		/// </summary>
		/// <param name="e">An <see cref="ScrollEventArgs"/> that contains the event data.</param>
		protected virtual void OnScroll(ScrollEventArgs e)
		{
			EventHandler<ScrollEventArgs> handler = Events[_scrollEventKey] as EventHandler<ScrollEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region MacroRecord

		/// <summary>
		/// Occurs each time a recordable change occurs.
		/// </summary>
		[Category("Scintilla"), Description("Occurs each time a recordable change occurs.")]
		public event EventHandler<MacroRecordEventArgs> MacroRecord
		{
			add { Events.AddHandler(_macroRecordEventKey, value); }
			remove { Events.RemoveHandler(_macroRecordEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="MacroRecord"/> event.
		/// </summary>
		/// <param name="e">An <see cref="MacroRecordEventArgs"/> that contains the event data.</param>
		protected virtual void OnMacroRecord(MacroRecordEventArgs e)
		{
			EventHandler<MacroRecordEventArgs> handler = Events[_macroRecordEventKey] as EventHandler<MacroRecordEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region FileDrop

		/// <summary>
		/// Occurs when a user drops a file on the <see cref="Scintilla"/> control.
		/// </summary>
		[Category("Scintilla"), Description("Occurs when a user drops a file on the control.")]
		public event EventHandler<FileDropEventArgs> FileDrop
		{
			add { Events.AddHandler(_fileDropEventKey, value); }
			remove { Events.RemoveHandler(_fileDropEventKey, value); }
		}

		/// <summary>
		/// Raises the <see cref="FileDrop"/> event.
		/// </summary>
		/// <param name="e">An <see cref="FileDropEventArgs"/> that contains the event data.</param>
		protected virtual void OnFileDrop(FileDropEventArgs e)
		{
			EventHandler<FileDropEventArgs> handler = Events[_fileDropEventKey] as EventHandler<FileDropEventArgs>;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#endregion

		#region Public Methods

		/// <summary>
		/// Appends a copy of the specified string to the end of this instance.
		/// </summary>
		/// <param name="text">The <see cref="String"/> to append.</param>
		/// <returns>A <see cref="Range"/> representing the appended text.</returns>
		public Range AppendText(string text)
		{
			int oldLength = TextLength;
			NativeInterface.AppendText(text.Length, text);
			return GetRange(oldLength, TextLength);
		}

		/// <summary>
		/// Inserts text at the current cursor position
		/// </summary>
		/// <param name="text">Text to insert</param>
		/// <returns>The range inserted</returns>
		public Range InsertText(string text)
		{
			NativeInterface.AddText(text.Length, text);
			return GetRange(_caret.Position, text.Length);
		}

		/// <summary>
		/// Inserts text at the given position
		/// </summary>
		/// <param name="position">The position to insert text in</param>
		/// <param name="text">Text to insert</param>
		/// <returns>The text range inserted</returns>
		public Range InsertText(int position, string text)
		{
			NativeInterface.InsertText(position, text);
			return GetRange(position, text.Length);
		}

		public char CharAt(int position)
		{
			return _ns.GetCharAt(position);
		}

		public Range GetRange(int startPosition, int endPosition)
		{
			return new Range(startPosition, endPosition, this);
		}

		public Range GetRange(int position)
		{
			return new Range(position, position + 1, this);
		}

		public Range GetRange()
		{
			return new Range(0, _ns.GetTextLength(), this);
		}

		public int GetColumn(int position)
		{
			return _ns.GetColumn(position);
		}

		public int FindColumn(int line, int column)
		{
			return _ns.FindColumn(line, column);
		}

		public int PositionFromPoint(int x, int y)
		{
			return _ns.PositionFromPoint(x, y);
		}

		public int PositionFromPointClose(int x, int y)
		{
			return _ns.PositionFromPointClose(x, y);
		}

		public int PointXFromPosition(int position)
		{
			return _ns.PointXFromPosition(position);
		}

		public int PointYFromPosition(int position)
		{
			return _ns.PointYFromPosition(position);
		}

		public void ZoomIn()
		{
			_ns.ZoomIn();
		}

		private void ZoomOut()
		{
			_ns.ZoomOut();
		}


		/// <summary>
		/// Checks that if the specified position is on comment.
		/// </summary>
		public bool PositionIsOnComment(int position)
		{
			//this.Colorize(0, -1);
			return PositionIsOnComment(position, _lexing.Lexer);
		}

		/// <summary>
		/// Checks that if the specified position is on comment.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="lexer"></param>
		/// <returns></returns>
		public bool PositionIsOnComment(int position, Lexer lexer)
		{
			int style = _styles.GetStyleAt(position);
			if ((lexer == Lexer.Python || lexer == Lexer.Lisp)
				&& style == 1
				|| style == 12)
			{
				return true; // python or lisp
			}
			else if ((lexer == Lexer.Cpp || lexer == Lexer.Pascal || lexer == Lexer.Tcl || lexer == Lexer.Bullant)
				&& style == 1
				|| style == 2
				|| style == 3
				|| style == 15
				|| style == 17
				|| style == 18)
			{
				return true; // cpp, tcl, bullant or pascal
			}
			else if ((lexer == Lexer.Hypertext || lexer == Lexer.Xml)
				&& style == 9
				|| style == 20
				|| style == 29
				|| style == 30
				|| style == 42
				|| style == 43
				|| style == 44
				|| style == 57
				|| style == 58
				|| style == 59
				|| style == 72
				|| style == 82
				|| style == 92
				|| style == 107
				|| style == 124
				|| style == 125)
			{
				return true; // html or xml
			}
			else if ((lexer == Lexer.Perl || lexer == Lexer.Ruby || lexer == Lexer.Clw || lexer == Lexer.Bash)
				&& style == 2)
			{
				return true; // perl, bash, clarion/clw or ruby
			}
			else if ((lexer == Lexer.Sql)
				&& style == 1
				|| style == 2
				|| style == 3
				|| style == 13
				|| style == 15
				|| style == 17
				|| style == 18)
			{
				return true; // sql
			}
			else if ((lexer == Lexer.VB || lexer == Lexer.Properties || lexer == Lexer.MakeFile || lexer == Lexer.Batch || lexer == Lexer.Diff || lexer == Lexer.Conf || lexer == Lexer.Ave || lexer == Lexer.Eiffel || lexer == Lexer.EiffelKw || lexer == Lexer.Tcl || lexer == Lexer.VBScript || lexer == Lexer.MatLab || lexer == Lexer.Fortran || lexer == Lexer.F77 || lexer == Lexer.Lout || lexer == Lexer.Mmixal || lexer == Lexer.Yaml || lexer == Lexer.PowerBasic || lexer == Lexer.ErLang || lexer == Lexer.Octave || lexer == Lexer.Kix || lexer == Lexer.Asn1)
				&& style == 1)
			{
				return true; // asn1, vb, diff, batch, makefile, avenue, eiffel, eiffelkw, vbscript, matlab, crontab, fortran, f77, lout, mmixal, yaml, powerbasic, erlang, octave, kix or properties
			}
			else if ((lexer == Lexer.Latex)
				&& style == 4)
			{
				return true; // latex
			}
			else if ((lexer == Lexer.Lua || lexer == Lexer.EScript || lexer == Lexer.Verilog)
				&& style == 1
				|| style == 2
				|| style == 3)
			{
				return true; // lua, verilog or escript
			}
			else if ((lexer == Lexer.Ada)
				&& style == 10)
			{
				return true; // ada
			}
			else if ((lexer == Lexer.Baan || lexer == Lexer.Pov || lexer == Lexer.Ps || lexer == Lexer.Forth || lexer == Lexer.MsSql || lexer == Lexer.Gui4Cli || lexer == Lexer.Au3 || lexer == Lexer.Apdl || lexer == Lexer.Vhdl || lexer == Lexer.Rebol)
				&& style == 1
				|| style == 2)
			{
				return true; // au3, apdl, baan, ps, mssql, rebol, forth, gui4cli, vhdl or pov
			}
			else if ((lexer == Lexer.Asm)
				&& style == 1
				|| style == 11)
			{
				return true; // asm
			}
			else if ((lexer == Lexer.Nsis)
				&& style == 1
				|| style == 18)
			{
				return true; // nsis
			}
			else if ((lexer == Lexer.Specman)
				&& style == 2
				|| style == 3)
			{
				return true; // specman
			}
			else if ((lexer == Lexer.Tads3)
				&& style == 3
				|| style == 4)
			{
				return true; // tads3
			}
			else if ((lexer == Lexer.CSound)
				&& style == 1
				|| style == 9)
			{
				return true; // csound
			}
			else if ((lexer == Lexer.Caml)
				&& style == 12
				|| style == 13
				|| style == 14
				|| style == 15)
			{
				return true; // caml
			}
			else if ((lexer == Lexer.Haskell)
				&& style == 13
				|| style == 14
				|| style == 15
				|| style == 16)
			{
				return true; // haskell
			}
			else if ((lexer == Lexer.Flagship)
				&& style == 1
				|| style == 2
				|| style == 3
				|| style == 4
				|| style == 5
				|| style == 6)
			{
				return true; // flagship
			}
			else if ((lexer == Lexer.Smalltalk)
				&& style == 3)
			{
				return true; // smalltalk
			}
			else if ((lexer == Lexer.Css)
				&& style == 9)
			{
				return true; // css
			}
			return false;
		}

		/// <summary>
		/// Adds a line end marker to the end of the document
		/// </summary>
		public void AddLastLineEnd()
		{
			EndOfLineMode eolMode = _endOfLine.Mode;
			string eolMarker = "\r\n";

			if (eolMode == EndOfLineMode.CR)
				eolMarker = "\r";
			else if (eolMode == EndOfLineMode.LF)
				eolMarker = "\n";

			int tl = TextLength;
			int start = tl - eolMarker.Length;

			if (start < 0 || GetRange(start, start + eolMarker.Length).Text != eolMarker)
				AppendText(eolMarker);
		}

		/// <summary>
		/// Gets a word from the specified position
		/// </summary>
		public string GetWordFromPosition(int position)
		{
			//	Chris Rickard 2008-07-28
			//	Fixing implementation to actually return the word at the position...
			//	Credit goes to Stumpii for the code.
			//	As a side note: I think the previous code was implemented based off
			//	some funky code I made for the snippet keyword detection, but since
			//	it doesn't reference this method there's no reason to keep the buggy
			//	behavior. I also removed the try..catch because in theory this 
			//	shouldn't throw and we REALLY shouldn't be eating exceptions at the
			//	System.Exception level. If some start popping up I can add some
			//	conditionals or catch more specific Exceptions.
			int startPosition = NativeInterface.WordStartPosition(position, true);
			int endPosition = NativeInterface.WordEndPosition(position, true);
			return GetRange(startPosition, endPosition).Text;
		}

		#endregion

		#region other stuff
		internal bool IsDesignMode
		{
			get
			{
				return DesignMode;
			}
		}

		private List<TopLevelHelper> _helpers = new List<TopLevelHelper>();
		protected internal List<TopLevelHelper> Helpers
		{
			get
			{
				return _helpers;
			}
			set
			{
				_helpers = value;
			}
		}
		#endregion

		#region ISupportInitialize Members
		private bool _isInitializing = false;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsInitializing
		{
			get
			{
				return _isInitializing;
			}
			set
			{
				_isInitializing = value;
			}
		}

		public void BeginInit()
		{
			_isInitializing = true;
		}

		public void EndInit()
		{
			_isInitializing = false;
			foreach (ScintillaHelperBase helper in _helpers)
			{
				helper.Initialize();
			}
        }

        #endregion
    }
}
