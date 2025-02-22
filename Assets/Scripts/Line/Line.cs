using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField]float moveSpeed = 0.6f;
    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        var offset = lineRenderer.material.mainTextureOffset;
        offset.x += moveSpeed*Time.deltaTime;
        
        lineRenderer.material.mainTextureOffset = offset;
    }
}
