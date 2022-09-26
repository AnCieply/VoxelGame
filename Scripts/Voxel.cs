using Godot;
using System.Collections.Generic;

public enum FaceDir {
    East = 0,
    West = 1,
    North = 2,
    South = 3,
    Top = 4,
    Bottom = 5
}

public enum VoxelType {
    Air,
    Grass,
    Dirt,
    Stone
}

public class VoxelParameters {
    public Vector2 texEast;
    public Vector2 texWest;
    public Vector2 texNorth;
    public Vector2 texSouth;
    public Vector2 texTop;
    public Vector2 texBottom;
    public bool isSolid;

    public VoxelParameters(Vector2 texEast, Vector2 texWest, 
                    Vector2 texNorth, Vector2 texSouth, 
                    Vector2 texTop, Vector2 texBottom, 
                    bool isSolid) 
    {
        this.texEast = texEast;
        this.texWest = texWest;
        this.texNorth = texNorth;
        this.texSouth = texSouth;
        this.texTop = texTop;
        this.texBottom = texBottom;
        this.isSolid = isSolid;
    }
}

public static class Voxel {
    public static readonly Vector2 TEX_ATLAS_SIZE = new Vector2(3, 2);
    
    private static Vector3[] eastVertices = {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.0f)
    };

    private static Vector3[] westVertices = {
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f)
    };

    private static Vector3[] northVertices = {
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    private static Vector3[] southVertices = {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 0.0f, 1.0f)
    };

    private static Vector3[] topVertices = {
        new Vector3(0.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 1.0f)
    };

    private static Vector3[] bottomVertices = {
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f)
    };

    public static Vector3[][] vertices = {
        eastVertices,  westVertices,
        northVertices, southVertices,
        topVertices,   bottomVertices
    };

    public static Dictionary<VoxelType, VoxelParameters> voxelList = new Dictionary<VoxelType, VoxelParameters>() {
        { VoxelType.Air, new VoxelParameters(new Vector2(0, 0),  new Vector2(0, 0), 
            new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0),new Vector2(0, 0), 
            false) 
        },
        { VoxelType.Grass, new VoxelParameters(new Vector2(2, 0),  new Vector2(2, 0), 
            new Vector2(2, 0), new Vector2(2, 0),
            new Vector2(0, 0),new Vector2(1, 0), 
            true) 
        },
        { VoxelType.Dirt, new VoxelParameters(new Vector2(1, 0),  new Vector2(1, 0), 
            new Vector2(1, 0), new Vector2(1, 0),
            new Vector2(1, 0),new Vector2(1, 0), 
            true) 
        },
        { VoxelType.Stone, new VoxelParameters(new Vector2(0, 1),  new Vector2(0, 1), 
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(0, 1),new Vector2(0, 1), 
            true) 
        }
    };
}