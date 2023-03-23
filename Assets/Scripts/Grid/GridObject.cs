using System.Collections.Generic;

public class GridObject
{
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;
    private List<Unit> _unitList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _unitList = new();
    }

    public override string ToString()
    {
        string unitString = string.Empty;
        foreach (Unit unit in _unitList)
        {
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        _unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }
}