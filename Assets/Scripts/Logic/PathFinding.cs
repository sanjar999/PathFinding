using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
    private List<Exit> _exits;


    private Tile[,] _grid;
    private List<Tile> _path = new List<Tile>();

    private Tile _currTile;
    private Tile _potentianNewTile;
    private float _distance = float.MaxValue;

    private Dictionary<float, Tile> _potentialTiles = new Dictionary<float, Tile>();

    private WaitForSeconds _wait;

    void Start()
    {
        _button.onClick.AddListener(Find);
        _enter = _gridGenerator.GetEnter;
        _exits = _gridGenerator.GetExits;
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
        _currTile.SetNum(0);
    }

    private void Find()
    {
        foreach (var exit in _exits)
        {
            StartCoroutine(FindCo(exit.transform));
        }
    }

    private IEnumerator FindCo(Transform exit)
    {
        var tileIndex = 1;
        Tile currTile = _currTile;
        Tile potentialNewTile = default;
        List<Tile> path = new List<Tile>();
        float distance = float.MaxValue;

        while (_distance > _minDistToExit)
        {
            yield return _wait;
            CheckUp(exit, ref potentialNewTile, ref currTile, ref distance);
            CheckDown(exit, ref potentialNewTile, ref currTile, ref distance);
            CheckRight(exit, ref potentialNewTile, ref currTile, ref distance);
            CheckLeft(exit, ref potentialNewTile, ref currTile, ref distance);


            if (potentialNewTile == null)
            {
                var currTileIndex = path.IndexOf(currTile);
                currTile.Type = TileType.no_go;
                path.Remove(currTile);
                currTile = path[currTileIndex - 1];
                distance = currTile._distToexit;
            }
            else
            {
                currTile = potentialNewTile;
                currTile.Type = TileType.path;
                path.Add(currTile);
            }

            potentialNewTile = null;
            currTile.SetNum(tileIndex);
            tileIndex++;

            if (Vector3.Distance(currTile.transform.position, exit.transform.position) < _minDistToExit)
                break;
        }
    }

    #region Checks

    private void CheckUp(Transform exit, ref Tile pt, ref Tile ct, ref float d)
    {
        if (ct.index.y == _grid.GetLength(0) - 1)
            return;

        CheckDist(0, 1, exit, ref pt, ref ct, ref d);
    }

    private void CheckDown(Transform exit, ref Tile pt, ref Tile ct, ref float d)
    {
        if (ct.index.y == 0)
            return;

        CheckDist(0, -1, exit, ref pt, ref ct, ref d);
    }

    private void CheckRight(Transform exit, ref Tile pt, ref Tile ct, ref float d)
    {
        if (ct.index.x == _grid.GetLength(0) - 1)
            return;

        CheckDist(1, 0, exit, ref pt, ref ct, ref d);
    }

    private void CheckLeft(Transform exit, ref Tile pt, ref Tile ct, ref float d)
    {
        if (ct.index.x == 0)
            return;

        CheckDist(-1, 0, exit, ref pt, ref ct, ref d);
    }

    private void CheckDist(int x, int y, Transform target, ref Tile potentianNewTile, ref Tile currTile,
        ref float distance)
    {
        var newTile = _grid[currTile.index.x + x, currTile.index.y + y];
        var newDist = Vector3.Distance(newTile.transform.position, target.position);

        if (newTile.Type == TileType.no_go || newTile.Type == TileType.obstacle)
            return;


        if (newDist < distance)
        {
            potentianNewTile = newTile;
            potentianNewTile.index = new Vector2Int(currTile.index.x + x, currTile.index.y + y);
            distance = newDist;
            potentianNewTile._distToexit = newDist;
            if (x == 1)
            {
                print("right");
            }

            if (x == -1)
            {
                print("left");
            }

            if (y == 1)
            {
                print("up");
            }

            if (y == -1)
            {
                print("down");
            }
        }
    }

    #endregion
}