using System;
using System.Collections.Generic;
using System.Text;

namespace ScintillaNet
{
	public class Snippet : IComparable<Snippet>
	{
		internal const char RealDelimeter = '\x1';

		public char DefaultDelimeter = '$';

		public Snippet(string shortcut, string code) : this(shortcut, code, '$', false) { }

		public Snippet(string shortcut, string code, char delimeter, bool isSurroundsWith)
		{
			_isSurroundsWith	= isSurroundsWith;
			_shortcut			= shortcut;
			_delimeter			= delimeter;
			Code				= code;			
		}


		private string _realCode;
		internal string RealCode
		{
			get
			{
				return _realCode;
			}
			set
			{
				_realCode = value;
			}
		}

		private string _shortcut;
		public string Shortcut
		{
			get
			{
				return _shortcut;
			}
			set
			{
				_shortcut = value;
			}
		}

		private char _delimeter;
		public char Delimeter
		{
			get
			{
				return _delimeter;
			}
			set
			{
				_delimeter = value;
			}
		}

		private string _code;
		public string Code
		{
			get
			{
				return _code;
			}
			set
			{
				_code = value;
				_realCode = _code.Replace(_delimeter, RealDelimeter);
			}
		}

		private List<string> _languages = new List<string>();
		public List<string> Languages
		{
			get
			{
				return _languages;
			}
			set
			{
				_languages = value;
			}
		}

		private bool _isSurroundsWith;
		public bool IsSurroundsWith
		{
			get
			{
				return _isSurroundsWith;
			}
			set
			{
				_isSurroundsWith = value;
			}
		}

		#region IComparable<Snippet> Members

		public int CompareTo(Snippet other)
		{
			return StringComparer.OrdinalIgnoreCase.Compare(_shortcut, other._shortcut);
		}

		#endregion
	}
}
