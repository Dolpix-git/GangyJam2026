using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CardGame.Run
{
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "run.sav");

        public static void Save(RunSaveData data)
        {
            using var stream = new FileStream(SavePath, FileMode.Create);
            new BinaryFormatter().Serialize(stream, data);
            Debug.Log($"[SaveSystem] Saved to {SavePath}");
        }

        public static RunSaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                return null;
            }

            using var stream = new FileStream(SavePath, FileMode.Open);
            return (RunSaveData)new BinaryFormatter().Deserialize(stream);
        }

        public static bool HasSave()
        {
            return File.Exists(SavePath);
        }

        public static void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }
    }
}