using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#define READABLE

namespace Zeuz
{
    public class JSONObject
    {
        public enum Type { NULL, STRING, NUMBER, OBJECT, ARRAY, BOOL, ERROR }

        public bool isContainer { get { return (type == Type.ARRAY || type == Type.OBJECT); } }
        public bool isError { get { return (type == Type.ERROR); } }
        public JSONObject parent;
        public Type type = Type.NULL;
        public int Count { get { return list.Count; } }
        public List<JSONObject> list = null;
        public List<string> keys = null;
        public string str;
        public double n;
        public bool b;

        public static JSONObject nullJO { get { return new JSONObject(JSONObject.Type.NULL); } }
        public static JSONObject obj { get { return new JSONObject(JSONObject.Type.OBJECT); } }
        public static JSONObject arr { get { return new JSONObject(JSONObject.Type.ARRAY); } }

        public JSONObject(JSONObject.Type t)
        {
            type = t;
            switch (t)
            {
                case Type.ARRAY:
                    list = new List<JSONObject>();
                    break;

                case Type.OBJECT:
                    list = new List<JSONObject>();
                    keys = new List<string>();
                    break;
            }
        }

        public JSONObject(bool b)
        {
            type = Type.BOOL;
            this.b = b;
        }

        public JSONObject(double f)
        {
            type = Type.NUMBER;
            this.n = f;
        }

        public JSONObject(string s)
        {
            type = Type.STRING;
            this.str = s;
        }

        public JSONObject(Dictionary<string, string> dic)
        {
            type = Type.OBJECT;
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                keys.Add(kvp.Key);
                list.Add(new JSONObject { type = Type.STRING, str = kvp.Value });
            }
        }

        public void Absorb(JSONObject obj)
        {
            list.AddRange(obj.list);
            keys.AddRange(obj.keys);
            str = obj.str;
            n = obj.n;
            b = obj.b;
            type = obj.type;
        }

        public JSONObject()
        {
        }

        public static explicit operator string(JSONObject o)
        {
            if (o == null) return null;
            switch (o.type)
            {
                case Type.STRING: return o.str;
                case Type.NUMBER: return "" + o.n;
                case Type.BOOL: return o.b ? "true" : "false";
                case Type.ARRAY: return "Array";
                case Type.OBJECT: return "Object";
                case Type.NULL: return "null";
            };
            return "Unknown";
        }

        public static explicit operator bool(JSONObject o)
        {
            if (o == null) return false;
            switch (o.type)
            {
                case Type.STRING: return true;
                case Type.NUMBER: return o.n != 0;
                case Type.BOOL: return o.b;
                case Type.ARRAY: return true;
                case Type.OBJECT: return true;
            };
            return false;
        }

        public static explicit operator double(JSONObject o)
        {
            if (o == null) return 0;
            switch (o.type)
            {
                case Type.STRING:
                    double f = 0;
                    double.TryParse(o.str, out f);
                    return f;

                case Type.NUMBER: return o.n;
                case Type.BOOL: return o.b ? 1 : 0;
            };
            return 0;
        }

        public static explicit operator float(JSONObject o)
        {
            if (o == null) return 0;
            switch (o.type)
            {
                case Type.STRING:
                    float f = 0;
                    float.TryParse(o.str, out f);
                    return f;

                case Type.NUMBER: return (float)o.n;
                case Type.BOOL: return o.b ? 1 : 0;
            };
            return 0;
        }

        public static explicit operator int(JSONObject o)
        {
            if (o == null) return 0;
            switch (o.type)
            {
                case Type.STRING:
                    int i = 0;
                    int.TryParse(o.str, out i);
                    return i;

                case Type.NUMBER: return (int)o.n;
                case Type.BOOL: return o.b ? 1 : 0;
            };
            return 0;
        }

        public static explicit operator long(JSONObject o)
        {
            if (o == null) return 0;
            switch (o.type)
            {
                case Type.STRING:
                    long i = 0;
                    long.TryParse(o.str, out i);
                    return i;

                case Type.NUMBER: return (long)o.n;
                case Type.BOOL: return o.b ? 1 : 0;
            };
            return 0;
        }

        public void Add(bool val)
        {
            Add(new JSONObject(val));
        }

        public void Add(float val)
        {
            Add(new JSONObject(val));
        }

