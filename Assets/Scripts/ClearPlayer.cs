using UnityEngine;
using System.Collections.Generic;

public class ClearPlayer : MonoBehaviour {
    // ------------- 設定値 -------------
    [SerializeField] private float width = 5f;   // 移動する四角の横幅
    [SerializeField] private float height = 5f;  // 移動する四角の縦幅
    [SerializeField] private float speed = 2f;   // プレイヤー & 犬の移動速度

    // 犬が動き始めるまでの遅延時間（プレイヤーが動き始めてから犬が動き始めるまで）
    public float dogDelay = 1f;

    // Dog オブジェクト (Inspector でアサイン)
    public GameObject dog;

    // ------------- 内部管理用 -------------
    // 〇プレイヤー用
    private int playerCornerIndex = 0;  // 今の辺(コーナーIndex)番号
    private float playerProgress = 0f;  // コーナーへの進捗 (0～1)

    // 〇犬用
    private int dogCornerIndex = 0;
    private float dogProgress = 0f;
    private float dogStartTimer = 0f;   // 犬が動き始めるまでのカウント

    private Vector3 startPoint;
    private List<Vector3> cornerPositions = new List<Vector3>();

    // アニメーション
    private Animator playerAnimator;
    private Animator dogAnimator;
    private const string IS_WALKING = "IsWalking";

    void Start() {
        // プレイヤーの初期位置
        startPoint = transform.position;
        playerAnimator = GetComponent<Animator>();

        if (dog != null) {
            dogAnimator = dog.GetComponent<Animator>();
        }

        // 1周の四角形コーナーを順番に登録
        // (最後に startPoint へ戻るようにしておく)
        cornerPositions.Add(startPoint);                                     // 0
        cornerPositions.Add(startPoint + new Vector3(width, 0, 0));          // 1
        cornerPositions.Add(startPoint + new Vector3(width, height, 0));     // 2
        cornerPositions.Add(startPoint + new Vector3(0, height, 0));         // 3
        cornerPositions.Add(startPoint);                                     // 4(戻る)

        // 念のため、インデックスが範囲を超えないようにチェック
        // (サンプルでは cornerPositions.Count == 5 なので問題なし)
    }

    void Update() {
        // ----------- プレイヤーを動かす -----------
        MovePlayer();

        // ----------- 犬が動き始めるタイミング管理 -----------
        // まだ dogDelay 秒経っていない場合は犬は動き始めない
        if (dogStartTimer < dogDelay) {
            dogStartTimer += Time.deltaTime;
        } else {
            // dogDelay を超えたら、犬がプレイヤーと同じようにコーナーを巡回開始
            MoveDog();
        }
    }

    /// <summary>
    /// プレイヤーを cornerPositions[playerCornerIndex] → [playerCornerIndex+1] で移動させる
    /// </summary>
    void MovePlayer() {
        // まだ次のコーナーへ進む余地があるかチェック
        if (playerCornerIndex >= cornerPositions.Count - 1) {
            // 最後まで行ったので、ループさせる場合は index を0に戻すなど
            playerCornerIndex = 0;
        }

        // 今の start/end
        Vector3 start = cornerPositions[playerCornerIndex];
        Vector3 end = cornerPositions[playerCornerIndex + 1];

        // progress を進める
        playerProgress += speed * Time.deltaTime;

        // 現在の位置を線形補間
        transform.position = Vector3.Lerp(start, end, playerProgress);

        // もし1辺を移動しきった(=1.0f以上)ら、次の辺へ進む
        if (playerProgress >= 1f) {
            playerProgress = 0f;
            playerCornerIndex++;

            // ループする場合、最後(4)の次は0へ戻すイメージ
            if (playerCornerIndex >= cornerPositions.Count - 1) {
                playerCornerIndex = 0;
            }
        }

        // アニメーション(歩く/立ち止まる)更新
        Vector2 dir = (end - start);
        UpdateAnimation(playerAnimator, dir);
    }

    /// <summary>
    /// 犬を cornerPositions[dogCornerIndex] → [dogCornerIndex+1] で移動させる
    /// </summary>
    void MoveDog() {
        if (dog == null) return;

        // 犬用コーナーインデックスが範囲オーバーしないかチェック
        if (dogCornerIndex >= cornerPositions.Count - 1) {
            dogCornerIndex = 0;
        }

        Vector3 dogStart = cornerPositions[dogCornerIndex];
        Vector3 dogEnd = cornerPositions[dogCornerIndex + 1];

        // 犬の進捗を進める
        dogProgress += speed * Time.deltaTime;

        // 線形補間で移動
        dog.transform.position = Vector3.Lerp(dogStart, dogEnd, dogProgress);

        // 次の辺へ切り替えチェック
        if (dogProgress >= 1f) {
            dogProgress = 0f;
            dogCornerIndex++;
            if (dogCornerIndex >= cornerPositions.Count - 1) {
                dogCornerIndex = 0;
            }
        }

        // 犬のアニメーション更新
        Vector2 dogDir = (dogEnd - dogStart);
        UpdateAnimation(dogAnimator, dogDir);
    }

    /// <summary>
    /// アニメーション更新(キャラが動いていれば IsWalking=true、向きも設定)
    /// </summary>
    void UpdateAnimation(Animator anim, Vector2 moveVec) {
        if (anim == null) return;

        bool isWalking = (moveVec.magnitude > 0.001f);
        anim.SetBool(IS_WALKING, isWalking);

        if (isWalking) {
            // X, Y は「移動ベクトルの正規化」を入れることが多い
            Vector2 dir = moveVec.normalized;
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
    }
}
