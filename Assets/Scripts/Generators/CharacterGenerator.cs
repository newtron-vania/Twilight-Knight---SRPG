using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private bool bTesting = false;

    [SerializeField] private GameObject playerCharacter;
    // Start is called before the first frame update
    public void CreateCharacter()
    {
        TileMap tileMap = GameManager.Instance.getTileMap();
        
        GameObject go = Instantiate(playerCharacter);
        GameManager.Instance.getTileMap().SetTileObject(4,0,1,playerCharacter);
        go.transform.position = tileMap.GetWorldPositionByTile(4, 0, 1) - new Vector3(0, 0.5f, 0);
        go.transform.localRotation = Quaternion.Euler(0, 90, 0); // 정면
        
        go = Instantiate(playerCharacter);
        GameManager.Instance.getTileMap().SetTileObject(4,6,1,playerCharacter);
        go.transform.position = tileMap.GetWorldPositionByTile(4, 6, 1) - new Vector3(0, 0.5f, 0);;
        go.transform.localRotation = Quaternion.Euler(0, -90, 0); // 정면
    }
}
