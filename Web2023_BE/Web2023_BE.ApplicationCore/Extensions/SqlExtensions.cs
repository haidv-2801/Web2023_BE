using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;

namespace Web2023_BE.ApplicationCore.Extensions
{
    public static class SqlExtensions
    {
        public static string GenerateInsertQuery(Type type, List<string> columns)
        {
            var columnToAdd = type.GetColumNames().Intersect(columns);

            if (!columnToAdd.Any()) return string.Empty;

            var query = new StringBuilder();
            query.Append($"INSERT INTO {type.GetTableName().ToLowerInvariant()} (");

            // Append the column names
            var columnNames = string.Join(", ", columnToAdd);
            query.Append(columnNames);

            query.Append(") VALUES (");

            // Append the parameter placeholders
            var parameterNames = columnToAdd.Select(key => $"@v_{key}");
            var parameterNamesString = string.Join(", ", parameterNames);
            query.Append(parameterNamesString);

            query.Append(");");

            return query.ToString();
        }

        public static string GenerateUpdateQuery(Type type, List<string> columns)
        {
            var columnToUpdate = type.GetColumNames().Where(f => f != type.GetKeyName()).Intersect(columns);
            
            if (!columnToUpdate.Any()) return string.Empty;

            var query = new StringBuilder();
            query.Append($"UPDATE {type.GetTableName().ToLowerInvariant()} SET ");

            // Append the column values as parameter placeholders
            var columnValuePairs = columnToUpdate.Select(pair => $"{pair} = @v_{pair}");
            var columnValuesString = string.Join(", ", columnValuePairs);
            query.Append(columnValuesString);

            // Append the WHERE clause with parameter placeholders
            query.Append($" WHERE {type.GetKeyName()} = @v_{type.GetKeyName()};");

            return query.ToString();
        }

        /// <summary>
        /// Ánh xạ các thuộc tính sang kiểu dynamic
        /// </summary>
        /// <param name="entity">Thực thể</param>
        /// <returns>Dan sách các biến động</returns>
        public static Dictionary<string, object> MappingDbType(this Type type, object data)
        {
            var excludes = type.GetExcludeColumnNames();
            var properties = type.GetProperties();
            var parameters = properties.Where(k => !excludes.Contains(k.Name)).ToDictionary(k => $"@{"v_" + k.Name}", v => v.GetValue(data));

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(data);
                    var propertyType = property.PropertyType;
                    if (propertyName != "EntityState")
                    {
                        parameters.Add($"@{"v_" + propertyName}", propertyValue);
                    }
                }
            }
            catch { }
            //2. Trả về danh sách các parameter
            return parameters;
        }
    }
}
