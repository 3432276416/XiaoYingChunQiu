using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float rotateSpeed = 10;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    private bool isWalking = false;


    private void Awake()
    {
        Instance = this;
        
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

        transform.position += direction * Time.deltaTime * moveSpeed;

        //if (direction != Vector3.zero)
        //{
        //    transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * rotateSpeed);
        //}
    }
    private void HandleInteraction()
    {
        RaycastHit hitInfo;
        bool isCollide = Physics.Raycast(transform.position, transform.forward, out  hitInfo, 2f, counterLayerMask);
       
        if(isCollide)
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
            {
                SetSelectedCounter(counter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    public void SetSelectedCounter(BaseCounter counter)
    {
      
    }

}
