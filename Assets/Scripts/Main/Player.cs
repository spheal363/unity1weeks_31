using System.Collections;
using UnityEngine;

public class Player : MapObject {
    private const string CLEAR_SCENE = "Clear";

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 5f;
    // 同じ座標にあるランタンを破壊するためのクラス
    [SerializeField] private InteractionLantern interactionLantern;
    [SerializeField] private AudioSource walkSE;

    private bool isWalking;
    private bool isMoving;
    private Vector2 moveDir;
    public Vector2Int nextPos;

    public enum Direction {
        Top,
        Right,
        Down,
        Left
    }
    public Direction direction;

    // 方向に応じた座標変化テーブル
    private readonly int[,] moveOffsets = {
        { 0, -1 },  // Top
        { 1,  0 },  // Right
        { 0,  1 },  // Down
        { -1, 0 }   // Left
    };

    protected virtual void Start() {
        InitializePlayer();
    }

    private void Update() {
        if (isMoving) return;

        HandleMovementInput();
        HandleLanternInput();
    }

    public bool GetIsWalking() {
        return isWalking;
    }

    public Vector2 GetMoveDir() {
        return moveDir;
    }

    /// <summary>
    /// プレイヤーの初期設定
    /// </summary>
    private void InitializePlayer() {
        direction = Direction.Down;
        currentPos = new Vector2Int(1, 1);
        transform.localPosition = mapGenerator.ConvertToScreenPosition(currentPos);
    }

    /// <summary>
    /// 移動入力を処理
    /// </summary>
    private void HandleMovementInput() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // 2つ以上のキーが押されていたら動作を停止
        if (Mathf.Abs(inputVector.x) > 0 && Mathf.Abs(inputVector.y) > 0) {
            isWalking = false;
            return;
        }

        moveDir = inputVector;
        isWalking = (moveDir != Vector2.zero);

        if (isWalking) {
            SetDirection();
            StartCoroutine(MovePlayer());
        }
    }

    /// <summary>
    /// ランタン設置の入力を処理
    /// </summary>
    private void HandleLanternInput() {
        if (gameInput.IsPlaceLanternPressed()) {
            interactionLantern.PlaceLantern();
        }
    }

    /// <summary>
    /// プレイヤーの移動方向を設定
    /// </summary>
    private void SetDirection() {
        if (moveDir.x > 0) {
            direction = Direction.Right;
        } else if (moveDir.x < 0) {
            direction = Direction.Left;
        } else if (moveDir.y > 0) {
            direction = Direction.Top;
        } else if (moveDir.y < 0) {
            direction = Direction.Down;
        }
    }

    /// <summary>
    /// プレイヤーを移動させる
    /// </summary>
    private IEnumerator MovePlayer() {
        isMoving = true;
        walkSE.Play();

        nextPos = currentPos + new Vector2Int(moveOffsets[(int)direction, 0], moveOffsets[(int)direction, 1]);

        // 移動先が壁でない場合のみ移動
        if (mapGenerator.GetNextMapType(nextPos) != MapGenerator.MAP_TYPE.WALL) {
            Vector2 startPos = transform.localPosition;
            Vector2 endPos = mapGenerator.ConvertToScreenPosition(nextPos);
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
                SceneChanger.ChangeScene(CLEAR_SCENE);
            }
        }

        walkSE.Stop();
        isMoving = false;
    }
}
