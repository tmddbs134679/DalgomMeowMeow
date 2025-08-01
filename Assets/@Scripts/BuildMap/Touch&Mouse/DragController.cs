using UnityEngine;

/// <summary>
/// DraggableObject 관리자 , CameraController와 상호작용
/// </summary>
public class DragController : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask exceptPlayer;

    public float longPressThreshold = 1.5f; // 게이지 충전 시간
    public float postFillHoldDuration = 0.2f; // 게이지 다 찬 후 유지 시간
    public float dragThreshold = 10f; // 드래그 판정 기준 (픽셀)

    public bool isDragging = false;
    public bool IsPointDown { get => isPointerDown; set => isPointerDown = value; }
    public float pointerDownTimer = 0f;

    private IDraggable currentTarget = null;

    private bool isPointerDown = false;
    private bool isDelay = false;
    private float delayTime = 0f;
    private Vector3 dragStartPos;

    private bool isGaugeFilled = false;
    private float postFillTimer = 0f;
    private bool isGaugeVisible = false;

    void Awake()
    {
        BuildingPlacer.Instance.dragController = this;
    }

    void Update()
    {
        // 지연 시작 처리 (클릭 시 0.3초 지연 후 포인터다운으로 인정)
        if (isDelay)
        {
            delayTime += Time.deltaTime;
            if (delayTime >= 0.3f)
            {
                delayTime = 0f;
                isDelay = false;
                isPointerDown = true;
            }
        }

        // 롱프레스 타이머 처리
        if (isPointerDown)
        {
            pointerDownTimer += Time.deltaTime;

            if (!isGaugeFilled)
            {
                  //  ShowGauge(true);
                if (pointerDownTimer >= longPressThreshold)
                {
                    isGaugeFilled = true;
                    postFillTimer = 0f;
                }
            }
            else
            {
                postFillTimer += Time.deltaTime;
                if (postFillTimer >= postFillHoldDuration)
                {
                    OnLongPress();
                    ResetPressState();
                }
            }
        }

        // 입력 감지 처리
        Vector3 inputPos = Vector3.zero;
        bool began = false, moved = false, ended = false;

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            inputPos = touch.position;
            began = touch.phase == TouchPhase.Began;
            moved = touch.phase == TouchPhase.Moved;
            ended = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }
        else
        {
            inputPos = Input.mousePosition;
            began = Input.GetMouseButtonDown(0);
            moved = Input.GetMouseButton(0);
            ended = Input.GetMouseButtonUp(0);
        }

        Ray ray = Camera.main.ScreenPointToRay(inputPos);

        if (began)
        {
            isDelay = true;
            ResetPressState();

            // 땅 클릭 처리
            if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
            {
                BuildingPlacer.Instance.OnGroundTouched(groundHit.point);
                BuildingPlacer.Instance.OnGroundTouchedSecond(groundHit.point); // 하나로 통합 가능성 있음
            }

            // 오브젝트 감지
            if (Physics.Raycast(ray, out var hit, 1000f, exceptPlayer))
            {
                var draggable = hit.collider.GetComponent<IDraggable>();
                if (draggable != null)
                {
                    currentTarget = draggable;
                    currentTarget.OnDragStart(hit.point);
                    dragStartPos = inputPos;
                    isDragging = false;
                }
            }
        }
        else if (moved && currentTarget != null)
        {
            if (!isDragging)
            {
                isDelay = true;
                float dist = Vector2.Distance(inputPos, dragStartPos);
                if (dist >= dragThreshold)
                {
                    isDragging = true;
                }
            }

            if (isDragging)
            {
                isDelay = false;
                if (Physics.Raycast(ray, out var moveHit, 1000f, groundLayer))
                {
                    currentTarget.OnDrag(moveHit.point);
                }
            }
        }
        else if (ended && currentTarget != null)
        {
            if (isDragging)
            {
                currentTarget.OnDragEnd();
            }
            else
            {
                currentTarget.OnClickRelease();
            }

            currentTarget = null;
            ResetPressState();
            isDragging = false;
        }
    }

    private void OnLongPress()
    {
        if (currentTarget is MonoBehaviour mb && mb.gameObject.layer == LayerMask.NameToLayer("Stage"))
            return;

        currentTarget?.OnLongPress();
    }

    private void ResetPressState()
    {
        isPointerDown = false;
        pointerDownTimer = 0f;
        postFillTimer = 0f;
        isGaugeFilled = false;
       // ShowGauge(false);
    }

    private void ShowGauge(bool show)
    {
        if (isGaugeVisible == show) return;
        isGaugeVisible = show;
        BuildingPlacer.Instance.uI_LongPressGauge.SetActive(show);
    }

    public void ChangeTarget(IDraggable draggable)
    {
        currentTarget = draggable;
    }
}
