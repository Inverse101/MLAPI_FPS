// Code generated by "apigen"; DO NOT EDIT.
//Service Locality
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeuz
{


public class LocalityLocationGetIn
{
	public List<string> LocationIDs=new List<string>();
	public List<string> DisplayNames=new List<string>();
	public List<string> Providers=new List<string>();
	public List<string> LocationType=new List<string>();
	public string WithinRegion="";
	public LocalityLocationGetIn(List<string> _locationids=null,List<string> _displaynames=null,List<string> _providers=null,List<string> _locationtype=null,string _withinregion="") {if(_locationids!=null)LocationIDs=_locationids;if(_displaynames!=null)DisplayNames=_displaynames;if(_providers!=null)Providers=_providers;if(_locationtype!=null)LocationType=_locationtype;WithinRegion=_withinregion;}
	public LocalityLocationGetIn(LocalityLocationGetIn _copy) { if (_copy == null) return;if(_copy.LocationIDs!=null)LocationIDs=_copy.LocationIDs;if(_copy.DisplayNames!=null)DisplayNames=_copy.DisplayNames;if(_copy.Providers!=null)Providers=_copy.Providers;if(_copy.LocationType!=null)LocationType=_copy.LocationType;WithinRegion=_copy.WithinRegion;}
};

public class Location : ItemCommon
{
	public string LocationID="";
	public List<string> Regions=new List<string>();
	public string DisplayName="";
	public string Provider="";
	public string Type="";
	public float Ranking=0;
	public List<string> PingServer=new List<string>();
	public string ProvisionTemplateName="";
	public Dictionary<string,string> ProvisionTemplateParams=new Dictionary<string,string>();
	public string ProvisionCluster="";
	public Location(string _locationid="",List<string> _regions=null,string _displayname="",string _provider="",string _type="",float _ranking=0,List<string> _pingserver=null,string _provisiontemplatename="",Dictionary<string,string> _provisiontemplateparams=null,string _provisioncluster="",ItemCommon _base=null) : base(_base) {LocationID=_locationid;if(_regions!=null)Regions=_regions;DisplayName=_displayname;Provider=_provider;Type=_type;Ranking=_ranking;if(_pingserver!=null)PingServer=_pingserver;ProvisionTemplateName=_provisiontemplatename;if(_provisiontemplateparams!=null)ProvisionTemplateParams=_provisiontemplateparams;ProvisionCluster=_provisioncluster;}
	public Location(Location _copy) { if (_copy == null) return;LocationID=_copy.LocationID;if(_copy.Regions!=null)Regions=_copy.Regions;DisplayName=_copy.DisplayName;Provider=_copy.Provider;Type=_copy.Type;Ranking=_copy.Ranking;if(_copy.PingServer!=null)PingServer=_copy.PingServer;ProvisionTemplateName=_copy.ProvisionTemplateName;if(_copy.ProvisionTemplateParams!=null)ProvisionTemplateParams=_copy.ProvisionTemplateParams;ProvisionCluster=_copy.ProvisionCluster;}
};

public class LocalityRegionGetIn
{
	public List<string> RegionIDs=new List<string>();
	public List<string> DisplayNames=new List<string>();
	public List<string> ContainsLocations=new List<string>();
	public LocalityRegionGetIn(List<string> _regionids=null,List<string> _displaynames=null,List<string> _containslocations=null) {if(_regionids!=null)RegionIDs=_regionids;if(_displaynames!=null)DisplayNames=_displaynames;if(_containslocations!=null)ContainsLocations=_containslocations;}
	public LocalityRegionGetIn(LocalityRegionGetIn _copy) { if (_copy == null) return;if(_copy.RegionIDs!=null)RegionIDs=_copy.RegionIDs;if(_copy.DisplayNames!=null)DisplayNames=_copy.DisplayNames;if(_copy.ContainsLocations!=null)ContainsLocations=_copy.ContainsLocations;}
};

public class Region : ItemCommon
{
	public string RegionID="";
	public string DisplayName="";
	public Region(string _regionid="",string _displayname="",ItemCommon _base=null) : base(_base) {RegionID=_regionid;DisplayName=_displayname;}
	public Region(Region _copy) { if (_copy == null) return;RegionID=_copy.RegionID;DisplayName=_copy.DisplayName;}
};

public class LocalityProviderGetIn
{
	public List<string> ProviderIDs=new List<string>();
	public List<string> Names=new List<string>();
	public bool GetClientProvisionable=false;
	public bool GetNonClientProvisionable=false;
	public LocalityProviderGetIn(List<string> _providerids=null,List<string> _names=null,bool _getclientprovisionable=false,bool _getnonclientprovisionable=false) {if(_providerids!=null)ProviderIDs=_providerids;if(_names!=null)Names=_names;GetClientProvisionable=_getclientprovisionable;GetNonClientProvisionable=_getnonclientprovisionable;}
	public LocalityProviderGetIn(LocalityProviderGetIn _copy) { if (_copy == null) return;if(_copy.ProviderIDs!=null)ProviderIDs=_copy.ProviderIDs;if(_copy.Names!=null)Names=_copy.Names;GetClientProvisionable=_copy.GetClientProvisionable;GetNonClientProvisionable=_copy.GetNonClientProvisionable;}
};

public class Provider : ItemCommon
{
	public string ProviderID="";
	public string Name="";
	public bool ClientProvisionable=false;
	public bool ProvisioningAllowed=false;
	public Provider(string _providerid="",string _name="",bool _clientprovisionable=false,bool _provisioningallowed=false,ItemCommon _base=null) : base(_base) {ProviderID=_providerid;Name=_name;ClientProvisionable=_clientprovisionable;ProvisioningAllowed=_provisioningallowed;}
	public Provider(Provider _copy) { if (_copy == null) return;ProviderID=_copy.ProviderID;Name=_copy.Name;ClientProvisionable=_copy.ClientProvisionable;ProvisioningAllowed=_copy.ProvisioningAllowed;}
};


public class ApiLocality
{ 
	public delegate void LocationGetDelegate(List<Location> result, string error);
    public class LocationGetRequest {  public List<Location> Result=new List<Location>();public string Error=""; public Request rq;
        public void Start(LocationGetDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(LocationGetDelegate del = null) { yield return rq.Send(); LocationGetDone(this); del?.Invoke(Result, Error); }
    }

    public static LocationGetRequest LocationGet(LocalityLocationGetIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        LocationGetRequest rq=new LocationGetRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_location_get", input);
        return rq;
    }

    public static void LocationGetDone(LocationGetRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (List<Location>)rd.ToObject(new List<Location>());
        }
    }


	public delegate void LocationCreateDelegate(Location result, string error);
    public class LocationCreateRequest {  public Location Result=new Location();public string Error=""; public Request rq;
        public void Start(LocationCreateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(LocationCreateDelegate del = null) { yield return rq.Send(); LocationCreateDone(this); del?.Invoke(Result, Error); }
    }

    public static LocationCreateRequest LocationCreate(Location input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        LocationCreateRequest rq=new LocationCreateRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_location_create", input);
        return rq;
    }

    public static void LocationCreateDone(LocationCreateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (Location)rd.ToObject(new Location());
        }
    }


	public delegate void LocationUpdateDelegate(Location result, string error);
    public class LocationUpdateRequest {  public Location Result=new Location();public string Error=""; public Request rq;
        public void Start(LocationUpdateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(LocationUpdateDelegate del = null) { yield return rq.Send(); LocationUpdateDone(this); del?.Invoke(Result, Error); }
    }

    public static LocationUpdateRequest LocationUpdate(Location input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        LocationUpdateRequest rq=new LocationUpdateRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_location_update", input);
        return rq;
    }

    public static void LocationUpdateDone(LocationUpdateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (Location)rd.ToObject(new Location());
        }
    }


	public delegate void LocationRemoveDelegate(string error);
    public class LocationRemoveRequest { public string Error=""; public Request rq;
        public void Start(LocationRemoveDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(LocationRemoveDelegate del = null) { yield return rq.Send(); LocationRemoveDone(this); del?.Invoke(Error); }
    }

    public static LocationRemoveRequest LocationRemove(string input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        LocationRemoveRequest rq=new LocationRemoveRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_location_remove", input);
        return rq;
    }

    public static void LocationRemoveDone(LocationRemoveRequest rq)
    {
        rq.Error = rq.rq.Error;
    }


	public delegate void RegionGetDelegate(List<Region> result, string error);
    public class RegionGetRequest {  public List<Region> Result=new List<Region>();public string Error=""; public Request rq;
        public void Start(RegionGetDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(RegionGetDelegate del = null) { yield return rq.Send(); RegionGetDone(this); del?.Invoke(Result, Error); }
    }

    public static RegionGetRequest RegionGet(LocalityRegionGetIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        RegionGetRequest rq=new RegionGetRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_region_get", input);
        return rq;
    }

    public static void RegionGetDone(RegionGetRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (List<Region>)rd.ToObject(new List<Region>());
        }
    }


	public delegate void RegionCreateDelegate(Region result, string error);
    public class RegionCreateRequest {  public Region Result=new Region();public string Error=""; public Request rq;
        public void Start(RegionCreateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(RegionCreateDelegate del = null) { yield return rq.Send(); RegionCreateDone(this); del?.Invoke(Result, Error); }
    }

    public static RegionCreateRequest RegionCreate(Region input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        RegionCreateRequest rq=new RegionCreateRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_region_create", input);
        return rq;
    }

    public static void RegionCreateDone(RegionCreateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (Region)rd.ToObject(new Region());
        }
    }


	public delegate void RegionUpdateDelegate(Region result, string error);
    public class RegionUpdateRequest {  public Region Result=new Region();public string Error=""; public Request rq;
        public void Start(RegionUpdateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(RegionUpdateDelegate del = null) { yield return rq.Send(); RegionUpdateDone(this); del?.Invoke(Result, Error); }
    }

    public static RegionUpdateRequest RegionUpdate(Region input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        RegionUpdateRequest rq=new RegionUpdateRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_region_update", input);
        return rq;
    }

    public static void RegionUpdateDone(RegionUpdateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (Region)rd.ToObject(new Region());
        }
    }


	public delegate void RegionRemoveDelegate(string error);
    public class RegionRemoveRequest { public string Error=""; public Request rq;
        public void Start(RegionRemoveDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(RegionRemoveDelegate del = null) { yield return rq.Send(); RegionRemoveDone(this); del?.Invoke(Error); }
    }

    public static RegionRemoveRequest RegionRemove(string input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        RegionRemoveRequest rq=new RegionRemoveRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_region_remove", input);
        return rq;
    }

    public static void RegionRemoveDone(RegionRemoveRequest rq)
    {
        rq.Error = rq.rq.Error;
    }


	public delegate void ProviderGetDelegate(List<Provider> result, string error);
    public class ProviderGetRequest {  public List<Provider> Result=new List<Provider>();public string Error=""; public Request rq;
        public void Start(ProviderGetDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(ProviderGetDelegate del = null) { yield return rq.Send(); ProviderGetDone(this); del?.Invoke(Result, Error); }
    }

    public static ProviderGetRequest ProviderGet(LocalityProviderGetIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        ProviderGetRequest rq=new ProviderGetRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_provider_get", input);
        return rq;
    }

    public static void ProviderGetDone(ProviderGetRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (List<Provider>)rd.ToObject(new List<Provider>());
        }
    }


	public delegate void ProviderCreateDelegate(Provider result, string error);
    public class ProviderCreateRequest {  public Provider Result=new Provider();public string Error=""; public Request rq;
        public void Start(ProviderCreateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(ProviderCreateDelegate del = null) { yield return rq.Send(); ProviderCreateDone(this); del?.Invoke(Result, Error); }
    }

    public static ProviderCreateRequest ProviderCreate(Provider input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        ProviderCreateRequest rq=new ProviderCreateRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_provider_create", input);
        return rq;
    }

    public static void ProviderCreateDone(ProviderCreateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (Provider)rd.ToObject(new Provider());
        }
    }


	public delegate void ProviderUpdateDelegate(Provider result, string error);
    public class ProviderUpdateRequest {  public Provider Result=new Provider();public string Error=""; public Request rq;
        public void Start(ProviderUpdateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(ProviderUpdateDelegate del = null) { yield return rq.Send(); ProviderUpdateDone(this); del?.Invoke(Result, Error); }
    }

    public static ProviderUpdateRequest ProviderUpdate(Provider input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        ProviderUpdateRequest rq=new ProviderUpdateRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_provider_update", input);
        return rq;
    }

    public static void ProviderUpdateDone(ProviderUpdateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (Provider)rd.ToObject(new Provider());
        }
    }


	public delegate void ProviderRemoveDelegate(string error);
    public class ProviderRemoveRequest { public string Error=""; public Request rq;
        public void Start(ProviderRemoveDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(ProviderRemoveDelegate del = null) { yield return rq.Send(); ProviderRemoveDone(this); del?.Invoke(Error); }
    }

    public static ProviderRemoveRequest ProviderRemove(string input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        ProviderRemoveRequest rq=new ProviderRemoveRequest();
        rq.rq = Client.CreateRequest(ctx, "locality_provider_remove", input);
        return rq;
    }

    public static void ProviderRemoveDone(ProviderRemoveRequest rq)
    {
        rq.Error = rq.rq.Error;
    }


}

} //namespace Zeuz