        public void Add(int val)
        {
            Add(new JSONObject(val));
        }

        public void Add(JSONObject obj)
        {
            if (obj!=null)
            {
                if (type != JSONObject.Type.ARRAY)
                {
                    type = JSONObject.Type.ARRAY;
                    Debug.LogWarning("tried to add an object to a non-array JSONObject.  We'll do it for you, but you might be doing something wrong.");
                }
                list.Add(obj);
            }
        }

        public void AddField(string name, bool val)
        {
            AddField(name, new JSONObject(val));
        }

        public void AddField(string name, float val)
        {
            AddField(name, new JSONObject(val));
        }

        public void AddField(string name, int val)
        {
            AddField(name, new JSONObject(val));
        }

        public void AddField(string name, string val)
        {
            AddField(name, new JSONObject { type = JSONObject.Type.STRING, str = val });
        }

        public void AddField(string name, JSONObject obj)
        {
            if (obj!=null)
            {
                if (type != JSONObject.Type.OBJECT)
                {
                    type = JSONObject.Type.OBJECT;
                    Debug.LogWarning("tried to add a field to a non-object JSONObject.  We'll do it for you, but you might be doing something wrong.");
                }
                keys.Add(name);
                list.Add(obj);
            }
        }

        public void SetField(string name, JSONObject obj)
        {
            if (HasField(name))
            {
                list.Remove(this[name]);
                keys.Remove(name);
            }
            AddField(name, obj);
        }

        public JSONObject GetField(string name)
        {
            if (type == JSONObject.Type.OBJECT)
                for (int i = 0; i < keys.Count; i++)
                    if ((string)keys[i] == name)
                        return (JSONObject)list[i];
            return null;
        }

        public bool HasField(string name)
        {
            if (type == JSONObject.Type.OBJECT)
                for (int i = 0; i < keys.Count; i++)
                    if ((string)keys[i] == name)
                        return true;
            return false;
        }

        public void Clear()
        {
            type = JSONObject.Type.NULL;
            list.Clear();
            keys.Clear();
            str = "";
            n = 0;
            b = false;
        }

        public JSONObject Copy()
        {
            JSONObject xCopy = new JSONObject(type);
            xCopy.Merge(this);
            return xCopy;
        }

        public void Merge(JSONObject obj)
        {
            MergeRecur(this, obj);
        }

        private static void MergeRecur(JSONObject left, JSONObject right)
        {
            if (left.type == JSONObject.Type.NULL)
                left.Absorb(right);
            else if (left.type == Type.OBJECT && right.type == Type.OBJECT)
            {
                for (int i = 0; i < right.list.Count; i++)
                {
                    string key = (string)right.keys[i];
                    if (right[i].isContainer)
                    {
                        if (left.HasField(key))
                            MergeRecur(left[key], right[i]);
                        else
                            left.AddField(key, right[i]);
                    }
                    else
                    {
                        if (left.HasField(key))
                            left.SetField(key, right[i]);
                        else
                            left.AddField(key, right[i]);
                    }
                }
            }
            else if (left.type == Type.ARRAY && right.type == Type.ARRAY)
            {
                if (right.Count > left.Count)
                {
                    Debug.LogError("Cannot merge arrays when right object has more elements");
                    return;
                }
                for (int i = 0; i < right.list.Count; i++)
                {
                    if (left[i].type == right[i].type)
                    {           //Only overwrite with the same type
                        if (left[i].isContainer)
                            MergeRecur(left[i], right[i]);
                        else
                        {
                            left[i] = right[i];
                        }
                    }
                }
            }
        }

        public string Print(bool pretty)
        {
            return Print(pretty,0);
        }

