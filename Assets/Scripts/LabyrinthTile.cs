using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "LabyrinthTile", menuName = "Tile/LabyrinthTile")]
/// <summary>
/// A tile that stores whether its passable or not.
/// </summary>
public class LabyrinthTile : Tile
{
    // I'm using a serialized private bool here so I don't have to bother writing a custom inspector for the property
    [SerializeField]
    private bool passable;

    /// <summary>
    /// Gets if an entity can walk through the tile
    /// </summary>
    public bool Passable
    {
        get
        {
            return passable;
        }
    }
}
