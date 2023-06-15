using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Enums
{
    public static class AppSettingKey
    {
        public static readonly string STUDENT_MAXIMUM_SYLLABUS_BOOK_BORROWED_EACH_TIME = "Loans:StudentMaximumSyllabusBookBorrowedEachTime";

        public static readonly string LECTURER_MAXIMUM_SYLLABUS_BOOK_BORROWED_EACH_TIME = "Loans:LecturerMaximumSyllabusBookBorrowedEachTime";

        public static readonly string STUDENT_MAXIMUM_REFERENCE_BOOK_BORROWED_EACH_TIME = "Loans:StudentMaximumReferenceBookBorrowedEachTime";

        public static readonly string LECTURE_MAXIMUM_REFERENCE_BOOK_BORROWED_EACH_TIME = "Loans:LectureMaximumReferenceBookBorrowedEachTime";

        public static readonly string MAXIMUM_SYLLABUS_BOOK_BORROWED_TIME = "Loans:MaximumSyllabusBookBorrowedTime";

        public static readonly string MAXIMUM_REFERENCE_BOOK_BORROWED_TIME = "Loans:MaximumReferenceBookBorrowedTime";

        public static readonly string DAY_CARRY_OUT_RETURN_BOOK = "Loans:DayCarryOutReturnBook";
    }
}
