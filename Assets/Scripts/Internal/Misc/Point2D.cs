using System;
using System.Collections.Generic;

[Serializable]
public struct Point2D
{
    public int x;
    public int y;

    public Point2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
