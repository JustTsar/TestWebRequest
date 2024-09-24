using System.IO;
using System.Text;
using UnityEngine;

namespace SaveSystem
{
    public class SaveFile
    {
        public static string _path;

        public static SaveFile CreateJson(string name)
        {
            Debug.Log("Create");
            _path = Path.Combine(Application.dataPath, name + ".json");
            return new PlainSaveFile();
        }
        

        public void Save(SavedData state)
        {
            WriteJson(state);
        }
        
        public bool TryReadJson<T>(out T content)
        {
            if (true)
            {
                content = ReadJson<T>();
                return true;
            }
            
            content = default;
            return false;
        }
        
        public T ReadJson<T>()
        {
            var json = ReadText();
            return Json.Parse<T>(json);
        }
        
        public string ReadText()
        {
            var bytes = ReadBytes();
            return Encoding.UTF8.GetString(bytes);
        }
        
        public  byte[] ReadBytes()
        {
            return File.ReadAllBytes(_path);
        }
        
        public void WriteJson<T>(T content)
        {
#if UNITY_EDITOR
            var json = Json.Stringify(content, true);
#else
            var json = Json.Stringify(content);
#endif
            WriteText(json);
        }
        
        public void WriteText(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            WriteBytes(data);
        }
        
        public  void WriteBytes(byte[] data)
        {
            File.WriteAllBytes(_path, data);
        }
    }
    

    public class PlainSaveFile : SaveFile
    {
        
    } 
}