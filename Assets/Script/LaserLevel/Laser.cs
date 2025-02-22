using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damageOverTime = 30;
  

    public GameObject HitEffect;
    public float HitOffset = 0;
    public bool uselaserRotation = false;

    public float MaxLength;  //最大长度
    private LineRenderer laser;
    private bool isEnd; //是否到达过终点了
    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1, 1, 1, 1);
    //private Vector4 laserSpeed = new Vector4(0, 0, 0, 0); {DISABLED AFTER UPDATE}
    //private Vector4 laserStartSpeed; {DISABLED AFTER UPDATE}
    //One activation per shoot
    private bool laserSaver = false;
    private bool UpdateSaver = false;

    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;

    void Start()
    {
        isEnd = false;
        //Get LineRender and ParticleSystem components from current prefab;  
        laser = GetComponent<LineRenderer>();
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
        //if (laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) laserStartSpeed = laser.material.GetVector("_SpeedMainTexUVNoiseZW");
        //Save [1] and [3] textures speed
        //{ DISABLED AFTER UPDATE}
        //laserSpeed = laserStartSpeed;
    }

    void Update()
    {
        //if (laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) laser.material.SetVector("_SpeedMainTexUVNoiseZW", laserSpeed);
        //SetVector("_TilingMainTexUVNoiseZW", Length); - old code, _TilingMainTexUVNoiseZW no more exist
        laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
        laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        //To set LineRender position
        if (laser != null && UpdateSaver == false)
        {
            laser.SetPosition(0, transform.position);
            RaycastHit hit; //DELETE THIS IF YOU WANT USE LASERS IN 2D
            //ADD THIS IF YOU WANNT TO USE LASERS IN 2D: RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, MaxLength);       
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxLength))//CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
            {

                //处理反射物体
                Prism prism = hit.collider.gameObject.GetComponentInParent<Prism>();
                if ( prism != null && prism.isShooting == false)
                {
                    if (prism.type == PrismType.Normal)
                    {
                        prism.CreateLaser();
                        prism.isShooting = true;
                    }
                    else if(prism.type==PrismType.End && isEnd==false)
                    {
                        Debug.Log("激光到达终点");
                        prism.LaserToEnd(this);
                        isEnd = true;
                       
                    }
                }

                //End laser position if collides with object
                laser.SetPosition(1, hit.point);

                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                if (uselaserRotation)
                    HitEffect.transform.rotation = transform.rotation;
                else
                    HitEffect.transform.LookAt(hit.point + hit.normal);

                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit.point));
                //Texture speed balancer {DISABLED AFTER UPDATE}
                //laserSpeed[0] = (laserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, hit.point));
                //laserSpeed[2] = (laserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, hit.point));
                //Destroy(hit.transform.gameObject); // destroy the object hit
                //hit.collider.SendMessage("SomeMethod"); // example
                /*if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<HittedObject>().TakeDamage(damageOverTime * Time.deltaTime);
                }*/
            }
            else
            {
                //End laser position if doesn't collide with object
                var EndPos = transform.position + transform.forward * MaxLength;
                laser.SetPosition(1, EndPos);
                HitEffect.transform.position = EndPos;
                foreach (var AllPs in Hit)
                {
                    if (AllPs.isPlaying) AllPs.Stop();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));
                //laserSpeed[0] = (laserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
                //laserSpeed[2] = (laserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
            }
            //Insurance against the appearance of a laser in the center of coordinates!
            if (laser.enabled == false && laserSaver == false)
            {
                laserSaver = true;
                laser.enabled = true;
            }
        }
    }

    public void DisablePrepare()
    {
        if (laser != null)
        {
            laser.enabled = false;
        }
        UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
    }
}
