using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//보여주기용 그리드맵,레이저 받고 프리뷰 저장하는 용도
public class GridMap : MonoBehaviour
{
    public int width;
    public int height;
    public float tileSize = 1f;

    // BoxCollider만 붙은 바닥 프리팹
    public GameObject blue;
    public GameObject red;

    public GameObject tile = null;
    [SerializeField] private ArrayMapPos _arrayMapPos;

    void Awake()
    {
        width = _arrayMapPos.Width;
        height = _arrayMapPos.Height;
    }
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (_arrayMapPos.GetTile(x, z).isBuild || _arrayMapPos.GetTile(x, z).isGround)
                {
                    tile = red;
                }
                else
                {
                    tile = blue;
                }

                Vector3 pos = new Vector3
                (
                    x * tileSize,
                    0f,
                    z * tileSize
                );

                Instantiate(tile, pos, Quaternion.identity, transform);
            }
        }

    }

    public void SaveMap()
    {

    }

    public void LoadMap()
    {

    }

}