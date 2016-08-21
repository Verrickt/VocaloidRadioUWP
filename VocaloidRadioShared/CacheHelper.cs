using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace VocaloidRadioShared
{
    public class CacheHelper
    {



        private static StorageFolder folder = ApplicationData.Current.LocalCacheFolder;

        public static async Task SaveResetCacheAsync<T>(string name, T cache)
        {
            var cachefile = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            using (var outputStream = await cachefile.OpenAsync(FileAccessMode.ReadWrite))
            {

                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(outputStream.AsStreamForWrite(), cache);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }

        }

        public static async Task<T> ReadCacheAsync<T>(string name) where T : class
        {
            Debug.WriteLine("ReadingCache:" + name);
            var cachefile = await folder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists);
            using (var inputStream = await cachefile.OpenAsync(FileAccessMode.Read))
            {
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    T cache = serializer.ReadObject(inputStream.AsStreamForRead()) as T;
                    return cache;
                }
                catch (SerializationException e)
                {
                    
                    Debug.WriteLine(e);
                    return default(T);
                }

            }

        }


    }
}