        public string Print(bool pretty, int depth)
        {   //Convert the JSONObject into a string
            if (depth++ > 1000)
            {
                Debug.Log("reached max depth!");
                return "";
            }
            string str = "";
            const string DoubleFixedPoint = "0.###################################################################################################################################################################################################################################################################################################################################################";
            switch (type)
            {
                case Type.STRING:
                    str = "\"" + this.str + "\"";
                    break;

                case Type.NUMBER:
                    if (n == Mathf.Infinity)
                        str = "+Inf";
                    else if (n == Mathf.NegativeInfinity)
                        str = "-Inf";
                    else if (n%1 == 0 && n>=long.MinValue && n<=long.MaxValue)
                        str += (long)n;
                    else
                        str += n.ToString(DoubleFixedPoint);
                    break;

                case JSONObject.Type.OBJECT:
                    if (list.Count > 0)
                    {
                        str = "{";
                        if (pretty)    //for a bit more readability, comment the define above to save space
                        {
                            str += "\n";
                            depth++;
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            //Debug.Log(">>"+type+" "+list.Count+" "+keys.Count);
                            if (i >= keys.Count) { break; };
                            string key = (string)keys[i];
                            JSONObject obj = (JSONObject)list[i];
                            if (obj!=null)
                            {
                                if (pretty)
                                {
                                    for (int j = 0; j < depth; j++)
                                        str += "\t"; //for a bit more readability
                                }
                                str += "\"" + key + "\":";
                                str += obj.Print(pretty,depth) + ",";
                                if (pretty)
                                {
                                    str += "\n";
                                }
                            }
                        }
                        if (pretty)
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                        str = str.Substring(0, str.Length - 1);
                        str += "}";
                    }
                    else str = "null";
                    break;

                case JSONObject.Type.ARRAY:
                    if (list.Count > 0)
                    {
                        str = "[";
                        if (pretty)
                        {
                            str += "\n"; //for a bit more readability
                            depth++;
                        }
                        foreach (JSONObject obj in list)
                        {
                            if (obj!=null)
                            {
                                if (pretty)
                                {
                                    for (int j = 0; j < depth; j++)
                                        str += "\t"; //for a bit more readability
                                }
                                str += obj.Print(pretty, depth) + ",";
                                if (pretty)
                                {
                                    str += "\n"; //for a bit more readability
                                }
                            }
                        }
                        if (pretty)
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                        str = str.Substring(0, str.Length - 1);
                        str += "]";
                    }
                    else str = "null";
                    break;

                case Type.BOOL:
                    if (b)
                        str = "true";
                    else
                        str = "false";
                    break;

                case Type.NULL:
                    str = "null";
                    break;
            }
            return str;
        }

        public JSONObject this[int index]
        {
            get
            {
                if (list.Count > index) return (JSONObject)list[index];
                else return null;
            }
            set
            {
                if (list.Count > index)
                    list[index] = value;
            }
        }

        public JSONObject this[string index]
        {
            get
            {
                return GetField(index);
            }
            set
            {
                SetField(index, value);
            }
        }

        public override string ToString()
        {
            return Print(false);
        }

