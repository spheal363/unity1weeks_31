using UnityEngine;

/// <summary>
/// �}�b�v��ɑ��݂���I�u�W�F�N�g�̊��N���X
/// </summary>
public abstract class MapObject : MonoBehaviour {
    // Player �� Lantern �ŋ��ʂ��Ďg������ MapGenerator ��ێ�������
    [SerializeField] protected MapGenerator mapGenerator;
    public Vector2Int currentPos { get; set; }

    // ���ʂ̏���������:
    //   ��) �w�肵�����W�ɃI�u�W�F�N�g��u���Ƃ��Ɏg��
    public virtual void Initialize(Vector2Int startPos) {
        currentPos = startPos;
        if (mapGenerator != null) {
            transform.localPosition = mapGenerator.ScreenPos(startPos);
        }
    }
}