using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    private float _dragSpeed;
    public Vector2 minLimit = new Vector2(-10, -10);
    public Vector2 maxLimit = new Vector2(10, 10);
    public float _clickThreshold = 10f;

    [SerializeField] private DragController dragController;
    [SerializeField] private BuildingPlacer buildingplacer;

    private Camera _cam;
    private Vector3 _dragOrigin;
    private bool isDragging = false;
    private Vector2 _touchStartPos;

    public LayerMask layerMask;
    private bool isAI;
    private bool isCatTouch;
    private bool _startedOnUI = false;

    void Start()
    {
        _cam = Camera.main;

#if UNITY_EDITOR || UNITY_STANDALONE
        _dragSpeed = 3f;
#else
        _dragSpeed = 1f; // 모바일/웹에서 낮춤
#endif
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButtonDown(0))
        {
            _startedOnUI = IsPointerOverUI();
            if (_startedOnUI) return;

            ClickCat(Input.mousePosition);
            _dragOrigin = Input.mousePosition;
            _touchStartPos = _dragOrigin;
            isDragging = false;
        }
        else if (Input.GetMouseButton(0))
        {
            if (_startedOnUI || buildingplacer?.isSelect == true || isAI) return;

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
            if (!isDragging && !isCatTouch)
                ClickBuilding(Input.mousePosition);

            isCatTouch = false;
            _startedOnUI = false;
            isDragging = false;
            isAI = false;
        }
#endif

        float zoomAmount = 0f;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
#if UNITY_EDITOR
        if (!Application.isFocused) return;
#endif
        zoomAmount = Input.GetAxis("Mouse ScrollWheel") * 10f;
#endif

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrev = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrev = touchOne.position - touchOne.deltaPosition;

            float prevMag = (touchZeroPrev - touchOnePrev).magnitude;
            float currMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMag = prevMag - currMag;
            zoomAmount = -deltaMag * 0.1f;
        }

        if (zoomAmount != 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomAmount, 6f, 10f);
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startedOnUI = IsPointerOverUI(touch.fingerId);
                if (_startedOnUI) return;

                _dragOrigin = touch.position;
                _touchStartPos = _dragOrigin;
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (_startedOnUI || buildingplacer?.isSelect == true || isAI) return;

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
                if (!isDragging && !isCatTouch)
                    ClickBuilding(touch.position);

                isCatTouch = false;
                _startedOnUI = false;
                isDragging = false;
                isAI = false;
            }
        }
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

    void ClickCat(Vector2 screenPos)
    {
        if (IsPointerOverUI()) return;
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
                break;
            }
        }
    }

    void ClickBuilding(Vector2 screenPos)
    {
        if (IsPointerOverUI()) return;
        if (BuildingPlacer.Instance.isSequenceRemove) return;

        Ray ray = _cam.ScreenPointToRay(screenPos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Building") ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("Stage"))
            {
                Managers.Debug.Log($" ClickBuilding:Building Clicked: {hit.collider.name}", Define.EDebugType.None);
                var clickable = hit.collider.GetComponent<BaseObject>();
                clickable?.OnClick();
                break;
            }
        }
    }

    bool IsPointerOverUI(int fingerId = -1)
    {
        if (EventSystem.current == null) return false;

#if UNITY_WEBGL && !UNITY_EDITOR
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);
        return raycastResults.Count > 0;
#else
        return EventSystem.current.IsPointerOverGameObject(fingerId);
#endif
    }
}