using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private bool bTesting;

    [SerializeField] private GameObject playerCharacter;

    // Start is called before the first frame update
    public void CreateCharacter()
    {
        var tileMap = GameManager.Instance.getTileMap();

        var character = GameManager.Instance.AddCharacter(true, "MaleWorrior");
        GameManager.Instance.getTileMap().SetTileObject(4, 0, 1, character.gameObject);
        character._characterData.CurrentTile = new Tile(4, 0, 1);
        character.transform.position = tileMap.GetWorldPositionByTile(4, 0, 1);


        character = GameManager.Instance.AddCharacter(false, "FeMaleWorrior");
        GameManager.Instance.getTileMap().SetTileObject(4, 1, 1, character.gameObject);
        character._characterData.CurrentTile = new Tile(4, 1, 1);
        character.transform.position = tileMap.GetWorldPositionByTile(4, 1, 1);
        ;
        character.transform.localRotation = Quaternion.Euler(0, -180, 0); // 정면
    }
}