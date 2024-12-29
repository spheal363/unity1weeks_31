using System.Collections;
using UnityEngine;

public class Player : MapObject {
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InteractionLantern interactionLantern;

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

    // �����ɉ��������W�ω��e�[�u��
    private readonly int[,] move = {
        { 0, -1 },  // TOP
        { 1,  0 },  // RIGHT
        { 0,  1 },  // DOWN
        { -1, 0 }   // LEFT
    };

    //--------------------------------------------------------------------------------
    // Unity�C�x���g
    //--------------------------------------------------------------------------------

    // MapObject �� Start() �� abstract/virtual �łȂ����
    // override �ł͂Ȃ����ʂ� Start() �Ƃ��Ă�OK
    protected virtual void Start() {
        // MapObject �̃v���p�e�B (CurrentPos �Ȃ�) ��������
        // �������� MapObject �� Initialize() ���g��
        direction = DIRECTION.DOWN;
        currentPos = new Vector2Int(1, 1);

        // mapGenerator �����ɃA�T�C������Ă��邱�Ƃ�O��ɍ��W���Z�b�g
        transform.localPosition = mapGenerator.ScreenPos(currentPos);

        Debug.Log(interactionLantern);
    }

    private void Update() {
        if (isMoving) return;

        // ���͎擾
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
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
    // Public ���\�b�h
    //--------------------------------------------------------------------------------

    public bool getIsWalking() {
        return isWalking;
    }

    public Vector2 getMoveDir() {
        return moveDir;
    }

    //--------------------------------------------------------------------------------
    // Private ���\�b�h
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
        // nextPos ���v�Z
        nextPos = currentPos + new Vector2Int(move[(int)direction, 0], move[(int)direction, 1]);

        // �ǂłȂ��ꍇ�݈̂ړ�
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
