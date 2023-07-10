using Bonnie.CharacterStats;
public class Character
{
    public CharacterStats Strength;
}

public class Item
{

    public void Equip(Character c)
    {
        c.Strength.AddModifier(new StatModifier(10, StatModType.Flat, this));
        c.Strength.AddModifier(new StatModifier(0.1f, StatModType.PercentMult, this));
    }

    public void Unequip(Character c)
    {
        c.Strength.RemoveAllModifiersFromSource(this);
    }
}
