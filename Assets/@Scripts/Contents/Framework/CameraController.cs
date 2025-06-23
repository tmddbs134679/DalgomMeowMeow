using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float _dragSpeed = 2f;
    public Vector2 minLimit = new Vector2(-10, -10);
    public Vector2 maxLimit = new Vector2(10, 10);
    public float _clickThreshold = 10f; // 클릭 간주 거리

    private Camera _cam;
    private Vector3 _dragOrigin;
    private bool isDragging = false;
    private Vector2 _touchStartPos;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI()) return;

            _dragOrigin = Input.mousePosition;
            _touchStartPos = _dragOrigin;
            isDragging = false;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _dragOrigin;
            float dist = Vector2.Distance(Input.mousePosition, _touchStartPos);

            if (dist > _clickThreshold)
            {
                isDragging = true;
                ApplyCameraMove(delta);
                _dragOrigin = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging)
            {
                ClickObject(Input.mousePosition);
            }
            isDragging = false;
        }

#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (IsPointerOverUI(touch.fingerId)) return;

            if (touch.phase == TouchPhase.Began)
            {
                _touchStartPos = touch.position;
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float dist = Vector2.Distance(touch.position, _touchStartPos);
                if (dist > _clickThreshold)
                {
                    isDragging = true;
                    ApplyCameraMove(touch.deltaPosition);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!isDragging)
                {
                    HandleTap(touch.position);
                }
                isDragging = false;
            }
        }
#endif
    }

    void ApplyCameraMove(Vector2 delta)
    {
        Vector3 move = new Vector3(-delta.x, 0, -delta.y) * _dragSpeed * Time.deltaTime;
        move = _cam.transform.TransformDirection(move);
        move.y = 0;

        Vector3 newPos = transform.position + move;
        newPos.x = Mathf.Clamp(newPos.x, minLimit.x, maxLimit.x);
        newPos.z = Mathf.Clamp(newPos.z, minLimit.y, maxLimit.y);
        transform.position = newPos;
    }

    void ClickObject(Vector2 screenPos)
    {
        Ray ray = _cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Click on: " + hit.collider.name);
        }
    }

    bool IsPointerOverUI(int fingerId = -1)
    {
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current.IsPointerOverGameObject(fingerId);
#endif
    }
}
