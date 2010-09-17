using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace ScintillaNet
{
	/// <summary>
	/// Manages DropMarkers, a Stack Based document bookmarking system.
	/// </summary>
	[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class DropMarkers : TopLevelHelper
	{
		internal DropMarkers(Scintilla scintilla) : base(scintilla) { }

		private Stack<DropMarker> _markerStack = new Stack<DropMarker>();
		private static Dictionary<string, Stack<DropMarker>> _sharedStack = new Dictionary<string, Stack<DropMarker>>();

		internal bool ShouldSerialize()
		{
			return ShouldSerializeSharedStackName();
		}

		#region SharedStackName
		private string _sharedStackName = string.Empty;

		/// <summary>
		/// Gets/Sets a shared name associated with other Scintilla controls. 
		/// </summary>
		/// <remarks>
		/// All Scintilla controls with the same SharedStackName share a common
		/// DropMarker stack. This is useful in MDI applications where you want
		/// the DropMarker stack not to be specific to one document.
		/// </remarks>
		public string SharedStackName
		{
			get
			{
				return _sharedStackName;
			}
			set
			{
				if (value == null)
					value = string.Empty;

				if (_sharedStackName == value)
					return;

				
				if (value == string.Empty)
				{
					//	If we had a shared stack name but are now clearing it
					//	we need to create our own private DropMarkerStack again
					_markerStack = new Stack<DropMarker>();

					//	If this was the last subscriber of a shared stack
					//	remove the name to free up resources
					if (_sharedStack.ContainsKey(_sharedStackName) && _sharedStack[_sharedStackName].Count == 1)
						_sharedStack.Remove(_sharedStackName);
				}
				else
				{
					//	We're using one of the shared stacks. Of course if it hasn't 
					//	already been registered with the list we need to create it.
					if (!_sharedStack.ContainsKey(_sharedStackName))
						_sharedStack[_sharedStackName] = new Stack<DropMarker>();

					_markerStack = _sharedStack[_sharedStackName];
				}

				_sharedStackName = value;
			}
		}

		private bool ShouldSerializeSharedStackName()
		{
			return _sharedStackName != string.Empty;
		}

		private void ResetSharedStackName()
		{
			_sharedStackName = string.Empty;
		} 
		#endregion

		/// <summary>
		/// Gets/Sets the Stack of DropMarkers 
		/// </summary>
		/// <remarks>
		/// You can manually set this to implement your own shared DropMarker stack
		/// between Scintilla Controls. 
		/// </remarks>
		/// <seealso cref="SharedStackName"/>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Stack<DropMarker> MarkerStack
		{
			get
			{
				return _markerStack;
			}

			//	That's right kids you can actually provide your own MarkerStack. This
			//	is really useful for MDI applications where you want a single master
			//	MarkerStack that will automatically switch documents (a la CodeRush).
			//	Of course you can let the control do this for you automatically by 
			//	setting the SharedStackName property of multiple instances.
			set
            {
            	_markerStack = value;
            }
		}

		private DropMarkerList _allDocumentDropMarkers = new DropMarkerList();

		/// <summary>
		/// Gets/Sets a list of All DropMarkers specific to this Scintilla control
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DropMarkerList AllDocumentDropMarkers
		{
			get
			{
				return _allDocumentDropMarkers;
			}
			set
			{
				_allDocumentDropMarkers = value;
			}
		}

		/// <summary>
		/// Drops a DropMarker at the current document position
		/// </summary>
		/// <remarks>
		/// Dropping a DropMarker creates a visual marker (red triangle)
		/// indicating the DropMarker point.
		/// </remarks>
		/// <returns>The newly created DropMarker</returns>
		public DropMarker Drop()
		{
			return Drop(NativeScintilla.GetCurrentPos());
		}

		/// <summary>
		/// Drops a DropMarker at the specified document position
		/// </summary>
		/// <param name="position"></param>
		/// <returns>The newly created DropMarker</returns>
		/// <remarks>
		/// Dropping a DropMarker creates a visual marker (red triangle)
		/// indicating the DropMarker point.
		/// </remarks>
		public DropMarker Drop(int position)
		{
			DropMarker dm = new DropMarker(position, position, getCurrentTopOffset(), Scintilla);
			_allDocumentDropMarkers.Add(dm);
			_markerStack.Push(dm);
			Scintilla.ManagedRanges.Add(dm);

			//	Force the Drop Marker to paint
			Scintilla.Invalidate(dm.GetClientRectangle());
			return dm;
		}

		/// <summary>
		/// Collects the last dropped DropMarker
		/// </summary>
		/// <remarks>
		/// When a DropMarker is collected the current document posision is moved
		/// to the DropMarker posision, the DropMarker is removed from the stack
		/// and the visual indicator is removed.
		/// </remarks>
		public void Collect()
		{
			while (_markerStack.Count > 0)
			{
				DropMarker dm = _markerStack.Pop();
				
				//	If the Drop Marker was deleted in the document by
				//	a user action it will be disposed but not removed
				//	from the marker stack. In this case just pretend
				//	like it doesn't exist and go on to the next one
				if (dm.IsDisposed)
					continue;

				//	The MarkerCollection fires a cancellable event.
				//	If it is canclled the Collect() method will return
				//	false. In this case we need to push the marker back
				//	on the stack so that it will still be collected in
				//	the future.
				if (!dm.Collect())
					_markerStack.Push(dm);

				return;
			}
		}

		private int getCurrentTopOffset()
		{
			return -1;
		}



	}

	/// <summary>
	/// Data structure used to store DropMarkers in the AllDocumentDropMarkers property.
	/// </summary>
	public class DropMarkerList : System.Collections.ObjectModel.KeyedCollection<Guid, DropMarker>
	{
		protected override Guid GetKeyForItem(DropMarker item)
		{
			return item.Key;
		}
	}

	/// <summary>
	/// Represents a DropMarker, currently a single document point.
	/// </summary>
	public class DropMarker : ManagedRange
	{
		private int _topOffset;
		/// <summary>
		/// Not currently used, the offset in pixels from the document view's
		/// top.
		/// </summary>
		public int TopOffset
		{
			get
			{
				return _topOffset;
			}
			set
			{
				_topOffset = value;
			}
		}

		private Guid _key = Guid.NewGuid();
		/// <summary>
		/// Uniquely identifies the DropMarker
		/// </summary>
		public Guid Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		internal DropMarker(int start, int end, int topOffset, Scintilla scintilla)
			: base(start, end, scintilla)
		{
			base.Start		= start;
			base.End		= end;
			this._topOffset = topOffset;
		}

		/// <summary>
		/// Overriden, changes the document position. Start and End should
		/// match.
		/// </summary>
		/// <param name="newStart">Document start position</param>
		/// <param name="newEnd">Document end position</param>
		public override void Change(int newStart, int newEnd)
		{
			Invalidate();			
			//	This actually changes Start and End
			base.Change(newStart, newEnd);
		}

		/// <summary>
		/// Forces a repaint of the DropMarker
		/// </summary>
		public void Invalidate()
		{
			if(Scintilla != null && Start > 0)
			{
				//	Invalidate the old Marker Location so that we don't get "Ghosts"
				Scintilla.Invalidate(GetClientRectangle());
			}
		}

		/// <summary>
		/// Overriden. Drop Markers are points, not a spanned range. Though this could change in the future.
		/// </summary>
		public override bool IsPoint
		{
			get
			{
				return Start == End;
			}
		}

		protected internal override void Paint(Graphics g)
		{
			base.Paint(g);

			if (IsDisposed)
				return;

			int x = NativeScintilla.PointXFromPosition(Start);
			int y = NativeScintilla.PointYFromPosition(Start) + NativeScintilla.TextHeight(0) - 2;

			//	Draw a red Triangle with a dark red border at the marker position
			g.FillPolygon(Brushes.Red, new Point[] { new Point(x-2, y+4), new Point(x, y), new Point(x+2, y+4) });
			g.DrawPolygon(Pens.DarkRed, new Point[] { new Point(x-2, y+4), new Point(x, y), new Point(x+2, y+4) });
		}

		/// <summary>
		/// Collects the DropMarker and causes it to be removed from all
		/// lists it belongs ti.
		/// </summary>
		/// <returns></returns>
		public bool Collect()
		{
			return Collect(true);
		}

		internal bool Collect(bool dispose)
		{
			DropMarkerCollectEventArgs e = new DropMarkerCollectEventArgs(this);
			Scintilla.OnDropMarkerCollect(e);

			if (e.Cancel)
				return false;

			GotoStart();

			if (dispose)
				Dispose();

			return true;
		}

		/// <summary>
		/// Overriden.
		/// </summary>
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				Scintilla.DropMarkers.AllDocumentDropMarkers.Remove(this);
				Invalidate();
				base.Dispose();
			}
		}		

		/// <summary>
		/// Gets the Client Rectangle in pixels of the DropMarker's visual indicator.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetClientRectangle()
		{
			int x = NativeScintilla.PointXFromPosition(Start);
			int y = NativeScintilla.PointYFromPosition(Start) + NativeScintilla.TextHeight(0) - 2;

			//	Invalidate the old Marker Location so that we don't get "Ghosts"
			return new Rectangle(x - 2, y, 5, 5);
		}

		public override bool Equals(object obj)
		{
			if (!IsSameHelperFamily(obj))
				return false;

			return ((DropMarker)obj).Key == this.Key;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
