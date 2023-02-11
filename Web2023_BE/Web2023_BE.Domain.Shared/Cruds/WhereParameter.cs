using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Cruds
{
    public class WhereParameter
    {
        private const string PrefixWhereParameter = " WHERE 1=1 ";
        private string _whereClause;
        private Dictionary<string, object> _whereValues = new Dictionary<string, object>();
        private bool _isAppendWhere = true;
        public string WhereClause
        {
            get
            {
                return this._whereClause;
            }
        }
        public Dictionary<string, object> WhereValues
        {
            get
            {
                return this._whereValues;
            }
        }
        public bool IsAppendWhere
        {
            get
            {
                return this._isAppendWhere;
            }
            set
            {
                this._isAppendWhere = value;
            }
        }
        public string CompiledWhereClause
        {
            get
            {
                return WhereParameter.FormatDynamicWhere(this);
            }
        }
        public void AddWhere(string sWhereClause, Dictionary<string, object> dictWhereValues)
        {
            StringBuilder stringBuilder = new StringBuilder(sWhereClause);
            this._whereClause += stringBuilder.ToString();
            this._whereClause = $"({this._whereClause})"; //NDQUAN 30.11.2020: trường hợp với báo cáo, chỉ filter theo giá trị trong filter và gồm nhiều điều kiện thì đang KHÔNG thêm () bao cả điều kiện này
            if (dictWhereValues != null && dictWhereValues.Count > 0)
            {
                foreach (KeyValuePair<string, object> current in dictWhereValues)
                {
                    string key = current.Key;
                    if (this._whereValues.ContainsKey(current.Key))
                    {
                        this._whereValues[key] = current.Value;
                    }
                    else
                    {
                        this._whereValues.Add(key, current.Value);
                    }
                }
            }
        }
        public void AddWhere(WhereParameter oWhereParameter)
        {
            if (oWhereParameter != null)
            {
                this.AddWhere(oWhereParameter.WhereClause, oWhereParameter.WhereValues);
            }
        }
        public static string FormatDynamicWhere(WhereParameter whereParams)
        {
            if (whereParams != null && !string.IsNullOrWhiteSpace(whereParams.WhereClause))
            {
                StringBuilder stringBuilder = new StringBuilder(whereParams.WhereClause);
                foreach (KeyValuePair<string, object> current in whereParams.WhereValues)
                {
                    KeyValuePair<string, object> keyValuePair = current;
                    if (current.Value != null)
                    {
                        if (current.Value.GetType() == typeof(string))
                        {
                            throw new Exception("Chỗ này xem lại chuyển hàm build vào service, model chỉ lưu trữ thông tin thôi");
                            //keyValuePair = new KeyValuePair<string, object>(current.Key, SecureUtil.SafeSqlLiteral(current.Value.ToString()));
                        }
                        stringBuilder.Replace("{" + keyValuePair.Key + "}", string.Concat(keyValuePair.Value) ?? "");
                    }
                }
                string result = string.Empty;
                if (whereParams.IsAppendWhere)
                {
                    result = WhereParameter.AppendWherePrefix(stringBuilder.ToString());
                }
                else
                {
                    result = stringBuilder.ToString();
                }
                return result;
            }
            return string.Empty;
        }
        private static string AppendWherePrefix(string whereClause)
        {
            if (!whereClause.Trim().StartsWith("WHERE ", StringComparison.OrdinalIgnoreCase))
            {
                if (!whereClause.Trim().StartsWith("AND ", StringComparison.OrdinalIgnoreCase))
                {
                    whereClause = "AND (" + whereClause + ")";
                }
                string prefixWhereParameter = PrefixWhereParameter;
                if (!string.IsNullOrEmpty(prefixWhereParameter))
                {
                    whereClause = prefixWhereParameter + whereClause;
                }
            }
            return whereClause;
        }

        public string GetWhereClause()
        {
            return this._whereClause;
        }

        public void AddWhere(string clause, object value)
        {
            if (!string.IsNullOrEmpty(this._whereClause))
            {
                this._whereClause += " AND ";
            }
            this._whereClause += clause;

            string paramName = "p" + (this.WhereValues.Count + 1).ToString();
            this.WhereValues.Add(paramName, value);
        }
        public void AddWhere(string clause, string paramName, object paramValue)
        {
            if (!string.IsNullOrEmpty(this._whereClause))
            {
                this._whereClause += " AND ";
            }
            this._whereClause += clause;

            this.WhereValues.Add(paramName, paramValue);
        }
    }
}
