using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeuz
{
    public class PropVal
    {
        public PropVal() { }
    }

    public class ACLPermission
    {
        public ACLPermission() { }
    }

    public class Item : ItemCommon
    {
        public Item() { }

        public Item FromJSON(JSONObject jo)
        {
            string id=(string)jo["ID"];
            IDType typ=GetIDType(id);
            switch(typ)
            {
                case IDType.Account: return (Item)jo.ToObject(new Account(), false);
                case IDType.Proj: return (Item)jo.ToObject(new Proj(), false);
                case IDType.Env: return (Item)jo.ToObject(new Env(), false);
                //case IDType.User: return (Item)jo.ToObject(new User(), false);
                case IDType.Developer: return (Item)jo.ToObject(new Developer(), false);
                case IDType.Team: return (Item)jo.ToObject(new Team(), false);
                //case IDType.Lobby: return (Item)jo.ToObject(new Lobby(), false);
                //case IDType.MatchMaking: return (Item)jo.ToObject(new MatchMaking(), false);
                //case IDType.Message: return (Item)jo.ToObject(new Message(), false);
                //case IDType.Inbox: return (Item)jo.ToObject(new Inbox(), false);
            }
            return (Item)jo.ToObject(this, false);
        }

        public IDType GetIDType(string id)
        {
            if (id.Length != 27) { return IDType.Invalid; }

            char[] idbytes = new char[18];

            for (int i = 0; i < 9; i++)
            {
                int t = 0;
                for (int j = 0; j < 3; j++)
                {
                    char c = id[j + i * 3];

                    if (c >= 'a' && c <= 'z') { t = t * 52 + (int)(c - 'a'); }
                    else if (c >= 'A' && c <= 'Z') { t = t * 52 + (int)(c - 'A') + 26; }
                    else return IDType.Invalid;
                }
                idbytes[i * 2 + 0] = (char)(t & 0xff);
                idbytes[i * 2 + 1] = (char)((t >> 8) & 0xff);
            }
            char v = (char)0;
            for (int i = 0; i <= 16; i++)
            {
                v = (char)(v ^ idbytes[i]);
            }
            if (v != idbytes[17])
            {
                return IDType.Invalid;
            }
            return (IDType)(idbytes[16]);
        }

    }

} //namespace Zeuz