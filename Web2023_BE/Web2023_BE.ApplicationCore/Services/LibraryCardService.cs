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
    public class LibraryCardService : BaseService<LibraryCard>, ILibraryCardService
    {
        #region Declare
        ILibraryCardRepository _libraryCardRepository;
        IConfiguration _config;
        #endregion

        #region Constructer
        public LibraryCardService(ILibraryCardRepository libraryCardRepository, IConfiguration config) : base(libraryCardRepository)
        {
            _libraryCardRepository = libraryCardRepository;
            _config = config;
        }

        #endregion

        #region Methods
        public async Task<LibraryCard> GetLibraryCardByAccountID(Guid accountID) => await _libraryCardRepository.GetLibraryCardByAccountID(accountID);

        public async Task<long> GetTotalLibraryCard() => await _libraryCardRepository.GetTotalLibraryCard();

        public async Task<string> GetNextBookOrderCode() => FunctionHelper.NextRecordCode(await _libraryCardRepository.GetNextCardCode());

        public async Task<string> GetNextCardCode()
        {
            var code = await _libraryCardRepository.GetNextCardCode();
            if (code == null) return "LC000001";
            else return FunctionHelper.NextRecordCode(code);
        }

        public async Task<int> AcceptMany(AcceptCardMany acceptCardMany)
        {
            string idConverted = string.Empty;
            if(acceptCardMany.IdSelected.Count() > 0)
            {
                idConverted = string.Join("|", acceptCardMany.IdSelected);
            }
            return await _libraryCardRepository.AcceptMany(idConverted, acceptCardMany.CardStatus);
        }
        #endregion
    }
}
