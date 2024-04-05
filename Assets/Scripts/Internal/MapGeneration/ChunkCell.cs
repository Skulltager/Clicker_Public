
using UnityEngine;

class ChunkCell
{
    public static readonly int[][] CELL_INDICES;
    public static readonly Vector3[][] CELL_VERTICES;

    public static readonly int[] LEFTDOOR_INDICES;
    public static readonly int[] TOPDOOR_INDICES;
    public static readonly int[] RIGHTDOOR_INDICES;
    public static readonly int[] BOTTOMDOOR_INDICES;

    public static readonly Vector3[] LEFTDOOR_VERTICES;
    public static readonly Vector3[] TOPDOOR_VERTICES;
    public static readonly Vector3[] RIGHTDOOR_VERTICES;
    public static readonly Vector3[] BOTTOMDOOR_VERTICES;

    public const float CELLSIZE = 1;
    public const float WALLHEIGHT = -1;
    public const float DOORHEIGHT = -0.5f;
    public const float DOORWIDTH = 0.05f;
    public const float WALLWIDTH = 0.1f;

    private const int BL = 1 << 0;
    private const int BC = 1 << 1;
    private const int BR = 1 << 2;
    private const int CL = 1 << 3;
    private const int CR = 1 << 4;
    private const int TL = 1 << 5;
    private const int TC = 1 << 6;
    private const int TR = 1 << 7;

