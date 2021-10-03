//@QnSBaseCode
//MdStart
using CommonBase.Extensions;
using Microsoft.AspNetCore.Mvc;
using SmartNQuick.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNQuick.WebApi.Controllers
{
    public abstract partial class ApiControllerBase : ControllerBase
    {
        static ApiControllerBase()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        protected ApiControllerBase()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
#if ACCOUNT_ON
        public Task<string> GetSessionTokenAsync()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];

            return GetSessionTokenAsync(authHeader);
        }
        public static async Task<string> GetSessionTokenAsync(string authHeader)
        {
            string result = string.Empty;

            if (authHeader.HasContent())
            {
                if (authHeader.StartsWith("Bearer"))
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var encodedToken = authHeader["Bearer ".Length..].Trim();

                    result = encoding.GetString(Convert.FromBase64String(encodedToken));
                }
                else if (authHeader.StartsWith("Basic"))
                {
                    string encodedUseridPassword = authHeader["Basic ".Length..].Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string useridPassword = encoding.GetString(Convert.FromBase64String(encodedUseridPassword));

                    int seperatorIndex = useridPassword.IndexOf(':');
                    var userid = useridPassword.Substring(0, seperatorIndex);
                    var password = useridPassword[(seperatorIndex + 1)..];
                    var login = await Logic.Factory.CreateAccountManager().LogonAsync(userid, password, string.Empty).ConfigureAwait(false);

                    result = login.SessionToken;
                }
                else if (authHeader.StartsWith("SessionToken"))
                {
                    result = authHeader["SessionToken ".Length..].Trim();
                }
                else
                {
                    result = authHeader;
                }
            }
            return result;
        }
#endif
        protected M ToModel<I, M>(I i)
            where M : I, ICopyable<I>, new()
        {
            var m = new M();

            m.CopyProperties(i);
            return m;
        }
        protected IQueryable<M> ToModel<I, M>(IEnumerable<I> entities)
            where M : I, ICopyable<I>, new()
        {
            var result = new List<M>();

            foreach (var item in entities)
            {
                result.Add(ToModel<I, M>(item));
            }
            return result.AsQueryable();
        }
    }
}
//MdEnd