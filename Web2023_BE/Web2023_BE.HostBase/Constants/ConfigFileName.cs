using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.HostBase
{
    /// <summary>
    /// Định nghĩa tên file cấu hình
    /// </summary>
    public static class ConfigFileName
    {
        /// <summary>
        /// Cấu hình NLog
        /// Món này sẽ fix đọc trong extension không cho app cấu hình
        /// </summary>
        internal const string NLog = "NLog.xml";
        /// <summary>
        /// Cấu hình mặc định chung
        /// </summary>
        public const string Default = "Default.json";
        /// <summary>
        /// Cấu hình xử lý authen token
        /// </summary>
        public const string Auth = "Auth.json";
        /// <summary>
        /// Cấu hình kết nối với database
        /// </summary>
        public const string Connections = "Connections.json";
        /// <summary>
        /// Cache
        /// </summary>
        public const string Cache = "Cache.json";
        /// <summary>
        /// Export
        /// </summary>
        public const string Export = "Export.json";
        /// <summary>
        /// App
        /// </summary>
        public const string App = "App.json";
        /// <summary>
        /// Register
        /// </summary>
        public const string Register = "Register.json";
        /// <summary>
        /// File
        /// </summary>
        public const string Storage = "Storage.json";
        /// <summary>
        /// Report
        /// </summary>
        public const string Report = "Report.json";
        /// <summary>
        /// Router
        /// </summary>
        public const string Router = "Router.json";
        /// <summary>
        /// Apm
        /// </summary>
        public const string ElasticApm = "ElasticApm.json";
        /// <summary>
        /// AuditingLog
        /// </summary>
        public const string AuditingLog = "AuditingLog.json";
        /// <summary>
        /// Import
        /// </summary>
        public const string Import = "Import.json";
        /// <summary>
        /// Sync
        /// </summary>
        public const string Sync = "Sync.json";
        /// <summary>
        /// Dịch vụ gửi email
        /// </summary>
        public const string EmailService = "EmailService.json";
        /// <summary>
        /// Dịch vụ gửi tin nhắn sms
        /// </summary>
        public const string SmsService = "SmsService.json";
    }
}
