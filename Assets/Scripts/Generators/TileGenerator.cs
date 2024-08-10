using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float padding = 1f;

    [SerializeField] private bool bTesting;
    [SerializeField] private GameObject Tile;

    [SerializeField] private CommandTest test;

    private void Start()
    {
        CreateTile();
        //SetCharacter();
        test.Init();
    }

    private void SetCharacter()
    {
        var characterGenerator = FindObjectOfType<CharacterGenerator>();
        if (characterGenerator) characterGenerator.CreateCharacter();
    }

    private void CreateTile()
    {
        var tileMap = GameManager.Instance.getTileMap();

        if (tileMap == null)
        {
            Debug.LogError("TileMap is not initialized.");
            return;
        }

        // Data 불러오기
        var TileMap = new GameObject("TileMap");

        TileMap.transform.position = Vector3.zero;
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            var tileObject = Instantiate(Tile, new Vector3(-height / 2f + padding * y, 0f, -width / 2f + padding * x),
                Quaternion.identity, TileMap.transform);
            var tile = tileMap.GetTile(x, y, 0);
            if (y == 0 && x == 0)
            {
                GameManager.Instance.getTileMap().WorldPositionOfZero = tileObject.transform.position;
                GameManager.Instance.getTileMap().Padding = padding;
            }

            if (tile is null)
            {
                Debug.LogWarning($"Tile at ({y}, {x}, 0) is null.");
                continue; // Skip this iteration if tile is null
            }

            tile.tileObject = tileObject;
        }
    }
}