        public string[] ToStringArray()
        {
            if (type == Type.ARRAY)
            {
                string[] result = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    result[i] = (string)this[i];
                }
                return result;
            }
            else Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
            return null;
        }

        public int[] ToIntArray()
        {
            if (type == Type.ARRAY)
            {
                int[] ret = new int[Count];
                for (int i = 0; i < Count; i++)
                {
                    ret[i] = (int)this[i];
                }
                return ret;
            }
            else Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
            return null;
        }

        public Dictionary<string, string> ToDictionary()
        {
            if (type == Type.OBJECT)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                for (int i = 0; i < list.Count; i++)
                {
                    JSONObject val = (JSONObject)list[i];
                    switch (val.type)
                    {
                        case Type.STRING: result.Add((string)keys[i], val.str); break;
                        case Type.NUMBER: result.Add((string)keys[i], val.n + ""); break;
                        case Type.BOOL: result.Add((string)keys[i], val.b + ""); break;
                        default: Debug.LogWarning("Omitting object: " + (string)keys[i] + " in dictionary conversion"); break;
                    }
                }
                return result;
            }
            else Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
            return null;
        }

        public static JSONObject FromObject(object o)
        {
            if (o == null) return JSONObject.nullJO;
            System.Type t = o.GetType();
            if (t == typeof(bool)) { return new JSONObject((bool)o); }
            if (t == typeof(string)) { return new JSONObject((string)o); }
            if (t == typeof(int)) { return new JSONObject((int)o); }
            if (t == typeof(long)) { return new JSONObject((long)o); }
            if (t == typeof(byte)) { return new JSONObject((byte)o); }
            if (t == typeof(sbyte)) { return new JSONObject((sbyte)o); }
            if (t == typeof(char)) { return new JSONObject((char)o); }
            if (t == typeof(decimal)) { return new JSONObject((double)(decimal)o); }
            if (t == typeof(double)) { return new JSONObject((double)o); }
            if (t == typeof(float)) { return new JSONObject((float)o); }
            if (t == typeof(uint)) { return new JSONObject((uint)o); }
            if (t == typeof(ulong)) { return new JSONObject((ulong)o); }
            if (t == typeof(short)) { return new JSONObject((short)o); }
            if (t == typeof(ushort)) { return new JSONObject((ushort)o); }
            if (t == typeof(JSONObject)) { return (JSONObject)o; }

            var methTo = t.GetMethod("ToJSON");
            if (methTo != null)
            {
                return (JSONObject)methTo.Invoke(o, null);
            }

            if (t.IsArray)
            {
                System.Array arr = (System.Array)o;
                JSONObject ret = JSONObject.arr;
                for (int i = 0; i < arr.Length; i++)
                {
                    ret.Add(FromObject(arr.GetValue(i)));
                }
                return ret;
            }
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            {
                JSONObject ret = JSONObject.arr;
                int n = (int)t.GetProperty("Count").GetValue(o, null);
                var methItem = t.GetProperty("Item");

                for (int i = 0; i < n; i++)
                {
                    object[] index = { i };
                    ret.Add(FromObject(methItem.GetValue(o, index)));
                }
                return ret;
            }
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                JSONObject ret = JSONObject.obj;
                IDictionaryEnumerator denum = ((IDictionary)o).GetEnumerator();
                while(denum.MoveNext())
                {
                    ret.AddField((string)denum.Key, FromObject(denum.Value));
                }
                return ret;
            }
            if ((t.IsClass || t.IsValueType))
            {
                JSONObject ret = JSONObject.obj;
                System.Reflection.FieldInfo[] members = t.GetFields();
                for (int i = 0; i < members.Length; i++)
                {
                    System.Reflection.FieldInfo mem = members[i];
                    string name = mem.Name;
                    ret.AddField(name, FromObject(mem.GetValue(o)));
                }
                return ret;
            }

            return JSONObject.nullJO;
        }


        public object ToObject(object o,bool allowFrom=true, System.Type t=null)
        {
            if(t==null&&o!=null) t = o.GetType();
            if (t == null) return o;

            if (t == typeof(bool)) { return (bool)this; }
            if (t == typeof(string)) { return (string)this;  }
            if (t == typeof(int)) { return (int)this;  }
            if (t == typeof(long)) { return (long)this;  }
            if (t == typeof(byte)) { return (byte)this;  }
            if (t == typeof(sbyte)) { return (sbyte)this;  }
            if (t == typeof(char)) { return (char)this;  }
            if (t == typeof(decimal)) { return (decimal)this;  }
            if (t == typeof(double)) { return (double)this;  }
            if (t == typeof(float)) { return (float)this;  }
            if (t == typeof(uint)) { return (uint)this;  }
            if (t == typeof(ulong)) { return (ulong)this;  }
            if (t == typeof(short)) { return (short)this;  }
            if (t == typeof(ushort)) { return (ushort)this;  }

            //propagate JSONObject for later deserialization
            if (t == typeof(object)) { return this; }
            if (t == typeof(JSONObject)) { return this; }

            if (o == null)
            {
                var ctor = t.GetConstructors()[0];
                var pms = ctor.GetParameters();
                var defparams = new object[pms.Length];
                for(int i=0;i< pms.Length;i++)
                {
                    //default parameters
                    defparams[i] = System.Type.Missing;
                }

                o = ctor.Invoke(defparams);
                //o = System.Activator.CreateInstance(t);
            }

            if (allowFrom)
            {
                var methFrom = t.GetMethod("FromJSON");
                if (methFrom != null)
                {
                    return methFrom.Invoke(o, new[] { this });
                }
            }
            if (t.IsArray&&type==Type.ARRAY)
            {
                System.Type et=t.GetElementType();
                System.Array arr = System.Array.CreateInstance(t, Count);
                for (int i = 0; i < Count; i++)
                {
                    object val = null;
                    try
                    {
                        val = System.Activator.CreateInstance(et);
                    }
                    catch (System.Exception /*e*/)
                    {
                        //maybe check
                        //Debug.Log(e);
                    }
                    val = this[i].ToObject(val,true,et);
                    arr.SetValue(val, i);
                }
                return arr;
            }
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>) && type == Type.ARRAY)
            {
                System.Type et=t.GetGenericArguments()[0];
                System.Type lt = typeof(List<>);
                o = System.Activator.CreateInstance(lt.MakeGenericType(et));
                var methAdd=o.GetType().GetMethod("Add");
                for (int i = 0; i < Count; i++)
                {
                    object val = null;
                    try
                    {
                        val = System.Activator.CreateInstance(et);
                    }
                    catch (System.Exception /*e*/)
                    {
                        //maybe check
                        //Debug.Log(e);
                    }
                    val=this[i].ToObject(val,true, et);
                    methAdd.Invoke(o, new[] { val });
                }
                return o;
            }
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && type == Type.OBJECT)
            {
                System.Type kt = t.GetGenericArguments()[0];
                System.Type vt = t.GetGenericArguments()[1];
                System.Type lt = typeof(Dictionary<,>);
                o = System.Activator.CreateInstance(lt.MakeGenericType(t.GetGenericArguments()));
                IDictionary dict = (IDictionary)o;

                for (int i = 0; i < Count; i++)
                {
                    object val = null;
                    try
                    {
                        val = System.Activator.CreateInstance(vt);
                    }
                    catch (System.Exception /*e*/)
                    {
                        //maybe check
                        //Debug.Log(e);
                    }
                    val = list[i].ToObject(val, true, vt);
                    dict[keys[i]] = val;
                }
                return o;
            }
            else if ((t.IsClass || t.IsValueType) && type == Type.NULL)
            {
                return null;
            }
            else if ((t.IsClass || t.IsValueType) && type == Type.OBJECT)
            {
                if (o == null) o = System.Activator.CreateInstance(t);

                System.Reflection.FieldInfo[] members = t.GetFields();
                for (int i = 0; i < members.Length; i++)
                {
                    System.Reflection.FieldInfo mem = members[i];
                    string name = mem.Name.ToLower().Replace("_", "");

                    for (int j = 0; j < keys.Count; j++)
                    {
                        string key = keys[j].ToLower().Replace("_", "");
                        if (key == name)
                        {
                            object val = mem.GetValue(o);
                            System.Type typ = null;
                            if (val == null) { typ = mem.FieldType; }
                            val = list[j].ToObject(val, true, typ);
                            mem.SetValue(o, val);
                            break;
                        }
                    }

                }
                return o;
            }

            return o;
        }
    }

    public class JSONParser
    {
        public class Error : JSONObject
        {
            public string Reason;
            public int Line;
            public int Col;

            public override string ToString() { return "JSON parse, ln:" + Line + " col:" + Col + " - " + Reason; }
        }

        private enum JsonToken
        {
            None = 0,
            CurlyOpen = 1,
            CurlyClose = 2,
            SquaredOpen = 3,
            SquaredClose = 4,
            Colon = 5,
            Comma = 6,
            String = 7,
            Number = 8,
            True = 9,
            False = 10,
            Null = 11,
            Letter = 12,
        };

        public static JSONObject parse(string json)
        {
            bool success = true;
            if (json == null || json.Length <= 0) { return null; };
            json = json.Replace("\r", "");
            int index = 0;
            JSONObject value = parseValue(json, ref index, ref success);
            return success ? value : null;
        }

        private static void GetLineCol(string json, int index, ref int line, ref int col)
        {
            line = 0;
            col = 0;
            for (int i = 0; i < json.Length && i < index; i++)
            {
                bool bNewLine = json[i] == '\n';
                if (bNewLine) { line++; col = 0; } else { col++; }
            }
        }

        private static JSONObject NewError(string json, int index, string reason)
        {
            JSONObject ret = new JSONObject(JSONObject.Type.ERROR); ;
            int Line = 0;
            int Col = 0;
            GetLineCol(json, index, ref Line, ref Col);
            ret.str= "JSON parse, ln:" + Line + " col:" + Col + " - " + reason;
            Debug.LogError(ret.str);
            return ret;
        }

        private static JSONObject parseValue(string json, ref int index, ref bool success)
        {
            JSONObject jobj;
            JsonToken tok = lookAhead(json, ref index);
            switch (tok)
            {
                case JsonToken.String:
                    return parseString(json, ref index, ref success);

                case JsonToken.Number:
                    return parseNumber(json, ref index);

                case JsonToken.CurlyOpen:
                    return parseObject(json, ref index, ref success);

                case JsonToken.SquaredOpen:
                    return parseArray(json, ref index, ref success);

                case JsonToken.True:
                    nextToken(json, ref index);
                    jobj = new JSONObject();
                    jobj.type = JSONObject.Type.BOOL;
                    jobj.b = true;
                    return jobj;

                case JsonToken.False:
                    nextToken(json, ref index);
                    jobj = new JSONObject();
                    jobj.type = JSONObject.Type.BOOL;
                    jobj.b = false;
                    return jobj;

                case JsonToken.Null:
                    nextToken(json, ref index);
                    jobj = new JSONObject();
                    jobj.type = JSONObject.Type.NULL;
                    return jobj;

                case JsonToken.None:
                    break;
            }

            //If there were no tokens, flag the failure and return an empty QVariant
            success = false;
            return NewError(json, index, "invtoken:" + tok.ToString());
        }

        private static JSONObject parseObject(string json, ref int index, ref bool success)
        {
            JSONObject jobj = new JSONObject(JSONObject.Type.OBJECT);

            JsonToken token;

            //Get rid of the whitespace and increment index
            nextToken(json, ref index);

            //Loop through all of the key/value pairs of the object
            bool done = false;
            while (!done)
            {
                //Get the upcoming token
                token = lookAhead(json, ref index);

                if (token == JsonToken.None)
                {
                    success = false;
                    return NewError(json, index, "Object: invtoken");
                }
                else if (token == JsonToken.Comma)
                {
                    nextToken(json, ref index);
                }
                else if (token == JsonToken.CurlyClose)
                {
                    nextToken(json, ref index);
                    return jobj;
                }
                else
                {
                    //Parse the key/value pair's name
                    string name = parseString(json, ref index, ref success).str;
                    if (!success) { return jobj; }
                    //Get the next token
                    token = nextToken(json, ref index);

                    //If the next token is not a colon, flag the failure
                    //return an empty QVariant
                    if (token != JsonToken.Colon)
                    {
                        success = false;
                        return NewError(json, index, "Object: colon expected");
                    }

                    //Parse the key/value pair's value
                    JSONObject value = parseValue(json, ref index, ref success);
                    if (!success) { return jobj; }

                    //Assign the value to the key in the map
                    jobj.keys.Add(name);
                    jobj.list.Add(value);
                }
            }

            //Return the map successfully
            return jobj;
        }

        private static JSONObject parseArray(string json, ref int index, ref bool success)
        {
            JSONObject jobj = new JSONObject(JSONObject.Type.ARRAY);

            nextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                JsonToken token = lookAhead(json, ref index);

                if (token == JsonToken.None)
                {
                    success = false;
                    return jobj;
                }
                else if (token == JsonToken.Comma)
                {
                    nextToken(json, ref index);
                }
                else if (token == JsonToken.SquaredClose)
                {
                    nextToken(json, ref index);
                    break;
                }
                else
                {
                    JSONObject value = parseValue(json, ref index, ref success);
                    if (!success) { return jobj; }
                    jobj.list.Add(value);
                }
            }

            return jobj;
        }

        private static JSONObject parseString(string json, ref int index, ref bool success)
        {
            string s = "";
            char c;

            eatWhitespace(json, ref index);

            c = json[index++];
            bool bQuoted = (c == '"');
            if (!bQuoted) { index--; };

            bool complete = false;
            while (!complete)
            {
                if (index == json.Length) { break; };

                c = json[index++];

                if (!bQuoted)
                {
                    if (!(char.IsLetterOrDigit(c) || c == '_'))
                    {
                        complete = true;
                        index--;
                        break;
                    };
                };

                if (c == '\"')
                {
                    complete = true;
                    break;
                }
                else if (c == '\\')
                {
                    if (index == json.Length)
                    {
                        break;
                    }

                    c = json[index++];

                    if (c == '\"')
                    {
                        s += ('\"');
                    }
                    else if (c == '\\')
                    {
                        s += ('\\');
                    }
                    else if (c == '/')
                    {
                        s += ('/');
                    }
                    else if (c == 'b')
                    {
                        s += ('\b');
                    }
                    else if (c == 'f')
                    {
                        s += ('\f');
                    }
                    else if (c == 'n')
                    {
                        s += ('\n');
                    }
                    else if (c == 'r')
                    {
                        s += ('\r');
                    }
                    else if (c == 't')
                    {
                        s += ('\t');
                    }
                }
                else
                {
                    s += (c);
                }
            }

            if (!complete)
            {
                success = false;
                return NewError(json, index, "String: incomplete");
            }

            JSONObject jobj = new JSONObject();
            jobj.str = s;
            jobj.type = JSONObject.Type.STRING;
            return jobj;
        }

        private static JSONObject parseNumber(string json, ref int index)
        {
            eatWhitespace(json, ref index);

            int lastIndex = lastIndexOfNumber(json, ref index);
            int charLength = (lastIndex - index) + 1;
            string numberStr;

            numberStr = json.Substring(index, charLength);

            index = lastIndex + 1;

            JSONObject jobj = new JSONObject();
            jobj.n = System.Convert.ToDouble(numberStr);
            jobj.type = JSONObject.Type.NUMBER;
            return jobj;
        }

        private static int lastIndexOfNumber(string json, ref int index)
        {
            int lastIndex;

            for (lastIndex = index; lastIndex < json.Length; lastIndex++)
            {
                if (("0123456789+-.eE").IndexOf(json[lastIndex]) == -1)
                {
                    break;
                }
            }

            return lastIndex - 1;
        }

        private static void eatWhitespace(string json, ref int index)
        {
            bool bInLineComment = false;
            bool bInBlockComment = false;

            for (; index < json.Length; index++)
            {
                char c = json[index];
                if (bInLineComment && c == '\n') { bInLineComment = false; continue; };
                if (bInBlockComment && c == '*' && index < json.Length - 1 && json[index + 1] == '/') { bInBlockComment = false; index++; continue; };
                if (bInLineComment || bInBlockComment) { continue; };

                if ((" \t\n\r").IndexOf(c) == -1)
                {
                    //line comment, skip to end of line
                    if (c == '/' && index < json.Length - 1)
                    {
                        if (json[index + 1] == '/') { bInLineComment = true; index++; };
                        if (json[index + 1] == '*')
                        {
                            bInBlockComment = true; index++;
                        };
                    };

                    if (!bInLineComment && !bInBlockComment) { break; };
                };
            };
        }

        private static JsonToken lookAhead(string json, ref int index)
        {
            int saveIndex = index;
            return nextToken(json, ref saveIndex);
        }

        private static JsonToken nextToken(string json, ref int index)
        {
            eatWhitespace(json, ref index);

            if (index == json.Length)
            {
                return JsonToken.None;
            }

            char c = json[index];
            index++;
            switch (c)
            {
                case '{': return JsonToken.CurlyOpen;
                case '}': return JsonToken.CurlyClose;
                case '[': return JsonToken.SquaredOpen;
                case ']': return JsonToken.SquaredClose;
                case ',': return JsonToken.Comma;
                case '"': return JsonToken.String;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-': return JsonToken.Number;
                case ':': return JsonToken.Colon;
            }
            index--;

            int remainingLength = json.Length - index;

            //True
            if (remainingLength >= 4)
            {
                if (json[index] == 't' && json[index + 1] == 'r' &&
                        json[index + 2] == 'u' && json[index + 3] == 'e')
                {
                    index += 4;
                    return JsonToken.True;
                }
            }

            //False
            if (remainingLength >= 5)
            {
                if (json[index] == 'f' && json[index + 1] == 'a' &&
                        json[index + 2] == 'l' && json[index + 3] == 's' &&
                        json[index + 4] == 'e')
                {
                    index += 5;
                    return JsonToken.False;
                }
            }

            //Null
            if (remainingLength >= 4)
            {
                if (json[index] == 'n' && json[index + 1] == 'u' &&
                        json[index + 2] == 'l' && json[index + 3] == 'l')
                {
                    index += 4;
                    return JsonToken.Null;
                }
            }

            if (char.IsLetter(c) || c == '_') { index++; return JsonToken.Letter; };

            return JsonToken.None;
        }
    }

} //namespace Zeuz