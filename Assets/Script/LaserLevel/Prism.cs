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
    public GameObject LaserPrefab; //����Ԥ����
    public Laser laser;
    public GameObject Crystal;
    public Vector3 addtionVec=new Vector3(0,1.5f,0);
    public PrismType type;
    public List<Vector3> forwards = new List<Vector3>(); //��ת�����ֶ�����
    public float rotateSpeed = 10;
    public int forwardIndex = 0;
    public GameObject Rotate_Text;
    private Vector3 textVec=new Vector3(-5.37f,1f,-3.93f);
   [SerializeField] public LaserType laserType;
    public bool isShooting=false;
    private void Start()
    {
        isShooting = false;
        forwardIndex = 0;
        if (LaserPrefab != null && type == PrismType.Start)
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
            
           GameObject oj=Instantiate(LaserPrefab, this.transform.position + addtionVec, this.transform.rotation, this.transform);
           laser = oj.GetComponent<Laser>();
           laser.SetLaserType(laserType);
           this.transform.localEulerAngles = forwards[0] - this.transform.localEulerAngles;
            isShooting=true;
           forwardIndex = 0;
        
    }


    public void RotatePrism()
    {
        if(this.IsShootingPrism()) //������������������
        {
            laser.shootingPrism.DestoryLaser();
        }
           
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

    public void ShowRotateText(bool isShow) //չʾ��ʾ��ת��ui
    {
        
            Rotate_Text.SetActive(isShow);
            Rotate_Text.transform.transform.localEulerAngles = Vector3.zero;
    }


    public void DestoryLaser()
    {
        Laser[] lasers=GetComponentsInChildren<Laser>(); 
        if(lasers!=null)
        {
            foreach(Laser laser in lasers)
            {
                if(laser.shootingPrism !=null)
                {
                
                    Prism pm=laser.shootingPrism.gameObject.GetComponent<Prism>();
                    pm.DestoryLaser(); //�ݹ�ݻټ���

                }
                isShooting = false;
                Destroy(laser.gameObject);
            }
        }
    }


   
    public bool IsShootingPrism() //����ļ����Ƿ���������
    {
        return laser.shootingPrism != null;
    }

    public void LaserToEnd(Laser laser)  //�����յ㴥������
    {
       EventManager.Instance.RaiseEvent(EventName.LaserToEnd,laser);
    }

   


  
}
