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
    [SerializeField] private AudioClip walkSE; // 歩行音のAudioClip

    private bool isShrinking = false;
    private int currentFrame = 0;
    private AudioSource audioSource; // AudioSourceコンポーネント

    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = walkSE;
        audioSource.loop = true; // ループ再生を有効にする
    }

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
        audioSource.Play(); // 歩行音を再生
        while (isShrinking) {
            image.sprite = walkSprites[currentFrame];
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            yield return new WaitForSeconds(frameRate);
        }
        audioSource.Stop(); // 歩行音を停止
    }

    public void ResetWalkSE() {
        if (audioSource != null) {
            audioSource.Stop();
        }
    }
}