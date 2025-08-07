using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public Vector2 minLimit = new Vector2(-10, -10);
    public Vector2 maxLimit = new Vector2(10, 10);
    public float clickThreshold = 10f;

    [SerializeField] private DragController dragController;
    [SerializeField] private BuildingPlacer buildingplacer;
    [SerializeField] private LayerMask layerMask;

    private Camera cam;
    private Vector3 dragOrigin;
    private Vector2 touchStartPos;
    private bool isDragging = false;
    private bool startedOnUI = false;
    private bool isCatTouch = false;
    private bool isAI = false;

    private const float minZoom = 6f;
    private const float maxZoom = 10f;

    private float dragSpeed =>
#if UNITY_EDITOR || UNITY_STANDALONE
        3f;
#else
        1.5f;
#endif

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleZoom();

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    #region Input Handlers

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startedOnUI = IsPointerOverUI();
            if (startedOnUI) return;

            ClickCat(Input.mousePosition);
            dragOrigin = Input.mousePosition;
            touchStartPos = dragOrigin;
            isDragging = false;
        }
        else if (Input.GetMouseButton(0))
        {
            if (startedOnUI || buildingplacer?.isSelect == true || isAI) return;

            Vector3 delta = Input.mousePosition - dragOrigin;
            float dist = Vector2.Distance(Input.mousePosition, touchStartPos);

            if (dist > clickThreshold)
            {
                isDragging = true;
                ApplyCameraMove(delta);
                dragOrigin = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging && !isCatTouch)
                ClickBuilding(Input.mousePosition);

            ResetFlags();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startedOnUI = IsPointerOverUI(touch.fingerId);
                if (startedOnUI) return;

                dragOrigin = touch.position;
                touchStartPos = dragOrigin;
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (startedOnUI || buildingplacer?.isSelect == true || isAI) return;

                Vector3 delta = (Vector3)touch.position - dragOrigin;
                float dist = Vector2.Distance(touch.position, touchStartPos);

                if (dist > clickThreshold)
                {
                    isDragging = true;
                    ApplyCameraMove(delta);
                    dragOrigin = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!isDragging && !isCatTouch)
                    ClickBuilding(touch.position);

                ResetFlags();
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float prevMag = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
            float currMag = (t0.position - t1.position).magnitude;

            float delta = prevMag - currMag;
            ApplyZoom(delta * 0.01f); // 감도 조절
        }
    }

    #endregion

    #region Zoom

void HandleZoom()
{
#if UNITY_EDITOR || UNITY_STANDALONE
    if (!Application.isFocused || EventSystem.current?.IsPointerOverGameObject() == true)
        return;

    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (Mathf.Abs(scroll) > 0.01f)
        ApplyZoom(-scroll * 1f);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
    if (EventSystem.current?.IsPointerOverGameObject() == true) return;

    float scroll = Input.mouseScrollDelta.y;
    if (Mathf.Abs(scroll) > 0.01f)
    {
        ApplyZoom(scroll * 1f); // 감도는 조절 가능
        return;
    }

    // 키보드 백업
    if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus)) // +
        ApplyZoom(-0.5f);
    else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)) // -
        ApplyZoom(0.5f);
#endif
}


    void ApplyZoom(float delta)
    {
        if (cam == null) return;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - delta, minZoom, maxZoom);
    }

    #endregion

    #region Camera Move

    void ApplyCameraMove(Vector2 delta)
    {
        Vector3 move = new Vector3(-delta.x, 0, -delta.y) * dragSpeed * Time.deltaTime;
        move = cam.transform.TransformDirection(move);
        move.y = 0;

        Vector3 newPos = transform.position + move;
        newPos.x = Mathf.Clamp(newPos.x, minLimit.x, maxLimit.x);
        newPos.z = Mathf.Clamp(newPos.z, minLimit.y, maxLimit.y);
        transform.position = newPos;
    }

    #endregion

    #region Click Logic

    void ClickCat(Vector2 screenPos)
    {
        if (IsPointerOverUI()) return;
        if (BuildingPlacer.Instance.isSequenceRemove) return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                isAI = true;
                Managers.Debug.Log($" ClickCat:Cat Clicked: {hit.collider.name}", Define.EDebugType.None);
                hit.collider.GetComponent<BaseObject>()?.OnClick();
                isCatTouch = true;
                break;
            }
        }
    }

    void ClickBuilding(Vector2 screenPos)
    {
        if (IsPointerOverUI()) return;
        if (BuildingPlacer.Instance.isSequenceRemove) return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Building") ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("Stage"))
            {
                Managers.Debug.Log($" ClickBuilding:Building Clicked: {hit.collider.name}", Define.EDebugType.None);
                hit.collider.GetComponent<BaseObject>()?.OnClick();
                break;
            }
        }
    }

    #endregion

    #region Utility

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

    void ResetFlags()
    {
        isCatTouch = false;
        startedOnUI = false;
        isDragging = false;
        isAI = false;
    }

    #endregion
}
