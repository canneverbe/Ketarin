using System;
using System.Collections.Generic;
using System.Text;

namespace ScintillaNet
{
    [Flags]
	public enum SearchFlags
	{
		Empty		= 0,
		WholeWord	= 2,
		MatchCase	= 4,
		WordStart	= 0x00100000,
		RegExp		= 0x00200000,
		Posix		= 0x00400000
	}
}
