using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameInput gameInput;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private float moveSpeed = 5f;

    private bool isWalking;
    private bool isMoving;
    private Vector2 moveDir;
    public Vector2Int currentPos, nextPos;

    public enum DIRECTION {
        TOP,
        RIGHT,
        DOWN,
        LEFT
    }
    public DIRECTION direction;

    int[,] move = {
        { 0, -1 },  // TOP�̏ꍇ
        { 1, 0 },   // RIGHT�̏ꍇ
        { 0, 1 },   // DOWN�̏ꍇ
        { -1, 0 }   // LEFT�̏ꍇ
    };

    private void Start() {
        direction = DIRECTION.DOWN;
        currentPos = new Vector2Int(1, 1);
        Debug.Log(mapGenerator.ScreenPos(currentPos));
        transform.localPosition = mapGenerator.ScreenPos(currentPos);
    }

    private void Update() {
        if (isMoving) return;

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        moveDir = new Vector2(inputVector.x, inputVector.y);
        isWalking = moveDir != Vector2.zero;

        if (isWalking) {
            _setDirection();
            StartCoroutine(_move());
        }
    }

    public bool getIsWalking() {
        return isWalking;
    }

    public Vector2 getMoveDir() {
        return moveDir;
    }

    void _setDirection() {
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

    IEnumerator _move() {
        isMoving = true;
        nextPos = currentPos + new Vector2Int(move[(int)direction, 0], move[(int)direction, 1]);

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
        }

        isMoving = false;
    }
}