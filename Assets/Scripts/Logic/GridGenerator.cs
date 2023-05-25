using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Tile _tile;
    [SerializeField] private ExitPos _exitPos;
    [SerializeField] private EnterPos _enterPos;
    [SerializeField] private Enter _enter;
    [SerializeField] private Exit _exit;
    [SerializeField] private int _resolution;
    [SerializeField] private float _gridOffset;
    [SerializeField] private float _offset;

    private Tile[,] _grid;
    private List<EnterPos> _enters;
    private List<ExitPos> _exits;

    private Enter m_enter;
    private Exit m_exit;

    public Enter GetEnter => m_enter;
    public Exit GetExit => m_exit;
    public Tile[,] GetGrid => _grid;

    void Awake()
    {
        _enters = new();
        _exits = new();

        _grid = new Tile[_resolution, _resolution];
        GenerateGrid();
        GenerateEnterPosz();
        GenerateExitsPosz();
        SpawnEnter();
        SpawnExit();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < _resolution; i++)
        {
            for (int j = 0; j < _resolution; j++)
            {
                var instance = Instantiate(_tile, transform);
                instance.transform.localPosition = new Vector3(i * _offset - _gridOffset, j * _offset - _gridOffset, 0);
                _grid[i, j] = instance;
            }
        }
    }

    private void GenerateEnterPosz()
    {
        for (int i = 1; i < _resolution-1; i++)
        {
            var instance = Instantiate(_enterPos, transform);
            instance.transform.localPosition = new Vector3(-1 * _offset - _gridOffset, i * _offset - _gridOffset, 0);
            instance.index = new Vector2Int(-1, i);
            _enters.Add(instance);
            instance = Instantiate(_enterPos, transform);
            instance.transform.localPosition =
                new Vector3(_resolution * _offset - _gridOffset, i * _offset - _gridOffset, 0);
            instance.index = new Vector2Int(_resolution, i);
            _enters.Add(instance);
        }
    }

    private void GenerateExitsPosz()
    {
        for (int i = 1; i < _resolution-1; i++)
        {
            var instance = Instantiate(_exitPos, transform);
            instance.transform.localPosition = new Vector3(i * _offset - _gridOffset, -1 * _offset - _gridOffset, 0);
            instance.index = new Vector2Int(i, -1);
            _exits.Add(instance);
            instance = Instantiate(_exitPos, transform);
            instance.transform.localPosition =
                new Vector3(i * _offset - _gridOffset, _resolution * _offset - _gridOffset, 0);
            instance.index = new Vector2Int(i, _resolution);
            _exits.Add(instance);
        }
    }

    private void SpawnEnter()
    {
        var randomPos = _enters[Random.Range(0, _enters.Count)];
        var instance = Instantiate(_enter);
        instance.transform.position = randomPos.transform.position;
        instance.index = randomPos.index;
        m_enter = instance;
    }

    private void SpawnExit()
    {
        var randomPos = _exits[Random.Range(0, _exits.Count)];
        var instance = Instantiate(_exit);
        instance.transform.position = randomPos.transform.position;
        instance.index = randomPos.index;
        m_exit = instance;
    }
}