using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject CellPrefab, CellBoard;
    [SerializeField] private int sizeX, sizeY;
    [SerializeField] private float cellSize;
    private Cell[,] cells;
    private List<Cell> unoccupiedCells;
    [SerializeField] private List<string> words;
    void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        CellBoard.GetComponent<GridLayoutGroup>().constraintCount = sizeX;
        CellBoard.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
        for (int row = 0; row < sizeX; row++)
        {
            for (int column = 0; column < sizeY; column++)
            {
                Cell cell = Instantiate(CellPrefab, CellBoard.transform).GetComponent<Cell>();
                cells[column, row] = cell;
                unoccupiedCells.Add(cell);
                cell.Locked(false);
                cell.GetComponent<Transform>().localScale = new Vector3(cellSize/100, cellSize/100, 1);
            }
        }
    }

    private void AddWord(string word)
    {
        List<Cell> potentialCells = new List<Cell>();
        for (int row = 0; row < sizeX; row++)
        {
            for (int col = 0; col < sizeY; col++)
            {
                potentialCells.Add(cells[col, row]);
            }
        }
        bool completed = false;
        bool blacklist = false;
        while(!blacklist && !completed && potentialCells.Count > 0)
        {
            int progress = 0;
            // when locking all cells in whitelist, don't forget to lock originCell too!!!
            List<Cell> whitelist = new List<Cell>();
            List<Vector2Int> directions = new List<Vector2Int>();
            directions.Add(new Vector2Int(0, -1));
            directions.Add(new Vector2Int(0, 1));
            directions.Add(new Vector2Int(1, 0));
            directions.Add(new Vector2Int(-1, 0));
            Cell originCell = potentialCells[Random.Range(0, potentialCells.Count-1)];
            Cell selectedCell = originCell;
            potentialCells.Remove(originCell);
            if (originCell.Locked() && originCell.Character() == word[0])
                blacklist = true;
            while(!blacklist && !completed && directions.Count > 0)
            {
                Vector2Int direction = directions[Random.Range(0, directions.Count-1)];
                directions.Remove(direction);
                while(!blacklist && !completed && progress < word.Length-1)
                {
                    if (!selectedCell.Locked())
                    {
                        selectedCell.Character(word[progress]);
                        whitelist.Add(selectedCell);
                    }
                    else if (selectedCell.Locked() && selectedCell.Character() != word[progress])
                    {
                        whitelist.Clear();
                        blacklist = true;
                    }
                    Vector2Int newCoordinate = selectedCell.Coordinate().x + direction.x, selectedCell.Coordinate().y + direction.y;
                    selectedCell = cells[];
                }
                blacklist = false;
            }
        }
    }

    private bool IsValidCoordinate(Vector2Int coordinate)
    {
        return (coordinate.x >= 0 && coordinate.x < sizeX && coordinate.y >= 0 && coordinate.y < sizeY);
    }
}
