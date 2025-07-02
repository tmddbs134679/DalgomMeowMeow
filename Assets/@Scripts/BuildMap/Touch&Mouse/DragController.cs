using UnityEngine;


/// <summary>
/// DraggableObject 관리자 ,cameracontroller와 상호작용
/// </summary>
public class DragController : MonoBehaviour
{
    public LayerMask groundLayer;
    public float longPressThreshold = 1.0f; // 몇 초 이상 눌러야 롱프레스인지
    public bool isDragging = false;
    public float dragThreshold = 10f; // 10픽셀 이상 움직이면 드래그로 간주

    private IDraggable currentTarget = null;
    private bool isPointerDown = false;
    private float pointerDownTimer = 0f;
    private Vector3 dragStartPos;   // 클릭한 화면상의 위치

    void Update()
    {
        if (isPointerDown)
        {
            pointerDownTimer += Time.deltaTime;

            if (pointerDownTimer >= longPressThreshold)
            {
                OnLongPress();
                isPointerDown = false; // 1회 실행 후 초기화
            }
        }



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
            isPointerDown = true;
            pointerDownTimer = 0f;

            if (Physics.Raycast(ray, out var hit))
            {
                var draggable = hit.collider.GetComponent<IDraggable>();
                if (draggable != null)
                {
                    currentTarget = draggable;
                    currentTarget.OnDragStart(hit.point);

                    // 클릭 위치 저장
                    dragStartPos = inputPos;
                    isDragging = false;
                }
            }
        }
        else if (moved && currentTarget != null)
        {
            // 아직 드래그 시작 안했으면 거리 체크
            if (!isDragging)
            {
                float dist = Vector2.Distance(inputPos, dragStartPos);
                if (dist >= dragThreshold)
                {
                    isDragging = true;
                }
            }

            // 드래그 중일 때만 이동
            if (isDragging)
            {
                if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
                {
                    currentTarget.OnDrag(groundHit.point);
                }
            }
        }
        else if (ended && currentTarget != null)
        {
            isPointerDown = false;
            pointerDownTimer = 0f;

            if (isDragging)
            {
                currentTarget.OnDragEnd();
            }

            isDragging = false;
            currentTarget = null;
        }
    }
    private void OnLongPress()
    {
        if (currentTarget != null)
            currentTarget.OnLongPress();
    }
}
