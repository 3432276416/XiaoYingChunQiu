using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoad : MonoBehaviour
{

     
     public AssetReference scene;

      private void Awake()
      {
          //º”‘ÿ≈‰÷√
          UnityLog.Init();
          Addressables.LoadSceneAsync(scene);
        
      }
}
