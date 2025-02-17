using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilitaryGame
{

    /// <summary>
    /// 相机移动相关脚本
    /// </summary>
    public class CameraMove : MonoBehaviour
    {

        float Minzoom, Maxzoom, zoom;
        float MovingSpeed;
        bool isDraging = false;

        Vector3 dragStartMousePosition;
        Vector3 dragStartCameraPosition;
        // Start is called before the first frame update
        void Start()
        {
            Minzoom = -100;
            Maxzoom = -10;
            MovingSpeed = 0.05f;
            // the starting zoom
            zoom = -40;
            AdjustZoom(zoom);
        }

        // Update is called once per frame
        void Update()
        {

            HandleDrag();
            float xDelta = Input.GetAxis("Horizontal");
            float yDelta = Input.GetAxis("Vertical");
            if (xDelta != 0f || yDelta != 0f)
            {
                AdjustPosition(xDelta, yDelta);
            }
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");//鼠标滚轮往上滚返回0.1，鼠标滚轮往下滚返回-0.1
            if (zoomDelta != 0f)
            {
                AdjustZoom(zoomDelta);
            }
        }
        /// <summary>
        /// 移动相机
        /// </summary>
        /// <param name="xDelta">x偏移值</param>
        /// <param name="yDelta">y偏移值</param>
        public void AdjustPosition(float xDelta, float yDelta)
        {
            if (transform.position.x > 38)
            {
                transform.localPosition = new Vector3((float)(transform.position.x - 0.1), transform.position.y, transform.position.z);
                return;
            }
            else if (transform.position.x < -25)
            {
                transform.localPosition = new Vector3((float)(transform.position.x + 0.1), transform.position.y, transform.position.z);
                return;
            }
            if (transform.position.y > 15)
            {
                transform.localPosition = new Vector3(transform.position.x, (float)(transform.position.y - 0.1), transform.position.z);
                return;
            }
            else if (transform.position.y < -34)
            {
                transform.localPosition = new Vector3(transform.position.x, (float)(transform.position.y + 0.1), transform.position.z);
                return;
            }

            transform.Translate(new Vector3(xDelta * MovingSpeed, yDelta * MovingSpeed));
        }
        void AdjustZoom(float Delta)
        {
            zoom = Mathf.Clamp01(zoom + Delta); //这个函数的作用是将值 zoom +Delta 限制在 0 和 1 之间。
            float distance = Mathf.Lerp(Minzoom, Maxzoom, zoom); //该函数用于在 a 和 b 之间进行线性插值，t 是插值系数。a：起始值（在这里是 Minzoom，表示最小缩放值）。b：结束值（在这里是 Maxzoom，表示最大缩放值）。t：插值参数，取值范围为[0, 1]。当 t 为 0 时，返回 a；当 t 为 1 时，返回 b；当 t 在 0 和 1 之间时，返回的是 a 和 b 之间相应比例的值。

            float x = transform.position.x;
            float y = transform.position.y;
            transform.localPosition = new Vector3(x, y, distance);//摄像头的z轴越大，离物体越近
        }
        void HandleDrag()
        {
            Vector3 mc = transform.position;

            // 如果按下鼠标中键
            if (Input.GetMouseButtonDown(2))
            {
                isDraging = true;

                // 获得一开始鼠标在屏幕上的坐标
                dragStartMousePosition = Input.mousePosition;
                dragStartMousePosition.z = mc.z;
                // 获得一开始屏幕的世界坐标
                dragStartCameraPosition = transform.position;
                Debug.Log(dragStartCameraPosition);
            }
            // 如果放开鼠标中键
            if (Input.GetMouseButtonUp(2))
            {
                isDraging = false;
            }

            if (isDraging)
            {

                // 获得当前鼠标在屏幕上的坐标
                Vector3 dragCurrentMousePosition = Input.mousePosition;
                dragCurrentMousePosition.z = mc.z;
                // 获得鼠标移动前后 在世界坐标系上的差值
                Vector3 distanceInWorldSpace = (Camera.main.transform.TransformPoint(dragStartMousePosition) - Camera.main.transform.TransformPoint(dragCurrentMousePosition)) / 60;

                // 得到相机在世界坐标系上的终点
                Vector3 newPos = dragStartCameraPosition + distanceInWorldSpace;
                // Debug.Log(newPos);
                // 移动相机到终点的位置
                transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
                // Debug.Log(transform.position);
            }
        }
    }
}