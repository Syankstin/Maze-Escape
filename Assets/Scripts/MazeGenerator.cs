using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.AI.Navigation;
public class MazeGenerator : MonoBehaviour {

    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;
    public MazeCell EntranceCell { get; private set; }

    public MazeCell ExitCell { get; private set; }

    public MazeCell EnemyCell { get; private set; }

    public event Action OnMazeGenerationComplete;

    [SerializeField, Range(0, 1)]
    private float _braidPercentage = 0.1f;

    private void Start() {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++) {
            for (int z = 0; z < _mazeDepth; z++) {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                _mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
            }
        }

        int enemyX = Random.Range(0, _mazeWidth - 1);
        int enemyZ = Random.Range(0, _mazeDepth);

        int entranceZ = Random.Range(0, _mazeDepth);
        int exitZ = Random.Range(0, _mazeDepth);
        EntranceCell = _mazeGrid[0, entranceZ];

        EnemyCell = _mazeGrid[enemyX, enemyZ];

        EntranceCell.Visit();

        Collider entranceCollider = EntranceCell.GetComponent<Collider>();
        if (entranceCollider != null) {
            entranceCollider.enabled = false;
        }

        ExitCell = _mazeGrid[_mazeWidth - 1, exitZ];

        GenerateMaze(null, EntranceCell);
        BraidMaze(_braidPercentage);
        GetComponent<NavMeshSurface>().BuildNavMesh();

        EntranceCell.ClearLeftWall();
        ExitCell.ClearRightWall();



        if (OnMazeGenerationComplete != null) {
            OnMazeGenerationComplete.Invoke();
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell) {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null) {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private void BraidMaze(float chance) {
        for (int x = 1; x < _mazeWidth - 1; x++) {
            for (int z = 1; z < _mazeDepth - 1; z++) {
                if(Random.value < chance) {
                    int choice = Random.Range(0, 2);
                    if (choice == 0 && x + 1 < _mazeWidth) {
                        _mazeGrid[x, z].ClearRightWall();
                        _mazeGrid[x + 1, z].ClearLeftWall();
                    }
                    else if (choice == 1 && z + 1 < _mazeDepth) {
                        _mazeGrid[x, z].ClearFrontWall();
                        _mazeGrid[x, z + 1].ClearBackWall();
                    }
                }
            }
        }
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell) {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell) {
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        if (x + 1 < _mazeWidth) {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false) {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0) {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false) {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth) {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false) {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0) {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false) {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell) {
        if (previousCell == null) {
            return;
        }

        if (previousCell.transform.localPosition.x < currentCell.transform.localPosition.x) {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.localPosition.x > currentCell.transform.localPosition.x) {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.localPosition.z < currentCell.transform.localPosition.z) {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.localPosition.z > currentCell.transform.localPosition.z) {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    void Update() {

    }

}
