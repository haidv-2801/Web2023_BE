using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.HostBase
{
    /// <summary>
    /// Lớp này xử lý loại bỏ múi giờ: Dữ liệu người dùng nhập thế nào tôn trọng như vậy
    /// </summary>
    public class CustomDateFormatConverter : DateTimeConverterBase
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                var str = reader.Value.ToString();
                if (str.Length > 20)
                {
                    str = str.Substring(0, 20);
                }
                return DateTime.Parse(str);
            }

            return null;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}
