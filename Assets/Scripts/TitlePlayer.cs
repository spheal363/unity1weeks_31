using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePlayer : MonoBehaviour {
    private const string MAIN_SCENE = "Main";
    [SerializeField] private float shrinkDuration = 1.0f; // 縮小にかかる時間
    [SerializeField] private float targetScale = 0.3f; // 目標スケール
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Image image; // Imageコンポーネント
    [SerializeField] private Sprite[] walkSprites; // 歩行アニメーション用のスプライト配列
    [SerializeField] private float frameRate = 0.1f; // フレームレート（画像の切り替え間隔）

    private bool isShrinking = false;
    private int currentFrame = 0;

    void Update() {
        if (!isShrinking && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
            tmpText.enabled = false; // tmpTextを見えなくする
            StartCoroutine(ShrinkAndLoadScene());
            StartCoroutine(PlayWalkAnimation());
        }
    }

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
            ChangeScene.ChangeToScene(MAIN_SCENE);
        }
    }

    private IEnumerator PlayWalkAnimation() {
        while (isShrinking) {
            image.sprite = walkSprites[currentFrame];
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}