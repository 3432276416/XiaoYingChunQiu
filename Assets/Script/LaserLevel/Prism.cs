using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PrismType
{
    Normal,
    Start,
    End,
    None
}


public class Prism : MonoBehaviour
{
    public GameObject Laser; //激光预制体
    public GameObject Crystal;
    public Vector3 addtionVec=new Vector3(0,1.5f,0);
    public PrismType type;
    public List<Vector3> forwards = new List<Vector3>(); //旋转朝向，手动更改
    public float rotateSpeed = 10;
    public int forwardIndex = 0;
    public bool isShooting; //是否正在反射光线
    public GameObject Rotate_Text;
    private Vector3 textVec=new Vector3(-5.37f,1f,-3.93f);
    private void Start()
    {
        isShooting = false;
        forwardIndex = 0;
        if (Laser != null && type == PrismType.Start)
        {
          CreateLaser();
        }

    }

    private void Update()
    {
        ////if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        ////{
        ////    RotatePrism();
        ////}
     
    }

    public void CreateLaser()
    {
      
            Instantiate(Laser, this.transform.position + addtionVec, this.transform.rotation, this.transform);
            this.transform.localEulerAngles = forwards[0] - this.transform.localEulerAngles;
            isShooting = true;
            forwardIndex = 0;
        
    }


    public void RotatePrism()
    {
           
        if (forwardIndex>=forwards.Count-1)
        {
            forwardIndex = 0;
        }
        else
        {
            forwardIndex++;
        }
        Vector3 direction = forwards[forwardIndex]-this.transform.localEulerAngles;
        transform.localEulerAngles += direction;

    }

    public void ShowRotateText(bool isShow) //展示提示旋转的ui
    {
        
            Rotate_Text.SetActive(isShow);
            Rotate_Text.transform.transform.localEulerAngles = Vector3.zero;
    }
    public void LaserToEnd(Laser laser)  //到达终点触发函数
    {
       EventManager.Instance.RaiseEvent(EventName.LaserToEnd,laser);
    }
  
}
