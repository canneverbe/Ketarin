using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
namespace ScintillaNet
{
	/// <summary>
	/// Manages Document Navigation, which is a snapshot history of movements within
	/// a document.
	/// </summary>
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class DocumentNavigation : TopLevelHelper
	{
		bool _supressNext = false;
		private Timer t = null;

		internal DocumentNavigation(Scintilla scintilla) : base(scintilla) 
		{
			t = new Timer();
			t.Interval = _navigationPointTimeout;
			t.Tick += new EventHandler(t_Tick);
			scintilla.SelectionChanged += new EventHandler(scintilla_SelectionChanged);
		}

		internal bool ShouldSerialize()
		{
			return ShouldSerializeIsEnabled() || ShouldSerializeMaxHistorySize();
		}

		public void Reset()
		{
			_backwardStack.Clear();
			_forewardStack.Clear();
			ResetIsEnabled();
			ResetMaxHistorySize();
		}

		private void t_Tick(object sender, EventArgs e)
		{
			t.Enabled = false;
			int pos = NativeScintilla.GetCurrentPos();
			if ((_forewardStack.Count == 0 || _forewardStack.Current.Start != pos) && (_backwardStack.Count == 0 || _backwardStack.Current.Start != pos))
				_backwardStack.Push(newRange(pos));
		}

		private void scintilla_SelectionChanged(object sender, EventArgs e)
		{
			if (!_isEnabled)
				return;

			if (!_supressNext)
			{
				t.Enabled = false;
				t.Enabled = true;
			}
			else
			{
				_supressNext = false;
			}
		}

		#region IsEnabled
		private bool _isEnabled = true;
		/// <summary>
		/// Gets/Sets whether Document Navigation is tracked. Defaults to true.
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			set
			{
				_isEnabled = value;
			}
		}
		private bool ShouldSerializeIsEnabled()
		{
			return !_isEnabled;
		}

		private void ResetIsEnabled()
		{
			_isEnabled = true;
		} 
		#endregion

		#region MaxHistorySize
		private int _maxHistorySize = 50;
		/// <summary>
		/// Maximum number of places the document navigation remembers. Defaults to 50.
		/// </summary>
		/// <remarks>
		/// When the max value is reached the oldest entries are removed.
		/// </remarks>
		public int MaxHistorySize
		{
			get
			{
				return _maxHistorySize;
			}
			set
			{
				_maxHistorySize = value;
				_backwardStack.MaxCount = value;
				_forewardStack.MaxCount = value;
			}
		}

		private bool ShouldSerializeMaxHistorySize()
		{
			return _maxHistorySize != 50;
		}

		private void ResetMaxHistorySize()
		{
			_maxHistorySize = 50;
		}

		#endregion

		#region BackwardStack
		public FakeStack _backwardStack = new FakeStack();
		/// <summary>
		/// List of entries that allow you to navigate backwards.
		/// </summary>
		/// <remarks>
		/// The ForwardStack and BackwardStack can be shared between multiple
		/// ScintillaNET objects. This is useful in MDI applications when you wish
		/// to have a shared document navigation that remembers positions in each
		/// document.
		/// </remarks>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FakeStack BackwardStack
		{
			get
			{
				return _backwardStack;
			}
			set
			{
				_backwardStack = value;
			}
		} 
		#endregion

		#region ForewardStack
		public FakeStack _forewardStack = new FakeStack();
		/// <summary>
		/// List of entries that allow you to navigate forwards.
		/// </summary>
		/// <remarks>
		/// The ForwardStack and BackwardStack can be shared between multiple
		/// ScintillaNET objects. This is useful in MDI applications when you wish
		/// to have a shared document navigation that remembers positions in each
		/// document.
		/// </remarks>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FakeStack ForewardStack
		{
			get
			{
				return _forewardStack;
			}
			set
			{
				_forewardStack = value;
			}
		} 
		#endregion

