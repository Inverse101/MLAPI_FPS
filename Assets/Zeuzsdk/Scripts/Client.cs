using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 0649 //is never assigned to, and will always have its default value

namespace Zeuz
{
    public enum SessionType
    {
        Invalid=-1,
        Developer,
        ApiKey,
        User
    }

    public class Context
    {
        public string Endpoint ="https://zcp.zeuz.io/api/v1";
        public string SessionID = "";
        public string SessionKey = "";
        public string UserID = "";
        public string DeveloperID = "";
        public string ProjID = "";
        public string EnvID = "";

        public string Login = "";
        public string PWHash = "";
        public SessionType Type=SessionType.Invalid;

        public static Context Def=new Context();  //default context
    };

    public class Request
    {
        public delegate void FinishDelegate(Request rq);
        public FinishDelegate OnFinished;

        public Context Ctx;
        public string Function;
        public Timestamp ReqTime;
        public object ReqData;

        public object ResponseData;
        public Timestamp ResponseTime;
        public string Error;
        public bool Finished;

        public int Retries;
        public UnityWebRequest wr;

        public IEnumerator Send()
        {
            if (wr == null) yield break;
            yield return wr.SendWebRequest();
            Finished = true;

            if (wr.isNetworkError)
            {
                Error = wr.error;
            }
            else
            {
                yield return Client.HandleResult(this,wr);
            }

            OnFinished?.Invoke(this);
            OnFinished = null;
            wr = null;
        }
    };

    public class CertHandler : CertificateHandler
    {
        //retired, remove soon
    const string certLetsEncryptX1 = @"
-----BEGIN CERTIFICATE-----
MIIEqDCCA5CgAwIBAgIRAJgT9HUT5XULQ+dDHpceRL0wDQYJKoZIhvcNAQELBQAw
PzEkMCIGA1UEChMbRGlnaXRhbCBTaWduYXR1cmUgVHJ1c3QgQ28uMRcwFQYDVQQD
Ew5EU1QgUm9vdCBDQSBYMzAeFw0xNTEwMTkyMjMzMzZaFw0yMDEwMTkyMjMzMzZa
MEoxCzAJBgNVBAYTAlVTMRYwFAYDVQQKEw1MZXQncyBFbmNyeXB0MSMwIQYDVQQD
ExpMZXQncyBFbmNyeXB0IEF1dGhvcml0eSBYMTCCASIwDQYJKoZIhvcNAQEBBQAD
ggEPADCCAQoCggEBAJzTDPBa5S5Ht3JdN4OzaGMw6tc1Jhkl4b2+NfFwki+3uEtB
BaupnjUIWOyxKsRohwuj43Xk5vOnYnG6eYFgH9eRmp/z0HhncchpDpWRz/7mmelg
PEjMfspNdxIknUcbWuu57B43ABycrHunBerOSuu9QeU2mLnL/W08lmjfIypCkAyG
dGfIf6WauFJhFBM/ZemCh8vb+g5W9oaJ84U/l4avsNwa72sNlRZ9xCugZbKZBDZ1
gGusSvMbkEl4L6KWTyogJSkExnTA0DHNjzE4lRa6qDO4Q/GxH8Mwf6J5MRM9LTb4
4/zyM2q5OTHFr8SNDR1kFjOq+oQpttQLwNh9w5MCAwEAAaOCAZIwggGOMBIGA1Ud
EwEB/wQIMAYBAf8CAQAwDgYDVR0PAQH/BAQDAgGGMH8GCCsGAQUFBwEBBHMwcTAy
BggrBgEFBQcwAYYmaHR0cDovL2lzcmcudHJ1c3RpZC5vY3NwLmlkZW50cnVzdC5j
b20wOwYIKwYBBQUHMAKGL2h0dHA6Ly9hcHBzLmlkZW50cnVzdC5jb20vcm9vdHMv
ZHN0cm9vdGNheDMucDdjMB8GA1UdIwQYMBaAFMSnsaR7LHH62+FLkHX/xBVghYkQ
MFQGA1UdIARNMEswCAYGZ4EMAQIBMD8GCysGAQQBgt8TAQEBMDAwLgYIKwYBBQUH
AgEWImh0dHA6Ly9jcHMucm9vdC14MS5sZXRzZW5jcnlwdC5vcmcwPAYDVR0fBDUw
MzAxoC+gLYYraHR0cDovL2NybC5pZGVudHJ1c3QuY29tL0RTVFJPT1RDQVgzQ1JM
LmNybDATBgNVHR4EDDAKoQgwBoIELm1pbDAdBgNVHQ4EFgQUqEpqYwR93brm0Tm3
pkVl7/Oo7KEwDQYJKoZIhvcNAQELBQADggEBANHIIkus7+MJiZZQsY14cCoBG1hd
v0J20/FyWo5ppnfjL78S2k4s2GLRJ7iD9ZDKErndvbNFGcsW+9kKK/TnY21hp4Dd
ITv8S9ZYQ7oaoqs7HwhEMY9sibED4aXw09xrJZTC9zK1uIfW6t5dHQjuOWv+HHoW
ZnupyxpsEUlEaFb+/SCI4KCSBdAsYxAcsHYI5xxEI4LutHp6s3OT2FuO90WfdsIk
6q78OMSdn875bNjdBYAqxUp2/LEIHfDBkLoQz0hFJmwAbYahqKaLn73PAAm1X2kj
f1w8DdnkabOLGeOVcj9LQ+s67vBykx4anTjURkbqZslUEUsn2k5xeua2zUk=
-----END CERTIFICATE-----
    ";

