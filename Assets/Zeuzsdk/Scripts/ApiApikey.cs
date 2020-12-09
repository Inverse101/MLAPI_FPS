// Code generated by "apigen"; DO NOT EDIT.
//Service Apikey
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeuz
{


public class APIKey : ItemCommon
{
	public string APIKeyID="";
	public string Key="";
	public string ProjID="";
	public string PWHash=""; //json:",omitempty"
	public bool Revoked=false;
	public Timestamp RevokedAt=new Timestamp();
	public Timestamp LastLogin=new Timestamp();
	public APIKey(string _apikeyid="",string _key="",string _projid="",string _pwhash="",bool _revoked=false,Timestamp _revokedat=new Timestamp(),Timestamp _lastlogin=new Timestamp(),ItemCommon _base=null) : base(_base) {APIKeyID=_apikeyid;Key=_key;ProjID=_projid;PWHash=_pwhash;Revoked=_revoked;RevokedAt=_revokedat;LastLogin=_lastlogin;}
	public APIKey(APIKey _copy) { if (_copy == null) return;APIKeyID=_copy.APIKeyID;Key=_copy.Key;ProjID=_copy.ProjID;PWHash=_copy.PWHash;Revoked=_copy.Revoked;RevokedAt=_copy.RevokedAt;LastLogin=_copy.LastLogin;}
};

public class ApikeyKeyGenerateIn
{
	public string ProjID=""; //arg:"required"
	public string Title=""; //arg:"required" help:"Display name, also shown in zeuz control panel."
	public ApikeyKeyGenerateIn(string _projid="",string _title="") {ProjID=_projid;Title=_title;}
	public ApikeyKeyGenerateIn(ApikeyKeyGenerateIn _copy) { if (_copy == null) return;ProjID=_copy.ProjID;Title=_copy.Title;}
};

public class ApikeyKeyPwHashIn
{
	public string APIKeyID=""; //arg:"required"
	public string ProjID=""; //arg:"required"
	public string PWHash=""; //arg:"required"
	public ApikeyKeyPwHashIn(string _apikeyid="",string _projid="",string _pwhash="") {APIKeyID=_apikeyid;ProjID=_projid;PWHash=_pwhash;}
	public ApikeyKeyPwHashIn(ApikeyKeyPwHashIn _copy) { if (_copy == null) return;APIKeyID=_copy.APIKeyID;ProjID=_copy.ProjID;PWHash=_copy.PWHash;}
};

public class ApikeyKeyGetIn
{
	public string APIKeyID=""; //arg:"" help:"APIKeyID"
	public string ProjID=""; //arg:"" help:"ProjID"
	public bool NotRevoked=false; //arg:"" help:"Only valid ones" default:"true"
	public ApikeyKeyGetIn(string _apikeyid="",string _projid="",bool _notrevoked=false) {APIKeyID=_apikeyid;ProjID=_projid;NotRevoked=_notrevoked;}
	public ApikeyKeyGetIn(ApikeyKeyGetIn _copy) { if (_copy == null) return;APIKeyID=_copy.APIKeyID;ProjID=_copy.ProjID;NotRevoked=_copy.NotRevoked;}
};

public class ApikeyKeyDeleteIn
{
	public string APIKeyID=""; //arg:"required"
	public string ProjID=""; //arg:"required"
	public ApikeyKeyDeleteIn(string _apikeyid="",string _projid="") {APIKeyID=_apikeyid;ProjID=_projid;}
	public ApikeyKeyDeleteIn(ApikeyKeyDeleteIn _copy) { if (_copy == null) return;APIKeyID=_copy.APIKeyID;ProjID=_copy.ProjID;}
};


public class ApiApikey
{ 
	public delegate void CreateDelegate(APIKey result, string error);
    public class CreateRequest {  public APIKey Result=new APIKey();public string Error=""; public Request rq;
        public void Start(CreateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(CreateDelegate del = null) { yield return rq.Send(); CreateDone(this); del?.Invoke(Result, Error); }
    }

    public static CreateRequest Create(APIKey input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        CreateRequest rq=new CreateRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_create", input);
        return rq;
    }

    public static void CreateDone(CreateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (APIKey)rd.ToObject(new APIKey());
        }
    }


	public delegate void GenerateDelegate(APIKey result, string error);
    public class GenerateRequest {  public APIKey Result=new APIKey();public string Error=""; public Request rq;
        public void Start(GenerateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(GenerateDelegate del = null) { yield return rq.Send(); GenerateDone(this); del?.Invoke(Result, Error); }
    }

    public static GenerateRequest Generate(ApikeyKeyGenerateIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        GenerateRequest rq=new GenerateRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_generate", input);
        return rq;
    }

    public static void GenerateDone(GenerateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (APIKey)rd.ToObject(new APIKey());
        }
    }


	public delegate void SetpwhashDelegate(APIKey result, string error);
    public class SetpwhashRequest {  public APIKey Result=new APIKey();public string Error=""; public Request rq;
        public void Start(SetpwhashDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(SetpwhashDelegate del = null) { yield return rq.Send(); SetpwhashDone(this); del?.Invoke(Result, Error); }
    }

    public static SetpwhashRequest Setpwhash(ApikeyKeyPwHashIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        SetpwhashRequest rq=new SetpwhashRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_setpwhash", input);
        return rq;
    }

    public static void SetpwhashDone(SetpwhashRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (APIKey)rd.ToObject(new APIKey());
        }
    }


	public delegate void GetallDelegate(List<APIKey> result, string error);
    public class GetallRequest {  public List<APIKey> Result=new List<APIKey>();public string Error=""; public Request rq;
        public void Start(GetallDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(GetallDelegate del = null) { yield return rq.Send(); GetallDone(this); del?.Invoke(Result, Error); }
    }

    public static GetallRequest Getall(ApikeyKeyGetIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        GetallRequest rq=new GetallRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_getall", input);
        return rq;
    }

    public static void GetallDone(GetallRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (List<APIKey>)rd.ToObject(new List<APIKey>());
        }
    }


	public delegate void GetDelegate(List<APIKey> result, string error);
    public class GetRequest {  public List<APIKey> Result=new List<APIKey>();public string Error=""; public Request rq;
        public void Start(GetDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(GetDelegate del = null) { yield return rq.Send(); GetDone(this); del?.Invoke(Result, Error); }
    }

    public static GetRequest Get(string input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        GetRequest rq=new GetRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_get", input);
        return rq;
    }

    public static void GetDone(GetRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (List<APIKey>)rd.ToObject(new List<APIKey>());
        }
    }


	public delegate void UpdateDelegate(APIKey result, string error);
    public class UpdateRequest {  public APIKey Result=new APIKey();public string Error=""; public Request rq;
        public void Start(UpdateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(UpdateDelegate del = null) { yield return rq.Send(); UpdateDone(this); del?.Invoke(Result, Error); }
    }

    public static UpdateRequest Update(APIKey input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        UpdateRequest rq=new UpdateRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_update", input);
        return rq;
    }

    public static void UpdateDone(UpdateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (APIKey)rd.ToObject(new APIKey());
        }
    }


	public delegate void UpdateaclDelegate(APIKey result, string error);
    public class UpdateaclRequest {  public APIKey Result=new APIKey();public string Error=""; public Request rq;
        public void Start(UpdateaclDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(UpdateaclDelegate del = null) { yield return rq.Send(); UpdateaclDone(this); del?.Invoke(Result, Error); }
    }

    public static UpdateaclRequest Updateacl(APIKey input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        UpdateaclRequest rq=new UpdateaclRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_updateacl", input);
        return rq;
    }

    public static void UpdateaclDone(UpdateaclRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (APIKey)rd.ToObject(new APIKey());
        }
    }


	public delegate void DeleteDelegate(string error);
    public class DeleteRequest { public string Error=""; public Request rq;
        public void Start(DeleteDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(DeleteDelegate del = null) { yield return rq.Send(); DeleteDone(this); del?.Invoke(Error); }
    }

    public static DeleteRequest Delete(ApikeyKeyDeleteIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        DeleteRequest rq=new DeleteRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_delete", input);
        return rq;
    }

    public static void DeleteDone(DeleteRequest rq)
    {
        rq.Error = rq.rq.Error;
    }


	public delegate void RevokeDelegate(string error);
    public class RevokeRequest { public string Error=""; public Request rq;
        public void Start(RevokeDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(RevokeDelegate del = null) { yield return rq.Send(); RevokeDone(this); del?.Invoke(Error); }
    }

    public static RevokeRequest Revoke(ApikeyKeyDeleteIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        RevokeRequest rq=new RevokeRequest();
        rq.rq = Client.CreateRequest(ctx, "apikey_revoke", input);
        return rq;
    }

    public static void RevokeDone(RevokeRequest rq)
    {
        rq.Error = rq.rq.Error;
    }


}

} //namespace Zeuz