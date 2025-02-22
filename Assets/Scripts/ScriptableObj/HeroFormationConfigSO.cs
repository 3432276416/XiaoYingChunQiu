using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroFormationConfigSO", menuName = "Config/HeroFormationConfigSO")]
public class HeroFormationConfigSO : ScriptableObject {
    //public HeroDataSO[] heroDataList = new HeroDataSO[6];//好汉数据，0-1上场，2-5支持好汉
    public List<HeroDataSO> heroDataList = new List<HeroDataSO>();
    // 在初始化里面设置formationConfig.heroDataLis.Capacity = 6; 即可固定6个
}