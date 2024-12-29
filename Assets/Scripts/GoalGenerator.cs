using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapGenerator;

public class GoalGenerator : MonoBehaviour {
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject goalPrefab;

    void Start() {
        SetGoal();
    }

    void SetGoal() {
        // ゴール地点を取得
        Vector2Int goalPosition = FindGoalPosition();

        // ワールド座標に変換
        Vector2 goalWorldPos = mapGenerator.ScreenPos(goalPosition);

        // ゴールオブジェクトを生成
        Instantiate(goalPrefab, goalWorldPos, Quaternion.identity);
        mapGenerator.SetMapType(goalPosition, MAP_TYPE.GOAL);
    }

    Vector2Int FindGoalPosition() {
        // マップデータから最も遠い地点を探索
        MapGenerator.MAP_TYPE[,] mapTable = mapGenerator.GetType().GetField("mapTable",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(mapGenerator)
            as MapGenerator.MAP_TYPE[,];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Vector2Int start = new Vector2Int(1, 1);

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int farthest = start;

        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };

        while (queue.Count > 0) {
            Vector2Int current = queue.Dequeue();
            farthest = current;

            for (int i = 0; i < 4; i++) {
                Vector2Int neighbor = new Vector2Int(current.x + dx[i], current.y + dy[i]);

                if (neighbor.x >= 0 && neighbor.y >= 0 &&
                    neighbor.x < mapTable.GetLength(0) && neighbor.y < mapTable.GetLength(1) &&
                    mapTable[neighbor.x, neighbor.y] == MapGenerator.MAP_TYPE.GROUND &&
                    !visited.Contains(neighbor)) {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return farthest;
    }
}
