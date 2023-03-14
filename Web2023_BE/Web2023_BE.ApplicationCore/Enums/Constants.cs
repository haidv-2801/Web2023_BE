using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Enums
{

    public static class MemberType
    {
        public static readonly int STUDENT = 1;
        public static readonly int GUEST = 2;
        public static readonly int LECTURER = 0;
    }
    public static class DataType
    {
        public static readonly string DATE_TIME = "DateTime";
        public static readonly string INTEGER = "Int32";
        public static readonly string GUID = "Guid";
    }

    public static class MenuType
    {
        public static readonly int HTML_RENDER = 0;
        public static readonly int REDIRECT = 1;
        public static readonly int NORMAL = 2;
        public static readonly int NONE_EVENT = 3;
    }

    public static class Operator
    {
        public static readonly string EQUAL = "=";
        public static readonly string NOT_EQUAL = "<>";
        public static readonly string AND = "AND";
        public static readonly string OR = "OR";
        public static readonly string NOT = "NOT";
        public static readonly string CONTAINS = "CONTAINS";
        public static readonly string START_WIDTH = "START_WIDTH";
        public static readonly string END_WIDTH = "END_WIDTH";
    }

    public static class SortType
    {
        public static readonly string DESC = "DESC";
        public static readonly string ASC = "ASC";
    }

    public static class BookFormat
    {
        public static readonly int EBOOK = 0;
        public static readonly int PAPER_BACK = 1;
    }

    public static class BookType
    {
        public static readonly int SYLLABUS = 0;
        public static readonly int REFERENCE_BOOK = 1;
    }

    public enum ReservationStatus
    {
        WAITING = 0,
        PENDING = 1,
        CANCELED = 2,
        RETURNED = 3,
        EXPIRED = 4,
        LENDING = 5,
        NONE = 6,
    }

    public enum BookStatus
    {
        AVAILABLE = 1,
        RESERVED = 3,
        LOANED = 2,
        LOST = 0
    }

    public enum CardStatus
    {
        CONFIRMING = 1,
        CONFIRMED = 2,
        REFUSE_COMFIRM = 3,
    }

    public enum SafeAddressType
    {
        IP,
        MAC
    }

    public static class DefaultCode
    {
        public static readonly string BOOK_ORDER = "BR000001";
        public static readonly string BOOK_ITEM = "B00001";
    }

    public static class CacheKey
    {
        public static readonly string ADDRESS_CACHE_KEY = "ADDRESS_CACHE";
    }

    public enum Roles
    {
        STAFF = 1,
        GUEST = 3,
        MEMBER = 2,
        ADMIN = 0
    }

    public enum CarouselSection
    {
        HomePage
    }

    public enum ModuleType
    {
        IMAGE
    }

    public enum RoleCode
    {
        PUBLIC,
        ADMIN,
       
    }
}
