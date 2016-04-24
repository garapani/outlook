using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotNetApp.Toolkit.Utilities
{
    public static class IsolatedStorageHelper
    {
        #region Methods

        public static T GetObject<T>(string key, IEnumerable<Type> types = null)
        {
            try
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                {
                    string serializedObject = IsolatedStorageSettings.ApplicationSettings[key].ToString();
                    return Deserialize<T>(serializedObject, types);
                }
            }
            catch
            {
            }

            return default(T);
        }

        public static void SaveObject<T>(string key, T objectToSave, IEnumerable<Type> types = null) where T : class
        {
            if (objectToSave != null)
            {
                try
                {
                    string serializedObject = Serialize(objectToSave, types);
                    IsolatedStorageSettings.ApplicationSettings[key] = serializedObject;
                }
                catch
                {
                }
            }
        }

        public static void DeleteObject(string key)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(key);
            }
            catch
            {
            }
        }

        public static void SaveSettingValueImmediately(string propertyName, object value)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[propertyName] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch
            {
            }
        }

        #endregion

        #region Private Methods

        private static string Serialize(object objectToSerialize, IEnumerable<Type> types)
        {
            string result = null;

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType(), types);
                    serializer.WriteObject(memoryStream, objectToSerialize);
                    memoryStream.Position = 0;

                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        private static T Deserialize<T>(string jsonString, IEnumerable<Type> types)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), types);
                return (T)serializer.ReadObject(memoryStream);
            }
        }

        #endregion
    }
}
