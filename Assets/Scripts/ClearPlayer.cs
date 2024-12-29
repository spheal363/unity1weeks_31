using UnityEngine;
using System.Collections.Generic;

public class ClearPlayer : MonoBehaviour {
    // ------------- �ݒ�l -------------
    [SerializeField] private float width = 5f;   // �ړ�����l�p�̉���
    [SerializeField] private float height = 5f;  // �ړ�����l�p�̏c��
    [SerializeField] private float speed = 2f;   // �v���C���[ & ���̈ړ����x

    // ���������n�߂�܂ł̒x�����ԁi�v���C���[�������n�߂Ă��猢�������n�߂�܂Łj
    public float dogDelay = 1f;

    // Dog �I�u�W�F�N�g (Inspector �ŃA�T�C��)
    public GameObject dog;

    // ------------- �����Ǘ��p -------------
    // �Z�v���C���[�p
    private int playerCornerIndex = 0;  // ���̕�(�R�[�i�[Index)�ԍ�
    private float playerProgress = 0f;  // �R�[�i�[�ւ̐i�� (0�`1)

    // �Z���p
    private int dogCornerIndex = 0;
    private float dogProgress = 0f;
    private float dogStartTimer = 0f;   // ���������n�߂�܂ł̃J�E���g

    private Vector3 startPoint;
    private List<Vector3> cornerPositions = new List<Vector3>();

    // �A�j���[�V����
    private Animator playerAnimator;
    private Animator dogAnimator;
    private const string IS_WALKING = "IsWalking";

    void Start() {
        // �v���C���[�̏����ʒu
        startPoint = transform.position;
        playerAnimator = GetComponent<Animator>();

        if (dog != null) {
            dogAnimator = dog.GetComponent<Animator>();
        }

        // 1���̎l�p�`�R�[�i�[�����Ԃɓo�^
        // (�Ō�� startPoint �֖߂�悤�ɂ��Ă���)
        cornerPositions.Add(startPoint);                                     // 0
        cornerPositions.Add(startPoint + new Vector3(width, 0, 0));          // 1
        cornerPositions.Add(startPoint + new Vector3(width, height, 0));     // 2
        cornerPositions.Add(startPoint + new Vector3(0, height, 0));         // 3
        cornerPositions.Add(startPoint);                                     // 4(�߂�)

        // �O�̂��߁A�C���f�b�N�X���͈͂𒴂��Ȃ��悤�Ƀ`�F�b�N
        // (�T���v���ł� cornerPositions.Count == 5 �Ȃ̂Ŗ��Ȃ�)
    }

    void Update() {
        // ----------- �v���C���[�𓮂��� -----------
        MovePlayer();

        // ----------- ���������n�߂�^�C�~���O�Ǘ� -----------
        // �܂� dogDelay �b�o���Ă��Ȃ��ꍇ�͌��͓����n�߂Ȃ�
        if (dogStartTimer < dogDelay) {
            dogStartTimer += Time.deltaTime;
        } else {
            // dogDelay �𒴂�����A�����v���C���[�Ɠ����悤�ɃR�[�i�[������J�n
            MoveDog();
        }
    }

    /// <summary>
    /// �v���C���[�� cornerPositions[playerCornerIndex] �� [playerCornerIndex+1] �ňړ�������
    /// </summary>
    void MovePlayer() {
        // �܂����̃R�[�i�[�֐i�ޗ]�n�����邩�`�F�b�N
        if (playerCornerIndex >= cornerPositions.Count - 1) {
            // �Ō�܂ōs�����̂ŁA���[�v������ꍇ�� index ��0�ɖ߂��Ȃ�
            playerCornerIndex = 0;
        }

        // ���� start/end
        Vector3 start = cornerPositions[playerCornerIndex];
        Vector3 end = cornerPositions[playerCornerIndex + 1];

        // progress ��i�߂�
        playerProgress += speed * Time.deltaTime;

        // ���݂̈ʒu����`���
        transform.position = Vector3.Lerp(start, end, playerProgress);

        // ����1�ӂ��ړ���������(=1.0f�ȏ�)��A���̕ӂ֐i��
        if (playerProgress >= 1f) {
            playerProgress = 0f;
            playerCornerIndex++;

            // ���[�v����ꍇ�A�Ō�(4)�̎���0�֖߂��C���[�W
            if (playerCornerIndex >= cornerPositions.Count - 1) {
                playerCornerIndex = 0;
            }
        }

        // �A�j���[�V����(����/�����~�܂�)�X�V
        Vector2 dir = (end - start);
        UpdateAnimation(playerAnimator, dir);
    }

    /// <summary>
    /// ���� cornerPositions[dogCornerIndex] �� [dogCornerIndex+1] �ňړ�������
    /// </summary>
    void MoveDog() {
        if (dog == null) return;

        // ���p�R�[�i�[�C���f�b�N�X���͈̓I�[�o�[���Ȃ����`�F�b�N
        if (dogCornerIndex >= cornerPositions.Count - 1) {
            dogCornerIndex = 0;
        }

        Vector3 dogStart = cornerPositions[dogCornerIndex];
        Vector3 dogEnd = cornerPositions[dogCornerIndex + 1];

        // ���̐i����i�߂�
        dogProgress += speed * Time.deltaTime;

        // ���`��Ԃňړ�
        dog.transform.position = Vector3.Lerp(dogStart, dogEnd, dogProgress);

        // ���̕ӂ֐؂�ւ��`�F�b�N
        if (dogProgress >= 1f) {
            dogProgress = 0f;
            dogCornerIndex++;
            if (dogCornerIndex >= cornerPositions.Count - 1) {
                dogCornerIndex = 0;
            }
        }

        // ���̃A�j���[�V�����X�V
        Vector2 dogDir = (dogEnd - dogStart);
        UpdateAnimation(dogAnimator, dogDir);
    }

    /// <summary>
    /// �A�j���[�V�����X�V(�L�����������Ă���� IsWalking=true�A�������ݒ�)
    /// </summary>
    void UpdateAnimation(Animator anim, Vector2 moveVec) {
        if (anim == null) return;

        bool isWalking = (moveVec.magnitude > 0.001f);
        anim.SetBool(IS_WALKING, isWalking);

        if (isWalking) {
            // X, Y �́u�ړ��x�N�g���̐��K���v�����邱�Ƃ�����
            Vector2 dir = moveVec.normalized;
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
    }
}
