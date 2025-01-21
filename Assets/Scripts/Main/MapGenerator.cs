using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <see cref="https://tanisugames.com/random_map2d/">穴掘り法で2D迷路を作る</see>
///

public class MapGenerator : MonoBehaviour {
    [SerializeField, Header("GROUND, WALL")] private GameObject[] prefabs;
    [SerializeField] private int width, height;

    private Vector2 centerPos;
    private float mapSize;

    public enum MAP_TYPE {
        GROUND,
        WALL,
        PLAYER,
        LANTERN,
        GOAL
    }

    private MAP_TYPE[,] mapTable;

    private void Awake() {
        LoadMapData();
        CreateMap();
    }

    /// <summary>
    /// マップデータを読み込み、マップテーブルを初期化する
    /// </summary>
    private void LoadMapData() {
        MazeCreator maze = new MazeCreator(width, height);
        int[,] mazeData = maze.CreateMaze();

        int rows = mazeData.GetLength(1);
        int cols = mazeData.GetLength(0);
        mapTable = new MAP_TYPE[cols, rows];

        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < cols; x++) {
                mapTable[x, y] = (MAP_TYPE)mazeData[x, y];
            }
        }
    }

    /// <summary>
    /// マップを生成する
    /// </summary>
    private void CreateMap() {
        mapSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
        centerPos = CalculateCenterPosition();

        for (int y = 0; y < mapTable.GetLength(1); y++) {
            for (int x = 0; x < mapTable.GetLength(0); x++) {
                Vector2Int pos = new Vector2Int(x, y);

                InstantiatePrefab(MAP_TYPE.GROUND, pos);
                InstantiatePrefab(mapTable[x, y], pos);

                if (mapTable[x, y] == MAP_TYPE.PLAYER) {
                    GameObject player = Instantiate(prefabs[(int)MAP_TYPE.PLAYER], transform);
                    player.transform.position = ConvertToScreenPosition(pos);
                    player.GetComponent<Player>().currentPos = pos;
                }
            }
        }
    }

    /// <summary>
    /// マップテーブルを取得する
    /// </summary>
    /// <returns>マップテーブル</returns>
    public MAP_TYPE[,] GetMapTable() {
        return mapTable;
    }

    /// <summary>
    /// マップの中心座標を計算する
    /// </summary>
    private Vector2 CalculateCenterPosition() {
        float centerX = (mapTable.GetLength(0) % 2 == 0) ?
            mapTable.GetLength(0) / 2 * mapSize - (mapSize / 2) :
            mapTable.GetLength(0) / 2 * mapSize;

        float centerY = (mapTable.GetLength(1) % 2 == 0) ?
            mapTable.GetLength(1) / 2 * mapSize - (mapSize / 2) :
            mapTable.GetLength(1) / 2 * mapSize;

        return new Vector2(centerX, centerY);
    }

    /// <summary>
    /// 指定したPrefabをマップ上に配置する
    /// </summary>
    private void InstantiatePrefab(MAP_TYPE type, Vector2Int position) {
        GameObject prefab = Instantiate(prefabs[(int)type], transform);
        prefab.transform.position = ConvertToScreenPosition(position);
    }

    /// <summary>
    /// マップ座標を画面座標に変換する
    /// </summary>
    public Vector2 ConvertToScreenPosition(Vector2Int mapPosition) {
        return new Vector2(
            mapPosition.x * mapSize - centerPos.x,
            -(mapPosition.y * mapSize - centerPos.y));
    }

    /// <summary>
    /// 指定した座標のマップタイプを取得する
    /// </summary>
    public MAP_TYPE GetNextMapType(Vector2Int position) {
        return mapTable[position.x, position.y];
    }

    /// <summary>
    /// 指定した座標のマップタイプを設定する
    /// </summary>
    public void SetMapType(Vector2Int position, MAP_TYPE type) {
        mapTable[position.x, position.y] = type;
    }
}
