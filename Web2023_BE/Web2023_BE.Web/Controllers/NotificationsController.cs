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

namespace Web2023_BE.Web.Controllers
{
    /// <summary>
    /// Controller bài đăng
    /// </summary>
    [ApiController]
    public class NotificationsController : BaseEntityController<Notification>
    {
        #region Declare
        IBaseService<Notification> _notificationService;
        #endregion

        #region Constructer
        public NotificationsController(IBaseService<Notification> notificationService, ILogger<Notification> logger) : base(notificationService, logger)
        {
            _notificationService = notificationService;
        }
        #endregion

        #region Methods
        #endregion
    }
}
