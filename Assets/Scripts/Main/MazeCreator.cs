using System;
using System.Collections.Generic;

public class MazeCreator {
    private const int PATH = 0;
    private const int WALL = 1;

    private int[,] maze;
    private int width;
    private int height;

    private enum DIRECTION {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    private List<Cell> startCells = new List<Cell>();

    public MazeCreator(int width, int height) {
        if (width < 5 || height < 5) throw new ArgumentOutOfRangeException("Maze dimensions must be at least 5x5.");

        this.width = MakeOdd(width);
        this.height = MakeOdd(height);

        maze = new int[this.width, this.height];
    }

    public int[,] CreateMaze() {
        InitializeMaze();
        Dig(1, 1);
        FinalizeMaze();
        return maze;
    }

    /// <summary>
    /// 初期状態で迷路を壁で満たす
    /// </summary>
    private void InitializeMaze() {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                maze[x, y] = (IsBorder(x, y)) ? PATH : WALL;
            }
        }
    }

    /// <summary>
    /// 外周部分を壁に設定
    /// </summary>
    private void FinalizeMaze() {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (IsBorder(x, y)) {
                    maze[x, y] = WALL;
                }
            }
        }
    }

    /// <summary>
    /// 再帰的に掘り進める
    /// </summary>
    private void Dig(int x, int y) {
        Random random = new Random();

        while (true) {
            List<DIRECTION> availableDirections = GetAvailableDirections(x, y);

            if (availableDirections.Count == 0) break;

            SetPATH(x, y);

            DIRECTION chosenDirection = availableDirections[random.Next(availableDirections.Count)];
            (x, y) = MoveAndDig(x, y, chosenDirection);
        }

        Cell nextCell = GetRandomStartCell();
        if (nextCell != null) {
            Dig(nextCell.X, nextCell.Y);
        }
    }

    /// <summary>
    /// 指定した座標から進行可能な方向を取得
    /// </summary>
    private List<DIRECTION> GetAvailableDirections(int x, int y) {
        List<DIRECTION> directions = new List<DIRECTION>();

        if (CanDig(x, y - 1, x, y - 2)) directions.Add(DIRECTION.UP);
        if (CanDig(x + 1, y, x + 2, y)) directions.Add(DIRECTION.RIGHT);
        if (CanDig(x, y + 1, x, y + 2)) directions.Add(DIRECTION.DOWN);
        if (CanDig(x - 1, y, x - 2, y)) directions.Add(DIRECTION.LEFT);

        return directions;
    }

    /// <summary>
    /// 指定方向に掘り進める
    /// </summary>
    private (int, int) MoveAndDig(int x, int y, DIRECTION direction) {
        switch (direction) {
            case DIRECTION.UP:
                SetPATH(x, --y);
                SetPATH(x, --y);
                break;
            case DIRECTION.RIGHT:
                SetPATH(++x, y);
                SetPATH(++x, y);
                break;
            case DIRECTION.DOWN:
                SetPATH(x, ++y);
                SetPATH(x, ++y);
                break;
            case DIRECTION.LEFT:
                SetPATH(--x, y);
                SetPATH(--x, y);
                break;
        }
        return (x, y);
    }

    /// <summary>
    /// 掘る対象のセルが有効かを確認
    /// </summary>
    private bool CanDig(int x1, int y1, int x2, int y2) {
        return IsWithinBounds(x1, y1) && IsWithinBounds(x2, y2) &&
               maze[x1, y1] == WALL && maze[x2, y2] == WALL;
    }

    /// <summary>
    /// 指定したセルを通路に設定
    /// </summary>
    private void SetPATH(int x, int y) {
        maze[x, y] = PATH;

        if (x % 2 == 1 && y % 2 == 1) {
            startCells.Add(new Cell { X = x, Y = y });
        }
    }

    /// <summary>
    /// スタートセルからランダムに取得
    /// </summary>
    private Cell GetRandomStartCell() {
        if (startCells.Count == 0) return null;

        Random random = new Random();
        int index = random.Next(startCells.Count);
        Cell cell = startCells[index];
        startCells.RemoveAt(index);
        return cell;
    }

    /// <summary>
    /// 範囲内かを確認
    /// </summary>
    private bool IsWithinBounds(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    /// 外周のセルかを確認
    /// </summary>
    private bool IsBorder(int x, int y) {
        return x == 0 || y == 0 || x == width - 1 || y == height - 1;
    }

    /// <summary>
    /// 奇数に調整
    /// </summary>
    private int MakeOdd(int value) {
        return (value % 2 == 0) ? value + 1 : value;
    }
}

public class Cell {
    public int X { get; set; }
    public int Y { get; set; }
}