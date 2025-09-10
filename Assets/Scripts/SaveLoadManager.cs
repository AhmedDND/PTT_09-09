using UnityEngine;
using System.IO;

public static class SaveLoadManager
{
    private static string savePath = Application.persistentDataPath + "/savegame.json";

    public static void SaveGame(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public static GameSaveData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return null;
        }

        string json = File.ReadAllText(savePath);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
        Debug.Log("Game loaded from: " + savePath);
        return data;
    }
}
