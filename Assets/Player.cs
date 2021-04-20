using UnityEngine;

public class Player : Card
{
    void OnMouseDown()
    {
        Debug.Log($"Current Power: {Power}.");
    }
}
