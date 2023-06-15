using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.ApplicationCore.Enums;
using Web2023_BE.ApplicationCore.Helpers;

namespace Web2023_BE.ApplicationCore.MiddleWare
{
    public class ClientIpCheckActionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private readonly string _safelist;
        private readonly HttpClient client;
        private readonly IMemoryCache _memoryCache;

        public ClientIpCheckActionFilter(string safelist, IMemoryCache memoryCache, ILogger logger)
        {
            _safelist = safelist;
            _logger = logger;
            _memoryCache = memoryCache;
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44364/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            var remoteIps = FunctionHelper.GetIPV4Address();
            var remoteMAC = PhysicalAddress.Parse(FunctionHelper.GetMACAddress());

            _logger.LogDebug("Remote IpAddress: {RemoteIp}", string.Join(",", remoteIps.Select(x => x.ToString())));
            _logger.LogDebug("Remote MACAddress: {RemoteMAC}", remoteMAC);


            //get default safe list from appsetting
            var addresses = new List<SafeAddress>();

            //#if DEBUG
            //            var defaultIps = _safelist.Split(";").ToList<string>();
            //            addresses = defaultIps.Select(item => new SafeAddress()
            //            {
            //                SafeAddressID = Guid.NewGuid(),
            //                SafeAddressValue = FunctionHelper.GetMACAddress(),
            //                Type = SafeAddressType.MAC
            //            }).ToList<SafeAddress>();
            //#else
            //check address from cache
            //if (!_memoryCache.TryGetValue<string>(CacheKey.ADDRESS_CACHE_KEY, out string cacheString))
            //{

            var response = await client.GetAsync("/api/SafeAddress");
            if (response.IsSuccessStatusCode)
            {
                addresses = await response.Content.ReadAsAsync<List<SafeAddress>>();
                //if (data.Count > 0)
                //{
                //    config expired time cache
                //    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                //    options.AbsoluteExpiration = DateTime.Now.AddDays(7);
                //    options.SlidingExpiration = TimeSpan.FromMinutes(1);
                //    _memoryCache.Set<string>(CacheKey.ADDRESS_CACHE_KEY, JsonConvert.SerializeObject(data), options);
                //}
                //else
                //{
                //    _memoryCache.Remove(CacheKey.ADDRESS_CACHE_KEY);
                //}
            }
            else
            {
                //get safe address failed then ip get default values
            }
            //}
            //if value is current in cache
            //if(!string.IsNullOrEmpty(cacheString)) addresses = JsonConvert.DeserializeObject<List<SafeAddress>>(cacheString);
            //#endif

            var badIp = true;

            foreach (var address in addresses)
            {

                if (address.Type == SafeAddressType.IP)
                {
                    var testIp = IPAddress.Parse(address.SafeAddressValue);

                    if (remoteIps.Any(x => x.Equals(testIp)))
                    {
                        badIp = false;
                        break;
                    }
                }
                else if (address.Type == SafeAddressType.MAC)
                {
                    var testMAC = PhysicalAddress.Parse(address.SafeAddressValue);

                    if (testMAC.Equals(remoteMAC))
                    {
                        badIp = false;
                        break;
                    }
                }
            }

            if (badIp)
            {
                _logger.LogWarning("Forbidden Request from IP: {RemoteIp}", string.Join(",", remoteIps.Select(x => x.ToString())));
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
