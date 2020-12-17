// Code generated by "apigen"; DO NOT EDIT.
//Service Allocation
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeuz
{


public class AllocationPagination
{
	public bool GetTotal=false;
	public int Offset=0;
	public int Limit=0;
	public string OrderBy=""; //arg:"required" help:"Possible values: <code>'payloadid'</code>, <code>'machineid'</code>, <code>'type'</code>, <code>'status'</code>"
	public bool OrderAsc=false;
	public AllocationPagination(bool _gettotal=false,int _offset=0,int _limit=0,string _orderby="",bool _orderasc=false) {GetTotal=_gettotal;Offset=_offset;Limit=_limit;OrderBy=_orderby;OrderAsc=_orderasc;}
	public AllocationPagination(AllocationPagination _copy) { if (_copy == null) return;GetTotal=_copy.GetTotal;Offset=_copy.Offset;Limit=_copy.Limit;OrderBy=_copy.OrderBy;OrderAsc=_copy.OrderAsc;}
};

public class AllocationGetIn : AllocationPagination
{
	public List<string> AllocationIDs=new List<string>();
	public bool GetDisabled=false;
	public bool GetEnabled=false;
	public string Region="";
	public string OS="";
	public AllocationGetIn(List<string> _allocationids=null,bool _getdisabled=false,bool _getenabled=false,string _region="",string _os="",AllocationPagination _base=null) : base(_base) {if(_allocationids!=null)AllocationIDs=_allocationids;GetDisabled=_getdisabled;GetEnabled=_getenabled;Region=_region;OS=_os;}
	public AllocationGetIn(AllocationGetIn _copy) { if (_copy == null) return;if(_copy.AllocationIDs!=null)AllocationIDs=_copy.AllocationIDs;GetDisabled=_copy.GetDisabled;GetEnabled=_copy.GetEnabled;Region=_copy.Region;OS=_copy.OS;}
};

public class MetricsEvalCondition
{
	public int TimeFrame=0;
	public double CoreUsageThreshold=0;
	public double MemoryUsageThreshold=0;
	public double IOBandwidthThreshold=0;
	public double InetBandwidthThreshold=0;
	public MetricsEvalCondition(int _timeframe=0,double _coreusagethreshold=0,double _memoryusagethreshold=0,double _iobandwidththreshold=0,double _inetbandwidththreshold=0) {TimeFrame=_timeframe;CoreUsageThreshold=_coreusagethreshold;MemoryUsageThreshold=_memoryusagethreshold;IOBandwidthThreshold=_iobandwidththreshold;InetBandwidthThreshold=_inetbandwidththreshold;}
	public MetricsEvalCondition(MetricsEvalCondition _copy) { if (_copy == null) return;TimeFrame=_copy.TimeFrame;CoreUsageThreshold=_copy.CoreUsageThreshold;MemoryUsageThreshold=_copy.MemoryUsageThreshold;IOBandwidthThreshold=_copy.IOBandwidthThreshold;InetBandwidthThreshold=_copy.InetBandwidthThreshold;}
};

public class ProviderResourceTypes
{
	public string Provider="";
	public List<string> ResourceTypes=new List<string>();
	public ProviderResourceTypes(string _provider="",List<string> _resourcetypes=null) {Provider=_provider;if(_resourcetypes!=null)ResourceTypes=_resourcetypes;}
	public ProviderResourceTypes(ProviderResourceTypes _copy) { if (_copy == null) return;Provider=_copy.Provider;if(_copy.ResourceTypes!=null)ResourceTypes=_copy.ResourceTypes;}
};

public class MachineScalingRules
{
	public bool EnableScaling=false;
	public int MaxCloudMachines=0;
	public bool UseAllBareMetalMachines=false;
	public int MaxBareMetalMachines=0;
	public int MinFreePayloadCapacity=0;
	public int MaxFreePayloadCapacity=0;
	public bool UseMetrics=false;
	public int CapEvalTimeFrame=0;
	public List<MetricsEvalCondition> BlockedConditions=new List<MetricsEvalCondition>();
	public List<MetricsEvalCondition> IdleConditions=new List<MetricsEvalCondition>();
	public List<ProviderResourceTypes> AllowedResourceTypes=new List<ProviderResourceTypes>();
	public MachineScalingRules(bool _enablescaling=false,int _maxcloudmachines=0,bool _useallbaremetalmachines=false,int _maxbaremetalmachines=0,int _minfreepayloadcapacity=0,int _maxfreepayloadcapacity=0,bool _usemetrics=false,int _capevaltimeframe=0,List<MetricsEvalCondition> _blockedconditions=null,List<MetricsEvalCondition> _idleconditions=null,List<ProviderResourceTypes> _allowedresourcetypes=null) {EnableScaling=_enablescaling;MaxCloudMachines=_maxcloudmachines;UseAllBareMetalMachines=_useallbaremetalmachines;MaxBareMetalMachines=_maxbaremetalmachines;MinFreePayloadCapacity=_minfreepayloadcapacity;MaxFreePayloadCapacity=_maxfreepayloadcapacity;UseMetrics=_usemetrics;CapEvalTimeFrame=_capevaltimeframe;if(_blockedconditions!=null)BlockedConditions=_blockedconditions;if(_idleconditions!=null)IdleConditions=_idleconditions;if(_allowedresourcetypes!=null)AllowedResourceTypes=_allowedresourcetypes;}
	public MachineScalingRules(MachineScalingRules _copy) { if (_copy == null) return;EnableScaling=_copy.EnableScaling;MaxCloudMachines=_copy.MaxCloudMachines;UseAllBareMetalMachines=_copy.UseAllBareMetalMachines;MaxBareMetalMachines=_copy.MaxBareMetalMachines;MinFreePayloadCapacity=_copy.MinFreePayloadCapacity;MaxFreePayloadCapacity=_copy.MaxFreePayloadCapacity;UseMetrics=_copy.UseMetrics;CapEvalTimeFrame=_copy.CapEvalTimeFrame;if(_copy.BlockedConditions!=null)BlockedConditions=_copy.BlockedConditions;if(_copy.IdleConditions!=null)IdleConditions=_copy.IdleConditions;if(_copy.AllowedResourceTypes!=null)AllowedResourceTypes=_copy.AllowedResourceTypes;}
};

public class PortDef
{
	public string Name="";
	public int Port=0;
	public PortDef(string _name="",int _port=0) {Name=_name;Port=_port;}
	public PortDef(PortDef _copy) { if (_copy == null) return;Name=_copy.Name;Port=_copy.Port;}
};

public class PayloadDef
{
	public string HostOS="";
	public string Image="";
	public List<string> Cmd=new List<string>();
	public List<PortDef> Ports=new List<PortDef>();
	public int ServerStats=0;
	public PayloadDef(string _hostos="",string _image="",List<string> _cmd=null,List<PortDef> _ports=null,int _serverstats=0) {HostOS=_hostos;Image=_image;if(_cmd!=null)Cmd=_cmd;if(_ports!=null)Ports=_ports;ServerStats=_serverstats;}
	public PayloadDef(PayloadDef _copy) { if (_copy == null) return;HostOS=_copy.HostOS;Image=_copy.Image;if(_copy.Cmd!=null)Cmd=_copy.Cmd;if(_copy.Ports!=null)Ports=_copy.Ports;ServerStats=_copy.ServerStats;}
};

public class PayloadQuota
{
	public double CpuCores=0;
	public int MemoryMB=0;
	public int StorageGB=0;
	public int IOBandwidthMBps=0;
	public int InetBandwidthMBps=0;
	public PayloadQuota(double _cpucores=0,int _memorymb=0,int _storagegb=0,int _iobandwidthmbps=0,int _inetbandwidthmbps=0) {CpuCores=_cpucores;MemoryMB=_memorymb;StorageGB=_storagegb;IOBandwidthMBps=_iobandwidthmbps;InetBandwidthMBps=_inetbandwidthmbps;}
	public PayloadQuota(PayloadQuota _copy) { if (_copy == null) return;CpuCores=_copy.CpuCores;MemoryMB=_copy.MemoryMB;StorageGB=_copy.StorageGB;IOBandwidthMBps=_copy.IOBandwidthMBps;InetBandwidthMBps=_copy.InetBandwidthMBps;}
};

public class SafetyLimits
{
	public double CoreMaxUsageThreshold=0;
	public int CpuRemainingCores=0;
	public int MemoryMBFree=0;
	public int StorageGBFree=0;
	public int IOMBpsFree=0;
	public int InetMBpsFree=0;
	public SafetyLimits(double _coremaxusagethreshold=0,int _cpuremainingcores=0,int _memorymbfree=0,int _storagegbfree=0,int _iombpsfree=0,int _inetmbpsfree=0) {CoreMaxUsageThreshold=_coremaxusagethreshold;CpuRemainingCores=_cpuremainingcores;MemoryMBFree=_memorymbfree;StorageGBFree=_storagegbfree;IOMBpsFree=_iombpsfree;InetMBpsFree=_inetmbpsfree;}
	public SafetyLimits(SafetyLimits _copy) { if (_copy == null) return;CoreMaxUsageThreshold=_copy.CoreMaxUsageThreshold;CpuRemainingCores=_copy.CpuRemainingCores;MemoryMBFree=_copy.MemoryMBFree;StorageGBFree=_copy.StorageGBFree;IOMBpsFree=_copy.IOMBpsFree;InetMBpsFree=_copy.InetMBpsFree;}
};

public class PayloadScalingRules
{
	public bool EnableScaling=false;
	public PayloadDef PayloadDef=new PayloadDef();
	public PayloadQuota PayloadQuota=new PayloadQuota();
	public PayloadQuota PayloadQuotaCloud=new PayloadQuota();
	public SafetyLimits SafetyLimits=new SafetyLimits();
	public int MinUnreservedPayloads=0;
	public int MaxUnreservedPayloads=0;
	public PayloadScalingRules(bool _enablescaling=false,PayloadDef _payloaddef=null,PayloadQuota _payloadquota=null,PayloadQuota _payloadquotacloud=null,SafetyLimits _safetylimits=null,int _minunreservedpayloads=0,int _maxunreservedpayloads=0) {EnableScaling=_enablescaling;if(_payloaddef!=null)PayloadDef=_payloaddef;if(_payloadquota!=null)PayloadQuota=_payloadquota;if(_payloadquotacloud!=null)PayloadQuotaCloud=_payloadquotacloud;if(_safetylimits!=null)SafetyLimits=_safetylimits;MinUnreservedPayloads=_minunreservedpayloads;MaxUnreservedPayloads=_maxunreservedpayloads;}
	public PayloadScalingRules(PayloadScalingRules _copy) { if (_copy == null) return;EnableScaling=_copy.EnableScaling;if(_copy.PayloadDef!=null)PayloadDef=_copy.PayloadDef;if(_copy.PayloadQuota!=null)PayloadQuota=_copy.PayloadQuota;if(_copy.PayloadQuotaCloud!=null)PayloadQuotaCloud=_copy.PayloadQuotaCloud;if(_copy.SafetyLimits!=null)SafetyLimits=_copy.SafetyLimits;MinUnreservedPayloads=_copy.MinUnreservedPayloads;MaxUnreservedPayloads=_copy.MaxUnreservedPayloads;}
};

public class ScalingRules
{
	public MachineScalingRules Machine=new MachineScalingRules();
	public PayloadScalingRules Payload=new PayloadScalingRules();
	public ScalingRules(MachineScalingRules _machine=null,PayloadScalingRules _payload=null) {if(_machine!=null)Machine=_machine;if(_payload!=null)Payload=_payload;}
	public ScalingRules(ScalingRules _copy) { if (_copy == null) return;if(_copy.Machine!=null)Machine=_copy.Machine;if(_copy.Payload!=null)Payload=_copy.Payload;}
};

public class AllocationInfo
{
	public string AllocationID="";
	public string ProjID="";
	public string EnvID="";
	public List<string> Regions=new List<string>();
	public List<string> Machines=new List<string>();
	public string Description="";
	public bool Enabled=false;
	public ScalingRules ScalingRules=new ScalingRules();
	public MachineSpec MachineMinSpec=new MachineSpec();
	public long ActivePayloads=0;
	public long ReservedPayloads=0;
	public AllocationInfo(string _allocationid="",string _projid="",string _envid="",List<string> _regions=null,List<string> _machines=null,string _description="",bool _enabled=false,ScalingRules _scalingrules=null,MachineSpec _machineminspec=null,long _activepayloads=0,long _reservedpayloads=0) {AllocationID=_allocationid;ProjID=_projid;EnvID=_envid;if(_regions!=null)Regions=_regions;if(_machines!=null)Machines=_machines;Description=_description;Enabled=_enabled;if(_scalingrules!=null)ScalingRules=_scalingrules;if(_machineminspec!=null)MachineMinSpec=_machineminspec;ActivePayloads=_activepayloads;ReservedPayloads=_reservedpayloads;}
	public AllocationInfo(AllocationInfo _copy) { if (_copy == null) return;AllocationID=_copy.AllocationID;ProjID=_copy.ProjID;EnvID=_copy.EnvID;if(_copy.Regions!=null)Regions=_copy.Regions;if(_copy.Machines!=null)Machines=_copy.Machines;Description=_copy.Description;Enabled=_copy.Enabled;if(_copy.ScalingRules!=null)ScalingRules=_copy.ScalingRules;if(_copy.MachineMinSpec!=null)MachineMinSpec=_copy.MachineMinSpec;ActivePayloads=_copy.ActivePayloads;ReservedPayloads=_copy.ReservedPayloads;}
};

public class AllocationGetOut
{
	public List<AllocationInfo> Items=new List<AllocationInfo>();
	public long Count=0;
	public AllocationGetOut(List<AllocationInfo> _items=null,long _count=0) {if(_items!=null)Items=_items;Count=_count;}
	public AllocationGetOut(AllocationGetOut _copy) { if (_copy == null) return;if(_copy.Items!=null)Items=_copy.Items;Count=_copy.Count;}
};

public class AllocationDef
{
	public string Description=""; //arg:"required" help:"Freetext name of allocation - also shown in zeuz control panel user interface"
	public List<string> Regions=new List<string>(); //arg:"required" help:"Possible regions can be figured out via [locality_region_get](/api/endpoints/locality#locality_region_get)"
	public ScalingRules ScalingRules=new ScalingRules(); //arg:"required"
	public MachineSpec MachineMinSpec=new MachineSpec(); //arg:"required"
	public AllocationDef(string _description="",List<string> _regions=null,ScalingRules _scalingrules=null,MachineSpec _machineminspec=null) {Description=_description;if(_regions!=null)Regions=_regions;if(_scalingrules!=null)ScalingRules=_scalingrules;if(_machineminspec!=null)MachineMinSpec=_machineminspec;}
	public AllocationDef(AllocationDef _copy) { if (_copy == null) return;Description=_copy.Description;if(_copy.Regions!=null)Regions=_copy.Regions;if(_copy.ScalingRules!=null)ScalingRules=_copy.ScalingRules;if(_copy.MachineMinSpec!=null)MachineMinSpec=_copy.MachineMinSpec;}
};

public class AllocationCreateIn
{
	public string ProjID=""; //arg:"required" help:"Project ID can be found in [zeuz control panel](http://zcp.zeuz.io/)"
	public string EnvID=""; //arg:"required" help:"Environment ID can be found in [zeuz control panel](http://zcp.zeuz.io/)"
	public AllocationDef AllocationDef=new AllocationDef(); //arg:"required"
	public AllocationCreateIn(string _projid="",string _envid="",AllocationDef _allocationdef=null) {ProjID=_projid;EnvID=_envid;if(_allocationdef!=null)AllocationDef=_allocationdef;}
	public AllocationCreateIn(AllocationCreateIn _copy) { if (_copy == null) return;ProjID=_copy.ProjID;EnvID=_copy.EnvID;if(_copy.AllocationDef!=null)AllocationDef=_copy.AllocationDef;}
};

public class AllocationUpdateIn
{
	public string AllocationID="";
	public AllocationDef AllocationDef=new AllocationDef();
	public bool Enabled=false;
	public AllocationUpdateIn(string _allocationid="",AllocationDef _allocationdef=null,bool _enabled=false) {AllocationID=_allocationid;if(_allocationdef!=null)AllocationDef=_allocationdef;Enabled=_enabled;}
	public AllocationUpdateIn(AllocationUpdateIn _copy) { if (_copy == null) return;AllocationID=_copy.AllocationID;if(_copy.AllocationDef!=null)AllocationDef=_copy.AllocationDef;Enabled=_copy.Enabled;}
};

public class AllocationRegionCount
{
	public string RegionID="";
	public int Count=0;
	public AllocationRegionCount(string _regionid="",int _count=0) {RegionID=_regionid;Count=_count;}
	public AllocationRegionCount(AllocationRegionCount _copy) { if (_copy == null) return;RegionID=_copy.RegionID;Count=_copy.Count;}
};

public class AllocationRequestServiceIn
{
	public string AllocationID="";
	public List<AllocationRegionCount> RegionCount=new List<AllocationRegionCount>();
	public int TimeoutSeconds=0; //arg:"required" help:"Duration of validity of the service request. (default: 900s ≙ 15 minutes)"
	public AllocationRequestServiceIn(string _allocationid="",List<AllocationRegionCount> _regioncount=null,int _timeoutseconds=0) {AllocationID=_allocationid;if(_regioncount!=null)RegionCount=_regioncount;TimeoutSeconds=_timeoutseconds;}
	public AllocationRequestServiceIn(AllocationRequestServiceIn _copy) { if (_copy == null) return;AllocationID=_copy.AllocationID;if(_copy.RegionCount!=null)RegionCount=_copy.RegionCount;TimeoutSeconds=_copy.TimeoutSeconds;}
};

public class AllocationPayloadPortMapping
{
	public int InternalPort=0;
	public int ExternalPort=0;
	public AllocationPayloadPortMapping(int _internalport=0,int _externalport=0) {InternalPort=_internalport;ExternalPort=_externalport;}
	public AllocationPayloadPortMapping(AllocationPayloadPortMapping _copy) { if (_copy == null) return;InternalPort=_copy.InternalPort;ExternalPort=_copy.ExternalPort;}
};

public class AllocationPayloadInfo
{
	public string PayloadID="";
	public string MachineID="";
	public string AllocationID="";
	public string EnvID="";
	public List<string> Regions=new List<string>();
	public PayloadDef PayloadDef=new PayloadDef();
	public bool Active=false;
	public bool Reservable=false;
	public bool Reserved=false;
	public string IP="";
	public List<AllocationPayloadPortMapping> PortMapping=new List<AllocationPayloadPortMapping>();
	public string Handling="";
	public string LocationType="";
	public Timestamp Created=new Timestamp();
	public Timestamp Modified=new Timestamp();
	public AllocationPayloadInfo(string _payloadid="",string _machineid="",string _allocationid="",string _envid="",List<string> _regions=null,PayloadDef _payloaddef=null,bool _active=false,bool _reservable=false,bool _reserved=false,string _ip="",List<AllocationPayloadPortMapping> _portmapping=null,string _handling="",string _locationtype="",Timestamp _created=new Timestamp(),Timestamp _modified=new Timestamp()) {PayloadID=_payloadid;MachineID=_machineid;AllocationID=_allocationid;EnvID=_envid;if(_regions!=null)Regions=_regions;if(_payloaddef!=null)PayloadDef=_payloaddef;Active=_active;Reservable=_reservable;Reserved=_reserved;IP=_ip;if(_portmapping!=null)PortMapping=_portmapping;Handling=_handling;LocationType=_locationtype;Created=_created;Modified=_modified;}
	public AllocationPayloadInfo(AllocationPayloadInfo _copy) { if (_copy == null) return;PayloadID=_copy.PayloadID;MachineID=_copy.MachineID;AllocationID=_copy.AllocationID;EnvID=_copy.EnvID;if(_copy.Regions!=null)Regions=_copy.Regions;if(_copy.PayloadDef!=null)PayloadDef=_copy.PayloadDef;Active=_copy.Active;Reservable=_copy.Reservable;Reserved=_copy.Reserved;IP=_copy.IP;if(_copy.PortMapping!=null)PortMapping=_copy.PortMapping;Handling=_copy.Handling;LocationType=_copy.LocationType;Created=_copy.Created;Modified=_copy.Modified;}
};


public class ApiAllocation
{ 
	public delegate void GetDelegate(AllocationGetOut result, string error);
    public class GetRequest {  public AllocationGetOut Result=new AllocationGetOut();public string Error=""; public Request rq;
        public void Start(GetDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(GetDelegate del = null) { yield return rq.Send(); GetDone(this); del?.Invoke(Result, Error); }
    }

    public static GetRequest Get(AllocationGetIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        GetRequest rq=new GetRequest();
        rq.rq = Client.CreateRequest(ctx, "allocation_get", input);
        return rq;
    }

    public static void GetDone(GetRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (AllocationGetOut)rd.ToObject(new AllocationGetOut());
        }
    }


	public delegate void CreateDelegate(AllocationInfo result, string error);
    public class CreateRequest {  public AllocationInfo Result=new AllocationInfo();public string Error=""; public Request rq;
        public void Start(CreateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(CreateDelegate del = null) { yield return rq.Send(); CreateDone(this); del?.Invoke(Result, Error); }
    }

    public static CreateRequest Create(AllocationCreateIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        CreateRequest rq=new CreateRequest();
        rq.rq = Client.CreateRequest(ctx, "allocation_create", input);
        return rq;
    }

    public static void CreateDone(CreateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (AllocationInfo)rd.ToObject(new AllocationInfo());
        }
    }


	public delegate void UpdateDelegate(AllocationInfo result, string error);
    public class UpdateRequest {  public AllocationInfo Result=new AllocationInfo();public string Error=""; public Request rq;
        public void Start(UpdateDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(UpdateDelegate del = null) { yield return rq.Send(); UpdateDone(this); del?.Invoke(Result, Error); }
    }

    public static UpdateRequest Update(AllocationUpdateIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        UpdateRequest rq=new UpdateRequest();
        rq.rq = Client.CreateRequest(ctx, "allocation_update", input);
        return rq;
    }

    public static void UpdateDone(UpdateRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (AllocationInfo)rd.ToObject(new AllocationInfo());
        }
    }


	public delegate void RequestServicesDelegate(List<AllocationPayloadInfo> result, string error);
    public class RequestServicesRequest {  public List<AllocationPayloadInfo> Result=new List<AllocationPayloadInfo>();public string Error=""; public Request rq;
        public void Start(RequestServicesDelegate del = null, MonoBehaviour co = null) {Client.StartCo(co,Run(del));}
        public IEnumerator Run(RequestServicesDelegate del = null) { yield return rq.Send(); RequestServicesDone(this); del?.Invoke(Result, Error); }
    }

    public static RequestServicesRequest RequestServices(AllocationRequestServiceIn input, Context ctx=null)
    {
        if (ctx == null) ctx = Context.Def;
        RequestServicesRequest rq=new RequestServicesRequest();
        rq.rq = Client.CreateRequest(ctx, "allocation_request_services", input);
        return rq;
    }

    public static void RequestServicesDone(RequestServicesRequest rq)
    {
        rq.Error = rq.rq.Error;
        JSONObject rd = rq.rq.ResponseData as JSONObject;
        if (rd != null)
        {
            rq.Result = (List<AllocationPayloadInfo>)rd.ToObject(new List<AllocationPayloadInfo>());
        }
    }


}

} //namespace Zeuz
