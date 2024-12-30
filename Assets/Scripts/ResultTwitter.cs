using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultTwitter : MonoBehaviour {
    // �������̏����l�ł͂Ȃ��A�C���X�y�N�^��̒l��ς��Ă�������
    [SerializeField] private string gameID = "unityroom_game_id"; // unityroom��œ��e�����Q�[����ID
    [SerializeField] private string tweetText1 = "���A����E�o�ł��܂����I�X�R�A";
    [SerializeField] private string tweetText2 = "�_���l���ł��܂���";
    [SerializeField] private string hashTags = "#unityroom #LightTheWay";

    // �� �X�R�A�}�l�[�W���[�Ƃ������̂����݂���Ɖ��肵�܂�
    // �M���̍쐬���Ă���Q�[���̎���ɍ��킹�Ă�������
    // �C���X�y�N�^��ŃX�R�A�}�l�[�W���[�R���|�[�l���g�����Q�[���I�u�W�F�N�g���Z�b�g
    public ScoreManager scoreManager;

    // �c�C�[�g�{�^������Ăяo�����J���\�b�h
    public void Tweet() {
        int score = scoreManager.GetScore(); // �� ���炩�̕��@�ŃX�R�A�̒l���擾(�M���̃Q�[������ɍ��킹�Ă�������)
        string tweetMessage = tweetText1 + score + tweetText2 + "\n" + hashTags;
        naichilab.UnityRoomTweet.Tweet(gameID, tweetMessage);
    }
}