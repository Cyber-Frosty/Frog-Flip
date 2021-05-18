using UnityEngine;

public class Player : Card
{
    public Sprite FrogSprite;
    public Texture2D TadpoleTexture;
    void OnMouseDown()
    {
        Debug.Log($"Current Power: {Power}.");
        if (level.map[level.PlayerX, level.PlayerY].TryGetComponent(out Player p))
            Debug.Log("Whops");
    }
}
