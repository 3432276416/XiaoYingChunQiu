using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnmFormationConfigSO", menuName = "Config/EnmFormationConfigSO", order = 0)]
public class EnmFormationConfigSO : ScriptableObject {
    public List<EnmDataSO> enmDataList;
}