        //current
        const string certLetsEncryptX3 = @"
-----BEGIN CERTIFICATE-----
MIIEkjCCA3qgAwIBAgIQCgFBQgAAAVOFc2oLheynCDANBgkqhkiG9w0BAQsFADA/
MSQwIgYDVQQKExtEaWdpdGFsIFNpZ25hdHVyZSBUcnVzdCBDby4xFzAVBgNVBAMT
DkRTVCBSb290IENBIFgzMB4XDTE2MDMxNzE2NDA0NloXDTIxMDMxNzE2NDA0Nlow
SjELMAkGA1UEBhMCVVMxFjAUBgNVBAoTDUxldCdzIEVuY3J5cHQxIzAhBgNVBAMT
GkxldCdzIEVuY3J5cHQgQXV0aG9yaXR5IFgzMIIBIjANBgkqhkiG9w0BAQEFAAOC
AQ8AMIIBCgKCAQEAnNMM8FrlLke3cl03g7NoYzDq1zUmGSXhvb418XCSL7e4S0EF
q6meNQhY7LEqxGiHC6PjdeTm86dicbp5gWAf15Gan/PQeGdxyGkOlZHP/uaZ6WA8
SMx+yk13EiSdRxta67nsHjcAHJyse6cF6s5K671B5TaYucv9bTyWaN8jKkKQDIZ0
Z8h/pZq4UmEUEz9l6YKHy9v6Dlb2honzhT+Xhq+w3Brvaw2VFn3EK6BlspkENnWA
a6xK8xuQSXgvopZPKiAlKQTGdMDQMc2PMTiVFrqoM7hD8bEfwzB/onkxEz0tNvjj
/PIzark5McWvxI0NHWQWM6r6hCm21AvA2H3DkwIDAQABo4IBfTCCAXkwEgYDVR0T
AQH/BAgwBgEB/wIBADAOBgNVHQ8BAf8EBAMCAYYwfwYIKwYBBQUHAQEEczBxMDIG
CCsGAQUFBzABhiZodHRwOi8vaXNyZy50cnVzdGlkLm9jc3AuaWRlbnRydXN0LmNv
bTA7BggrBgEFBQcwAoYvaHR0cDovL2FwcHMuaWRlbnRydXN0LmNvbS9yb290cy9k
c3Ryb290Y2F4My5wN2MwHwYDVR0jBBgwFoAUxKexpHsscfrb4UuQdf/EFWCFiRAw
VAYDVR0gBE0wSzAIBgZngQwBAgEwPwYLKwYBBAGC3xMBAQEwMDAuBggrBgEFBQcC
ARYiaHR0cDovL2Nwcy5yb290LXgxLmxldHNlbmNyeXB0Lm9yZzA8BgNVHR8ENTAz
MDGgL6AthitodHRwOi8vY3JsLmlkZW50cnVzdC5jb20vRFNUUk9PVENBWDNDUkwu
Y3JsMB0GA1UdDgQWBBSoSmpjBH3duubRObemRWXv86jsoTANBgkqhkiG9w0BAQsF
AAOCAQEA3TPXEfNjWDjdGBX7CVW+dla5cEilaUcne8IkCJLxWh9KEik3JHRRHGJo
uM2VcGfl96S8TihRzZvoroed6ti6WqEBmtzw3Wodatg+VyOeph4EYpr/1wXKtx8/
wApIvJSwtmVi4MFU5aMqrSDE6ea73Mj2tcMyo5jMd6jmeWUHK8so/joWUoHOUgwu
X4Po1QYz+3dszkDqMp4fklxBwXRsW10KXzPMTZ+sOPAveyxindmjkW8lGy+QsRlG
PfZ+G6Z6h7mjem0Y+iWlkYcV4PIWL1iwBi8saCbGS5jN2p8M+X+Q7UNKEkROb3N6
KOqkqm57TH2H3eDJAkSnh6/DNFu0Qg==
-----END CERTIFICATE-----

