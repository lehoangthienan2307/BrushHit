using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "LevelDatabase", fileName = "LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    public LevelObject[] levels;
    public GameObject GetRound(int levelIndex, int roundIndex)
    {
        return levels[levelIndex].rounds[roundIndex].round;
    }
    public int GetRoundCount(int levelIndex)
    {
        return levels[levelIndex].rounds.Length;
    }
    public Color GetColored(int levelIndex, int roundIndex)
    {
        return levels[levelIndex].rounds[roundIndex].colored;
    }
    public Color GetUnColored(int levelIndex, int roundIndex)
    {
        return levels[levelIndex].rounds[roundIndex].uncolored;
    }
    public bool HasNextLevel(int levelIndex)
    {
        return levels[levelIndex].haveNextLevel;
    }
}

[Serializable]
public class LevelObject
{
    public int level;
    //public LevelObject nextLevel;
    public bool haveNextLevel;
    public RoundObject[] rounds;
}
[Serializable]
public class RoundObject
{
    public GameObject round;
    public Color uncolored;
    public Color colored;
}


