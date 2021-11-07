//@BaseCode
//MdStart
#if ACCOUNT_ON
using SmartNQuick.Adapters.Exceptions;
using SmartNQuick.Transfer.Models.Modules.Account;
using SmartNQuick.Transfer.Models.Persistence.Account;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
#endif

namespace SmartNQuick.Adapters.Service
{
    public partial class InvokeServiceAdapter : ServiceAdapterObject
    {
        static InvokeServiceAdapter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public InvokeServiceAdapter(string baseUri)
            : base(baseUri)
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
#if ACCOUNT_ON
        public InvokeServiceAdapter(string baseUri, string sessionToken)
            : base(baseUri, sessionToken)
        {
            Constructing();
            Constructed();
        }
        #region Account methods
        public async Task<LoginSession> LogonAsync(JsonWebLogon logon)
        {
            using var client = GetClient(BaseUri);
            var jsonData = JsonSerializer.Serialize(logon);
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            var response = await client.PostAsync("Account/JsonWebLogon", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<LoginSession>(resultData, DeserializerOptions).ConfigureAwait(false);

                return result;
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<LoginSession> LogonAsync(Logon logon)
        {
            using var client = GetClient(BaseUri);
            var jsonData = JsonSerializer.Serialize(logon);
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            var response = await client.PostAsync("Account/Logon", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<LoginSession>(resultData, DeserializerOptions).ConfigureAwait(false);

                return result;
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task LogoutAsync(string sessionToken)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/Logout/{sessionToken}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task ChangePasswordAsync(string sessionToken, string oldPwd, string newPwd)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/ChangePassword/{sessionToken}/{oldPwd}/{newPwd}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task ChangePasswordForAsync(string sessionToken, string email, string newPwd)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/ChangePasswordFor/{sessionToken}/{email}/{newPwd}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task ResetFailedCountForAsync(string sessionToken, string email)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/ResetFailedCountFor/{sessionToken}/{email}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/HasRole/{sessionToken}/{role}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<bool>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<bool> IsSessionAliveAsync(string sessionToken)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/IsSessionAlive/{sessionToken}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<bool>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<IEnumerable<string>> QueryRolesAsync(string sessionToken)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/QueryRoles/{sessionToken}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<IEnumerable<string>>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<LoginSession> QueryLoginAsync(string sessionToken)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"Account/QueryLogin/{sessionToken}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<LoginSession>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        #endregion Account methods
#endif
    }
}
//MdEnd