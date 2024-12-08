using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Archiventure
{
    public class SerializationManager
    {

        public const string SaveDirectory = "/saves/";
        public const string FileName = "SaveData.save";

        public static bool Save(SaveData CurrentSaveData)
        {
            var path = Application.persistentDataPath + SaveDirectory;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string json = JsonUtility.ToJson(CurrentSaveData, true);
            File.WriteAllText(path + FileName, json);

            //This is copying the path to the save file on your computer.
            GUIUtility.systemCopyBuffer = path;

            return true;
        }

        public static SaveData Load()
        {
            string FullPath = Application.persistentDataPath + SaveDirectory + FileName;
            SaveData tempData = new SaveData();

            if (File.Exists(FullPath))
            {
                string json = File.ReadAllText(FullPath);
                tempData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                Debug.LogError("Save file does not exist!");
            }

            return tempData;
        }
    }
}
