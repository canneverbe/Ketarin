using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ScintillaNet.Configuration
{
	public class SnippetsConfigList : List<SnippetsConfig>
	{
		private bool? _inherit;
		public bool? Inherit
		{
			get
			{
				return _inherit;
			}
			set
			{
				_inherit = value;
			}
		}

		private Color _activeSnippetColor;
		public Color ActiveSnippetColor
		{
			get
			{
				return _activeSnippetColor;
			}
			set
			{
				_activeSnippetColor = value;
			}
		}

		private int? _activeSnippetIndicator;
		public int? ActiveSnippetIndicator
		{
			get
			{
				return _activeSnippetIndicator;
			}
			set
			{
				_activeSnippetIndicator = value;
			}
		}

		private IndicatorStyle? _activeSnippetIndicatorStyle;
		public IndicatorStyle? ActiveSnippetIndicatorStyle
		{
			get
			{
				return _activeSnippetIndicatorStyle;
			}
			set
			{
				_activeSnippetIndicatorStyle = value;
			}
		}

		private IndicatorStyle? _inactiveSnippetIndicatorStyle;
		public IndicatorStyle? InactiveSnippetIndicatorStyle
		{
			get
			{
				return _inactiveSnippetIndicatorStyle;
			}
			set
			{
				_inactiveSnippetIndicatorStyle = value;
			}
		}


		private Color _inactiveSnippetColor;
		public Color InactiveSnippetColor
		{
			get
			{
				return _inactiveSnippetColor;
			}
			set
			{
				_inactiveSnippetColor = value;
			}
		}

		private int? _inactiveSnippetIndicator;
		public int? InactiveSnippetIndicator
		{
			get
			{
				return _inactiveSnippetIndicator;
			}
			set
			{
				_inactiveSnippetIndicator = value;
			}
		}

		private bool? _isEnabled;
		public bool? IsEnabled
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

		private char? _defaultDelimeter;
		public char? DefaultDelimeter
		{
			get
			{
				return _defaultDelimeter;
			}
			set
			{
				_defaultDelimeter = value;
			}
		}

		private bool? _isOneKeySelectionEmbedEnabled;
		public bool? IsOneKeySelectionEmbedEnabled
		{
			get
			{
				return _isOneKeySelectionEmbedEnabled;
			}
			set
			{
				_isOneKeySelectionEmbedEnabled = value;
			}
		}
	}

	public class SnippetsConfig
	{
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
			}
		}

		private char? _delimeter;
		public char? Delimeter
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

		//	Really all snippets can be used as SurroundsWith. The only
		//	thing this really controls is whether or not the snippet 
		//	appears in the Surrounds With List. Really.
		private bool? _isSurroundsWith;
		public bool? IsSurroundsWith
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
	}
}
