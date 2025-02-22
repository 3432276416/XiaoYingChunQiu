using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO")]
public class MapConfigSO : ScriptableObject {
    public List<RoomBlueprint> blueprints;
}

[Serializable]
public class RoomBlueprint
{
    public int min,max;
    public RoomType roomType;

}