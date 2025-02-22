using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }


    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float rotateSpeed = 10;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask layerMask;
    public Camera MainCamera;
    private Prism selectedPrism; //选择的反射物体
    private float interactDistance = 2f;
    private Vector3 lastInteractDir;

    private bool isWalking = false;


    private void Awake()
    {
        Instance = this;
        EventManager.Instance.AddListener(EventName.InteractPrism, InteractPrism);
    }

    private void OnEnable()
    {
        if (gameObject.scene == SceneManager.GetSceneByName(SceneName.LaserLevel)) //当前玩家在 laserLevel关卡中
        {
            Debug.Log("玩家当前在激光关卡");
            EventManager.Instance.RaiseEvent(EventName.TeachLaserLevel, EventName.TeachLaserLevel);
        }
    }
    private void Start()
    {

       
    }


    private void Update()
    {
        HandleInteraction();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    public bool IsWalking
    {
        get
        {
            return isWalking;
        }
    }



    private void HandleMovement()
    {
        Vector3 direction = gameInput.GetMovementDirectionNormalized();

        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0, vertical);

        isWalking = direction != Vector3.zero;

        if (!Physics.Raycast(transform.position, direction, 1f)) //检测是否有障碍物
        {
            transform.position += direction * Time.deltaTime * moveSpeed;
            MainCamera.transform.position += direction * Time.deltaTime * moveSpeed;
        }

        if (direction != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, -direction, Time.deltaTime * rotateSpeed);
        }
    }
    private void HandleInteraction()
    {
        //Vector3 moveDir = gameInput.GetMovementDirectionNormalized();

        //if (moveDir != Vector3.zero)
        //{
        //    lastInteractDir = moveDir;
        //}
        float radius = 1.6f; // 半径大小
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (Collider hit in hitColliders)
        {
            if (hit.transform.TryGetComponent<Prism>(out Prism prism))
            {
              
                selectedPrism = prism;
                prism.ShowRotateText(true);
                return;
            }
        }
        if(selectedPrism != null)
        {
            selectedPrism.ShowRotateText(false);
        }
        selectedPrism = null;
       

    }


    public void InteractPrism(object name,EventArgs args) //确认移动转台
    {
        Debug.Log("确认移动转台");
        if (selectedPrism != null)
        {
            selectedPrism.RotatePrism();
        }
    }

}

