using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Data;
using Web2023_BE.ApplicationCore;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Entities;
using Microsoft.AspNetCore.Cors;
using Web2023_BE.Entities;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller partner
    /// </summary>
    [ApiController]
    public class PartnersController : BaseEntityController<Partner>
    {
        #region Declare
        IPartnerService _partnerService;
        ILogger<Partner> _logger;
        #endregion

        #region Constructer
        public PartnersController(IPartnerService partnerService, ILogger<Partner> logger) : base(partnerService, logger)
        {
            _partnerService = partnerService;
            _logger = logger;
        }
        #endregion

        #region Methods
        #endregion
    }
}
