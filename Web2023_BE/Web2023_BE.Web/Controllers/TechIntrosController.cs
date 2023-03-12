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
    /// Controller giới thiệu công nghệ
    /// </summary>
    [ApiController]
    public class TechIntrosController : BaseEntityController<TechIntro>
    {
        #region Declare
        ITechIntroService _techIntroService;
        ILogger<TechIntro> _logger;
        #endregion

        #region Constructer
        public TechIntrosController(ITechIntroService techIntroService, ILogger<TechIntro> logger) : base(techIntroService, logger)
        {
            _techIntroService = techIntroService;
            _logger = logger;
        }
        #endregion

        #region Methods
        #endregion
    }
}
