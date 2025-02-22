using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Room : MonoBehaviour
{
    public RoomState state;
    public RoomDataSO data;
    [SerializeField]SpriteRenderer spriteRenderer;
    public int row;
    public int col;
    public List<Vector2Int> LinkTo = new();

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown() {
        if (state == RoomState.Attainable)
            loadRoomEvent.RaiseEvent(this,this);
    }
    public void SetupRoom(int row, int col,RoomDataSO data)
    {
        this.row = row;
        this.col = col;
        this.data = data;

        spriteRenderer.sprite = data.icon;
        spriteRenderer.color = state switch
        {
            RoomState.Locked => new Color(0.5f,0.5f,0.5f,0.5f),
            RoomState.Visited => Color.yellow,
            RoomState.Attainable => Color.green,
            _ => throw new System.NotImplementedException(),
        };
    }
}
