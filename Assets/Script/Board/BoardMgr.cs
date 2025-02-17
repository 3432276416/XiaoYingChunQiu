using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardMgr : MonoBehaviour
{

    public GameObject GridPrefab;
    [SerializeField]public float xstart, ystart; //生成棋盘的初始位置
    [SerializeField]public int xsize,ysize; //棋盘大小,n x n
    void Start()
    {
       CreateBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateBoard()
    {

        for (int i = 0; i < xsize; i++)
        {
            for(int j = 0; j < ysize; j++)
            {
               GameObject grid=Instantiate(GridPrefab,this.transform);
                grid.transform.position = GetBoardPos(i,j);
            }
        }
    }

    Vector3 GetBoardPos(int x,int y)
    {


        return new Vector3(xstart+x*1.2f,ystart+y*1.2f,0);
    }

}
