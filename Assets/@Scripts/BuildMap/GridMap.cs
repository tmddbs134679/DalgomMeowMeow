using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
  public int width = 10;
    public int height = 10;
    public float tileSize = 1f;

    public GameObject tilePrefab; // BoxCollider만 붙은 바닥 프리팹

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
                Vector3 position = new Vector3(x * tileSize, 0f, z * tileSize);
                Instantiate(tilePrefab, position, Quaternion.identity, transform);
            }
        }
    }

}
