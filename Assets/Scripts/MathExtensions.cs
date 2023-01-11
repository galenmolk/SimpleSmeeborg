using System;

public static class MathExtensions
{
    public static bool IsWithin(this int value, int lowerBounds, int upperBounds)
    {
        return value > lowerBounds && value < upperBounds;
    }
}
