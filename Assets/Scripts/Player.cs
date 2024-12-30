using System.Collections;
using UnityEngine;

public class Player : MapObject {
    private const string CLEAR_SCENE = "Clear";
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InteractionLantern interactionLantern;
    [SerializeField] private AudioSource walkSE;

    private bool isWalking;
    private bool isMoving;
    private Vector2 moveDir;
    public Vector2Int nextPos;

    public enum DIRECTION {
        TOP,
        RIGHT,
        DOWN,
        LEFT
    }
    public DIRECTION direction;

    // 方向に応じた座標変化テーブル
    private readonly int[,] move = {
        { 0, -1 },  // TOP
        { 1,  0 },  // RIGHT
        { 0,  1 },  // DOWN
        { -1, 0 }   // LEFT
    };

    //--------------------------------------------------------------------------------
    // Unityイベント
    //--------------------------------------------------------------------------------

    protected virtual void Start() {
        direction = DIRECTION.DOWN;
        currentPos = new Vector2Int(1, 1);

        // mapGenerator が既にアサインされていることを前提に座標をセット
        transform.localPosition = mapGenerator.ScreenPos(currentPos);
    }

    private void Update() {
        if (isMoving) return;

        // 入力取得
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // 2つ以上のキーが押されていたら動作を停止
        if (Mathf.Abs(inputVector.x) > 0 && Mathf.Abs(inputVector.y) > 0) {
            isWalking = false;
            return;
        }

        moveDir = new Vector2(inputVector.x, inputVector.y);
        isWalking = (moveDir != Vector2.zero);

        if (isWalking) {
            _setDirection();
            StartCoroutine(_move());
        }

        if (gameInput.IsPlaceLanternPressed()) {
            interactionLantern.PlaceLantern();
        }
    }

    //--------------------------------------------------------------------------------
    // Public メソッド
    //--------------------------------------------------------------------------------

    public bool getIsWalking() {
        return isWalking;
    }

    public Vector2 getMoveDir() {
        return moveDir;
    }

    //--------------------------------------------------------------------------------
    // Private メソッド
    //--------------------------------------------------------------------------------

    private void _setDirection() {
        if (moveDir.x > 0) {
            direction = DIRECTION.RIGHT;
        } else if (moveDir.x < 0) {
            direction = DIRECTION.LEFT;
        } else if (moveDir.y > 0) {
            direction = DIRECTION.TOP;
        } else if (moveDir.y < 0) {
            direction = DIRECTION.DOWN;
        }
    }

    private IEnumerator _move() {
        isMoving = true;
        walkSE.Play(); // 歩行音を再生

        // nextPos を計算
        nextPos = currentPos + new Vector2Int(move[(int)direction, 0], move[(int)direction, 1]);

        Debug.Log(mapGenerator.GetNextMapType(nextPos));

        // 壁でない場合のみ移動
        if (mapGenerator.GetNextMapType(nextPos) != MapGenerator.MAP_TYPE.WALL) {
            Vector2 startPos = transform.localPosition;
            Vector2 endPos = mapGenerator.ScreenPos(nextPos);
            float elapsedTime = 0f;

            while (elapsedTime < 1f / moveSpeed) {
                transform.localPosition = Vector2.Lerp(startPos, endPos, (elapsedTime * moveSpeed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = endPos;
            currentPos = nextPos;

            // ゴールに到達したらクリアシーンへ
            if (mapGenerator.GetNextMapType(currentPos) == MapGenerator.MAP_TYPE.GOAL) {
                ChangeScene.ChangeToScene(CLEAR_SCENE);
            }
        }

        walkSE.Stop(); // 歩行音を停止
        isMoving = false;
    }
}
