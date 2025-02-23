using UnityEngine;

public class BoardMgr : MonoBehaviour
{

    public GameObject GridPrefab;
    [SerializeField]public float xstart, ystart; //�������̵ĳ�ʼλ��
    [SerializeField]public int xsize,ysize; //���̴�С,n x n
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
