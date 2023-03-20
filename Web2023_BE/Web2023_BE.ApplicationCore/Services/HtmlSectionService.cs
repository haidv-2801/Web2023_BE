using ClosedXML.Excel;
using Web2023_BE.ApplicationCore.Entities;
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Interfaces.IRepositories;
using Web2023_BE.ApplicationCore.Extensions;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public class HtmlSectionService : BaseService<HtmlSection>, IHtmlSectionService
    {
        #region Declare
        IBaseRepository<HtmlSection> _baseRepository;
        IConfiguration _config;
        #endregion

        #region Constructer
        public HtmlSectionService(IBaseRepository<HtmlSection> footerRepository, IConfiguration config) : base(footerRepository)
        {
            _baseRepository = footerRepository;
            _config = config;
        }
        #endregion

        #region Methods
        #endregion
    }
}
