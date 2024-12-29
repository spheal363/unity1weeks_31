using UnityEngine;
using TMPro;
using System.Collections;

public class TextBlinking : MonoBehaviour {
    private TMP_Text tmpText;        // �_�ł�������TextMeshPro�̃I�u�W�F�N�g
    public float blinkDuration = 0.5f;  // �_�ł̎����i�b�j
    private Coroutine blinkCoroutine;

    void Start() {
        tmpText = GetComponent<TMP_Text>();
        StartBlinking();
    }

    public void StartBlinking() {
        if (blinkCoroutine != null) {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkGradientCoroutine());
    }

    IEnumerator BlinkGradientCoroutine() {
        float elapsedTime = 0f;

        while (true) {
            elapsedTime += Time.deltaTime;
            float alpha = (Mathf.Sin((elapsedTime / blinkDuration) * Mathf.PI * 2) + 1) / 2; // �T�C���g��0�`1�͈̔͂ɐ��K��
            Color color = tmpText.color;
            color.a = alpha;  // �A���t�@�l��ݒ�
            tmpText.color = color;

            yield return null; // �t���[�����ƂɍX�V
        }
    }
}