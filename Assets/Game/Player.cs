using UnityEngine;

public class Player : Card
{
    public Sprite FrogSprite;
    public Sprite TadpoleSprite;
    void OnMouseDown()
    {
        Debug.Log($"Current Power: {Power}.");
        if (level.Map[level.PlayerX, level.PlayerY].TryGetComponent(out Player p))
            Debug.Log("Whops");
    }
}
