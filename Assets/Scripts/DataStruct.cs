using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public enum PowerUpType
{
    SpeedUp,
    DoubleShip,
    MultiShoot
}

public enum GameTag
{
    DeadZone,
    Player,
    Enemy,
    SceneToplimit
}

public enum PrefKey
{
    BestScore,
    MusicEnabled,
    SoundEnabled
}


[System.Serializable]
public class PowerUpDrop
{
    public GameObject powerUpPrefab;
    [Range(0f, 1f)] public float dropChance;
}
