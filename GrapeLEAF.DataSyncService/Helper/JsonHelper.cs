using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeLEAF.DataSyncService.Helper
{
    public static class JsonHelper
    {
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T ToModel<T>(this string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return default(T);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public T ToModel<T>(this string s, T model)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public object ToModel(this string json, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ToJson(this object target, bool isConvertSingleQuotes = false)
        {
            if (target == null)
                return "";
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();

            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            jsSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            });

            jsSettings.NullValueHandling = NullValueHandling.Ignore;

            var result = JsonConvert.SerializeObject(target, Formatting.None, jsSettings);

            if (isConvertSingleQuotes)
                result = result.Replace("\"", "'");

            return result;
        }

        public static string ToJsonWithoutBrackets(this object target, bool isConvertSingleQuotes = false)
        {
            var result = ToJson(target, isConvertSingleQuotes);

            if (result == "{}")
                return result;

            return result.TrimStart('{').TrimEnd('}');
        }

        static public string ToJsonByForm(this string s)
        {
            Dictionary<string, string> dicdata = new Dictionary<string, string>();

            try
            {
                var data = s.Split('&');

                for (int i = 0; i < data.Length; i++)
                {
                    var dk = data[i].Split('=');

                    StringBuilder sb = new StringBuilder(dk[1]);

                    for (int j = 2; j <= dk.Length - 1; j++)
                        sb.Append(dk[j]);

                    dicdata.Add(dk[0], sb.ToString());
                }
            }
            catch
            {
            }
            return dicdata.ToJson();
        }

        static public Dictionary<string, string> ToDictionary(this string s)
        {
            Dictionary<string, string> dicdata = new Dictionary<string, string>();
            try
            {
                var data = s.Split('&');
                for (int i = 0; i < data.Length; i++)
                {
                    var dk = data[i].Split('=');

                    StringBuilder sb = new StringBuilder(dk[1]);

                    for (int j = 2; j <= dk.Length - 1; j++)
                        sb.Append(dk[j]);

                    dicdata.Add(dk[0], sb.ToString());
                }
            }
            catch
            {
            }

            return dicdata;
        }
    }
}
