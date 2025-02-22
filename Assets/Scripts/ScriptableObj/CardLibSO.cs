using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardLibSO", menuName = "Card/CardLibSO")]
public class CardLibSO : ScriptableObject {
    public List<CardLibEntry> cardLibList;
}

[System.Serializable]
public struct CardLibEntry
{
    public CardDataSO cardData;
    public int amount;
}