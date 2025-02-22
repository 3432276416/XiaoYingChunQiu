using UnityEngine;

[CreateAssetMenu(fileName = "EnmDataSO", menuName = "Card/EnmDataSO", order = 0)]
public class EnmDataSO : ScriptableObject {
    public Sprite heroSprite; //图片
    public int HP;
    public int Attack; //攻击
}