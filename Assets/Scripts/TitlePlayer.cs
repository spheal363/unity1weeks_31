using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePlayer : MonoBehaviour {
    private const string MAIN_SCENE = "Main";
    [SerializeField] private float shrinkDuration = 1.0f; // �k���ɂ����鎞��
    [SerializeField] private float targetScale = 0.3f; // �ڕW�X�P�[��
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Image image; // Image�R���|�[�l���g
    [SerializeField] private Sprite[] walkSprites; // ���s�A�j���[�V�����p�̃X�v���C�g�z��
    [SerializeField] private float frameRate = 0.1f; // �t���[�����[�g�i�摜�̐؂�ւ��Ԋu�j

    private bool isShrinking = false;
    private int currentFrame = 0;

    void Update() {
        if (!isShrinking && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
            tmpText.enabled = false; // tmpText�������Ȃ�����
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