    ";

        //upcoming
        const string certLetsEncryptE1 = @"
-----BEGIN CERTIFICATE-----
MIICxjCCAk2gAwIBAgIRALO93/inhFu86QOgQTWzSkUwCgYIKoZIzj0EAwMwTzEL
MAkGA1UEBhMCVVMxKTAnBgNVBAoTIEludGVybmV0IFNlY3VyaXR5IFJlc2VhcmNo
IEdyb3VwMRUwEwYDVQQDEwxJU1JHIFJvb3QgWDIwHhcNMjAwOTA0MDAwMDAwWhcN
MjUwOTE1MTYwMDAwWjAyMQswCQYDVQQGEwJVUzEWMBQGA1UEChMNTGV0J3MgRW5j
cnlwdDELMAkGA1UEAxMCRTEwdjAQBgcqhkjOPQIBBgUrgQQAIgNiAAQkXC2iKv0c
S6Zdl3MnMayyoGli72XoprDwrEuf/xwLcA/TmC9N/A8AmzfwdAVXMpcuBe8qQyWj
+240JxP2T35p0wKZXuskR5LBJJvmsSGPwSSB/GjMH2m6WPUZIvd0xhajggEIMIIB
BDAOBgNVHQ8BAf8EBAMCAYYwHQYDVR0lBBYwFAYIKwYBBQUHAwIGCCsGAQUFBwMB
MBIGA1UdEwEB/wQIMAYBAf8CAQAwHQYDVR0OBBYEFFrz7Sv8NsI3eblSMOpUb89V
yy6sMB8GA1UdIwQYMBaAFHxClq7eS0g7+pL4nozPbYupcjeVMDIGCCsGAQUFBwEB
BCYwJDAiBggrBgEFBQcwAoYWaHR0cDovL3gyLmkubGVuY3Iub3JnLzAnBgNVHR8E
IDAeMBygGqAYhhZodHRwOi8veDIuYy5sZW5jci5vcmcvMCIGA1UdIAQbMBkwCAYG
Z4EMAQIBMA0GCysGAQQBgt8TAQEBMAoGCCqGSM49BAMDA2cAMGQCMHt01VITjWH+
Dbo/AwCd89eYhNlXLr3pD5xcSAQh8suzYHKOl9YST8pE9kLJ03uGqQIwWrGxtO3q
YJkgsTgDyj2gJrjubi1K9sZmHzOa25JK1fUpE8ZwYii6I4zPPS/Lgul/
-----END CERTIFICATE-----

    ";


