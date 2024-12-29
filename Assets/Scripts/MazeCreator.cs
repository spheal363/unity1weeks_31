using System;
using System.Collections;
using System.Collections.Generic;


public class MazeCreator {

    const int PATH = 0;
    const int WALL = 1;

    int[,] maze;
    int w;
    int h;

    enum DIRECTION {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }

    List<Cell> startCells = new List<Cell>();

    public MazeCreator(int _w, int _h) {
        if (_w < 5 || _h < 5) throw new ArgumentOutOfRangeException();
        if (_w % 2 == 0) _w++;
        if (_h % 2 == 0) _h++;
        w = _w;
        h = _h;
        maze = new int[w, h];
    }

    public int[,] CreateMaze() {
        for (int y = 0; y < h; y++) {
            for (int x = 0; x < w; x++) {
                if (x == 0 || y == 0 || x == w - 1 || y == h - 1) {
                    maze[x, y] = PATH;
                } else {
                    maze[x, y] = WALL;
                }
            }
        }

        Dig(1, 1);
        for (int y = 0; y < h; y++) {
            for (int x = 0; x < w; x++) {
                if (x == 0 || y == 0 || x == w - 1 || y == h - 1) {
                    maze[x, y] = WALL;
                }
            }
        }

        return maze;
    }


    void Dig(int _x, int _y) {
        Random rnd = new Random();
        while (true) {
            List<DIRECTION> directions = new List<DIRECTION>();
            if (maze[_x, _y - 1] == WALL && maze[_x, _y - 2] == WALL)
                directions.Add(DIRECTION.UP);
            if (maze[_x + 1, _y] == WALL && maze[_x + 2, _y] == WALL)
                directions.Add(DIRECTION.RIGHT);
            if (maze[_x, _y + 1] == WALL && maze[_x, _y + 2] == WALL)
                directions.Add(DIRECTION.DOWN);
            if (maze[_x - 1, _y] == WALL && maze[_x - 2, _y] == WALL)
                directions.Add(DIRECTION.LEFT);

            if (directions.Count == 0) break;
            SetPath(_x, _y);
            int directionIndex = rnd.Next(directions.Count);
            switch (directions[directionIndex]) {
                case DIRECTION.UP:
                    SetPath(_x, --_y);
                    SetPath(_x, --_y);
                    break;
                case DIRECTION.RIGHT:
                    SetPath(++_x, _y);
                    SetPath(++_x, _y);
                    break;
                case DIRECTION.DOWN:
                    SetPath(_x, ++_y);
                    SetPath(_x, ++_y);
                    break;
                case DIRECTION.LEFT:
                    SetPath(--_x, _y);
                    SetPath(--_x, _y);
                    break;
            }
            Cell cell = GetStartCell();
            if (cell != null) {
                Dig(cell.X, cell.Y);
            }
        }
    }

    void SetPath(int _x, int _y) {
        maze[_x, _y] = PATH;
        if (_x % 2 == 1 && _y % 2 == 1) {
            startCells.Add(new Cell() { X = _x, Y = _y });
        }
    }

    Cell GetStartCell() {
        if (startCells.Count == 0) return null;
        Random rnd = new Random();
        int idx = rnd.Next(startCells.Count);
        Cell cell = startCells[idx];
        startCells.RemoveAt(idx);
        return cell;
    }
}

public class Cell {
    public int X, Y;
}