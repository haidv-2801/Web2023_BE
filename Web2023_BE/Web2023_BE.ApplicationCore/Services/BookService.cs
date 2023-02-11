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
using System.Threading.Tasks;
using Web2023_BE.ApplicationCore.Helpers;
using Web2023_BE.ApplicationCore.Enums;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public class BookService : BaseService<BookItem>, IBookService
    {
        #region Declare
        IBookRepository _bookRepository;
        #endregion

        #region Constructer
        public BookService(IBookRepository bookRepository) : base(bookRepository)
        {
            _bookRepository = bookRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Validate tùy chỉn theo màn hình nhân viên
        /// </summary>
        /// <param name="entity">Thực thể nhân viên</param>
        /// <returns>(true-false)</returns>
        protected override bool ValidateCustom(BookItem book)
        {
            bool isValid = true;

            //1. Đọc các property
            var properties = book.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (isValid && property.IsDefined(typeof(IDuplicate), false))
                {
                    //1.1 Check trùng
                    isValid = ValidateDuplicate(book, property);
                }

                if (isValid && property.IsDefined(typeof(IEmailFormat), false))
                {
                    //1.2 Kiểm tra định dạng email
                    isValid = ValidateEmail(book, property);
                }
            }
            return isValid;
        }

        /// <summary>
        /// Validate trùng
        /// </summary>
        /// <param name="entity">Thực thể</param>
        /// <param name="propertyInfo">Thuộc tính của thực thể</param>
        /// <returns>(true-đúng false-sai)</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        private bool ValidateDuplicate(BookItem book, PropertyInfo propertyInfo)
        {
            bool isValid = true;

            //1. Tên trường
            var propertyName = propertyInfo.Name;

            //2. Tên hiển thị
            var propertyDisplayName = GetAttributeDisplayName(propertyName);

            //3. Tùy chỉnh nguồn dữ liệu để validate, trạng thái thêm hoắc sửa
            var entityDuplicate = _bookRepository.GetEntityByProperty(book, propertyInfo);

            if (entityDuplicate != null)
            {
                isValid = false;

                _serviceResult.TOECode = TOECode.InValid;
                _serviceResult.Messasge = Properties.Resources.Msg_NotValid;
                _serviceResult.Data = string.Format(Properties.Resources.Msg_Duplicate, propertyDisplayName);
            }

            return isValid;
        }

        /// <summary>
        /// Validate định dạng email
        /// </summary>
        /// <param name="book"></param>
        /// <param name="propertyInfo"></param>
        /// <returns>(true-đúng false-sai)</returns>
        /// CREATED BY: DVHAI (07/07/2021)
        private bool ValidateEmail(BookItem book, PropertyInfo propertyInfo)
        {
            bool isValid = true;

            //1. Tên trường
            var propertyName = propertyInfo.Name;

            //2. Tên hiển thị
            var propertyDisplayName = GetAttributeDisplayName(propertyName);

            //3. Gía trị
            var value = propertyInfo.GetValue(book);

            //Không validate required
            if (string.IsNullOrEmpty(value.ToString()))
                return isValid;


            isValid = new EmailAddressAttribute().IsValid(value.ToString());
            //4. Gán message lỗi
            if (!isValid)
            {
                _serviceResult.TOECode = TOECode.InValid;
                _serviceResult.Messasge = Properties.Resources.Msg_NotValid;
                _serviceResult.Data = string.Format(Properties.Resources.Msg_NotFormat, propertyDisplayName);
            }

            return isValid;
        }

        /// <summary>
        /// Lấy giá trị theo tên thuộc tíh
        /// </summary>
        /// <param name="book">Thông tin nhân viên</param>
        /// <param name="propName">Tên thuộc tính</param>
        /// <returns>Giá trị</returns>
        /// CREATED BY: DVHAI (12/07/2021)
        private object GetValueByProperty(BookItem book, string propName)
        {
            var propertyInfo = book.GetType().GetProperty(propName);

            //Trường hợp là datetime thì format lại
            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {
                var value = propertyInfo.GetValue(book, null);
                var date = Convert.ToDateTime(value).ToString("dd/MM/yyyy");

                return value != null ? date : "";
            }

            return propertyInfo.GetValue(book, null);
        }

        protected override async Task<BookItem> CustomValueWhenInsert(BookItem entity)
        {
            var nextCode = await _bookRepository.GetNextBookCode();
            if (nextCode == null)
            {
                nextCode = DefaultCode.BOOK_ITEM;
            }
            else
            {
                nextCode = FunctionHelper.NextRecordCode(nextCode);
            }
            entity.BookCode = nextCode;
            return entity;
        }

        public async Task<string> GetNextBookCode()
        {
            var nextCode = await _bookRepository.GetNextBookCode();
            if (nextCode == null)
            {
                nextCode = DefaultCode.BOOK_ITEM;
            }
            else
            {
                nextCode = FunctionHelper.NextRecordCode(nextCode);
            }
            return (string)nextCode;
        }

        public async Task<long> GetTotalBook() => await _bookRepository.GetTotalBook();

        #endregion
    }
}
