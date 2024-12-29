using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapGenerator : MonoBehaviour {

    [SerializeField, Header("GROUND, WALL")] private GameObject[] prefabs;
    [SerializeField] private int width, height;

    private Vector2 centerPos;
    private float mapSize;

    public enum MAP_TYPE {
        GROUND,
        WALL,
        PLAYER,
        LANTERN
    }
    MAP_TYPE[,] mapTable;

    void Awake() {
        _loadMapData();
        _createMap();
    }

    void _loadMapData() {
        // MazeCreator���C���X�^���X��
        MazeCreator maze = new MazeCreator(width, height);
        // ���H�f�[�^�p�񎟌��z��𐶐�
        int[,] mazeDatas = maze.CreateMaze();
        // �}�b�v�̏c�̒����擾
        int row = mazeDatas.GetLength(1);
        // �}�b�v�̉��̒����擾
        int col = mazeDatas.GetLength(0);
        // �}�b�v�e�[�u��������
        mapTable = new MAP_TYPE[col, row];

        // �}�b�v�e�[�u���쐬
        for (int y = 0; y < row; y++) {
            for (int x = 0; x < col; x++) {
                // ���H�f�[�^��MAP_TYPE�ɃL���X�g���ă}�b�v�e�[�u���Ɋi�[
                mapTable[x, y] = (MAP_TYPE)mazeDatas[x, y];
            }
        }
    }

    void _createMap() {
        //�ǉ��T�C�Y���擾����
        mapSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;

        //�񂪋����̏ꍇ
        if (mapTable.GetLength(0) % 2 == 0) {
            centerPos.x = mapTable.GetLength(0) / 2 * mapSize - (mapSize / 2);
        } else {
            centerPos.x = mapTable.GetLength(0) / 2 * mapSize;
        }
        //�s�������̏ꍇ
        if (mapTable.GetLength(1) % 2 == 0) {
            centerPos.y = mapTable.GetLength(1) / 2 * mapSize - (mapSize / 2);
        } else {
            centerPos.y = mapTable.GetLength(1) / 2 * mapSize;
        }

        for (int y = 0; y < mapTable.GetLength(1); y++) {
            for (int x = 0; x < mapTable.GetLength(0); x++) {
                Vector2Int pos = new Vector2Int(x, y);

                //�ȉ�2�s�C���@�������Ɏ��g��transform��n��
                GameObject _ground = Instantiate(prefabs[(int)MAP_TYPE.GROUND], transform);
                GameObject _map = Instantiate(prefabs[(int)mapTable[x, y]], transform);

                _ground.transform.position = ScreenPos(pos);
                _map.transform.position = ScreenPos(pos);

                if (mapTable[x, y] == MAP_TYPE.PLAYER) {
                    _map.GetComponent<Player>().currentPos = pos;
                }
            }
        }
    }

    public Vector2 ScreenPos(Vector2Int _pos) {
        return new Vector2(
            _pos.x * mapSize - centerPos.x,
            -(_pos.y * mapSize - centerPos.y));
    }

    public MAP_TYPE GetNextMapType(Vector2Int _pos) {
        return mapTable[_pos.x, _pos.y];
    }

    // SetMapType���\�b�h��ǉ�
    public void SetMapType(Vector2Int _pos, MAP_TYPE type) {
        mapTable[_pos.x, _pos.y] = type;
    }
}