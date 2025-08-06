using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private float _dragSpeed;
    public Vector2 minLimit = new Vector2(-10, -10);
    public Vector2 maxLimit = new Vector2(10, 10);
    public float _clickThreshold = 10f; // 	클릭 or 미세한 움직임 판단범위

    [SerializeField] private DragController dragController;
    [SerializeField] private BuildingPlacer buildingplacer;
    private Camera _cam;
    private Vector3 _dragOrigin;
    private bool isDragging = false;
    private Vector2 _touchStartPos;

    public LayerMask layerMask;
    private bool isAI;

    private bool isCatTouch;

void Start()
{
    _cam = Camera.main;

#if UNITY_EDITOR || UNITY_STANDALONE
    _dragSpeed = 3f;
#else
    _dragSpeed = 1f; // 모바일에서 너무 빠르니까 낮춰줌
#endif
}

    private bool _startedOnUI = false;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // PC 또는 에디터에서 마우스로 드래그 처리
        if (Input.GetMouseButtonDown(0))
        {
            _startedOnUI = IsPointerOverUI(); // UI 위에서 눌렀는지 기록
            if (_startedOnUI) return;

            ClickCat(Input.mousePosition); // 고양이일 때만
            _dragOrigin = Input.mousePosition;
            _touchStartPos = _dragOrigin;
            isDragging = false;
        }
        else if (Input.GetMouseButton(0))
        {
            if (_startedOnUI) return; // UI 위에서 시작했으면 아예 이동 막기
            if (buildingplacer == null) return;
            if (buildingplacer.isSelect) return;
            if (isAI) return;

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
            if (!isDragging && !isCatTouch) ClickBuilding(Input.mousePosition); // 건물일 때만
            isCatTouch = false;
            if (_startedOnUI)
            {
                _startedOnUI = false; // 다시 초기화
                return;
            }

            isDragging = false;
            isAI = false;
        }
#endif

        float zoomAmount = 0;

#if UNITY_EDITOR || UNITY_STANDALONE
        // PC에서 마우스 스크롤로 확대/축소 처리
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 에디터 게임뷰 포커스 아닐 때도 무시
#if UNITY_EDITOR
        if (!Application.isFocused)
        {
            return;
        }
#endif

        zoomAmount = Input.GetAxis("Mouse ScrollWheel") * 10f;
#endif

     if (Input.touchCount == 2)
{
    Touch touchZero = Input.GetTouch(0);
    Touch touchOne = Input.GetTouch(1);

    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

    // 🔄 방향 반전
    zoomAmount = -deltaMagnitudeDiff * 0.1f;
}


        if (zoomAmount != 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomAmount, 6f, 10f);
        }

        // 모바일에서 터치로 카메라 드래그 처리
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startedOnUI = IsPointerOverUI(touch.fingerId); // UI 위에서 눌렀는지 확인
                if (_startedOnUI) return;

                _dragOrigin = touch.position;
                _touchStartPos = _dragOrigin;
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (_startedOnUI) return; // UI 위에서 시작했으면 아예 이동 막기
                if (buildingplacer == null) return;
                if (buildingplacer.isSelect) return;
                if (isAI) return;

                Vector3 delta = (Vector3)touch.position - _dragOrigin;
                float dist = Vector2.Distance(touch.position, _touchStartPos);

                if (dist > _clickThreshold)
                {
                    isDragging = true;
                    ApplyCameraMove(delta);
                    _dragOrigin = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!isDragging && !isCatTouch) ClickBuilding(touch.position); // 건물일 때만
                isCatTouch = false;
                if (_startedOnUI)
                {
                    _startedOnUI = false; // 다시 초기화
                    return;
                }

                isDragging = false;
                isAI = false;
            }
        }
    }

void ApplyCameraMove(Vector2 delta)
{
#if UNITY_EDITOR || UNITY_STANDALONE
    float dragMultiplier = _dragSpeed * Time.deltaTime;
#else
    float dragMultiplier = _dragSpeed * 0.01f; // 모바일은 더 작게
#endif

    Vector3 move = new Vector3(-delta.x, 0, -delta.y) * dragMultiplier;
    move = _cam.transform.TransformDirection(move);
    move.y = 0;

    Vector3 newPos = transform.position + move;
    newPos.x = Mathf.Clamp(newPos.x, minLimit.x, maxLimit.x);
    newPos.z = Mathf.Clamp(newPos.z, minLimit.y, maxLimit.y);
    transform.position = newPos;
}


    void ClickCat(Vector2 screenPos)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (BuildingPlacer.Instance.isSequenceRemove) return;
        Ray ray = _cam.ScreenPointToRay(screenPos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                isAI = true;
                Managers.Debug.Log($" ClickCat:Cat Clicked: {hit.collider.name}", Define.EDebugType.None);
                var clickable = hit.collider.GetComponent<BaseObject>();
                clickable?.OnClick();
                isCatTouch = true;
                break; // 가장 가까운 Player만 처리하고 끝냄
            }
        }
    }

    void ClickBuilding(Vector2 screenPos)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (BuildingPlacer.Instance.isSequenceRemove) return;
        Ray ray = _cam.ScreenPointToRay(screenPos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Building"))
            {
                Managers.Debug.Log($" ClickBuilding:Building Clicked: {hit.collider.name}", Define.EDebugType.None);
                var clickable = hit.collider.GetComponent<BaseObject>();
                clickable?.OnClick();
                break; // 가장 가까운 Building만 처리
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Stage"))
            {
                Managers.Debug.Log($" ClickBuilding:Building Clicked: {hit.collider.name}", Define.EDebugType.None);
                var clickable = hit.collider.GetComponent<BaseObject>();
                clickable?.OnClick();
                break; // 가장 가까운 Building만 처리
            }
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
