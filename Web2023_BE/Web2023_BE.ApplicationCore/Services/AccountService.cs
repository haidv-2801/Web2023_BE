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
using Web2023_BE.ApplicationCore.Extensions;
using Web2023_BE.ApplicationCore.Authorization;
using Web2023_BE.ApplicationCore.Enums;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        #region Declare
        IAccountRepository _accountRepository;
        ILibraryCardService _libraryCardService;
        IConfiguration _config;
        private IJwtUtils _jwtUtils;
        #endregion

        #region Constructer
        public AccountService(IAccountRepository accountRepository, ILibraryCardService libraryCardService, IConfiguration config, IJwtUtils jwtUtils) : base(accountRepository)
        {
            _accountRepository = accountRepository;
            _config = config;
            _libraryCardService = libraryCardService;
            _jwtUtils = jwtUtils;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách nhân viên phân trang, tìm kiếm
        /// </summary>
        /// <param name="filterValue">Giá trị tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="offset">Số trang</param>
        /// <returns>Danh sách nhân viên</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        public ServiceResult GetAccountsFilterPaging(string filterValue, int pageSize, int pageNumber)
        {
            //.1 Lấy danh sách nhân viên phân trang
            var postDbResponse = (DbResponse)_accountRepository.GetAccountsFilterPaging(filterValue, pageSize, pageNumber);

            //2. Lấy tổng số bản ghi
            long totalRecord = postDbResponse.TotalRecords;

            //3. Trả về kết quả tính toán
            _serviceResult.Data = new
            {
                totalRecord = totalRecord,
                totalPage = totalRecord % pageSize == 0 ? (totalRecord / pageSize) : (totalRecord / pageSize) + 1,
                pageSize = pageSize,
                pageNumber = pageNumber,
                pageData = postDbResponse.Data
            };

            return _serviceResult;
        }

        /// <summary>
        /// Validate tùy chỉn theo màn hình nhân viên
        /// </summary>
        /// <param name="entity">Thực thể nhân viên</param>
        /// <returns>(true-false)</returns>
        protected override bool ValidateCustom(Account post)
        {
            bool isValid = true;

            //1. Đọc các property
            var properties = post.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (isValid && property.IsDefined(typeof(IDuplicate), false))
                {
                    //1.1 Check trùng
                    isValid = ValidateDuplicate(post, property);
                }

                if (isValid && property.IsDefined(typeof(IEmailFormat), false))
                {
                    //1.2 Kiểm tra định dạng email
                    isValid = ValidateEmail(post, property);
                }
            }
            return isValid;
        }

        /// <summary>
        /// Custom băm lại mật khẩu khi insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override Account CustomValueWhenUpdate(Account entity)
        {
            //entity.Password = CreateMD5(entity.Password);
            return entity;
        }

        /// <summary>
        /// Custom băm lại mật khẩu khi insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override async Task<Account> CustomValueWhenInsert(Account entity)
        {
            entity.Password = CreateMD5(entity.Password);
            return entity;
        }

        /// <summary>
        /// Validate trùng
        /// </summary>
        /// <param name="entity">Thực thể</param>
        /// <param name="propertyInfo">Thuộc tính của thực thể</param>
        /// <returns>(true-đúng false-sai)</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        private bool ValidateDuplicate(Account post, PropertyInfo propertyInfo)
        {
            bool isValid = true;

            //1. Tên trường
            var propertyName = propertyInfo.Name;

            //2. Tên hiển thị
            var propertyDisplayName = _modelType.GetColumnDisplayName(propertyName);

            var propertyValue = (string)propertyInfo.GetValue(post);

            //3. Tùy chỉnh nguồn dữ liệu để validate, trạng thái thêm hoắc sửa
            var entityDuplicate = _accountRepository.GetEntityByProperty(post, propertyInfo);

            if (entityDuplicate != null && !string.IsNullOrEmpty(propertyValue))
            {
                isValid = false;

                _serviceResult.Code = Code.InValid;
                _serviceResult.Messasge = Properties.Resources.Msg_NotValid;
                _serviceResult.Data = string.Format(Properties.Resources.Msg_Duplicate, propertyDisplayName);
            }

            return isValid;
        }

        /// <summary>
        /// Validate định dạng email
        /// </summary>
        /// <param name="post"></param>
        /// <param name="propertyInfo"></param>
        /// <returns>(true-đúng false-sai)</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        private bool ValidateEmail(Account post, PropertyInfo propertyInfo)
        {
            bool isValid = true;

            //1. Tên trường
            var propertyName = propertyInfo.Name;

            //2. Tên hiển thị
            var propertyDisplayName = _modelType.GetColumnDisplayName(propertyName);

            //3. Gía trị
            var value = propertyInfo.GetValue(post);

            //Không validate required
            if (string.IsNullOrEmpty(value.ToString()))
                return isValid;


            isValid = new EmailAddressAttribute().IsValid(value.ToString());
            //4. Gán message lỗi
            if (!isValid)
            {
                _serviceResult.Code = Code.InValid;
                _serviceResult.Messasge = Properties.Resources.Msg_NotValid;
                _serviceResult.Data = string.Format(Properties.Resources.Msg_NotFormat, propertyDisplayName);
            }

            return isValid;
        }

        /// <summary>
        /// Lấy giá trị theo tên thuộc tíh
        /// </summary>
        /// <param name="post">Thông tin nhân viên</param>
        /// <param name="propName">Tên thuộc tính</param>
        /// <returns>Giá trị</returns>
        /// CREATED BY: DVHAI (12/07/2021)
        private object GetValueByProperty(Account post, string propName)
        {
            var propertyInfo = post.GetType().GetProperty(propName);

            //Trường hợp là datetime thì format lại
            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {
                var value = propertyInfo.GetValue(post, null);
                var date = Convert.ToDateTime(value).ToString("dd/MM/yyyy");

                return value != null ? date : "";
            }

            return propertyInfo.GetValue(post, null);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        public async Task<object> Login(AccountLoginDTO userRequest)
        {
            userRequest.Password = CreateMD5(userRequest.Password);

            var account = (Account)(await _accountRepository.Login(userRequest));
            string token = "";
            if (account != null)
            {
                if (account.Status == false) return null;

                //get role
                var roles = (await _accountRepository.GetRolesByAccountID(account.AccountID.ToString()));

                //get member info
                //var memberInfo = await _libraryCardService.GetLibraryCardByAccountID(account.AccountID);
                if(account != null)
                {
                    var role = roles.Select(item =>item.RoleType).ToList();
                    string roleString = string.Join(",", role.ToArray());
                    token = _jwtUtils.GenerateJwtToken(roleString, account.UserName);
                }

                return new { userInfo = new { roles = roles.Select(item => new { RoleName = item.RoleName, RoleType = item.RoleType }), Email = account.Email, UserName = account.UserName, FullName = account.FullName, PhoneNumer = account.PhoneNumber, CreatedDate = account.CreatedDate, Avatar = account.Avatar, UserID = account.AccountID }, token = GenerateJSONWebToken(account)};
            }

            return account;
        }

        /// <summary>
        /// Tạo token để xác thực
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJSONWebToken(Account userInfo)
        {
            var test = DateTime.Now.AddMinutes(1);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("CreatedDate", userInfo.CreatedDate.Value.ToString("dd-MM-yyyy")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_config["Authenication:TimeExpired"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Băm md5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Change pw
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ServiceResult> ChangePassword(Guid entityId, AccountPasswordChangeDTO entity)
        {
            Account acc = await _accountRepository.GetEntityById(entityId);

            if (acc == null)
            {
                _serviceResult.Data = 0;
                _serviceResult.Code = Code.Fail;
                _serviceResult.Messasge = Properties.Resources.Msg_Failed;
            }
            else
            {
                string pwHashing = CreateMD5(entity.OldPassword);
                if (pwHashing == acc.Password)
                {
                    entity.Password = CreateMD5(entity.Password);
                    int rowsAffect = _accountRepository.UpdateAccountPassword(entityId, entity);
                    _serviceResult.Data = rowsAffect;

                    if (rowsAffect > 0)
                    {
                        _serviceResult.Code = Code.Valid;
                        _serviceResult.Messasge = Properties.Resources.Msg_Success;
                    }
                    else
                    {
                        _serviceResult.Code = Code.Fail;
                        _serviceResult.Messasge = Properties.Resources.Msg_Failed;
                    }
                }
                else
                {
                    _serviceResult.Data = 0;
                    _serviceResult.Code = Code.InValid;
                    _serviceResult.Messasge = string.Format(Properties.Resources.Msg_InCorrect, "Mật khẩu cũ");
                }

            }

            return _serviceResult;
        }
        #endregion
    }
}
