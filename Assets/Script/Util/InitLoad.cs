using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoad : MonoBehaviour
{

     
     public AssetReference scene;

      private void Awake()
      {
          //��������
          UnityLog.Init();
          Addressables.LoadSceneAsync(scene);
        
      }
}
