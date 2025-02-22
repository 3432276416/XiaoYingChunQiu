using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject {
    public List<RoomMapData> roomMapDataList;
    public List<LinePos> LinePosList;
}

[System.Serializable]
public class RoomMapData
{
    public float posx,posy;
    public int row,col;
    public RoomDataSO data;
    public RoomState state;
    public List<Vector2Int> LinkTo;
}

[System.Serializable]
public class LinePos
{
    public SerializeVector3 startPos,endPos;
}