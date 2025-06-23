using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public int width;
    public int height;
    public float tileSize = 1f;

    public GameObject tilePrefab; // BoxCollider만 붙은 바닥 프리팹

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
                Vector3 pos = new Vector3
                (
                (x - z) * tileSize * 1f,
                 0f,
                (x + z) * tileSize * 1f
                );
                Instantiate(tilePrefab, pos, Quaternion.identity, transform);

                Vector3 pos2 = new Vector3
(
((x + tileSize / 2) - (z + tileSize / 2)) * tileSize * 1f,
 0f,
((x + tileSize / 2) + (z + tileSize / 2)) * tileSize * 1f
);
                Instantiate(tilePrefab, pos2, Quaternion.identity, transform);
            }
        }
    }

}