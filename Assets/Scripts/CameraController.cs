//using UnityEngine;

//namespace Archiventure {

//    public class CameraController2D : MonoBehaviour
//    {
//        public bool movement;
//        public float movementTime;
//        private bool multiTouch;

//        public float zoomOutMin = 1;
//        public float zoomOutMax = 8;

//        [Header("Limits")]
//        public float leftLimit;
//        public float rightLimit;
//        public float bottomLimit;
//        public float upperLimit;

//        private Vector3 dragStartPosition;
//        private Vector3 dragCurrentPosition;
//        private Vector3 newPosition;

//        public GameManager gameManager;

//        void Start()
//        {
//            multiTouch = false;
//            newPosition = transform.position;
//        }

//        void Update()
//        {
//            HandlerMouseInput();
//        }

//        void HandlerMouseInput()
//        {
//            if (movement == true)
//            {
//                if (Input.GetMouseButtonDown(0))
//                //if (Mouse.current.leftButton.isPressed)
//                {
//                    multiTouch = false;

//                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

//                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                    float entry;
//                    if (plane.Raycast(ray, out entry))
//                    {
//                        dragStartPosition = ray.GetPoint(entry);
//                    }
//                }
//                if (Input.touchCount == 2)
//                {
//                    multiTouch = true;
//                    Touch touchZero = Input.GetTouch(0);
//                    Touch touchOne = Input.GetTouch(1);

//                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
//                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

//                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
//                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

//                    float difference = currentMagnitude - prevMagnitude;

//                    zoom(difference * 0.003f);
//                }
//                if (Input.GetMouseButton(0) && multiTouch == false)
//                {
//                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

//                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                    float entry;
//                    if (plane.Raycast(ray, out entry))
//                    {
//                        dragCurrentPosition = ray.GetPoint(entry);

//                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
//                    }
//                }
//                zoom(Input.GetAxis("Mouse ScrollWheel"));
//            }
//            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
//            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, upperLimit), transform.position.z);

//        }

//        void zoom(float increment)
//        {
//            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
//        }

//        private void OnDrawGizmosSelected()
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(rightLimit, upperLimit));
//            Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
//            Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(leftLimit, bottomLimit));
//            Gizmos.DrawLine(new Vector2(rightLimit, upperLimit), new Vector2(rightLimit, bottomLimit));
//        }


//    }

//}

using UnityEngine;
using UnityEngine.InputSystem;

namespace Archiventure
{
    public class CameraController2D : MonoBehaviour
    {
        public bool movement;
        public float movementTime;
        private bool multiTouch;
        public float zoomOutMin = 1;
        public float zoomOutMax = 8;
        [Header("Limits")]
        public float leftLimit;
        public float rightLimit;
        public float bottomLimit;
        public float upperLimit;
        private Vector3 dragStartPosition;
        private Vector3 dragCurrentPosition;
        private Vector3 newPosition;
        public GameManager gameManager;

        void Start()
        {
            multiTouch = false;
            newPosition = transform.position;
        }

        void Update()
        {
            HandlerMouseInput();
        }

        void HandlerMouseInput()
        {
            if (movement)
            {
                // 处理鼠标按下
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    multiTouch = false;
                    Plane plane = new Plane(Vector3.forward, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                    float entry;
                    if (plane.Raycast(ray, out entry))
                    {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                }

                // 处理触摸
                if (Touchscreen.current != null && Touchscreen.current.touches.Count == 2)
                {
                    multiTouch = true;
                    var touch0 = Touchscreen.current.touches[0];
                    var touch1 = Touchscreen.current.touches[1];

                    // 获取当前位置
                    Vector2 touch0Pos = touch0.position.ReadValue();
                    Vector2 touch1Pos = touch1.position.ReadValue();

                    // 获取上一帧位置
                    Vector2 touch0PrevPos = touch0Pos - touch0.delta.ReadValue();
                    Vector2 touch1PrevPos = touch1Pos - touch1.delta.ReadValue();

                    float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
                    float currentMagnitude = (touch0Pos - touch1Pos).magnitude;
                    float difference = currentMagnitude - prevMagnitude;
                    zoom(difference * 0.003f);
                }

                // 处理鼠标拖动
                if (Mouse.current.leftButton.isPressed && !multiTouch)
                {
                    Plane plane = new Plane(Vector3.forward, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                    float entry;
                    if (plane.Raycast(ray, out entry))
                    {
                        dragCurrentPosition = ray.GetPoint(entry);
                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }
                }

                // 处理滚轮缩放
                if (Mouse.current.scroll.ReadValue().y != 0)
                {
                    zoom(Mouse.current.scroll.ReadValue().y * 0.1f); // 可能需要调整这个乘数
                }
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                transform.position.z
            );
        }

        void zoom(float increment)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(rightLimit, upperLimit));
            Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
            Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(leftLimit, bottomLimit));
            Gizmos.DrawLine(new Vector2(rightLimit, upperLimit), new Vector2(rightLimit, bottomLimit));
        }
    }
}
