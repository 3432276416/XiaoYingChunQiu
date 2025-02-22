using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置文件")]
    public MapConfigSO mapConfig;
    [Header("预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;
    [Header("地图加载")]
    public MapLayoutSO mapLayout;
    float screenHeight;
    float screenWidth;
    float colwidth;
    Vector3 generatePoint;

    List<Room> rooms = new();
    List<LineRenderer> lines = new();

    public float border;

    [SerializeField]List<RoomDataSO> roomDatas = new();
    Dictionary<RoomType,RoomDataSO> roomDataDist = new();
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        colwidth = screenWidth / mapConfig.blueprints.Count;

        foreach (var data in roomDatas)
        {
            roomDataDist.Add(data.type, data);
        }
    }

    private void OnEnable() {
        if (mapLayout.roomMapDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
    }
    public void CreateMap()
    {
        List<Room> previousColRoom = new();
        for (int i = 0; i < mapConfig.blueprints.Count; i++)
        {
            var blueprint = mapConfig.blueprints[i];
            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max + 1);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            generatePoint = new Vector3(-screenWidth / 2 + border + colwidth * i, startHeight, 0);

            var newPos = generatePoint;

            List<Room> currentColRoom = new();
            if (i == mapConfig.blueprints.Count - 1)
            {
                newPos.x = screenWidth / 2 - border * 2;
            }
            for (int j = 0; j < amount; j++)
            {
                newPos.y = startHeight - screenHeight / (amount + 1) * j;
                var room = Instantiate(roomPrefab, newPos, quaternion.identity, transform);
                var newType = GetRandomRoomType(mapConfig.blueprints[i].roomType);
                if (i == 0)
                {
                    room.state = RoomState.Attainable;
                }
                else
                {
                    room.state = RoomState.Locked;
                }
                room.SetupRoom(j,i,GetRoomData(newType));

                rooms.Add(room);
                currentColRoom.Add(room);
            }

            if (previousColRoom.Count > 0)
            {
                CreateConnection(previousColRoom, currentColRoom);
            }

            previousColRoom = currentColRoom;
        }

        SaveMap();
    }

    /// <summary>
    /// 重新生成地图，调试用
    /// </summary>
    [ContextMenu("RegenerateMap")]
    public void RegenerateMap()
    {
        foreach (var room in rooms)
            Destroy(room.gameObject);

        foreach (var line in lines)
            Destroy(line.gameObject);
        rooms.Clear();
        lines.Clear();
        CreateMap();
    }

    void CreateConnection(List<Room> col1, List<Room> col2)
    {
        HashSet<Room> connectedCol2Room = new(); //已经连上线的第二列的房间
        //下面实现随机路径连线
        foreach (var room in col1)
        {
            var tarRoom = ConnectToRandomRoom(room, col2,false);
            connectedCol2Room.Add(tarRoom);
        }

        foreach (var room in col2)
        {
            if (!connectedCol2Room.Contains(room))
                ConnectToRandomRoom(room, col1,true);
        }
    }

    /// <summary>
    /// 此函数能随机连一个房间，并返回该房间
    /// </summary>
    /// <param name="room">起点房间</param>
    /// <param name="col2">终点房间所在列</param>
    /// <returns>返回该房间，将他加到hashset中</returns>
    Room ConnectToRandomRoom(Room room, List<Room> col2,bool check)
    {
        var tarRoom = col2[UnityEngine.Random.Range(0, col2.Count)];

        //创建连线
        var line = Instantiate(linePrefab, transform);

        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, tarRoom.transform.position);

        if (!check)
        {
            room.LinkTo.Add(new(tarRoom.col,tarRoom.row));
        }
        else
        {
            tarRoom.LinkTo.Add(new(room.col,room.row));
        }
        lines.Add(line);
        return tarRoom;
    }

    RoomDataSO GetRoomData(RoomType type)
    {
        return roomDataDist[type];
    }

    RoomType GetRandomRoomType(RoomType flags)
    {
        string[] types = flags.ToString().Split(',');
        string type = types[UnityEngine.Random.Range(0, types.Length)];
        RoomType tarType = (RoomType)Enum.Parse(typeof(RoomType), type);
        return tarType;
    }

    private void SaveMap()
    {
        mapLayout.roomMapDataList = new();

        for (int i = 0; i < rooms.Count; i++)
        {
            mapLayout.roomMapDataList.Add(
                new RoomMapData{
                    posx = rooms[i].transform.position.x,
                    posy = rooms[i].transform.position.y,
                    row = rooms[i].row,
                    col = rooms[i].col,
                    data = rooms[i].data,
                    state = rooms[i].state,
                    LinkTo = rooms[i].LinkTo,
                }
            );
        }

        mapLayout.LinePosList = new();

        for (int i = 0; i < lines.Count; i++)
        {
            mapLayout.LinePosList.Add(
                new LinePos{
                    startPos = new SerializeVector3(lines[i].GetPosition(0)),
                    endPos = new SerializeVector3(lines[i].GetPosition(1))
                }
            );
        }
    }

    void LoadMap()
    {
        foreach (var item in mapLayout.roomMapDataList)
        {
            var newPos = new Vector3(item.posx,item.posy,0);
            var newRoom = Instantiate(roomPrefab, newPos, quaternion.identity,transform);
            newRoom.state = item.state;
            newRoom.LinkTo = item.LinkTo;
            newRoom.SetupRoom(item.row, item.col, item.data);
            rooms.Add(newRoom);
        }

        foreach (var item in mapLayout.LinePosList)
        {
            var line = Instantiate(linePrefab,transform);
            line.SetPosition(0,item.startPos.ToVector3());
            line.SetPosition(1,item.endPos.ToVector3());

            lines.Add(line);
        }
    }
}

