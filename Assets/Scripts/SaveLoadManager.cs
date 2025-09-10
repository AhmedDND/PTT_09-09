using UnityEngine;
using System.IO;

public static class SaveLoadManager
{
    private static string SAVEPATH = Application.persistentDataPath + "/savegame.json";

    public static void SaveGame(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SAVEPATH, json);
        Debug.Log("Game saved to: " + SAVEPATH);
    }

    public static GameSaveData LoadGame()
    {
        if (!File.Exists(SAVEPATH))
        {
            Debug.LogWarning("No save file found!");
            return null;
        }

        string json = File.ReadAllText(SAVEPATH);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
        Debug.Log("Game loaded from: " + SAVEPATH);
        return data;
    }

    public static void DeleteSaveFile()
    {
        if (File.Exists(SAVEPATH))
        {
            File.Delete(SAVEPATH);
            Debug.Log("Save file deleted: " + SAVEPATH);
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }
}
