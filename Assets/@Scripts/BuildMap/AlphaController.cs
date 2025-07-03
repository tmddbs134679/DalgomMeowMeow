using UnityEngine;

public class AlphaController : MonoBehaviour
{
    public void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);

            // 기존 _BaseColor 값 가져오기
            Color baseColor = renderer.sharedMaterial.GetColor("_BaseColor");
            baseColor.a = 0.5f;
            mpb.SetColor("_BaseColor", baseColor);

            renderer.SetPropertyBlock(mpb);
        }
    }
}
