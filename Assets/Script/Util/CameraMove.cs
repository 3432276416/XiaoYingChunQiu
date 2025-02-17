using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilitaryGame
{

    /// <summary>
    /// ����ƶ���ؽű�
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
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");//���������Ϲ�����0.1�����������¹�����-0.1
            if (zoomDelta != 0f)
            {
                AdjustZoom(zoomDelta);
            }
        }
        /// <summary>
        /// �ƶ����
        /// </summary>
        /// <param name="xDelta">xƫ��ֵ</param>
        /// <param name="yDelta">yƫ��ֵ</param>
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
            zoom = Mathf.Clamp01(zoom + Delta); //��������������ǽ�ֵ zoom +Delta ������ 0 �� 1 ֮�䡣
            float distance = Mathf.Lerp(Minzoom, Maxzoom, zoom); //�ú��������� a �� b ֮��������Բ�ֵ��t �ǲ�ֵϵ����a����ʼֵ���������� Minzoom����ʾ��С����ֵ����b������ֵ���������� Maxzoom����ʾ�������ֵ����t����ֵ������ȡֵ��ΧΪ[0, 1]���� t Ϊ 0 ʱ������ a���� t Ϊ 1 ʱ������ b���� t �� 0 �� 1 ֮��ʱ�����ص��� a �� b ֮����Ӧ������ֵ��

            float x = transform.position.x;
            float y = transform.position.y;
            transform.localPosition = new Vector3(x, y, distance);//����ͷ��z��Խ��������Խ��
        }
        void HandleDrag()
        {
            Vector3 mc = transform.position;

            // �����������м�
            if (Input.GetMouseButtonDown(2))
            {
                isDraging = true;

                // ���һ��ʼ�������Ļ�ϵ�����
                dragStartMousePosition = Input.mousePosition;
                dragStartMousePosition.z = mc.z;
                // ���һ��ʼ��Ļ����������
                dragStartCameraPosition = transform.position;
                Debug.Log(dragStartCameraPosition);
            }
            // ����ſ�����м�
            if (Input.GetMouseButtonUp(2))
            {
                isDraging = false;
            }

            if (isDraging)
            {

                // ��õ�ǰ�������Ļ�ϵ�����
                Vector3 dragCurrentMousePosition = Input.mousePosition;
                dragCurrentMousePosition.z = mc.z;
                // �������ƶ�ǰ�� ����������ϵ�ϵĲ�ֵ
                Vector3 distanceInWorldSpace = (Camera.main.transform.TransformPoint(dragStartMousePosition) - Camera.main.transform.TransformPoint(dragCurrentMousePosition)) / 60;

                // �õ��������������ϵ�ϵ��յ�
                Vector3 newPos = dragStartCameraPosition + distanceInWorldSpace;
                // Debug.Log(newPos);
                // �ƶ�������յ��λ��
                transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
                // Debug.Log(transform.position);
            }
        }
    }
}