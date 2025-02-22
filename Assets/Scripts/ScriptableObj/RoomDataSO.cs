
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Room/RoomDataSO")]
public class RoomDataSO : ScriptableObject {
    public Sprite icon;
    public RoomType type;
    public AssetReference sceneToLoad;

}