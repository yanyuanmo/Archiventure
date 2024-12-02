using UnityEngine;

namespace Archiventure {

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
            if (movement == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    multiTouch = false;

                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float entry;
                    if (plane.Raycast(ray, out entry))
                    {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                }
                if (Input.touchCount == 2)
                {
                    multiTouch = true;
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;

                    zoom(difference * 0.003f);
                }
                if (Input.GetMouseButton(0) && multiTouch == false)
                {
                    Plane plane = new Plane(Vector3.forward, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float entry;
                    if (plane.Raycast(ray, out entry))
                    {
                        dragCurrentPosition = ray.GetPoint(entry);

                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }
                }
                zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, upperLimit), transform.position.z);

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

        //// [Header("Camera Settings")]
        //// [SerializeField] private float minZoom = 2f;  
        //// [SerializeField] private float maxZoom = 8f;  
        //// [SerializeField] private float smoothSpeed = 10f;  
        //// [SerializeField] private Vector2 mapSize = new Vector2(100f, 100f);  
        //[Header("Camera Settings")]
        //[SerializeField, Range(1f, 5f), Tooltip("Minimum zoom level (closest to ground)")]
        //private float minZoom = 2f;

        //[SerializeField, Range(5f, 15f), Tooltip("Maximum zoom level (furthest from ground)")]
        //private float maxZoom = 8f;

        //[SerializeField, Range(1f, 20f), Tooltip("Camera movement and zoom smoothing")]
        //private float smoothSpeed = 10f;

        //[Header("Map Boundaries")]
        //[SerializeField, Tooltip("Map size in units (width, height)")]
        //private Vector2 mapSize = new Vector2(100f, 100f);
        //private Camera cam;
        //private Vector3 dragOrigin;
        //private Vector3 targetPosition;
        //private float targetZoom;
        //private bool isDragging = false;
        //private float initialDistance;

        //void Awake()
        //{
        //    cam = Camera.main;
        //    targetPosition = transform.position;
        //    targetZoom = cam.orthographicSize;
        //}

        //void Update()
        //{
        //    HandleTouchInput();

        //    // Move camera smoothly
        //    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
        //    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * smoothSpeed);
        //}

        //void HandleTouchInput()
        //{

        //    if (Input.touchCount == 1)
        //    {
        //        Touch touch = Input.GetTouch(0);


        //        if (touch.phase == TouchPhase.Began)
        //        {
        //            isDragging = true;
        //            dragOrigin = cam.ScreenToWorldPoint(touch.position);
        //        }

        //        // When draging
        //        if (touch.phase == TouchPhase.Moved && isDragging)
        //        {
        //            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(touch.position);
        //            Vector3 newPosition = transform.position + difference;

        //            // Limit the position of the camera
        //            float halfHeight = cam.orthographicSize;
        //            float halfWidth = halfHeight * cam.aspect;

        //            newPosition.x = Mathf.Clamp(newPosition.x, -mapSize.x/2 + halfWidth, mapSize.x/2 - halfWidth);
        //            newPosition.y = Mathf.Clamp(newPosition.y, -mapSize.y/2 + halfHeight, mapSize.y/2 - halfHeight);
        //            newPosition.z = transform.position.z;

        //            targetPosition = newPosition;
        //        }

        //        if (touch.phase == TouchPhase.Ended)
        //        {
        //            isDragging = false;
        //        }
        //    }
        //    else if (Input.touchCount == 2)
        //    {
        //        Touch touch0 = Input.GetTouch(0);
        //        Touch touch1 = Input.GetTouch(1);

        //        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        //        {
        //            initialDistance = Vector2.Distance(touch0.position, touch1.position);
        //        }
        //        else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
        //        {
        //            float currentDistance = Vector2.Distance(touch0.position, touch1.position);
        //            float difference = currentDistance - initialDistance;

        //            targetZoom = Mathf.Clamp(cam.orthographicSize - difference * 0.01f, minZoom, maxZoom);
        //            initialDistance = currentDistance;
        //        }
        //    }
        //}

        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;

        //    Vector3 topLeft = new Vector3(-mapSize.x/2, mapSize.y/2, 0);
        //    Vector3 topRight = new Vector3(mapSize.x/2, mapSize.y/2, 0);
        //    Vector3 bottomLeft = new Vector3(-mapSize.x/2, -mapSize.y/2, 0);
        //    Vector3 bottomRight = new Vector3(mapSize.x/2, -mapSize.y/2, 0);

        //    Gizmos.DrawLine(topLeft, topRight);
        //    Gizmos.DrawLine(topRight, bottomRight);
        //    Gizmos.DrawLine(bottomRight, bottomLeft);
        //    Gizmos.DrawLine(bottomLeft, topLeft);
        //}
    }

}
