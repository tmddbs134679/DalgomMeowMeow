using UnityEngine;

public class temp : MonoBehaviour
{
    void Start()
    {
        CreateCatPaw();
    }

    void CreateCatPaw()
    {
        GameObject root = new GameObject("CatPaw");

        // 큰 패드
        GameObject bigPad = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        bigPad.transform.SetParent(root.transform);
        bigPad.transform.localScale = new Vector3(0.6f, 0.05f, 0.8f);
        bigPad.transform.localPosition = new Vector3(0, 0, 0);
        bigPad.transform.localRotation = Quaternion.Euler(90, 0, 0);

        // 작은 패드 3개
        float[] xOffsets = { -0.3f, 0f, 0.3f };
        for (int i = 0; i < 3; i++)
        {
            GameObject toe = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            toe.transform.SetParent(root.transform);
            toe.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
            toe.transform.localPosition = new Vector3(xOffsets[i], 0, 0.6f);
            toe.transform.localRotation = Quaternion.Euler(90, 0, 0);

        }
    }
}
