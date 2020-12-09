using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeuz
{

    public struct Timestamp
    {
        public long value;
        private static long offset;

        public Timestamp(long v=0) { value = v; }

        public static Timestamp Now(bool adjusted=true)
        {
            return FromDateTime(System.DateTime.Now, adjusted);
        }

        public static Timestamp FromDateTime(DateTime dt, bool adjusted = true)
        {
            Timestamp ret=new Timestamp(FileTimeToUnixUS(dt.ToFileTimeUtc() / 10) + UNIX_TOTS);
            if (adjusted) ret.value += offset;
            return ret;
        }

        public DateTime ToDateTime()
        {
            long ft=FileTimeFromUnixUS(value - UNIX_TOTS) * 10;
            return DateTime.FromFileTime(ft);
        }

        public Timestamp FromJSON(JSONObject jo) { value = (long)jo; return this; }
        public JSONObject ToJSON() { return new JSONObject(value); }

        public static void AdjustOffset(long o) { offset = o; }

        const long UNIX_TOTS=2208988800000000;
        const long EPOCH_TO_UNIX = 11644473600000000;
        static long FileTimeToUnixUS(long ft)   {return (ft - EPOCH_TO_UNIX);}
        static long FileTimeFromUnixUS(long ux) { return (ux + EPOCH_TO_UNIX); }
    }

} //namespace Zeuz