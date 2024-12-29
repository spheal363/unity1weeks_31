using UnityEngine;

public class TitleManager : MonoBehaviour {
    public GameObject titleCanvas; // �^�C�g����ʂ�Canvas
    public GameObject instructionCanvas; // ���������ʂ�Canvas
    private bool isInstructionVisible = false; // ���ݑ���������\������Ă��邩�ǂ���
    private TextBlinking textBlinking;

    void Start() {
        textBlinking = titleCanvas.GetComponentInChildren<TextBlinking>();
    }

    void Update() {
        // �X�y�[�X�L�[�������ꂽ�Ƃ��̏���
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!isInstructionVisible) {
                // ���������ʂ�\������
                ShowInstructions();
            } else {
                // ���������ʂ��\���ɂ���
                HideInstructions();
            }
        }
    }

    void ShowInstructions() {
        // �^�C�g����ʂ��\���A���������\��
        titleCanvas.SetActive(false);
        instructionCanvas.SetActive(true);
        isInstructionVisible = true;
    }

    void HideInstructions() {
        // �^�C�g����ʂ�\���A����������\��
        titleCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
        isInstructionVisible = false;
        textBlinking.StartBlinking(); // TextBlinking�̃R���[�`�����ĊJ
    }
}