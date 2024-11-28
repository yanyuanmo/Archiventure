using UnityEngine;

namespace Archiventure {

    public class CameraController2D : MonoBehaviour
    {
        // [Header("Camera Settings")]
        // [SerializeField] private float minZoom = 2f;  
        // [SerializeField] private float maxZoom = 8f;  
        // [SerializeField] private float smoothSpeed = 10f;  
        // [SerializeField] private Vector2 mapSize = new Vector2(100f, 100f);  
        [Header("Camera Settings")]
        [SerializeField, Range(1f, 5f), Tooltip("Minimum zoom level (closest to ground)")]
        private float minZoom = 2f;
        
        [SerializeField, Range(5f, 15f), Tooltip("Maximum zoom level (furthest from ground)")]
        private float maxZoom = 8f;
        
        [SerializeField, Range(1f, 20f), Tooltip("Camera movement and zoom smoothing")]
        private float smoothSpeed = 10f;
        
        [Header("Map Boundaries")]
        [SerializeField, Tooltip("Map size in units (width, height)")]
        private Vector2 mapSize = new Vector2(100f, 100f);
        private Camera cam;
        private Vector3 dragOrigin;
        private Vector3 targetPosition;
        private float targetZoom;
        private bool isDragging = false;
        private float initialDistance;
        
        void Awake()
        {
            cam = Camera.main;
            targetPosition = transform.position;
            targetZoom = cam.orthographicSize;
        }
        
        void Update()
        {
            HandleTouchInput();
            
            // Move camera smoothly
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * smoothSpeed);
        }
        
        void HandleTouchInput()
        {
        
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                
            
                if (touch.phase == TouchPhase.Began)
                {
                    isDragging = true;
                    dragOrigin = cam.ScreenToWorldPoint(touch.position);
                }
                
                // When draging
                if (touch.phase == TouchPhase.Moved && isDragging)
                {
                    Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(touch.position);
                    Vector3 newPosition = transform.position + difference;
                    
                    // Limit the position of the camera
                    float halfHeight = cam.orthographicSize;
                    float halfWidth = halfHeight * cam.aspect;
                    
                    newPosition.x = Mathf.Clamp(newPosition.x, -mapSize.x/2 + halfWidth, mapSize.x/2 - halfWidth);
                    newPosition.y = Mathf.Clamp(newPosition.y, -mapSize.y/2 + halfHeight, mapSize.y/2 - halfHeight);
                    newPosition.z = transform.position.z;
                    
                    targetPosition = newPosition;
                }
                
                if (touch.phase == TouchPhase.Ended)
                {
                    isDragging = false;
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    initialDistance = Vector2.Distance(touch0.position, touch1.position);
                }
                else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                    float difference = currentDistance - initialDistance;
                    
                    targetZoom = Mathf.Clamp(cam.orthographicSize - difference * 0.01f, minZoom, maxZoom);
                    initialDistance = currentDistance;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            
            Vector3 topLeft = new Vector3(-mapSize.x/2, mapSize.y/2, 0);
            Vector3 topRight = new Vector3(mapSize.x/2, mapSize.y/2, 0);
            Vector3 bottomLeft = new Vector3(-mapSize.x/2, -mapSize.y/2, 0);
            Vector3 bottomRight = new Vector3(mapSize.x/2, -mapSize.y/2, 0);
            
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }

}