    static ChunkCell()
    {
        CELL_INDICES = new int[256][];
        CELL_VERTICES = new Vector3[256][];

        CELL_INDICES[0] = new int[]
        {
        0, 1, 2,
        0, 2, 3,
        };
        CELL_VERTICES[0] = new Vector3[]
        {
        new Vector3(0, 0, 0),
        new Vector3(0, CELLSIZE, 0),
        new Vector3(CELLSIZE, CELLSIZE, 0),
        new Vector3(CELLSIZE, 0, 0),
        };

        CELL_INDICES[BL] = new int[]
        {
        0, 1, 2,
        0, 2, 5,
        4, 5, 2,
        4, 2, 3,

        6, 7, 8,
        6, 8, 9,

        //5, 8, 7,
        //5, 7, 0,
        //4, 9, 8,
        //4, 8, 5,
        };

        CELL_VERTICES[BL] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE, 0),
        new Vector3(CELLSIZE, CELLSIZE, 0),
        new Vector3(CELLSIZE, 0, 0),
        new Vector3(WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, 0, WALLHEIGHT),
        };

        CELL_INDICES[TL] = CELL_INDICES[BL];
        CELL_VERTICES[TL] = Rotate90Degrees(CELL_VERTICES[BL]);

        CELL_INDICES[TR] = CELL_INDICES[BL];
        CELL_VERTICES[TR] = Rotate180Degrees(CELL_VERTICES[BL]);

        CELL_INDICES[BR] = CELL_INDICES[BL];
        CELL_VERTICES[BR] = Rotate270Degrees(CELL_VERTICES[BL]);

        CELL_INDICES[BC] = new int[]
        {
        0, 1, 2,
        0, 2, 3,

        4, 5, 6,
        4, 6, 7,

        //3, 6, 5,
        //3, 5, 0,
        };
        CELL_VERTICES[BC] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE, 0),
        new Vector3(CELLSIZE, CELLSIZE, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),
        };

        CELL_INDICES[CL] = CELL_INDICES[BC];
        CELL_VERTICES[CL] = Rotate90Degrees(CELL_VERTICES[BC]);

        CELL_INDICES[TC] = CELL_INDICES[BC];
        CELL_VERTICES[TC] = Rotate180Degrees(CELL_VERTICES[BC]);

        CELL_INDICES[CR] = CELL_INDICES[BC];
        CELL_VERTICES[CR] = Rotate270Degrees(CELL_VERTICES[BC]);

        CELL_INDICES[BL + TR] = new int[]
        {
        0, 1, 2,
        0, 2, 3,
        0, 3, 7,
        6, 7, 3,
        6, 3, 4,
        6, 4, 5,

        8, 9, 10,
        8, 10, 11,

        12, 13, 14,
        12, 14, 15,

        //7, 10, 9,
        //7, 9, 0,

        //6, 11, 10,
        //6, 10, 7,

        //3, 12, 15,
        //3, 15, 4,

        //2, 13, 12,
        //2, 12, 3,
        };

        CELL_VERTICES[BL + TR] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, 0, 0),
        new Vector3(WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, 0, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        };

        CELL_INDICES[BR + TL] = CELL_INDICES[BL + TR];
        CELL_VERTICES[BR + TL] = Rotate90Degrees(CELL_VERTICES[BL + TR]);


        CELL_INDICES[BC + TC] = new int[]
        {
        0, 1, 2,
        0, 2, 3,

        4, 5, 6,
        4, 6, 7,

        8, 9, 10,
        8, 10, 11,

        //3, 6, 5,
        //3, 5, 0,

        //1, 8, 11,
        //1, 11, 2,
        };

        CELL_VERTICES[BC + TC] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),

        new Vector3(0, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        };

        CELL_INDICES[CL + CR] = CELL_INDICES[BC + TC];
        CELL_VERTICES[CL + CR] = Rotate90Degrees(CELL_VERTICES[BC + TC]);

        CELL_INDICES[BC + CL] = new int[]
        {
        0, 1, 2,
        0, 2, 3,

        4, 5, 6,
        4, 6, 7,
        4, 7, 8,
        4, 8, 9,

        //0, 7, 6,
        //0, 6, 1,

        //0, 3, 8,
        //0, 8, 7,
        };

        CELL_VERTICES[BC + CL] = new Vector3[]
        {
        new Vector3(WALLWIDTH, WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE, CELLSIZE, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),
        };

        CELL_INDICES[TC + CL] = CELL_INDICES[BC + CL];
        CELL_VERTICES[TC + CL] = Rotate90Degrees(CELL_VERTICES[BC + CL]);

        CELL_INDICES[TC + CR] = CELL_INDICES[BC + CL];
        CELL_VERTICES[TC + CR] = Rotate180Degrees(CELL_VERTICES[BC + CL]);

        CELL_INDICES[BC + CR] = CELL_INDICES[BC + CL];
        CELL_VERTICES[BC + CR] = Rotate270Degrees(CELL_VERTICES[BC + CL]);

        CELL_INDICES[BC + CL + TC] = new int[]
        {
        0, 1, 2,
        0, 2, 3,

        4, 5, 8,
        4, 8, 9,
        4, 9, 10,
        4, 10, 11,

        5, 6, 7,
        5, 7, 8,

        //0, 9, 8,
        //0, 8, 1,

        //1, 8, 7,
        //1, 7, 2,

        //3, 10, 9,
        //3, 9, 0,
        };

        CELL_VERTICES[BC + CL + TC] = new Vector3[]
        {
        new Vector3(WALLWIDTH, WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),
        };

        CELL_INDICES[CL + TC + CR] = CELL_INDICES[BC + CL + TC];
        CELL_VERTICES[CL + TC + CR] = Rotate90Degrees(CELL_VERTICES[BC + CL + TC]);

        CELL_INDICES[TC + CR + BC] = CELL_INDICES[BC + CL + TC];
        CELL_VERTICES[TC + CR + BC] = Rotate180Degrees(CELL_VERTICES[BC + CL + TC]);

        CELL_INDICES[CR + BC + CL] = CELL_INDICES[BC + CL + TC];
        CELL_VERTICES[CR + BC + CL] = Rotate270Degrees(CELL_VERTICES[BC + CL + TC]);


        CELL_INDICES[BC + TL] = new int[]
        {
        0, 1, 2,
        0, 2, 5,
        2, 3, 4,
        2, 4, 5,

        6, 7, 8,
        6, 8, 9,

        10, 11, 12,
        10, 12, 13,

        //0, 5, 8,
        //0, 8, 7,

        //1, 10 , 13,
        //1, 13, 2,

        //2, 13, 12,
        //2, 12, 3,
        };

        CELL_VERTICES[BC + TL] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE, CELLSIZE, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),

        new Vector3(0, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        };

        CELL_INDICES[CL + TR] = CELL_INDICES[BC + TL];
        CELL_VERTICES[CL + TR] = Rotate90Degrees(CELL_VERTICES[BC + TL]);

        CELL_INDICES[TC + BR] = CELL_INDICES[BC + TL];
        CELL_VERTICES[TC + BR] = Rotate180Degrees(CELL_VERTICES[BC + TL]);

        CELL_INDICES[CR + BL] = CELL_INDICES[BC + TL];
        CELL_VERTICES[CR + BL] = Rotate270Degrees(CELL_VERTICES[BC + TL]);

        CELL_INDICES[BC + TL + TR] = new int[]
        {
        0, 1, 2,
        0, 2, 5,
        0, 5, 6,
        0, 6, 7,

        2, 3, 4,
        2, 4, 5,

        8, 9, 10,
        8, 10, 11,

        12, 13, 14,
        12, 14, 15,

        16, 17, 18,
        16, 18, 19,

        //7, 10, 9,
        //7, 9, 0,

        //1, 12, 15,
        //1, 15, 2,
        //2, 15, 14,
        //2, 14, 3,

        //4, 17, 16,
        //4, 16, 5,
        //5, 16, 19,
        //5, 19, 6
        };

        CELL_VERTICES[BC + TL + TR] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),

        new Vector3(0, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        };

        CELL_INDICES[CL + TR + BR] = CELL_INDICES[BC + TL + TR];
        CELL_VERTICES[CL + TR + BR] = Rotate90Degrees(CELL_VERTICES[BC + TL + TR]);

        CELL_INDICES[TC + BR + BL] = CELL_INDICES[BC + TL + TR];
        CELL_VERTICES[TC + BR + BL] = Rotate180Degrees(CELL_VERTICES[BC + TL + TR]);

        CELL_INDICES[CR + BL + TL] = CELL_INDICES[BC + TL + TR];
        CELL_VERTICES[CR + BL + TL] = Rotate270Degrees(CELL_VERTICES[BC + TL + TR]);

        CELL_INDICES[BL + BR + TL + TR] = new int[]
        {
        0, 1, 2,
        0, 2, 11,
        2, 3, 4,
        2, 4, 5,
        8, 5, 6,
        8, 6, 7,
        10, 11, 8,
        10, 8, 9,
        11, 2, 5,
        11, 5, 8,

        12,13,14,
        12,14,15,

        16,17,18,
        16,18,19,

        20, 21, 22,
        20, 22, 23,

        24, 25, 26,
        24, 26, 27,

        //11, 14, 13,
        //11, 13, 0,
        //10, 15, 14,
        //10, 14, 11,

        //1, 16, 19,
        //1, 19, 2,
        //2, 19, 18,
        //2, 18, 3,

        //4, 21, 20,
        //4, 20, 5,
        //5, 20, 23,
        //5, 23, 6,

        //7, 26, 25,
        //7, 25, 8,
        //8, 25, 24,
        //8, 24, 9,
        };

        CELL_VERTICES[BL + BR + TL + TR] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),
        new Vector3(CELLSIZE - WALLWIDTH, WALLWIDTH, 0),
        new Vector3(CELLSIZE - WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, 0, WALLHEIGHT),

        new Vector3(0, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, 0, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),
        };

        CELL_INDICES[BL + BR] = new int[]
        {
        0, 1, 2,
        0, 2, 3,

        6, 7, 4,
        6, 4, 5,

        8, 9, 10,
        8, 10, 11,

        12, 13, 14,
        12, 14, 15,

        //6, 11, 10,
        //6, 10, 7,
        //7, 10, 9,
        //7, 9, 0,

        //3, 14, 13,
        //3, 13, 4,
        //4, 13, 12,
        //4, 12, 5
        };

        CELL_VERTICES[BL + BR] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE, 0),
        new Vector3(CELLSIZE, CELLSIZE, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),
        new Vector3(CELLSIZE - WALLWIDTH, WALLWIDTH, 0),
        new Vector3(CELLSIZE - WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, 0, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, 0, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),
        };

        CELL_INDICES[TL + BL] = CELL_INDICES[BL + BR];
        CELL_VERTICES[TL + BL] = Rotate90Degrees(CELL_VERTICES[BL + BR]);

        CELL_INDICES[TR + TL] = CELL_INDICES[BL + BR];
        CELL_VERTICES[TR + TL] = Rotate180Degrees(CELL_VERTICES[BL + BR]);

        CELL_INDICES[TR + BR] = CELL_INDICES[BL + BR];
        CELL_VERTICES[TR + BR] = Rotate270Degrees(CELL_VERTICES[BL + BR]);

        CELL_INDICES[BL + BR + TL] = new int[]
        {
        0, 1, 2,
        0, 2, 9,
        8, 9, 6,
        8, 6, 7,
        9, 3, 4,
        9, 4, 5,

        10, 11, 12,
        10, 12, 13,

        14, 15, 16,
        14, 16, 17,

        18, 19, 20,
        18, 20, 21,

        //8, 13, 12,
        //8, 12, 9,
        //9, 12, 11,
        //9, 11, 0,

        //1, 14, 17,
        //1, 17, 2,
        //2, 17, 16,
        //2, 16, 3,

        //5, 20, 19,
        //5, 19, 6,
        //6, 19, 18,
        //6, 18, 7,
        };

        CELL_VERTICES[BL + BR + TL] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE, CELLSIZE, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),
        new Vector3(CELLSIZE - WALLWIDTH, WALLWIDTH, 0),
        new Vector3(CELLSIZE - WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, 0, 0),
        new Vector3(WALLWIDTH, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(WALLWIDTH, 0, WALLHEIGHT),

        new Vector3(0, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, 0, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),
        };

        CELL_INDICES[BL + TL + TR] = CELL_INDICES[BR + BL + TL];
        CELL_VERTICES[BL + TL + TR] = Rotate90Degrees(CELL_VERTICES[BR + BL + TL]);

        CELL_INDICES[TL + TR + BR] = CELL_INDICES[BR + BL + TL];
        CELL_VERTICES[TL + TR + BR] = Rotate180Degrees(CELL_VERTICES[BR + BL + TL]);

        CELL_INDICES[TR + BR + BL] = CELL_INDICES[BR + BL + TL];
        CELL_VERTICES[TR + BR + BL] = Rotate270Degrees(CELL_VERTICES[BR + BL + TL]);

        CELL_INDICES[CL + BC + TR] = new int[]
        {
        0, 1, 3,
        0, 3, 5,
        3, 1, 2,
        3, 4, 5,

        6, 7, 8,
        6, 8, 9,
        6, 9, 10,
        6, 10, 11,

        12, 13, 14,
        12, 14, 15,

        //0, 9, 8,
        //0, 8, 1,
        //5, 10, 9,
        //5, 9, 0,

        //2, 13, 12,
        //2, 12, 3,
        //3, 12, 15,
        //3, 15, 4,
        };

        CELL_VERTICES[CL + BC + TR] = new Vector3[]
        {
        new Vector3(WALLWIDTH, WALLWIDTH, 0),
        new Vector3(WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(WALLWIDTH, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        };

        CELL_INDICES[TC + CL + BR] = CELL_INDICES[CL + BC + TR];
        CELL_VERTICES[TC + CL + BR] = Rotate90Degrees(CELL_VERTICES[CL + BC + TR]);

        CELL_INDICES[CR + TC + BL] = CELL_INDICES[CL + BC + TR];
        CELL_VERTICES[CR + TC + BL] = Rotate180Degrees(CELL_VERTICES[CL + BC + TR]);

        CELL_INDICES[BC + CR + TL] = CELL_INDICES[CL + BC + TR];
        CELL_VERTICES[BC + CR + TL] = Rotate270Degrees(CELL_VERTICES[CL + BC + TR]);

        CELL_INDICES[BC + TR] = new int[]
        {
        0, 1, 2,
        0, 2, 3,
        0, 3, 4,
        0, 4, 5,

        6, 7, 8,
        6, 8, 9,

        10, 11, 12,
        10, 12, 13,

        //5, 8, 7,
        //5, 7, 0,

        //2, 11, 10,
        //2, 10, 3,
        //3, 10, 13,
        //3, 13, 4,
        };

        CELL_VERTICES[BC + TR] = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, 0),
        new Vector3(0, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, 0),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, 0),
        new Vector3(CELLSIZE, WALLWIDTH, 0),

        new Vector3(0, 0, WALLHEIGHT),
        new Vector3(0, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE, 0, WALLHEIGHT),

        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        new Vector3(CELLSIZE - WALLWIDTH, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE, WALLHEIGHT),
        new Vector3(CELLSIZE, CELLSIZE - WALLWIDTH, WALLHEIGHT),
        };

        CELL_INDICES[CL + BR] = CELL_INDICES[BC + TR];
        CELL_VERTICES[CL + BR] = Rotate90Degrees(CELL_VERTICES[BC + TR]);

        CELL_INDICES[TC + BL] = CELL_INDICES[BC + TR];
        CELL_VERTICES[TC + BL] = Rotate180Degrees(CELL_VERTICES[BC + TR]);

        CELL_INDICES[CR + TL] = CELL_INDICES[BC + TR];
        CELL_VERTICES[CR + TL] = Rotate270Degrees(CELL_VERTICES[BC + TR]);


        LEFTDOOR_INDICES = new int[]
        {
        0, 1, 2,
        0, 2, 3,
        };

        LEFTDOOR_VERTICES = new Vector3[]
        {
        new Vector3(0, WALLWIDTH, DOORHEIGHT),
        new Vector3(0, CELLSIZE - WALLWIDTH, DOORHEIGHT),
        new Vector3(DOORWIDTH, CELLSIZE - WALLWIDTH, DOORHEIGHT),
        new Vector3(DOORWIDTH, WALLWIDTH, DOORHEIGHT),
        };

        TOPDOOR_INDICES = LEFTDOOR_INDICES;
        TOPDOOR_VERTICES = Rotate90Degrees(LEFTDOOR_VERTICES);

        RIGHTDOOR_INDICES = LEFTDOOR_INDICES;
        RIGHTDOOR_VERTICES = Rotate180Degrees(LEFTDOOR_VERTICES);

        BOTTOMDOOR_INDICES = LEFTDOOR_INDICES;
        BOTTOMDOOR_VERTICES = Rotate270Degrees(LEFTDOOR_VERTICES);

    }

    private static Vector3[] Rotate90Degrees(Vector3[] vertices)
    {
        Vector3[] newArray = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            newArray[i] = new Vector3(vertices[i].y, CELLSIZE - vertices[i].x, vertices[i].z);
        return newArray;
    }

    private static Vector3[] Rotate180Degrees(Vector3[] vertices)
    {
        Vector3[] newArray = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            newArray[i] = new Vector3(CELLSIZE - vertices[i].x, CELLSIZE - vertices[i].y, vertices[i].z);
        return newArray;
    }

    private static Vector3[] Rotate270Degrees(Vector3[] vertices)
    {
        Vector3[] newArray = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            newArray[i] = new Vector3(CELLSIZE - vertices[i].y, vertices[i].x, vertices[i].z);
        return newArray;
    }
}
