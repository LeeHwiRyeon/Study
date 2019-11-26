using System;
using System.IO;
using System.Xml.Serialization;

namespace DotNetFrameworkVersionManager
{
    public static class Util
    {
        public static T Load<T>(string config_file)
        {
            T init = default;
            if (File.Exists(config_file)) {
                var deserializer = new XmlSerializer(typeof(T));
                using (var reader = new StreamReader(config_file)) {
                    try {
                        init = (T)deserializer.Deserialize(reader);
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                    }
                }
            }
            return init;
        }

        public static void Save<T>(T cfg, string config_file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(config_file)) {
                serializer.Serialize(writer, cfg);
            }
        }
    }
}
