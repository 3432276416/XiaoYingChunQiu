using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroLibSO", menuName = "Hero/HeroLibSO")]
public class HeroLibSO : ScriptableObject {
    public List<HeroLibEntry> heroLibList;
}

[System.Serializable]
public struct HeroLibEntry
{
    public HeroDataSO heroData;
    public int amount;
}