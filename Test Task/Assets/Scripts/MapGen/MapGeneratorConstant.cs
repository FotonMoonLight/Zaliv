using UnityEngine;

public static class MapGeneratorConstant
{
    //Константы генератора, без необходимости не изменять
    public const double PerlinDouble = 0.95;

    public const int MaxExclusive = 999999; // Можно увелисть но ничего не даст
    public const int MinInclusive = 100;// Не устанавливать минимум ниже 100, черевато ошибками генерации
    public const int SetZero = 0;
}