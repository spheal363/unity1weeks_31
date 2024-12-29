using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    private int lanternCount;
    private float clearTime;

    private int score;
    private TMP_Text scoreText;

    // �����^���X�R�A�֘A
    [SerializeField] private int maxLanternScore = 2000;  // �����^������̍ő�X�R�A
    [SerializeField] private int lanternPenalty = 50;    // �����^��1������̃y�i���e�B

    // �^�C���X�R�A�֘A
    [SerializeField] private int maxTimeScore = 8000;     // �^�C������̍ő�X�R�A
    [SerializeField] private float timePenalty = 50f;    // 1�b������̃y�i���e�B�i�傫���قǃN���A���Ԃ̉e�����傫���j

    void Start() {
        // Goal�N���X�Ȃǂ��烉���^�����ƃN���A���Ԃ��擾�i��j
        lanternCount = Goal.GetLanternCount();
        clearTime = Goal.GetElapsedTime();

        // ���g�ɃA�^�b�`���ꂽTMP_Text���Q��
        scoreText = GetComponent<TMP_Text>();

        // --- ���Z�����ŃX�R�A�v�Z ---
        // 1) �����^���X�R�A
        //    maxLanternScore ���烉���^���� �~ �y�i���e�B�������ĎZ�o���A0�����ɂ͂��Ȃ�
        int lanternScore = maxLanternScore - (lanternCount * lanternPenalty);
        lanternScore = Mathf.Max(0, lanternScore);

        // 2) �^�C���X�R�A
        //    maxTimeScore ���� (�N���A���� �~ �y�i���e�B) �������ĎZ�o���A0�����ɂ͂��Ȃ�
        float timeScore = maxTimeScore - (clearTime * timePenalty);
        timeScore = Mathf.Max(0f, timeScore);

        // 3) ���v�X�R�A���Z�o (int�ɃL���X�g)
        score = lanternScore + (int)timeScore;

        // �\�����X�V
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        // 3����؂�ŕ\���i#,0 �̓J���}��؂�j
        scoreText.text = score.ToString("#,0");
    }
}