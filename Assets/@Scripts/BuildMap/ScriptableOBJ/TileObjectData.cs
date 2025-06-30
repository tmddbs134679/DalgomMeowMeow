using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// isLoadBuild->gridmapSO에서 가져온 실질 데이터
/// </summary>
public class TileObjectData : MonoBehaviour
{

    public bool isLoadBuild;
    public ArrayMapPos arrayMapPos;
   public Color color;
    public void SetTile()
    {
        arrayMapPos.SetTile(isLoadBuild, (int)transform.position.x, (int)transform.position.z);
    }

void OnDrawGizmosSelected()
{
    Gizmos.color = color;

    Vector3 drawPosition = transform.position;

    // 만약 color가 빨간색일 때 살짝 위로 올림
    if (color == Color.red)
        drawPosition += Vector3.one * 0.01f;

    Gizmos.DrawWireCube(drawPosition, Vector3.one);
}
}
