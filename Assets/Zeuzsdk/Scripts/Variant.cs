using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeuz
{
    public class Variant
    {
        public object Value=null;

        public Variant() {}
        public Variant(object _value) { Value = _value; }

        public string GetString()
        {
            if (Value == null) return "";
            return (string)JSONObject.FromObject(Value);
        }
        public bool GetBool()
        {
            if (Value == null) return false;
            return (bool)JSONObject.FromObject(Value);
        }
        public float GetFloat()
        {
            if (Value == null) return 0;
            return (float)JSONObject.FromObject(Value);
        }
        public double GetDouble()
        {
            if (Value == null) return 0;
            return (double)JSONObject.FromObject(Value);
        }
        public int GetInt()
        {
            if (Value == null) return 0;
            return (int)JSONObject.FromObject(Value);
        }
        public long GetLong()
        {
            if (Value == null) return 0;
            return (long)JSONObject.FromObject(Value);
        }
        public JSONObject GetJSONObject()
        {
            return JSONObject.FromObject(Value);
        }

        public Variant FromJSON(JSONObject jo) { Value=jo; return this; }
        public JSONObject ToJSON() { return GetJSONObject(); }

    };

};
