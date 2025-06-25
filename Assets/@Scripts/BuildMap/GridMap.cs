using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 저장되어있는 데이터맵 가져와서 그리드맵 생성및 타일에 정보전달
/// </summary>
public class GridMap : MonoBehaviour
{
    public int width;
    public int height;
    public float tileSize = 1f;

    // BoxCollider만 붙은 바닥 프리팹
    public GameObject blue;
    public GameObject red;

    public GameObject[,] tile = null;
    [SerializeField] private ArrayMapPos _arrayMapPos;

    void Awake()
    {
        width = _arrayMapPos.Width;
        height = _arrayMapPos.Height;
        tile = new GameObject[width, height];
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
                if (_arrayMapPos.GetTile(x, z).isNotBuild || _arrayMapPos.GetTile(x, z).isNotGround)
                {
                    tile[x, z] = red;
                    tile[x, z].GetComponent<TileObjectData>().color = Color.red;
                }
                else
                {
                    tile[x, z] = blue;
                    tile[x, z].GetComponent<TileObjectData>().color = Color.blue;
                }

                Vector3 pos = new Vector3
                (
                    x * tileSize,
                    0f,
                    z * tileSize
                );

                Instantiate(tile[x, z], pos, Quaternion.identity, transform);
                tile[x, z].GetComponent<TileObjectData>().isLoadBuild = !_arrayMapPos.GetTile(x, z).isNotBuild;
            }
        }

    }

    public void SaveMap()
    {

    }

    public void LoadMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (_arrayMapPos.GetTile(x, z).isNotBuild || _arrayMapPos.GetTile(x, z).isNotGround)
                {
                    tile[x, z].GetComponent<TileObjectData>().color = Color.red;
                }
                else
                {

                    tile[x, z].GetComponent<TileObjectData>().color = Color.blue;
                }
            }
        }
    }

}