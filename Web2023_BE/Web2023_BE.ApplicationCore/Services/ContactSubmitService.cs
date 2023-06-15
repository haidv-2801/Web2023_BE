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
using Web2023_BE.ApplicationCore.Helpers;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public class ContactSubmitService : BaseService<ContactSubmit>, IContactSubmitService
    {
        #region Declare
        IContactSubmitRepository _libraryCardRepository;
        IConfiguration _config;
        #endregion

        #region Constructer
        public ContactSubmitService(IContactSubmitRepository libraryCardRepository, IConfiguration config) : base(libraryCardRepository)
        {
            _libraryCardRepository = libraryCardRepository;
            _config = config;
        }

        #endregion

        #region Methods
       
        #endregion
    }
}
