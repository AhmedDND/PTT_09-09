using System.Collections.Generic;

[System.Serializable]
public class CardDataSave
{
    public string CardName;
    public bool IsFlipped;
    public bool IsMatched;
}

[System.Serializable]
public class GameSaveData
{
    public List<CardDataSave> Cards = new List<CardDataSave>();
    public float RoundTime;
    public int TotalScore;
    public int TurnsTaken;
    public int CurrentCombo;
    public int Rows;
    public int Columns;
    public string CategoryName;
    public Difficulty Difficulty;
}