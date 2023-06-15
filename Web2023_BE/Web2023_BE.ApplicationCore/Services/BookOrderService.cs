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
using Newtonsoft.Json;
using Web2023_BE.ApplicationCore.Helpers;
using Web2023_BE.ApplicationCore.Enums;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;
using Web2023_BE.ApplicationCore.Extensions;

namespace Web2023_BE.ApplicationCore.Interfaces
{
    public class BookOrderService : BaseService<BookOrder>, IBookOrderService
    {
        #region Declare
        IBookOrderRepository _bookOrderRepository;
        IBookRepository _bookRepository;
        IBaseRepository<BookItem> _baseRepository;
        IBookService _bookService;
        IBaseRepository<Notification> _notificationRepository;
        IAccountService _accountService;
        IConfiguration _config;
        #endregion

        #region Constructer
        public BookOrderService(IBookOrderRepository bookOrderRepository, IBookService bookService, IBaseRepository<BookItem> baseRepository, IBaseRepository<Notification> notificationRepository, IBookRepository bookRepository, IConfiguration config, IAccountService accountService) : base(bookOrderRepository)
        {
            _bookOrderRepository = bookOrderRepository;
            _bookService = bookService;
            _baseRepository = baseRepository;
            _notificationRepository = notificationRepository;
            _bookRepository = bookRepository;
            _config = config;
            _accountService = accountService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Validate tùy chỉn theo màn hình nhân viên
        /// </summary>
        /// <param name="entity">Thực thể nhân viên</param>
        /// <returns>(true-false)</returns>
        protected override bool ValidateCustom(BookOrder book)
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
        private bool ValidateDuplicate(BookOrder book, PropertyInfo propertyInfo)
        {
            bool isValid = true;

            //1. Tên trường
            var propertyName = propertyInfo.Name;

            //2. Tên hiển thị
            var propertyDisplayName = _modelType.GetColumnDisplayName(propertyName);

            //3. Tùy chỉnh nguồn dữ liệu để validate, trạng thái thêm hoắc sửa
            var entityDuplicate = _bookOrderRepository.GetEntityByProperty(book, propertyInfo);

            if (entityDuplicate != null)
            {
                isValid = false;

                _serviceResult.Code = Web2023_BE.Entities.Enums.InValid;
                _serviceResult.Messasge = Properties.Resources.Msg_NotValid;
                _serviceResult.Data = string.Format(Properties.Resources.Msg_Duplicate, propertyDisplayName);
            }

            return isValid;
        }

        public async Task<ServiceResult> InsertBookOrder(BookOrder bookOrder)
        {
            var bookOrderInformations = (List<BookOrderInformationDTO>)JsonConvert.DeserializeObject<List<BookOrderInformationDTO>>(bookOrder.BookOrderInformation);
            var totalLoans = bookOrderInformations.Select(item => item.quantity).Sum();

            //get user current borrow
            var user = await _accountService.GetEntityById(bookOrder.AccountID);

            if (user == null)
            {
                _serviceResult.Code = Web2023_BE.Entities.Enums.InValid;
                _serviceResult.Messasge = "Tài khoản không tồn tại";
                return _serviceResult;
            }

            //validate total loans
            bool isValid = bookOrderInformations.Any(item => !ValidateBookLoanAmount(1, item.BookType, totalLoans));


            //validate exits book
            var bookIDs = bookOrderInformations.Select(book => "'" + book.id + "'").ToList();
            var query = "SELECT * FROM BOOK WHERE BookID IN";
            if (bookIDs.Count > 0)
            {
                query += string.Format(" ({0}) ", string.Join(",", bookIDs));
            }
            else
            {
                _serviceResult.Data = 0;
                _serviceResult.Code = Web2023_BE.Entities.Enums.Fail;
                _serviceResult.Messasge = "Danh sách id đầu vào trống.";
                return _serviceResult;
            }

            var listBookByIDS = (List<BookItem>)await _baseRepository.QueryUsingCommandTextAsync(query);
            if (listBookByIDS.Count < bookOrderInformations.Count)
            {
                _serviceResult.Data = 0;
                _serviceResult.Code = Web2023_BE.Entities.Enums.Fail;
                _serviceResult.Messasge = "Số lượng sách hiện có nhỏ hơn đầu vào.";
                return _serviceResult;
            }

            if (listBookByIDS.Any(book => book.Amount == 0 || book.Available == 0 || book.Reserved == book.Amount))
            {
                _serviceResult.Data = 0;
                _serviceResult.Code = Web2023_BE.Entities.Enums.Fail;
                _serviceResult.Messasge = "Một trong số những sách đã chọn không có sẵn.";
                return _serviceResult;
            }

            string maxCode = await _bookOrderRepository.GetNextBookOrderCode();
            if (maxCode == null) maxCode = DefaultCode.BOOK_ORDER;
            bookOrder.BookOrderCode = FunctionHelper.NextRecordCode(maxCode);
            int rowEffects = await _bookOrderRepository.Insert(bookOrder);
            int rowNotificationEffects = await _notificationRepository.Insert(new Notification()
            {
                Content = string.Format("Bạn có 1 yêu cầu mượn mới từ {0}", bookOrder.CreatedBy),
                To = Guid.Empty,
                From = bookOrder.AccountID,
                CreatedDate = DateTime.Now,
                CreatedBy = bookOrder.CreatedBy,
                ModifiedDate = DateTime.Now,
                ModifiedBy = bookOrder.ModifiedBy
            });

            if (rowEffects > 0)
            {
                _serviceResult.Data = bookOrder.BookOrderID;
                _serviceResult.Code = Web2023_BE.Entities.Enums.Success;
                _serviceResult.Messasge = "Thêm mới thành công.";
            }
            else
            {
                _serviceResult.Data = 0;
                _serviceResult.Code = Web2023_BE.Entities.Enums.Fail;
                _serviceResult.Messasge = "Thêm mới thất bại.";
            }

            return _serviceResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberType"></param>
        /// <param name="totalLoans"></param>
        /// <returns></returns>
        private bool ValidateBookLoanAmount(int memberType, int bookType, long totalLoans)
        {
            string key = string.Empty;

            if (memberType == MemberType.GUEST)
            {
            }
            else if (memberType == MemberType.LECTURER)
            {
                key = bookType == BookType.SYLLABUS ? AppSettingKey.LECTURER_MAXIMUM_SYLLABUS_BOOK_BORROWED_EACH_TIME : AppSettingKey.LECTURE_MAXIMUM_REFERENCE_BOOK_BORROWED_EACH_TIME;
            }
            else if (memberType == MemberType.STUDENT)
            {
                key = bookType == BookType.SYLLABUS ? AppSettingKey.STUDENT_MAXIMUM_SYLLABUS_BOOK_BORROWED_EACH_TIME : AppSettingKey.STUDENT_MAXIMUM_SYLLABUS_BOOK_BORROWED_EACH_TIME;
            }

            return totalLoans <= long.Parse(_config[key]);
        }

        public async Task<string> GetNextBookOrderCode() => FunctionHelper.NextRecordCode(await _bookOrderRepository.GetNextBookOrderCode());

        public async Task<long> GetTotalBookOrdered()
        {
            var bookOrders = await _bookOrderRepository.GetEntities();
            var jtokenArray = bookOrders.Select(book => JsonConvert.DeserializeObject(book.BookOrderInformation) as JArray);
            var sum = jtokenArray.Select(item => GetTotalBookOrderJArray(item)).Sum();
            return sum;
        }

        public async Task<IEnumerable<BookOrderTopReportDTO>> TopBookBorrowed()
        {
            var bookOrders = await _bookOrderRepository.GetEntities();
            var bookOrderInfoArray = bookOrders.Select(book => JsonConvert.DeserializeObject<List<BookOrderInformationDTO>>(book.BookOrderInformation));
            var flattenArray = new List<BookOrderInformationDTO>();
            foreach (var item in bookOrderInfoArray)
            {
                flattenArray.AddRange(item);
            }

            var res = flattenArray.GroupBy(x => x.id).Select(p => new BookOrderTopReportDTO { BookName = p.First().BookName, BookCode = p.First().BookCode, Quantity = p.Sum(i => i.quantity) }).ToList<BookOrderTopReportDTO>();

            return res;
        }

        private long GetTotalBookOrderJArray(JArray array)
        {
            var castEnum = array.AsEnumerable();
            long sum = castEnum.Sum(item => ((JToken)(item))["quantity"] as dynamic);
            return sum;
        }


        private List<object> GetBookJArray(JArray array)
        {
            var castEnum = array.AsEnumerable();
            var ids = castEnum.Select(item => new { id = ((JToken)(item))["bookID"].Values<string>().First(), quantity = ((JToken)item)["quantity"].Values<long>().First() }).ToList<object>();
            return ids;
        }

        public async Task<long> CountTotalBookUserLoanByOrderStatus(List<string> orderStatus, Guid accountID)
        {
            string joinText = $"|{string.Join("|", orderStatus)}|";
            return await _bookOrderRepository.CountTotalBookUserLoanByOrderStatus(joinText, accountID);
        }
        #endregion
    }
}
