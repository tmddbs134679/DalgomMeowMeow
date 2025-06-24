using UnityEngine;

public class TouchOrMouseDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        Vector3 inputPosition = Vector3.zero;
        bool began = false, moved = false, ended = false;

        // 📱 터치 입력
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPosition = touch.position;

            began = touch.phase == TouchPhase.Began;
            moved = touch.phase == TouchPhase.Moved;
            ended = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }
        // 🖱 마우스 입력
        else if (Input.GetMouseButtonDown(0))
        {
            inputPosition = Input.mousePosition;
            began = true;
        }
        else if (Input.GetMouseButton(0))
        {
            inputPosition = Input.mousePosition;
            moved = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inputPosition = Input.mousePosition;
            ended = true;
        }

        // 💡 Raycast로 대상 체크
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (began)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    isDragging = true;
                    offset = transform.position - hit.point;
                }
            }
        }
        else if (moved && isDragging)
        {
            if (Physics.Raycast(ray, out hit))
            {
                transform.position = hit.point + offset;
            }
        }
        else if (ended)
        {
            isDragging = false;
        }
    }
}
