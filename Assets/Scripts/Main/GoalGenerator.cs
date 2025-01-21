using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGenerator : MonoBehaviour {
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject goalPrefab;

    /// <summary>
    /// 開始時にゴール地点を設定
    /// </summary>
    private void Start() {
        if (mapGenerator == null || goalPrefab == null) {
            Debug.LogError("MapGeneratorまたはGoalPrefabが設定されていません。");
            return;
        }

        SetGoal();
    }

    /// <summary>
    /// ゴール地点を設定
    /// </summary>
    private void SetGoal() {
        Vector2Int goalPosition = FindGoalPosition();
        if (goalPosition == Vector2Int.zero) {
            Debug.LogError("ゴール位置が見つかりませんでした。");
            return;
        }

        Vector2 goalWorldPos = mapGenerator.ConvertToScreenPosition(goalPosition);
        Instantiate(goalPrefab, goalWorldPos, Quaternion.identity);
        mapGenerator.SetMapType(goalPosition, MapGenerator.MAP_TYPE.GOAL);
    }

    /// <summary>
    /// ゴール地点を探索
    /// マップ内でプレイヤーの初期位置(1,1)から最も遠い地点を返す
    /// </summary>
    /// <returns>ゴール地点の座標</returns>
    private Vector2Int FindGoalPosition() {
        var mapTable = mapGenerator.GetMapTable(); // 新たにpublicメソッドに変更する必要あり

        if (mapTable == null) {
            Debug.LogError("マップテーブルが取得できませんでした。");
            return Vector2Int.zero;
        }

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

                if (IsValidPosition(neighbor, mapTable, visited)) {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return farthest;
    }

    /// <summary>
    /// 指定した座標が有効かどうかを判定
    /// </summary>
    private bool IsValidPosition(Vector2Int position, MapGenerator.MAP_TYPE[,] mapTable, HashSet<Vector2Int> visited) {
        return position.x >= 0 && position.y >= 0 &&
               position.x < mapTable.GetLength(0) && position.y < mapTable.GetLength(1) &&
               mapTable[position.x, position.y] == MapGenerator.MAP_TYPE.GROUND &&
               !visited.Contains(position);
    }
}
