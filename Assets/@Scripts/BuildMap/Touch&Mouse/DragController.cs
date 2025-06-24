using UnityEngine;


public class DragController : MonoBehaviour
{
    private IDraggable currentTarget = null;
    public LayerMask groundLayer;

    public bool isDragUse=false;
    void Update()
    {
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
            // 먼저 드래그 대상 체크
            if (Physics.Raycast(ray, out var hit))
            {
                 
                var draggable = hit.collider.GetComponent<IDraggable>();
                if (draggable != null)
                {
                    currentTarget = draggable;
                    currentTarget.OnDragStart(hit.point);
                }
            }
        }
        else if (moved && currentTarget != null)
        {
           isDragUse=true;
            // 이번엔 바닥만 체크
            if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
            {
                currentTarget.OnDrag(groundHit.point); // 바닥의 위치 전달
            }
        }
        else if (ended && currentTarget != null)
        {
            isDragUse=false;
            currentTarget.OnDragEnd();
            currentTarget = null;
        }
    }
}
