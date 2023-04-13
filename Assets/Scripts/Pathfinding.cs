using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    [SerializeField] private Transform _gridDebugObjectPrefab;

    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;
    [SerializeField] private LayerMask _whatIsObstacle;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Pathfinding!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridSystem = new GridSystem<PathNode>(_width, _height, _cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //_gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = _gridSystem.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up,
                        raycastOffsetDistance * 10, _whatIsObstacle))
                {
                    GetNode(new GridPosition(x, z)).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            //We reached the final node.
            if (currentNode == endNode)
            {
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tempGCost = currentNode.GetGCost() +
                                CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tempGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tempGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance._x);
        int zDistance = Mathf.Abs(gridPositionDistance._z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();

        // //We took the neighbour which at LEFT of our object.
        // neighbourList.Add(GetNode(gridPosition.North));
        // //We took the neighbour which at LEFT DOWN of our object.
        // neighbourList.Add(GetNode(gridPosition._x - 1, gridPosition._z - 1));
        // //We took the neighbour which at LEFT UP of our object.
        // neighbourList.Add(GetNode(gridPosition._x - 1, gridPosition._z + 1));
        // //We took the neighbour which at RIGHT of our object.
        // neighbourList.Add(GetNode(gridPosition._x + 1, gridPosition._z + 0));
        // //We took the neighbour which at RIGHT DOWN of our object.
        // neighbourList.Add(GetNode(gridPosition._x + 1, gridPosition._z - 1));
        // //We took the neighbour which at RIGHT UP of our object.
        // neighbourList.Add(GetNode(gridPosition._x + 1, gridPosition._z + 1));
        // //We took the neighbour which at UP of our object.
        // neighbourList.Add(GetNode(gridPosition._x + 0, gridPosition._z + 1));
        // //We took the neighbour which at DOWN of our object.
        // neighbourList.Add(GetNode(gridPosition._x + 0, gridPosition._z - 1));

        if (_gridSystem.IsValidGridPosition(gridPosition.North))
        {
            neighbourList.Add(_gridSystem.GetGridObject(gridPosition.North));
        }

        if (_gridSystem.IsValidGridPosition(gridPosition.East))
        {
            neighbourList.Add(GetNode(gridPosition.East));
            // neighbourList.Add(_gridSystem.GetGridObject(gridPosition.East));
        }

        if (_gridSystem.IsValidGridPosition(gridPosition.South))
        {
            neighbourList.Add(_gridSystem.GetGridObject(gridPosition.South));
        }

        if (_gridSystem.IsValidGridPosition(gridPosition.West))
        {
            neighbourList.Add(_gridSystem.GetGridObject(gridPosition.West));
        }

        if (_gridSystem.IsValidGridPosition(gridPosition.NorthEast))
        {
            neighbourList.Add(_gridSystem.GetGridObject(gridPosition.NorthEast));
        }

        if (_gridSystem.IsValidGridPosition(gridPosition.NorthWest))
        {
            neighbourList.Add(_gridSystem.GetGridObject(gridPosition.NorthWest));
        }

        if (_gridSystem.IsValidGridPosition(gridPosition.SouthEast))
        {
            neighbourList.Add(_gridSystem.GetGridObject(gridPosition.SouthEast));
        }

        if (_gridSystem.IsValidGridPosition(gridPosition.SouthWest))
        {
            neighbourList.Add(_gridSystem.GetGridObject(gridPosition.SouthWest));
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int index = 0; index < pathNodeList.Count; index++)
        {
            gridPositionList.Add(pathNodeList[index].GetGridPosition());
        }

        return gridPositionList;
    }

    private PathNode GetNode(GridPosition gridPosition)
    {
        return _gridSystem.GetGridObject(gridPosition);
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startPosition, GridPosition endPosition)
    {
        FindPath(startPosition, endPosition, out int pathLength);
        return pathLength;
    }
}