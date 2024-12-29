using System;
using UnityEngine;
using static MapGenerator;

public class InteractionLantern : MonoBehaviour {
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private float lanternCount = 2;
    [SerializeField] private MapGenerator mapGenerator;

    private Player player;

    // �����^���j��v����ʒm����C�x���g
    public static event Action<Vector2Int> OnLanternDestroyRequested;

    private void Start() {
        player = GetComponent<Player>();
    }

    public void PlaceLantern() {
        // ���݂̃v���C���[�ʒu����Ƀ����^���ݒu/�j��
        Vector2Int lanternPos = player.currentPos;

        // �V�K�����^���̐ݒu
        if (lanternCount > 0 && (mapGenerator.GetNextMapType(lanternPos) == MAP_TYPE.GROUND)) {

            GameObject lanternObj = Instantiate(
                lanternPrefab,
                mapGenerator.ScreenPos(lanternPos),
                Quaternion.identity,
                mapGenerator.transform
            );
            lanternObj.GetComponent<Lantern>().currentPos = lanternPos;
            mapGenerator.SetMapType(lanternPos, MAP_TYPE.LANTERN);
            lanternCount--;
        }
        // ���Ƀ����^��������Ȃ��� (�j�󃊃N�G�X�g�𓊂���)
        else if(mapGenerator.GetNextMapType(lanternPos) == MAP_TYPE.LANTERN) {
            mapGenerator.SetMapType(lanternPos, MAP_TYPE.GROUND);
            lanternCount++;

            OnLanternDestroyRequested?.Invoke(lanternPos);
        }
    }
}