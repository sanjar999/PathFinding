using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private GridGenerator _gridGenerator;
    [SerializeField] private Button _button;
    [SerializeField] private float _minDistToExit = 0.16f;

    [SerializeField] [Range(0, 1)] private float _upMovePriority;
    [SerializeField] [Range(0, 1)] private float _downMovePriority;
    [SerializeField] [Range(0, 1)] private float _leftMovePriority;
    [SerializeField] [Range(0, 1)] private float _rightMovePriority;

    [SerializeField] private bool _isPriorityPath;
    [SerializeField] private bool _isDistancePath;

    [SerializeField] private float _stepSpeed = 1f;
    
    private Enter _enter;
    private Exit _exit;

    private Tile[,] _grid;
    private List<Tile> _path = new List<Tile>();

    private Tile _currTile;
    private Tile _potentianNewTile;
    private float _distance = float.MaxValue;

    private Dictionary<float, Tile> _potentialTiles = new Dictionary<float, Tile>();

    private int _tileIndex;
    private WaitForSeconds _wait;
    void Start()
    {
     
        _button.onClick.AddListener(Find);
        _enter = _gridGenerator.GetEnter;
        _exit = _gridGenerator.GetExit;
        _grid = _gridGenerator.GetGrid;

        _wait = new WaitForSeconds(_stepSpeed);
        
        GetFirstTile();
    }

    private void GetFirstTile()
    {
        if (_enter.index.x == -1)
        {
            _currTile = _grid[0, _enter.index.y];
            _currTile.index = new Vector2Int(0, _enter.index.y);
        }
        else
        {
            _currTile = _grid[_grid.GetLength(0) - 1, _enter.index.y];
            _currTile.index = new Vector2Int(_grid.GetLength(0) - 1, _enter.index.y);
        }

        _currTile.Type = TileType.path;
        _path.Add(_currTile);
        _currTile.SetNum(_tileIndex);
        _tileIndex++;
    }

    private void Find()
    {
        StartCoroutine(FindCo());
    }

    private IEnumerator FindCo()
    {
        while (_distance > _minDistToExit)
        {
            yield return _wait;
            _potentialTiles.Clear();
            CheckUp();
            CheckDown();
            CheckRight();
            CheckLeft();

            var potentialTiles = _potentialTiles.OrderBy(i => i.Key);

            if (potentialTiles.Count() == 0)
            {
                var currTileIndex = _path.IndexOf(_currTile);
                _currTile.Type = TileType.no_go;
                _path.Remove(_currTile);
                _currTile = _path[currTileIndex - 1];
            }
            else
            {
                _currTile = potentialTiles.First().Value;
                _currTile.Type = TileType.path;
                _path.Add(_currTile);
            }

            _currTile.SetNum(_tileIndex);
            _tileIndex++;

            if (Vector3.Distance(_currTile.transform.position, _exit.transform.position) < _minDistToExit)
                break;
        }
    }

    #region Checks

    private void CheckUp()
    {
        if (_currTile.index.y == _grid.GetLength(0) - 1)
            return;

        CheckSide(0, 1, _upMovePriority);
    }

    private void CheckDown()
    {
        if (_currTile.index.y == 0)
            return;

        CheckSide(0, -1, _downMovePriority);
    }

    private void CheckRight()
    {
        if (_currTile.index.x == _grid.GetLength(0) - 1)
            return;

        CheckSide(1, 0, _rightMovePriority);
    }

    private void CheckLeft()
    {
        if (_currTile.index.x == 0)
            return;

        CheckSide(-1, 0, _leftMovePriority);
    }

    private void CheckSide(int x, int y, float priority)
    {
        var newTile = _grid[_currTile.index.x + x, _currTile.index.y + y];
        var newDist = Vector3.Distance(newTile.transform.position, _exit.transform.position);

        // if (newDist > _distance + _minDistToExit * 0.8f)
        //     return;

        if (newTile.Type == TileType.no_go || newTile.Type == TileType.obstacle)
            return;

        _potentianNewTile = newTile;
        _potentianNewTile.index = new Vector2Int(_currTile.index.x + x, _currTile.index.y + y);
        _distance = newDist;

        if (_path.Contains(newTile))
            return;

        if (_isPriorityPath)
            _potentialTiles.Add(1 - priority, newTile);
        if (_isDistancePath)
        {
            if (_potentialTiles.ContainsKey(newDist))
                newDist = Random.Range(0, 0.01f);

            _potentialTiles.Add(newDist, newTile);
        }
    }

    #endregion
}