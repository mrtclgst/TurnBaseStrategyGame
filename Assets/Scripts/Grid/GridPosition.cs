using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int _x;
    public int _z;

    public GridPosition(int x, int z)
    {
        this._x = x;
        this._z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_x, _z);
    }

    public override string ToString()
    {
        return $"x: {_x} z: {_z}";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a._x == b._x && a._z == b._z;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a._x + b._x, a._z + b._z);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a._x - b._x, a._z - b._z);
    }

    public GridPosition North
    {
        get => this + new GridPosition(0, 1);
    }

    public GridPosition NorthEast
    {
        get => this + new GridPosition(1, 1);
    }

    public GridPosition NorthWest
    {
        get => this + new GridPosition(-1, 1);
    }

    public GridPosition South
    {
        get => this + new GridPosition(0, -1);
    }

    public GridPosition SouthEast
    {
        get => this + new GridPosition(1, -1);
    }

    public GridPosition SouthWest
    {
        get => this + new GridPosition(-1, -1);
    }

    public GridPosition West
    {
        get => this + new GridPosition(-1, 0);
    }

    public GridPosition East
    {
        get => this + new GridPosition(1, 0);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }
}