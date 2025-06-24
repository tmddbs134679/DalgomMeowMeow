using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float _dragSpeed = 2f;
    public Vector2 minLimit = new Vector2(-10, -10);
    public Vector2 maxLimit = new Vector2(10, 10);
    public float _clickThreshold = 10f; // Ŭ�� ���� �Ÿ�

    private Camera _cam;
    private Vector3 _dragOrigin;
    private bool isDragging = false;
    private Vector2 _touchStartPos;

    [SerializeField] private DragController dragController;
    void Start()
    {
        _cam = Camera.main;
    }

   private bool _startedOnUI = false;

void Update()
{
#if UNITY_EDITOR
    if (Input.GetMouseButtonDown(0))
    {
        _startedOnUI = IsPointerOverUI(); //  UI 위에서 눌렀는지 기록
        if (_startedOnUI) return;

        _dragOrigin = Input.mousePosition;
        _touchStartPos = _dragOrigin;
        isDragging = false;
    }
    else if (Input.GetMouseButton(0))
    {
        if (_startedOnUI) return; //  UI 위에서 시작했으면 아예 이동 막기

        Vector3 delta = Input.mousePosition - _dragOrigin;
        float dist = Vector2.Distance(Input.mousePosition, _touchStartPos);

        if (dist > _clickThreshold && !dragController.isDragUse)
        {
            isDragging = true;
            ApplyCameraMove(delta);
            _dragOrigin = Input.mousePosition;
        }
    }
    else if (Input.GetMouseButtonUp(0))
    {
        if (_startedOnUI) {
            _startedOnUI = false; // 다시 초기화
            return;
        }

        if (!isDragging)
        {
            ClickObject(Input.mousePosition);
        }
        isDragging = false;
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
            
            var clickable = hit.collider.GetComponent<BaseObject>();
            if (clickable != null)
                clickable.OnClick();
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