        //root
        const string dstRootX3 = @"
-----BEGIN CERTIFICATE-----
MIIDSjCCAjKgAwIBAgIQRK+wgNajJ7qJMDmGLvhAazANBgkqhkiG9w0BAQUFADA/
MSQwIgYDVQQKExtEaWdpdGFsIFNpZ25hdHVyZSBUcnVzdCBDby4xFzAVBgNVBAMT
DkRTVCBSb290IENBIFgzMB4XDTAwMDkzMDIxMTIxOVoXDTIxMDkzMDE0MDExNVow
PzEkMCIGA1UEChMbRGlnaXRhbCBTaWduYXR1cmUgVHJ1c3QgQ28uMRcwFQYDVQQD
Ew5EU1QgUm9vdCBDQSBYMzCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEB
AN+v6ZdQCINXtMxiZfaQguzH0yxrMMpb7NnDfcdAwRgUi+DoM3ZJKuM/IUmTrE4O
rz5Iy2Xu/NMhD2XSKtkyj4zl93ewEnu1lcCJo6m67XMuegwGMoOifooUMM0RoOEq
OLl5CjH9UL2AZd+3UWODyOKIYepLYYHsUmu5ouJLGiifSKOeDNoJjj4XLh7dIN9b
xiqKqy69cK3FCxolkHRyxXtqqzTWMIn/5WgTe1QLyNau7Fqckh49ZLOMxt+/yUFw
7BZy1SbsOFU5Q9D8/RhcQPGX69Wam40dutolucbY38EVAjqr2m7xPi71XAicPNaD
aeQQmxkqtilX4+U9m5/wAl0CAwEAAaNCMEAwDwYDVR0TAQH/BAUwAwEB/zAOBgNV
HQ8BAf8EBAMCAQYwHQYDVR0OBBYEFMSnsaR7LHH62+FLkHX/xBVghYkQMA0GCSqG
SIb3DQEBBQUAA4IBAQCjGiybFwBcqR7uKGY3Or+Dxz9LwwmglSBd49lZRNI+DT69
ikugdB/OEIKcdBodfpga3csTS7MgROSR6cz8faXbauX+5v3gTt23ADq1cEmv8uXr
AvHRAosZy5Q6XkjEGB5YGV8eAlrwDPGxrancWYaLbumR9YbK+rlmM6pZW87ipxZz
R8srzJmwN0jP41ZL9c8PDHIyh8bwRLtTcm1D9SZImlJnt1ir/md2cXjbDaJWFBM5
JDGFoqgCWjBH4d1QB7wCCZAA62RjYJsWvIjJEubSfZGL+T0yjWW06XyxV3bqxbYo
Ob8VZRzI9neWagqNdwvYkQsEjgfbKbYK7p2CNTUQ
-----END CERTIFICATE-----
";
    protected override bool ValidateCertificate(byte[] certificateData)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateData);
            bool valid = certificate.Verify();
            if (valid) return true;

            //unity sometimes fails to validate the let's encrypt cert of zcp.zeuz.io, manually provide the chain
            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority | X509VerificationFlags.IgnoreInvalidName;
            chain.ChainPolicy.ExtraStore.Add(new X509Certificate2(Encoding.ASCII.GetBytes(certLetsEncryptX1)));
            chain.ChainPolicy.ExtraStore.Add(new X509Certificate2(Encoding.ASCII.GetBytes(certLetsEncryptX3)));
            chain.ChainPolicy.ExtraStore.Add(new X509Certificate2(Encoding.ASCII.GetBytes(certLetsEncryptE1)));
            chain.ChainPolicy.ExtraStore.Add(new X509Certificate2(Encoding.ASCII.GetBytes(dstRootX3)));
            if (!chain.Build(certificate))
            {
                Debug.LogError("X509Chain.Build failed!");
                return false;
            }
            if (chain.ChainElements.Count <= 0) return false;

            //check root CA
            if (chain.ChainElements[chain.ChainElements.Count - 1].Certificate.Thumbprint == "DAC9024F54D8F6DF94935FB1732638CA6AD77C13") return true;

