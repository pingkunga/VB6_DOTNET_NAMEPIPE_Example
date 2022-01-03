using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeClient
{
    public class JsonCustomDateType : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
                return true;
            else
                return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //if (objectType == typeof(IList)) 
            if (reader.Value == null)
            {
                if (objectType == typeof(DateTime))
                    return DateTime.Now;
                else if (objectType == typeof(DateTime?))
                    return null;
            }
            double t;
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (double.TryParse(reader.Value.ToString(), out t))
            {
                try
                {
                    dtDateTime = dtDateTime.AddMilliseconds(t).ToLocalTime();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                dtDateTime = DateTime.Parse(reader.Value.ToString());
            }

            return dtDateTime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType().Equals(typeof(DateTime)) || value.GetType().Equals(typeof(DateTime?)))
                writer.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.fff K";
            writer.WriteValue(value);
        }
    }
}
