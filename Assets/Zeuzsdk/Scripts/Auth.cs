using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeuz
{
    public class Auth
    {
        public delegate void LoginDelegate(string error);
        public class LoginRequest
        {
            private Context Ctx = null;
            private int Tries = 0;

            public string Error = "";
            public ApiAuth.LoginRequest rq;
            public LoginRequest(ApiAuth.LoginRequest r,Context ctx) { rq = r; Ctx = ctx; }

            public void Start(LoginDelegate del = null, MonoBehaviour co = null) { Client.StartCo(co, Run(del)); }
            public IEnumerator Run(LoginDelegate del = null) {
                yield return rq.Run();
                if (rq.rq.Error == Client.ErrorRequestExpired && Tries < 3)
                {
                    Debug.LogWarning("Local clock offset is too large, retry after correnting clock to:" + Timestamp.Now().ToDateTime().ToLocalTime().ToShortTimeString());
                    LoginRequest retry = LoginPWHash(Ctx.Login, Ctx.PWHash, Ctx.Type, Ctx);
                    retry.Tries = Tries + 1;
                    yield return retry.Run();
                }
                else
                {
                    LoginDone(this);
                    del?.Invoke(Error);
                }
            }
        }

        public static LoginRequest Login(string name,string password, SessionType logintype = SessionType.Developer, Context ctx = null)
        {
            string sPWHash = Client.PWHash(name, password);
            return LoginPWHash(name, sPWHash,logintype, ctx);
        }

        public static LoginRequest LoginPWHash(string name, string pwhash, SessionType logintype = SessionType.Developer, Context ctx = null)
        {
            if (ctx == null) ctx = Context.Def;

            AuthLoginIn al = new AuthLoginIn();
            al.Nonce = System.Guid.NewGuid().ToString();
            al.Time = Timestamp.Now();
            al.Login = name;
            if (logintype == SessionType.User) al.IsUser = true;
            if (logintype == SessionType.ApiKey) al.IsApi = true;

            string sT = "" + al.Time.value;
            al.Hash = Client.StringHash(al.Nonce + sT + pwhash);

            ctx.Login = name;
            ctx.PWHash = pwhash;
            ctx.Type = logintype;

            return new LoginRequest(ApiAuth.Login(al, ctx), ctx);
        }

        public static void LoginDone(LoginRequest rq)
        {
            rq.Error = rq.rq.Error;
            if (rq.Error != null && rq.Error.Length > 0) return;
            JSONObject rd = rq.rq.rq.ResponseData as JSONObject;
            if (rd == null) return;
            AuthLoginResult Result = (AuthLoginResult)rd.ToObject(new AuthLoginResult());
            Context ctx = rq.rq.rq.Ctx;

            ctx.SessionID = Result.SessionID;
            ctx.UserID = Result.User;
            ctx.DeveloperID = Result.Dev;
            ctx.SessionKey = Client.StringHash(Result.SessionNonce + ctx.PWHash);
            Debug.Log("Login success, SessionID: " + ctx.SessionID);
        }
    }

} //namespace Zeuz