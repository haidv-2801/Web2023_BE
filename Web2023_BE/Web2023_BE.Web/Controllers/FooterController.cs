using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Data;
using Web2023_BE.ApplicationCore.Interfaces;
using Web2023_BE.ApplicationCore.Entities;
using Web2023_BE.Web.Controllers;
using Microsoft.AspNetCore.Cors;
using Web2023_BE.Entities;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller footer
    /// </summary>
    [ApiController]
    public class HtmlSectionsController : BaseEntityController<HtmlSection>
    {
        #region Declare
        IHtmlSectionService _HtmlSectionService;
        ILogger<HtmlSection> _logger;
        #endregion

        #region Constructer
        public HtmlSectionsController(IHtmlSectionService HtmlSectionService, ILogger<HtmlSection> logger) : base(HtmlSectionService, logger)
        {
            _HtmlSectionService = HtmlSectionService;
            _logger = logger;
        }
        #endregion
    }
}
