using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DaySettings", menuName = "FolderName/DaySettings", order = 1)]
public class DaySettings : ScriptableObject
{

    public List<SpawnCondition> SpawnConditions = new List<SpawnCondition>();
    public int EnviriomentAmount;
}

[System.Serializable]
public class SpawnCondition
{
    public EnemyType Type;
    public int limit;
    public int interval;
}
