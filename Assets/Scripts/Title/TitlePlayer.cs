using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePlayer : MonoBehaviour {
    private const string MAIN_SCENE = "Main";

    [SerializeField] private float shrinkDuration = 1.0f; // 縮小にかかる時間
    [SerializeField] private float targetScale = 0.3f; // 目標スケール
    [SerializeField] private TMP_Text tmpText; // タイトルテキスト
    [SerializeField] private Image image; // Imageコンポーネント
    [SerializeField] private Sprite[] walkSprites; // 歩行アニメーション用のスプライト配列
    [SerializeField] private float frameRate = 0.1f; // フレームレート（画像の切り替え間隔）
    [SerializeField] private AudioSource walkSE; // 歩行中の効果音

    private bool isShrinking = false; // プレイヤーが縮小中かどうか
    private int currentFrame = 0; // 現在のアニメーションフレーム

    private void Start() {
    }

    private void Update() {
        HandleInput();
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    private void HandleInput() {
        if (!isShrinking && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
            tmpText.enabled = false; // タイトルテキストを非表示にする
            StartCoroutine(ShrinkAndLoadScene());
            StartCoroutine(PlayWalkAnimation());
        }
    }

    /// <summary>
    /// 縮小のコルーチンと、シーン遷移を行う
    /// </summary>
    private IEnumerator ShrinkAndLoadScene() {
        isShrinking = true;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration) {
            transform.localScale = Vector3.Lerp(originalScale, targetScaleVector, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScaleVector;

        if (transform.localScale.x <= targetScale) {
            SceneChanger.ChangeScene(MAIN_SCENE);
        }
    }

    /// <summary>
    /// 歩行アニメーションを再生
    /// </summary>
    private IEnumerator PlayWalkAnimation() {
        walkSE.Play(); // 歩行音を再生

        while (isShrinking) {
            image.sprite = walkSprites[currentFrame];
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            yield return new WaitForSeconds(frameRate);
        }

        walkSE.Stop(); // 歩行音を停止
    }

    /// <summary>
    /// 歩行音をリセット
    /// </summary>
    public void ResetWalkSE() {
        if (walkSE != null) {
            walkSE.Stop();
        }
    }
}
