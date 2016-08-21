using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VocaloidRadioShared
{
    public class JsonHelper
    {
        public static string ToJson<T>(T obj)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream _ms = new MemoryStream())
            {
                serializer.WriteObject(_ms,obj);
                var array = _ms.ToArray();
                return Encoding.UTF8.GetString(array);
            }
        }

        public static T FromJson<T>(string value)
        {
            try
            {
                var deserializer = new DataContractSerializer(typeof(T));
                using (MemoryStream _ms = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                {
                    T obj = (T)deserializer.ReadObject(_ms);
                    return obj;
                }
            }
            catch (SerializationException e)
            {
                throw new SerializationException("Unable to deserialize JSON: " + value,e);
            }
        }


    }
}
