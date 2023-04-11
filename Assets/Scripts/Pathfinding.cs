using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObjectPrefab;
    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;

    private void Awake()
    {
        GridSystem<PathNode> _gridSystem = new GridSystem<PathNode>(10, 10, 2,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }
}