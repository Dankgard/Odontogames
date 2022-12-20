using Newtonsoft.Json;
using StrapiForUnity;
using System.IO;

namespace JSONUtils {
    public class JSONReader {
        public T[] ReadJSONFile<T>(string filePath)
        {
            // Leemos el contenido del archivo
            string json = File.ReadAllText(filePath);

            string newJson = "{" + json + "}";
            json = newJson;

            // Convertimos el contenido del archivo a un array
            T[] array = JsonConvert.DeserializeObject<T[]>(json);

            return array;
        }

        public void SerializeObject(object obj, string filePath)
        {
            // Asume que tienes un objeto serializable llamado "myObject"
            string json = JsonConvert.SerializeObject(obj);

            // Escribe la cadena JSON en un archivo
            System.IO.File.WriteAllText("jsonfile.json", json);
        }

        public dynamic JsonToString(string filepath)
        {
            string jsonString;
            using (StreamReader r = new StreamReader("jsonfile.json"))
            {
                jsonString = r.ReadToEnd();
            }

            // Convierte la cadena JSON en un objeto de tipo dynamic
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonString);

            return jsonObject;
        }
    }
}
