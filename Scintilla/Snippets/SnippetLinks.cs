using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ScintillaNet
{
	public class SnippetLinkCollection : IDictionary<string, SnippetLink>, IList<SnippetLink>
	{
		List<SnippetLink> _snippetLinks = new List<SnippetLink>();

		#region IDictionary<string,SnippetLink> Members

		public void Add(string key, SnippetLink value)
		{
			if(!key.Equals(value.Key, StringComparison.CurrentCultureIgnoreCase))
				throw new ArgumentException("Key argument must == value.Key", "key");
			else if(ContainsKey(key))
				throw new ArgumentException("Key already exists", "key");

			_snippetLinks.Add(value);
		}

		public bool ContainsKey(string key)
		{
			foreach(SnippetLink sl in _snippetLinks)
				if(sl.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
					return true;

			return false;
		}

		public ICollection<string> Keys
		{
			get 
			{
				string[] keys = new string[_snippetLinks.Count];
				for(int i = 0; i < _snippetLinks.Count; i++)
				{
					keys[i] = _snippetLinks[i].Key;
				}
				return keys;
			}
		}

		public bool Remove(string key)
		{
			for(int i = 0; i < _snippetLinks.Count; i++)
			{
				if(_snippetLinks[i].Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
				{
					_snippetLinks.RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		public bool TryGetValue(string key, out SnippetLink value)
		{
			value = null;
			for(int i = 0; i < _snippetLinks.Count; i++)
			{
				if(_snippetLinks[i].Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
				{
					value = _snippetLinks[i];
					return true;
				}
			}
			return false;
		}

		public ICollection<SnippetLink> Values
		{
			get
			{
				SnippetLink[] values = new SnippetLink[_snippetLinks.Count];
				for(int i = 0; i < _snippetLinks.Count; i++)
				{
					values[i] = _snippetLinks[i];
				}
				return values;
			}
		}

		public SnippetLink this[string key]
		{
			get
			{
				for(int i = 0; i < _snippetLinks.Count; i++)
				{
					if(_snippetLinks[i].Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
						return _snippetLinks[i];
				}
				throw new KeyNotFoundException();
			}
			set
			{
				if(!key.Equals(value.Key, StringComparison.CurrentCultureIgnoreCase))
					throw new ArgumentException("Key argument must == value.Key", "key");

				for(int i = 0; i < _snippetLinks.Count; i++)
				{
					if(_snippetLinks[i].Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
					{
						_snippetLinks[i] = value;
						return;
					}
				}

				_snippetLinks.Add(value);
			}
		}

		#endregion

		#region ICollection<KeyValuePair<string,SnippetLink>> Members

		public void Add(KeyValuePair<string, SnippetLink> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			List<ManagedRange> rageList = new List<ManagedRange>();

			foreach (SnippetLink sl in _snippetLinks)
			{
				foreach (Range r in sl.Ranges)
				{
					ManagedRange mr = r as ManagedRange;
					rageList.Add(mr);
				}
			}

			_snippetLinks.Clear();

			foreach (ManagedRange mr in rageList)
				mr.Dispose();
		}

		public bool Contains(KeyValuePair<string, SnippetLink> item)
		{
			return ContainsKey(item.Key);
		}

		public void CopyTo(KeyValuePair<string, SnippetLink>[] array, int arrayIndex)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Count
		{
			get { return _snippetLinks.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(KeyValuePair<string, SnippetLink> item)
		{
			return Remove(item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,SnippetLink>> Members

		public IEnumerator<KeyValuePair<string, SnippetLink>> GetEnumerator()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _snippetLinks.GetEnumerator();
		}

		#endregion

		#region IList<SnippetLink> Members

		public int IndexOf(SnippetLink item)
		{
			return _snippetLinks.IndexOf(item);
		}

		public void Insert(int index, SnippetLink item)
		{
			_snippetLinks.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_snippetLinks.RemoveAt(index);
		}

		public SnippetLink this[int index]
		{
			get
			{
				return _snippetLinks[index];
			}
			set
			{
				_snippetLinks[index] = value;
			}
		}

		#endregion

		#region ICollection<SnippetLink> Members

		public void Add(SnippetLink item)
		{
			Add(item.Key, item);
		}

		public bool Contains(SnippetLink item)
		{
			return _snippetLinks.Contains(item);
		}

		public void CopyTo(SnippetLink[] array, int arrayIndex)
		{
			_snippetLinks.CopyTo(array, arrayIndex);
		}

		public bool Remove(SnippetLink item)
		{
			return _snippetLinks.Remove(item);
		}

		#endregion

		#region IEnumerable<SnippetLink> Members

		IEnumerator<SnippetLink> IEnumerable<SnippetLink>.GetEnumerator()
		{
			return _snippetLinks.GetEnumerator();
		}

		#endregion

		private SnippetLinkEnd _endPoint = null;
		public SnippetLinkEnd EndPoint
		{
			get
			{
				return _endPoint;
			}
			set
			{
				_endPoint = value;
			}
		}

		private int _activeLinkIndex = -1;

		public SnippetLink ActiveSnippetLink
		{
			get
			{
				if (_activeLinkIndex < 0 || _activeLinkIndex >= _snippetLinks.Count)
					return null;

				return _snippetLinks[_activeLinkIndex];
			}
			set
			{
				if(value == null)
				{
					_activeLinkIndex = -1;
					return;
				}
				_activeLinkIndex = _snippetLinks.IndexOf(value);
			}
		}

		public SnippetLink NextActiveSnippetLink
		{
			get
			{
				int newIndex = _activeLinkIndex;
				if(newIndex < 0)
					return null;
				else if(++newIndex >= _snippetLinks.Count)
					newIndex = 0;

				return _snippetLinks[newIndex];
			}
		}

		public SnippetLink PreviousActiveSnippetLink
		{
			get
			{
				int newIndex = _activeLinkIndex;
				if (newIndex < 0)
					return null;
				else if (--newIndex < 0)
					newIndex = _snippetLinks.Count - 1;

				return _snippetLinks[newIndex];
			}
		}

		private bool _isActive = false;
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;
			}
		}

		private SnippetLinkRange _activeRange = null;
		public SnippetLinkRange ActiveRange
		{
			get
			{
				return _activeRange;
			}
			set
			{
				_activeRange = value;
			}
		}

	}

	public class SnippetLink
	{
		private string _key;
		public string Key
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

		private List<SnippetLinkRange> _ranges = new List<SnippetLinkRange>();
		public List<SnippetLinkRange> Ranges
		{
			get
			{
				return _ranges;
			}
			set
			{
				_ranges = value;
			}
		}

		public SnippetLink(string key)
		{
			_key = key;
		}
	}

	public class SnippetLinkRange : ManagedRange
	{
		private string _key;
		public string Key
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
		private List<SnippetLinkRange> _parent;
		public List<SnippetLinkRange> Parent
		{
			get
			{
				return _parent;
			}
			set
			{
				_parent = value;
			}
		}

		public SnippetLinkRange(int start, int end, Scintilla scintilla, string key)
			: base()
		{
			Scintilla = scintilla;
			Start = start;
			End = end;
			_key = key;
		}

		public override void Dispose()
		{
			if (!IsDisposed)
			{
				_parent.Remove(this);
				base.Dispose();
			}
		}


		internal void Init()
		{
			Scintilla.ManagedRanges.Add(this);
		}

		private bool _active = false;
		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				_active = value;

				if (value)
				{
					ClearIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
					SetIndicator(Scintilla.Snippets.ActiveSnippetIndicator);					
				}
				else
				{
					SetIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
					ClearIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
				}
			}
		}
	}

}
