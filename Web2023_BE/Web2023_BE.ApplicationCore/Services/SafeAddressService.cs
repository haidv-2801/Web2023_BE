using ClosedXML.Excel;
using Web2023_BE.ApplicationCore.Entities;
using Microsoft.AspNetCore.Http;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Enums;
using System.Net.NetworkInformation;
using Web2023_BE.ApplicationCore.Helpers;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public class SafeAddressService : BaseService<SafeAddress>, ISafeAddressService
    {
        #region Declare
        ISafeAddressRepository _safeAddressRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<SafeAddress> _logger;
        #endregion

        #region Constructer
        public SafeAddressService(ISafeAddressRepository safeAddressRepository, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, ILogger<SafeAddress> logger) : base(safeAddressRepository)
        {
            _safeAddressRepository = safeAddressRepository;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        /// <summary>
        /// Kiểm tra ip hay mac của máy truy cập có trong mạng cho phép không
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckIsInNetworkAllowed()
        {
            var addresses = await _safeAddressRepository.GetEntities();
            //var remoteIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            var remoteIps = FunctionHelper.GetIPV4Address();
            var remoteMAC = PhysicalAddress.Parse(FunctionHelper.GetMACAddress());

            var badIp = true;

            _logger.LogInformation("[IP ADDRESS]: " + remoteIps.ToString());
            _logger.LogInformation("[MAC ADDRESS]: " + remoteMAC.ToString());


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

             return badIp == true ? false : true;
        }
        #endregion

        protected override void AfterDelete()
        {
            _memoryCache.Remove(CacheKey.ADDRESS_CACHE_KEY);
        }

        protected override void AfterInsert()
        {
            _memoryCache.Remove(CacheKey.ADDRESS_CACHE_KEY);
        }

        protected override void AfterUpdate()
        {
            _memoryCache.Remove(CacheKey.ADDRESS_CACHE_KEY);
        }

        #region Methods
        #endregion
    }
}
