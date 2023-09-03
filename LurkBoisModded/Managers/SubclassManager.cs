using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using PluginAPI.Core;
using System.IO;
using LurkBoisModded.Base;

namespace LurkBoisModded.Managers
{
    public class SubclassManager
    {
        private static Serializer _serializer = (Serializer)new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        private static Deserializer _deserializer = (Deserializer)new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        private static Dictionary<string, SubclassBase> _loadedSubClasses = new Dictionary<string, SubclassBase>();
        private static string _path = Plugin.instance.SubclassPath;

        public static void Init()
        {
            if (!Directory.Exists(Plugin.instance.SubclassPath))
            {
                Log.Warning("Subclass folder does not exist. Generating.");
                Directory.CreateDirectory(Plugin.instance.SubclassPath);
            }
            int files = Directory.EnumerateFiles(Plugin.instance.SubclassPath).Count();
            int counter = 0;
            foreach(string fileName in Directory.EnumerateFiles(Plugin.instance.SubclassPath))
            {
                string procFileName = fileName.Replace(".yml", string.Empty);
                if (GetSubclass(procFileName) != null)
                {
                    counter++;
                }
            }
            Log.Info($"Loaded {counter}/{files} subclasses");
        }

        public static SubclassBase GetSubclass(string fileName, SubclassBase defaultValue = null, bool writeDefaultValueToDisk = false)
        {
            if (_loadedSubClasses.ContainsKey(fileName))
            {
                return _loadedSubClasses[fileName];
            }
            else
            {
                //fileName += ".yml";
                SubclassBase subclass = ReadFromDisk(fileName);
                if (subclass == null)
                {
                    Log.Warning("Failed to get subclass based on filename. Filename: " + fileName);
                    if (defaultValue != null)
                    {
                        if (writeDefaultValueToDisk)
                        {
                            WriteToDisk(defaultValue);
                        }
                        return defaultValue;
                    }
                    return null;
                }
                else
                {
                    _loadedSubClasses.Add(fileName, subclass);
                    return subclass;
                }
            }
        }

        public static void SetSubclass(SubclassBase subClass, bool overwrite = false)
        {
            if (_loadedSubClasses.ContainsKey(subClass.FileName))
            {
                if (overwrite)
                {
                    _loadedSubClasses[subClass.FileName] = subClass;
                    WriteToDisk(subClass, overwrite: overwrite);
                }
            }
            else
            {
                _loadedSubClasses.Add(subClass.FileName, subClass);
                WriteToDisk(subClass, overwrite: overwrite);
            }
        }

        public static void ClearCache(bool saveCacheToDisk, bool overwrite = false)
        {
            if (saveCacheToDisk)
            {
                foreach (string key in _loadedSubClasses.Keys)
                {
                    WriteToDisk(_loadedSubClasses[key], overwrite: overwrite);
                }
            }
            _loadedSubClasses.Clear();
        }

        private static void WriteToDisk(SubclassBase subclass, bool overwrite = false)
        {
            string _filepath = Path.Combine(_path, subclass.FileName) + ".yml";
            if (!File.Exists(_filepath))
            {
                string output = _serializer.Serialize(subclass);
                File.WriteAllText(_filepath, output);
            }
            else
            {
                if (overwrite)
                {
                    File.Delete(_filepath);
                    string output = _serializer.Serialize(subclass);
                    File.WriteAllText(_filepath, output);
                }
            }
        }

        private static SubclassBase ReadFromDisk(string filename)
        {
            string _filepath = Path.Combine(_path, filename) + ".yml";
            if (!File.Exists(_filepath))
            {
                Log.Debug(_filepath);
                Log.Warning("Failed to get subclass: No such file. Subclass filename: " + filename);
                return null;
            }
            else
            {
                string data = File.ReadAllText(_filepath);
                SubclassBase subclass = _deserializer.Deserialize<SubclassBase>(data);
                return subclass;
            }
        }
    }
}
