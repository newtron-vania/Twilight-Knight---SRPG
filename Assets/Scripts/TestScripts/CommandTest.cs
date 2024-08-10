using UnityEngine;

public class CommandTest : MonoBehaviour
{
    public GameObject testCharacter;
    public Character testCharacterOfCharacer;


    private readonly GUIStyle _style = new();
    private string commandChangeText = ""; // 커맨드 변경 실행
    private TileMap tileMap; // 타일맵을 참조할 변수
    private string tileText = ""; // 타일 좌표 화면에 출력

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
            SelectTileAsTarget();
        // 키보드 입력을 통해 명령을 변경하거나 실행합니다.

        // 1번 키를 누르면 공격 명령 실행
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            commandChangeText = string.Format("Executing Attack Command Result : {0}",
                testCharacterOfCharacer.attackCommand.GetType());
            testCharacterOfCharacer.ExecuteAttackCommand();
        }

        // 2번 키를 누르면 이동 명령 실행
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            commandChangeText = string.Format("Executing Move Command Result : {0}",
                testCharacterOfCharacer.moveCommand.GetType());
            testCharacterOfCharacer.ExecuteMoveCommand();
        }

        // 3번 키를 누르면 첫 번째 스킬 명령 실행
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            commandChangeText = string.Format("Executing Skill Command Result : {0}",
                testCharacterOfCharacer.moveCommand.GetType());
            testCharacterOfCharacer.ExecuteSkillCommand(0); // 첫 번째 스킬
        }

        // Q키를 누르면 공격 명령을 새로운 공격 명령으로 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            commandChangeText = "Changing Attack Command to StrongAttack Command";
            ACommandBehaviour newAttackCommand = new StrongAttack(); // 새로운 공격 명령 생성
            testCharacterOfCharacer.ChangeCommand(Define.CommandType.Attack, newAttackCommand);
        }

        // W키를 누르면 이동 명령을 새로운 이동 명령으로 변경
        if (Input.GetKeyDown(KeyCode.W))
        {
            commandChangeText = "Changing Move Command to Attack Command";
            ACommandBehaviour newMoveCommand = new BasicAttack(); // 새로운 이동 명령 생성
            testCharacterOfCharacer.ChangeCommand(Define.CommandType.Move, newMoveCommand);
        }
    }

    private void OnGUI()
    {
        // 화면의 왼쪽 상단에 디버그 메시지 출력
        GUI.Label(new Rect(10, 10, 500, 60), tileText, _style);
        GUI.Label(new Rect(10, 100, 500, 60), commandChangeText, _style);
    }

    public void Init()
    {
        tileMap = GameManager.Instance.getTileMap();

        testCharacterOfCharacer = GameManager.Instance.AddCharacter(true, "MaleWorrior");
        GameManager.Instance.getTileMap().SetTileObject(4, 0, 1, testCharacterOfCharacer.gameObject);
        testCharacterOfCharacer._characterData.CurrentTile = new Tile(4, 0, 1);
        testCharacterOfCharacer.transform.position = tileMap.GetWorldPositionByTile(4, 0, 1);
        testCharacter = testCharacterOfCharacer.gameObject;

        _style.normal.textColor = Color.white;
        _style.fontSize = 24;
    }

    private void SelectTileAsTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 카메라에서 마우스 위치로 레이캐스팅
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스팅이 "Tile" 레이어의 콜라이더와 충돌할 경우
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tile")))
            {
                // 목표 위치 설정 (충돌 지점 + 0.5)
                Debug.Log($"raycasting target name : {hit.collider.name}");
                var targetPosition = hit.collider.transform.position + new Vector3(0f, 1f, 0f);
                var targetTile = tileMap.GetTileByWorldPosition(targetPosition);

                (testCharacterOfCharacer.attackCommand as ITarget).SetTarget(targetTile);
                (testCharacterOfCharacer.moveCommand as ITarget).SetTarget(targetTile);
                foreach (var VARIABLE in testCharacterOfCharacer.skillCommandList)
                    (VARIABLE as ITarget).SetTarget(targetTile);

                tileText = "target Position = " + targetTile.x + " " + targetTile.y + " " + targetTile.z;
            }
        }
    }
}