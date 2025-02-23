using System;
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
    private Prism selectedPrism; //ѡ��ķ�������
    private float interactDistance = 2f;
    private Vector3 lastInteractDir;
    public bool isSuccess; //�Ƿ�ͨ�ص�ǰ�ٿ�
    private bool isWalking = false;


    private void Awake()
    {
        isSuccess = false;
        Instance = this;
        EventManager.Instance.AddListener(EventName.InteractPrism, InteractPrism);
        EventManager.Instance.AddListener(EventName.LaserToEnd, LaserLevelSuccess);
        EventManager.Instance.AddListener(EventName.EnterNextLevel,NextLevel);
    }

    private void OnEnable()
    {
        isSuccess = false;
    }
    private void Start()
    {
        if (gameObject.scene == SceneManager.GetSceneByName(SceneName.LaserLevel1)) //��ǰ����� laserLevel�ؿ���
        {
            Debug.Log("��ҵ�ǰ�ڼ���ؿ�1");
            EventManager.Instance.RaiseEvent(EventName.TeachLaserLevel, this);
        }
        if (gameObject.scene == SceneManager.GetSceneByName(SceneName.LaserLevel2)) //��ǰ����� laserLevel�ؿ���
        {
            Debug.Log("��ҵ�ǰ�ڼ���ؿ�2");
            EventManager.Instance.RaiseEvent(EventName.TeachObstacle, this);
        }

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

        if (!Physics.Raycast(transform.position, direction, 1f)) //����Ƿ����ϰ���
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
        float radius = 1.6f; // �뾶��С
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


    public void InteractPrism(object name,EventArgs args) //ȷ���ƶ�ת̨
    {
      
        if (selectedPrism != null)
        {
            selectedPrism.RotatePrism();
        }
    }

    public void LaserLevelSuccess(object name, EventArgs args)
    {
        EventManager.Instance.RaiseEvent(EventName.LaserLevelSuccess, this);
        isSuccess = true;
    }

    public void NextLevel(object name, EventArgs args)
    {
        if (isSuccess)
        {
            if (this.gameObject.scene == SceneManager.GetSceneByName(SceneName.LaserLevel1))
            {
                EventManager.Instance.RaiseEvent(EventName.LoadLaserLevel2, this);
            }
             
        }
    }

}

