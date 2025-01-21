using UnityEngine;
using System.Collections.Generic;

public class ClearPlayer : MonoBehaviour {
    [Header("移動設定")]
    [SerializeField] private float width = 5f;   // 移動する四角の横幅
    [SerializeField] private float height = 5f;  // 移動する四角の縦幅
    [SerializeField] private float speed = 2f;   // プレイヤー & 犬の移動速度
    [SerializeField] private float dogDelay = 1f; // 犬が動き始めるまでの遅延時間

    [Header("参照オブジェクト")]
    [SerializeField] private GameObject dog;

    private int playerCornerIndex = 0;
    private float playerProgress = 0f;

    private int dogCornerIndex = 0;
    private float dogProgress = 0f;
    private float dogStartTimer = 0f;

    private Vector3 startPoint;
    private List<Vector3> cornerPositions = new List<Vector3>();

    private Animator playerAnimator;
    private Animator dogAnimator;

    private const string IS_WALKING = "IS_WALKING";

    void Start() {
        InitializePositions();
        InitializeAnimators();
    }

    void Update() {
        MoveCharacter(ref playerCornerIndex, ref playerProgress, transform, playerAnimator);

        if (dogStartTimer < dogDelay) {
            dogStartTimer += Time.deltaTime;
        } else {
            MoveCharacter(ref dogCornerIndex, ref dogProgress, dog.transform, dogAnimator);
        }
    }

    /// <summary>
    /// 四角形のコーナーポジションと初期位置を設定
    /// </summary>
    private void InitializePositions() {
        startPoint = transform.position;
        cornerPositions.Add(startPoint);
        cornerPositions.Add(startPoint + new Vector3(width, 0, 0));
        cornerPositions.Add(startPoint + new Vector3(width, height, 0));
        cornerPositions.Add(startPoint + new Vector3(0, height, 0));
        cornerPositions.Add(startPoint);
    }

    /// <summary>
    /// アニメーターを初期化
    /// </summary>
    private void InitializeAnimators() {
        playerAnimator = GetComponent<Animator>();
        if (dog != null) {
            dogAnimator = dog.GetComponent<Animator>();
        }
    }

    /// <summary>
    /// キャラクターを四角形のパスに沿って移動させる
    /// </summary>
    /// <param name="cornerIndex">現在のコーナーインデックス</param>
    /// <param name="progress">移動進捗</param>
    /// <param name="characterTransform">キャラクターのTransform</param>
    /// <param name="animator">アニメーター</param>
    private void MoveCharacter(ref int cornerIndex, ref float progress, Transform characterTransform, Animator animator) {
        if (cornerIndex >= cornerPositions.Count - 1) {
            cornerIndex = 0;
        }

        Vector3 start = cornerPositions[cornerIndex];
        Vector3 end = cornerPositions[cornerIndex + 1];

        progress += speed * Time.deltaTime;
        characterTransform.position = Vector3.Lerp(start, end, progress);

        if (progress >= 1f) {
            progress = 0f;
            cornerIndex++;
            if (cornerIndex >= cornerPositions.Count - 1) {
                cornerIndex = 0;
            }
        }

        Vector2 direction = end - start;
        UpdateAnimation(animator, direction);
    }

    /// <summary>
    /// アニメーションを更新
    /// </summary>
    /// <param name="anim">対象のアニメーター</param>
    /// <param name="moveVec">移動ベクトル</param>
    private void UpdateAnimation(Animator anim, Vector2 moveVec) {
        if (anim == null) return;

        bool isWalking = moveVec.magnitude > 0.001f;
        anim.SetBool(IS_WALKING, isWalking);

        if (isWalking) {
            Vector2 dir = moveVec.normalized;
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
    }
}
