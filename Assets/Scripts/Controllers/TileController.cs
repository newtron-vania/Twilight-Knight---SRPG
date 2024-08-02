using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float padding = 1f;

    [SerializeField] private bool bTesting = false;
    [SerializeField] private GameObject Tile;

    private void Start()
    {
        CreateTile();
    }

    private void CreateTile()
    {
        // Data 불러오기
        GameObject TileMap = new GameObject("TileMap");
        TileMap.transform.position = Vector3.zero;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject tile = Instantiate(Tile, new Vector3( -width / 2f + padding * x, 0f, -height / 2f + padding * y), Quaternion.identity, TileMap.transform);
            }
        }
    }
}
