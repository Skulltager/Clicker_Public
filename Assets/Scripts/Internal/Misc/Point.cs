﻿
using System;
using System.Text;
using UnityEngine;

[Serializable]
public struct Point
{
    public int xIndex;
    public int yIndex;

    public Point(int xIndex, int yIndex)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
    }

    public Point AddDirection(DiagonalDirection direction)
    {
        switch (direction)
        {
            case DiagonalDirection.BottomLeft:
                return new Point(xIndex - 1, yIndex - 1);
            case DiagonalDirection.BottomCenter:
                return new Point(xIndex, yIndex - 1);
            case DiagonalDirection.BottomRight:
                return new Point(xIndex + 1, yIndex - 1);
            case DiagonalDirection.CenterLeft:
                return new Point(xIndex - 1, yIndex);
            case DiagonalDirection.CenterRight:
                return new Point(xIndex + 1, yIndex);
            case DiagonalDirection.TopLeft:
                return new Point(xIndex - 1, yIndex + 1);
            case DiagonalDirection.TopCenter:
                return new Point(xIndex, yIndex + 1);
            case DiagonalDirection.TopRight:
                return new Point(xIndex + 1, yIndex + 1);
        }

        throw new EntryPointNotFoundException("Missing something weird");
    }

    public Point AddDirection(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.Bottom:
                return new Point(xIndex, yIndex - 1);
            case CardinalDirection.Top:
                return new Point(xIndex, yIndex + 1);
            case CardinalDirection.Left:
                return new Point(xIndex - 1, yIndex);
            case CardinalDirection.Right:
                return new Point(xIndex + 1, yIndex);
        }

        throw new EntryPointNotFoundException("Missing something weird");
    }

    public int DistanceToPoint(Point other)
    {
        return Mathf.Abs(xIndex - other.xIndex) + Mathf.Abs(yIndex - other.yIndex);
    }

    public static Point operator +(Point a, Point b)
    {
        return new Point(a.xIndex + b.xIndex, a.yIndex + b.yIndex);
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.xIndex - b.xIndex, a.yIndex - b.yIndex);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.xIndex == b.xIndex && a.yIndex == b.yIndex;
    }

    public static bool operator !=(Point a, Point b)
    {
        return a.xIndex != b.xIndex || a.yIndex != b.yIndex;
    }
}