		#region NavigationPointTimeout
		private int _navigationPointTimeout = 200;
		/// <summary>
		/// Time in milliseconds to wait before a Navigation Point is set. Default is 200
		/// </summary>
		/// <remarks>
		/// In text editing, the current caret position is constantly changing. Rather than capture every
		/// change in position, ScintillaNET captures the current position [NavigationPointTimeout]ms after a 
		/// position changes, only then is it eligable for another snapshot
		/// </remarks>
		public int NavigationPointTimeout
		{
			get
			{
				return _navigationPointTimeout;
			}
			set
			{
				_navigationPointTimeout = value;
			}
		}

		private bool ShouldSerializeNavigationPointTimeout()
		{
			return _navigationPointTimeout != 200;
		}

		private void ResetNavigationPointTimeout()
		{
			_navigationPointTimeout = 200;
		} 
		#endregion

		/// <summary>
		/// Causes the current position to navigate to the last snapshotted document position.
		/// </summary>
		public void NavigateBackward()
		{
			if (_backwardStack.Count == 0)
				return;

			int currentPos = Scintilla.Caret.Position;
			if (currentPos == _backwardStack.Current.Start && _backwardStack.Count == 1)
				return;

			int pos = _backwardStack.Pop().Start;

			if (pos != currentPos)
			{
				_forewardStack.Push(newRange(currentPos));
				Scintilla.Caret.Goto(pos);
			}
			else
			{
				_forewardStack.Push(newRange(pos));
				Scintilla.Caret.Goto(_backwardStack.Current.Start);
			}

			_supressNext = true;
		}

		/// <summary>
		/// After 1 or more backwards navigations this command navigates to the previous
		/// backwards navigation point.
		/// </summary>
		public void NavigateForward()
		{
			if (!CanNavigateForward)
				return;

			int pos = _forewardStack.Pop().Start;
			_backwardStack.Push(newRange(pos));
			Scintilla.Caret.Goto(pos);

			_supressNext = true;
		}

		/// <summary>
		/// Returns true if ScintillaNET can perform a successful backward navigation.
		/// </summary>
		[Browsable(false)]
		public bool CanNavigateBackward
		{
			get
            {
				if (_backwardStack.Count == 0 || (NativeScintilla.GetCurrentPos() == _backwardStack.Current.Start && _backwardStack.Count == 1))
					return false;

				return true;
            }
		}

		/// <summary>
		/// Returns true if ScintillaNET can perform a successful forward navigation.
		/// </summary>
		[Browsable(false)]
		public bool CanNavigateForward
		{
			get
			{
				return _forewardStack.Count > 0;
			}
		}

		private NavigationPont newRange(int pos)
		{
			NavigationPont mr = new NavigationPont(pos, Scintilla);
			Scintilla.ManagedRanges.Add(mr);
			return mr;
		}

		/// <summary>
		/// Mostly behaves like a stack but internally maintains a List for more flexability
		/// </summary>
		/// <remarks>
		/// FakeStack is not a general purpose datastructure and can only hold NavigationPoint objects
		/// </remarks>
		public class FakeStack : List<NavigationPont>
		{
			private int _maxCount = 50;
            public int MaxCount
            {
            	get 
            	{
            		return _maxCount; 
            	}
            	set
            	{
            		_maxCount = value;
            	}
            }

			public NavigationPont Pop()
			{
				NavigationPont ret = this[Count - 1];
				RemoveAt(Count - 1);
				return ret;
			}

			public void Push(NavigationPont value)
			{
				Add(value);
				if (Count > MaxCount)
					RemoveAt(0);
			}

			public NavigationPont Current
			{
				get
				{
					return this[Count - 1];
				}
			}
		}

		/// <summary>
		/// Represents a point in the document used for navigation.
		/// </summary>
		public class NavigationPont : ManagedRange
		{
			/// <summary>
			/// Initializes a new instance of the NavigationPont class.
			/// </summary>
			public NavigationPont(int pos, Scintilla scintilla) : base(pos, pos, scintilla)
			{
			}

			/// <summary>
			/// Overriden.
			/// </summary>
			public override void Dispose()
			{
				Scintilla.DocumentNavigation.ForewardStack.Remove(this);
				Scintilla.DocumentNavigation.BackwardStack.Remove(this);

				base.Dispose();
			}
		}
	}
}
