//@BaseCode
//MdStart
using SmartNQuick.Adapters.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartNQuick.Adapters.Service
{
    internal partial class GenericServiceAdapter<TContract, TModel> : ServiceAdapterObject, Contracts.Client.IAdapterAccess<TContract>
        where TContract : Contracts.IIdentifiable
        where TModel : TContract, Contracts.ICopyable<TContract>, new()
    {
        static GenericServiceAdapter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        public GenericServiceAdapter(string baseUri, string extUri)
            : base(baseUri)
        {
            Constructing();
            ExtUri = extUri;
            Constructed();
        }
//#if ACCOUNT_ON
        public GenericServiceAdapter(string sessionToken, string baseUri, string extUri)
            : base(sessionToken, baseUri)
        {
            Constructing();
            ExtUri = extUri;
            Constructed();
        }
//#endif
        partial void Constructing();
        partial void Constructed();

        public string ExtUri
        {
            get;
        }

        public int MaxPageSize
        {
            get
            {
                return Task.Run(async () =>
                {
                    using var client = GetClient(BaseUri);
                    HttpResponseMessage response = await client.GetAsync($"{ExtUri}/MaxPageSize").ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        return Convert.ToInt32(stringData);
                    }
                    else
                    {
                        string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                        System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                        throw new AdapterException((int)response.StatusCode, errorMessage);
                    }
                }).Result;
            }
        }

        protected static TModel ToModel(TContract entity)
        {
            var result = new TModel();

            result.CopyProperties(entity);
            return result;
        }
        protected static IEnumerable<TModel> ToModel(IEnumerable<TContract> entities)
        {
            var result = new List<TModel>();

            foreach (var item in entities)
            {
                result.Add(ToModel(item));
            }
            return result;
        }

        public async Task<int> CountAsync()
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"{ExtUri}/Count").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Convert.ToInt32(stringData);
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<int> CountByAsync(string predicate)
        {
            using var client = GetClient(BaseUri);
            HttpResponseMessage response = await client.GetAsync($"{ExtUri}/Count/{predicate}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Convert.ToInt32(stringData);
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }

        public async Task<TContract> GetByIdAsync(int id)
        {
            using var client = GetClient(BaseUri);
            var response = await client.GetAsync($"{ExtUri}/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<IEnumerable<TContract>> GetAllAsync()
        {
            using var client = GetClient(BaseUri);
            var response = await client.GetAsync($"{ExtUri}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel[]>(contentData, DeserializerOptions).ConfigureAwait(false) as IEnumerable<TContract>;
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
            //var result = new List<TContract>();
            //var index = 0;
            //var pageSize = MaxPageSize;
            //int qryCount;
            //do
            //{
            //    var qry = await GetPageListAsync(index++, pageSize).ConfigureAwait(false);

            //    qryCount = qry.Count();
            //    result.AddRange(qry);
            //} while (qryCount > 0 && qryCount % pageSize == 0);
            //return result;
        }
        public async Task<IEnumerable<TContract>> GetPageListAsync(int pageIndex, int pageSize)
        {
            using var client = GetClient(BaseUri);
            var response = await client.GetAsync($"{ExtUri}/{pageIndex}/{pageSize}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel[]>(contentData, DeserializerOptions).ConfigureAwait(false) as IEnumerable<TContract>;
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }

        public async Task<IEnumerable<TContract>> QueryAllAsync(string predicate)
        {
            using var client = GetClient(BaseUri);
            var response = await client.GetAsync($"{ExtUri}/Query/{predicate}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel[]>(contentData, DeserializerOptions).ConfigureAwait(false) as IEnumerable<TContract>;
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<IEnumerable<TContract>> QueryPageListAsync(string predicate, int pageIndex, int pageSize)
        {
            using var client = GetClient(BaseUri);
            var response = await client.GetAsync($"{ExtUri}/{predicate}/{pageIndex}/{pageSize}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel[]>(contentData, DeserializerOptions).ConfigureAwait(false) as IEnumerable<TContract>;
            }
            else
            {
                string stringData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string errorMessage = $"{response.ReasonPhrase}: {stringData}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }

        public async Task<TContract> CreateAsync()
        {
            using var client = GetClient(BaseUri);
            var response = await client.GetAsync($"{ExtUri}/Create").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel>(contentData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string errorMessage = $"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<TContract> InsertAsync(TContract entity)
        {
            entity.CheckArgument(nameof(entity));

            using var client = GetClient(BaseUri);
            var jsonData = JsonSerializer.Serialize(ToModel(entity));
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            var response = await client.PostAsync($"{ExtUri}", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel>(resultData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string errorMessage = $"{response.ReasonPhrase}: { await response.Content.ReadAsStringAsync().ConfigureAwait(false) }";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<IQueryable<TContract>> InsertAsync(IEnumerable<TContract> entities)
        {
            entities.CheckArgument(nameof(entities));

            using var client = GetClient(BaseUri);
            var jsonData = JsonSerializer.Serialize(ToModel(entities));
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            var response = await client.PostAsync($"{ExtUri}/Array", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<List<TModel>>(resultData, DeserializerOptions).ConfigureAwait(false);

                return result.AsQueryable() as IQueryable<TContract>;
            }
            else
            {
                string errorMessage = $"{response.ReasonPhrase}: { await response.Content.ReadAsStringAsync().ConfigureAwait(false) }";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<TContract> UpdateAsync(TContract entity)
        {
            entity.CheckArgument(nameof(entity));

            using var client = GetClient(BaseUri);
            var jsonData = JsonSerializer.Serialize(ToModel(entity));
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            HttpResponseMessage response = await client.PutAsync($"{ExtUri}", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return await JsonSerializer.DeserializeAsync<TModel>(resultData, DeserializerOptions).ConfigureAwait(false);
            }
            else
            {
                string errorMessage = $"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
        public async Task<IQueryable<TContract>> UpdateAsync(IEnumerable<TContract> entities)
        {
            entities.CheckArgument(nameof(entities));

            using var client = GetClient(BaseUri);
            var jsonData = JsonSerializer.Serialize(ToModel(entities));
            var contentData = new StringContent(jsonData, Encoding.UTF8, MediaType);
            HttpResponseMessage response = await client.PutAsync($"{ExtUri}/Array", contentData).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<List<TModel>>(resultData, DeserializerOptions).ConfigureAwait(false);

                return result.AsQueryable() as IQueryable<TContract>;
            }
            else
            {
                string errorMessage = $"{response.ReasonPhrase}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var client = GetClient(BaseUri);
            var response = await client.DeleteAsync($"{ExtUri}/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode == false)
            {
                string errorMessage = $"{response.ReasonPhrase}: { await response.Content.ReadAsStringAsync().ConfigureAwait(false) }";

                System.Diagnostics.Debug.WriteLine("{0} ({1})", (int)response.StatusCode, errorMessage);
                throw new AdapterException((int)response.StatusCode, errorMessage);
            }
        }
    }
}
//MdEnd