using System;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public static class Json
    {
        static Json() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                    DateParseHandling = DateParseHandling.None,
            };
        }
        
        public static string Stringify<T>(T obj, bool prettyPrint = false)
        {
            return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None);
        }
        
        public static T Parse<T>(string json)
        {
               return JsonConvert.DeserializeObject<T>(json); 
        }

        public static bool TryParse<T>(string json, out T result) 
        { 
            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                result = default;
                return false;
            }
        }
    }
}