using UnityEngine;

public class Node
{
    // This attributes represents the position of the node.
    public Vector2 Position;
    // This attributes represents the node is occupied or not.
    public bool IsOccupied = false;
    // This attributes represents the gCost of the node.
    public int GCost = int.MaxValue;
    // This attributes represents the hCost of the node.
    public int HCost = default;
    // This attributes represents the fCost of the node.
    public int FCost;
    // This attributes represents the previous node.
    public Node PreviousValidNode = null;
}
