namespace Warships;

public enum EShipType
{
    Single,
    Double,
    Triple,
    Quadruple
}

public static class EShipTypeExtensions
{
    public static int Length(this EShipType type)
    {
        return type switch
        {
            EShipType.Single => 1,
            EShipType.Double => 2,
            EShipType.Triple => 3,
            EShipType.Quadruple => 4,
            _ => 0
        };
    }
}