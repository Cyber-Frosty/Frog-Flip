using UnityEngine;


public class CardClass
{
    public int Power;
    public char CardType;

    public CardClass(int power, char cardType)
    {
        Power = power;
        CardType = cardType;
    }

    public bool IsCardType(char cardType)
    {
        return cardType == CardType;
    }

    public static CardClass[,] GetMapCardsClassFromMapGameObjects(GameObject[,] map)
    {
        var (width, height) = (map.GetLength(0), map.GetLength(1));
        var mapCardsClassForTests = new CardClass[width, height];
        for (int i = 0; i < width; i++)
        for (int j = 0; j < height; j++)
        {
            if (map[i, j].TryGetComponent(out Mob mob))
                mapCardsClassForTests[i, j] = new CardClass(mob.Power, 'M');
            if (map[i, j].TryGetComponent(out Food food))
                mapCardsClassForTests[i, j] = new CardClass(food.Power, 'F');
            if (map[i, j].TryGetComponent(out Location location))
                mapCardsClassForTests[i, j] = new CardClass(location.Power, 'L');
            if (map[i, j].TryGetComponent(out Player player))
                mapCardsClassForTests[i, j] = new CardClass(player.Power, 'P');
        }

        return mapCardsClassForTests;
    }
    
    public static CardClass[,] GetMapCardsClassFromLines(string[] lines)
    {
        var map = new CardClass[4, 4];
        for (int i = 0; i < lines.Length; i++)
        {
            var str = lines[i].Split(' ');
            for (int j = 0; j < str.Length; j++)
            {
                var x = j;
                var y = 3 - i;
                var power = int.Parse(str[j].Substring(1));
                map[x, y] = new CardClass(power, str[j][0]);
            }
        }

        return map;
    }
}
