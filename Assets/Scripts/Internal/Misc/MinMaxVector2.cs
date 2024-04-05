using UnityEngine;

public struct MinMaxVector2
{
    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;

    public int xSize => xMax - xMin;
    public int ySize => yMax - yMin;

    public MinMaxVector2(ChunkRoom chunkRoom)
    {
        xMin = int.MaxValue;
        xMax = int.MinValue;
        yMin = int.MaxValue;
        yMax = int.MinValue;

        foreach (Point point in chunkRoom.chunkPoints)
        {
            xMin = Mathf.Min(point.xIndex, xMin);
            xMax = Mathf.Max(point.xIndex, xMax);
            yMin = Mathf.Min(point.yIndex, yMin);
            yMax = Mathf.Max(point.yIndex, yMax);
        }

        xMin -= 1;
        xMax += 1;
        yMin -= 1;
        yMax += 1;
    }

    public static MinMaxVector2 AssignHighestOffsets(MinMaxVector2 source, MinMaxVector2 assignee)
    {
        source.xMin = Mathf.Min(source.xMin, assignee.xMin);
        source.xMax = Mathf.Max(source.xMax, assignee.xMax);
        source.yMin = Mathf.Min(source.yMin, assignee.yMin);
        source.yMax = Mathf.Max(source.yMax, assignee.yMax);
        return source;
    }

    public static MinMaxVector2 CreateEmpty()
    {
        return new MinMaxVector2()
        {
            xMin = int.MaxValue,
            xMax = int.MinValue,
            yMin = int.MaxValue,
            yMax = int.MinValue,
        };
    }
}