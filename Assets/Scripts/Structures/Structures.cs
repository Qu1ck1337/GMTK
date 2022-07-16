using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitStats
{
    public int Health;
    public int Damage;
    [Range(0f, 1f)]
    public float DodgeChance;
    [Range(0f, 1f)]
    public float Protection;
    public int CellsCount;
}

[System.Serializable]
public struct TemporaryBuffs
{
    public int DamageBoost;
    public float DodgeBoost;
    public int ExtraTurn;
}
