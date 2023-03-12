using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Web2023_BE.ApplicationCore.Entities;

namespace Web2023_BE.ApplicationCore.Extensions
{
    public static class MethodExtensions
    {
        /// <summary>
        /// Lấy tên class
        /// </summary>
        /// <returns></returns>
        public static string GetTableName(this Type type)
        {
            var configTable = GetConfigTable(type);
            if (configTable == null) return type.Name;
            return configTable.TableName;
        }

        /// <summary>
        /// Lấy trường unique trong table
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueColumns(this Type type)
        {
            var configTable = GetConfigTable(type);
            return configTable.UniqueColumns;
        }

        /// <summary>
        /// Lấy tên trường hiển thị
        /// </summary>
        /// <returns></returns>
        public static string GetColumnDisplayName(this Type type, string name)
        {
            var obj = type.GetProperty(name).GetCustomAttributes(typeof(DisplayAttribute),
                                               false).Cast<DisplayAttribute>().SingleOrDefault();
            if(obj == null) return name;  

            return obj.Name;
        }

        /// <summary>
        /// Lấy trạng thái table có trường deleted không
        /// </summary>
        /// <returns></returns>
        public static bool GetHasDeletedColumn(this Type type)
        {
            var configTable = GetConfigTable(type);
            return configTable.HasDeletedColumn;
        }

        /// <summary>
        /// Lấy config table
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static ConfigTables GetConfigTable(this Type type)
        {
            var configTable = type.GetCustomAttributes(typeof(ConfigTables), true).FirstOrDefault() as ConfigTables;
            
            if(configTable == null)
            {
                configTable = new ConfigTables();
            }

            return configTable;
        }

        /// <summary>
        /// Lấy giá trị khóa chính
        /// </summary>
        /// <returns></returns>
        public static object GetKeyValue(this Type type, object data)
        {
            var propeties = type.GetProperties();
            var key = propeties.FirstOrDefault(f => f.IsDefined(typeof(KeyAttribute), true));
            if (key == null) return null;
            return key.GetValue(data);
        }

        /// <summary>
        /// Lấy tên khóa chính
        /// </summary>
        /// <returns></returns>
        public static string GetKeyName(this Type type)
        {
            var propeties = type.GetProperties();
            var key = propeties.FirstOrDefault(f => f.IsDefined(typeof(KeyAttribute), true));
           
            if (key == null)
            {
                throw new ArgumentException($"{type.GetTableName()} Không có primarykey");
            };

            return key.Name;
        }

        /// <summary>
        /// Lấy danh sách tên cột trong đối tượng
        /// </summary>
        /// <returns></returns>
        public static List<string> GetColumNames(this Type type)
        {
            var propeties = type.GetProperties().Where(f => !f.IsDefined(typeof(IExclude), true)).Select(f => f.Name);

            return propeties.ToList();
        }

        /// <summary>
        /// Lấy giá trị theo tên
        /// </summary>
        /// <returns></returns>
        public static object GetValueByFieldName(this Type type, object data, string name)
        {
            var property = type.GetProperties().FirstOrDefault(f => f.Name == name);
            if (property == null) return null;
            return property.GetValue(data);
        }
    }
}
