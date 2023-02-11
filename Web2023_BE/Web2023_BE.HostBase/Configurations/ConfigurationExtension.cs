using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Web2023_BE.Domain.Configs;
using Microsoft.Extensions.Options;
using Web2023_BE.HostBase;

namespace MISA.Legder.HostBase.Configurations
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Tên biến EnvironmentVariable chứa đường dẫn tới thư mục gốc
        /// </summary>
        private const string ROOT_FOLDER = "ROOT_FOLDER";
        /// <summary>
        /// Version dll, hiển thị khi gọi /health
        /// </summary>
        public static string DLLVersion = "";
        /// <summary>
        /// Cấu hình môi trường
        /// </summary>
        public static string Env = "";

        /// <summary>
        /// Thư mục config
        /// </summary>
        private static string _configFolder = null;

        /// <summary>
        /// Thư mục config
        /// </summary>
        private static string _rootFolder = null;

        /// <summary>
        /// Lấy thư mục gốc lưu file
        /// </summary>
        public static string GetRootFolder()
        {
            if (string.IsNullOrEmpty(_rootFolder))
            {
                string basePath = AppContext.BaseDirectory;
                string rootFolder = Environment.GetEnvironmentVariable(ROOT_FOLDER) ?? "";

                rootFolder = Path.Combine(basePath, rootFolder);
                _rootFolder = rootFolder;

                Console.WriteLine($"RootFolder: {_rootFolder}");
            }

            return _rootFolder;
        }

        /// <summary>
        /// Lấy thư mục cấu hình
        /// </summary>
        public static string GetConfigFolder()
        {
            if (string.IsNullOrEmpty(_configFolder))
            {
                var rootFolder = GetRootFolder();
                var configName = GetConfigFolderName();
                _configFolder = Path.Combine(rootFolder, configName);

                Console.WriteLine($"ConfigFolder: {_configFolder}");
            }

            return _configFolder;
        }

        /// <summary>
        /// Đọc các file config truyền vào và file spec theo {type.namespace}.json nếu có
        /// </summary>
        public static IHostBuilder ConfigureAppsettings(this IHostBuilder builder, Type type, IEnumerable<string> files)
        {
            DLLVersion = type.Assembly.GetName().Version.ToString();
            Console.WriteLine($"------ DLLVersion {DLLVersion}");

            Console.WriteLine($"ConfigureAppsettings files {string.Join(", ", files)}");
            string configFolder = GetConfigFolder();

            //default config
            var file = Path.Combine(configFolder, ConfigFileName.Default);
            AddFileConfig(builder, file);

            foreach (var item in files)
            {
                file = Path.Combine(configFolder, item);
                AddFileConfig(builder, file);
            }

            //specifix file with type
            file = Path.Combine(configFolder, $"{type.Namespace}.json");
            AddFileConfig(builder, file);

            return builder;
        }

        /// <summary>
        /// Thêm file cấu hình
        /// </summary>
        private static void AddFileConfig(IHostBuilder builder, string file)
        {
            if (File.Exists(file))
            {
                builder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(file);
                });
            }
            else
            {
                Console.WriteLine($"Not found: {file}");
            }
        }

        /// <summary>
        /// Đọc cấu hình NLog
        /// </summary>
        /// <param name="builder">builder</param>
        /// <param name="nlogFile">Tên file config, tình huống app nào có cần đặc thù thì truyền vào</param>
        public static IHostBuilder ConfigureNLog(this IHostBuilder builder, string nlogFile = ConfigFileName.NLog)
        {
            string configFolder = GetConfigFolder();
            var file = Path.Combine(configFolder, nlogFile);

            //Nếu có tồn tại file NLog thì mới sử dụng NLog
            if (File.Exists(file))
            {
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();   //Không log ra màn hình nữa
                    logging.AddNLog(file);
                })
                .UseNLog();

                try
                {
                    var logFactory = NLog.LogManager.LoadConfiguration(file);
                    var rules = logFactory.Configuration.LoggingRules;
                    var ls = new List<string>();
                    foreach (var item in rules)
                    {
                        ls.Add(item.ToString());
                    }
                    Console.WriteLine($"------ NLog config: { string.Join("  --  ", ls)}");
                }
                catch (Exception ex)
                {
                    //nothing
                    Console.WriteLine($"EXCEPTION ViewLogLevel: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Not found: {file}");
            }

            return builder;
        }

        public static Task WriteResponse(HttpContext context, Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport result)
        {
            var sb = BuildResponseText();

            return context.Response.WriteAsync(sb.ToString());
        }

        public static Task WriteResponse(HttpContext context, Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport result, string customText)
        {
            var sb = BuildResponseText();
            sb.AppendLine($"{customText}");

            return context.Response.WriteAsync(sb.ToString());
        }

        private static StringBuilder BuildResponseText()
        {
            var sb = new StringBuilder();
            //context.Response.ContentType = "application/json; charset=utf-8";
            sb.AppendLine("Healthy");
            sb.AppendLine($"Machine {Environment.MachineName}");
            sb.AppendLine($"SiteName {AppDomain.CurrentDomain.FriendlyName}");
            sb.AppendLine($"DLL Version {DLLVersion}");
            //sb.AppendLine($"Product Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");

            sb.AppendLine($"Env {Env}");

            return sb;
        }

        /// <summary>
        /// Lấy tên thư mục config
        /// Tách hàm để ai debug trỏ lên qc có checkin code lên cũng k lỗi
        /// </summary>
        private static string GetConfigFolderName()
        {
            var name = "Config";
#if DEBUG
            //Debug lên QC - Cần thì unrem
            //name = "ConfigQC";
#endif
            return name;
        }

        /// <summary>
        /// Cấu hình khởi tạo CenterConfig
        /// </summary>
        public static TConfig InitConfig<TConfig>(this IServiceCollection services, IConfiguration configuration)
            where TConfig : DefaultConfig
        {
            var config = Activator.CreateInstance<TConfig>();
            new ConfigureFromConfigurationOptions<TConfig>(configuration).Configure(config);
            services.AddSingleton(config);
            services.AddSingleton((DefaultConfig)config);

            Console.WriteLine($"------ AppConfig: {JsonConvert.SerializeObject(config)}");
            Env = config.Env;

            return config;
        }
    }
}
