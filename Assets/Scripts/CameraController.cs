using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Archiventure
{
    public class CameraController2D : MonoBehaviour
    {
        public bool movement;
        public float movementTime;
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
        private Camera cam;
        private float initialFingerDistance;
        private float initialOrthographicSize;
        public GameManager gameManager;

        private void OnEnable()
        {
            // 启用增强型触摸支持
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            // 禁用增强型触摸支持
            EnhancedTouchSupport.Disable();
        }

        void Start()
        {
            newPosition = transform.position;
            cam = Camera.main;
        }

        void Update()
        {
            if (!movement) return;

            // 处理触摸输入
            if (Touch.activeTouches.Count > 0)
            {
                HandleTouchInput();
            }
            // 处理鼠标输入
            else
            {
                HandleMouseInput();
            }

            // 应用相机移动和边界限制
            ApplyMovementAndBounds();
        }

        void HandleTouchInput()
        {
            // 处理单指拖动
            if (Touch.activeTouches.Count == 1)
            {
                var touch = Touch.activeTouches[0];

                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    // 触摸开始，记录起始位置
                    dragStartPosition = GetWorldPosition(touch.screenPosition);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    // 触摸移动，计算新位置
                    dragCurrentPosition = GetWorldPosition(touch.screenPosition);
                    newPosition = transform.position + (dragStartPosition - dragCurrentPosition);
                }
            }
            // 处理双指缩放
            else if (Touch.activeTouches.Count == 2)
            {
                var touch0 = Touch.activeTouches[0];
                var touch1 = Touch.activeTouches[1];

                if (touch0.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                    touch1.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    // 双指触摸开始，记录初始距离和缩放值
                    initialFingerDistance = Vector2.Distance(touch0.screenPosition, touch1.screenPosition);
                    initialOrthographicSize = cam.orthographicSize;
                }
                else if (touch0.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                         touch1.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    // 计算当前双指距离
                    float currentFingerDistance = Vector2.Distance(touch0.screenPosition, touch1.screenPosition);

                    // 计算缩放比例
                    float zoomScale = initialFingerDistance / currentFingerDistance;

                    // 应用缩放
                    float newOrthographicSize = initialOrthographicSize * zoomScale;
                    cam.orthographicSize = Mathf.Clamp(newOrthographicSize, zoomOutMin, zoomOutMax);
                }
            }
        }

        void HandleMouseInput()
        {
            // 鼠标左键拖动
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                dragStartPosition = GetWorldPosition(Mouse.current.position.ReadValue());
            }
            if (Mouse.current.leftButton.isPressed)
            {
                dragCurrentPosition = GetWorldPosition(Mouse.current.position.ReadValue());
                newPosition = transform.position + (dragStartPosition - dragCurrentPosition);
            }

            // 鼠标滚轮缩放
            float scrollValue = Mouse.current.scroll.ReadValue().y;
            if (scrollValue != 0)
            {
                float newSize = cam.orthographicSize - scrollValue * 0.5f;
                cam.orthographicSize = Mathf.Clamp(newSize, zoomOutMin, zoomOutMax);
            }
        }

        private Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -cam.transform.position.z));
            return worldPosition;
        }

        void ApplyMovementAndBounds()
        {
            // 应用平滑移动
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);

            // 应用边界限制
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                transform.position.z
            );
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