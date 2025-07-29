using UnityEngine;
using UnityEngine.UI;

public class Mole_ImgRaySet : MonoBehaviour
{
    private Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }
    void Start()
    {
        img.alphaHitTestMinimumThreshold = 0.1f; // 알파 0.1 이하만 통과
    }
}