            Debug.LogError("Certificate invalid:" + certificate.Subject + " " + certificate.Thumbprint);
            return false;
        }
    }

    public class Client : Singleton<Client>
    {
        public const string ErrorRequestExpired = "request_expired";
        public const string ErrorSessionExpired = "session_expired";
        public const string ErrorInvalidSession = "invalid_session";

        public override void Awake()
        {
            hideFlags = HideFlags.HideAndDontSave;
        }

        public static string StringHash(string val)
        {
            return System.Convert.ToBase64String(SHA3.Sha3.Calc(val,256));
        }

        public static string PWHash(string login, string pw)
        {
            var bsalt = System.Text.Encoding.UTF8.GetBytes("zeuz" + login.Normalize(System.Text.NormalizationForm.FormKC));
            var bpw = System.Text.Encoding.UTF8.GetBytes(pw.Normalize(System.Text.NormalizationForm.FormKC));
            //pwhash="a"+base64(scrypt(password,salt:"zeuz"+login,1024,8, 1, 32). ("a" = version 1)
            var res =libscrypt.SCrypt.scrypt(bpw, bsalt, 1024, 8, 1, 32);
            return "a" + System.Convert.ToBase64String(res);
        }

        [Serializable]
        struct ClientRequest {
            public string Session;
            public Timestamp Time;
            public string ReqId;
            public string SignHash;
            public object Data;
        }
        [Serializable]
        struct ClientResponse {
            public Timestamp Time;
            public string Error;
            public object Data;
        }

        public static void StartCo(MonoBehaviour co, IEnumerator routine)
        {
            GetMonoBehaviour(co).StartCoroutine(routine);
        }
        public static MonoBehaviour GetMonoBehaviour(MonoBehaviour co) { if (co == null) co = Client.Instance; return co; }

        public static Request CreateRequest(Context ctx, string fnc, object input, Request res=null)
        {
            string apiURL;
            if (ctx.EnvID.Length > 0)
            {
                apiURL = ctx.Endpoint + "/" + ctx.ProjID + "/" + ctx.EnvID + "/" + fnc;
            }
            else if (ctx.ProjID.Length > 0)
            {
                apiURL = ctx.Endpoint + "/" + ctx.ProjID + "/" + fnc;
            }
            else
            {
                apiURL = ctx.Endpoint + "/" + fnc;
            }

            Timestamp time = Timestamp.Now();
            string stime = "" + time.value;
            string reqid = Guid.NewGuid().ToString();

            string chksum = StringHash(stime + reqid + ctx.SessionKey);

            ClientRequest rq;
            rq.Session = ctx.SessionID;
            rq.Time = time;
            rq.ReqId = reqid;
            rq.SignHash = chksum;
            rq.Data = input;

            UnityWebRequest wr = new UnityWebRequest(apiURL);
            wr.method = UnityWebRequest.kHttpVerbPOST;
            wr.redirectLimit = 1;
            wr.timeout = 60;

            JSONObject jo = JSONObject.FromObject(rq);
            string json = jo.ToString();
            UploadHandler uploader = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            uploader.contentType = "application/json";
            wr.uploadHandler = uploader;
            wr.downloadHandler = new DownloadHandlerBuffer();
            wr.certificateHandler = new CertHandler();

            Debug.Log("CreateRequest: " + apiURL + " " + json);

            if (res == null) res = new Request();
            res.ReqData = input;
            res.wr = wr;
            res.ReqTime = Timestamp.Now(false);
            res.Ctx = ctx;
            res.Function = fnc;
            return res;
        }

        public static IEnumerator HandleResult(Request rq, UnityWebRequest wr)
        {
            string result = Encoding.UTF8.GetString(wr.downloadHandler.data);
            Debug.Log("HandleResult: " + result);
            JSONObject jsobj =JSONParser.parse(result);

            if(jsobj==null)
            {
                rq.Error = "invalid_result";
            }
            else if (jsobj.isError)
            {
                rq.Error = (string)jsobj;
            }
            else
            {
                ClientResponse resp;
                resp.Error = null;
                resp.Data = new JSONObject();
                resp.Time = new Timestamp();
                resp=(ClientResponse)jsobj.ToObject(resp);

                rq.Error = resp.Error;
                rq.ResponseTime = resp.Time;
                rq.ResponseData = resp.Data;
                if (rq.Error!=null && rq.Error.Length == 0) rq.Error = null;

                //NTP adjust clock to server
                if (resp.Time.value != 0)
                {
                    long now = Timestamp.Now(false).value;
                    long dur = now - rq.ReqTime.value;
                    long mid = rq.ReqTime.value + dur / 2;
                    long ofs = resp.Time.value - mid;
                    Timestamp.AdjustOffset(ofs);
                }

                if(ErrorRequestExpired == rq.Error && rq.Function!="auth_login" && rq.Retries<3)
                {
                    Debug.LogWarning("Local clock offset is too large, retry after correnting clock to:"+ Timestamp.Now().ToDateTime().ToLocalTime().ToShortTimeString());
                    rq.Retries++;
                    yield return CreateRequest(rq.Ctx, rq.Function, rq.ReqData, rq).Send();
                }

                if((rq.Error!=null&&rq.Ctx.PWHash.Length > 0)
                &&(rq.Error.StartsWith(ErrorInvalidSession)||rq.Error.StartsWith(ErrorSessionExpired)))
                {
                    Debug.LogWarning("session expired relogin");
                    Auth.LoginRequest retry = Auth.LoginPWHash(rq.Ctx.Login, rq.Ctx.PWHash, rq.Ctx.Type, rq.Ctx);
                    yield return retry.Run();
                    if (retry.Error != null && retry.Error.Length > 0) {
                        Debug.LogError("relogin failed:"+ retry.Error);
                        yield break;
                    };
                    Debug.LogWarning("relogin succeeded, retrying request");
                    yield return CreateRequest(rq.Ctx, rq.Function, rq.ReqData, rq).Send();
                }
            }
        }
    }


} //namespace Zeuz

