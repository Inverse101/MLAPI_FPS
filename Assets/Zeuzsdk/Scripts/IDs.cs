using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeuz
{
    public enum IDType : byte
    {
        Invalid = 0,
	    Account = 0x01,
	    Proj = 0x10,
	    Env = 0x11,
	    User = 0x12,
	    Developer = 0x13,
	    Team = 0x14,
	    Session = 0x20,
	    TempFile = 0x31,
	    Lobby = 0x32,
	    MatchMaking = 0x33,
	    Message = 0x34,
	    Inbox = 0x35,
	    ConfigRule = 0x40,
    };

} //namespace Zeuz