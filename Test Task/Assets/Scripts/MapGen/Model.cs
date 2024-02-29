using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    //Размеры карты
    public int Height;
    public int Weight;

    // Размер зоны спавна случайных событий (Если поставить по нулям, спавна ивентов не будет
    public int MinSpawnValue;
    public int MaxSpawnValue;

    // Размер приближеения карты (Чем больше значение тем меньше разнообразие ландшафта)
    public float Zoom;

    //Установка значений для кнопки Small
    public void SetSmallSetting()
    {
        Height = 16;
        Weight = 16;
        Zoom = 5f;
        MinSpawnValue = 3;
        MaxSpawnValue = 7;
    }

    // Установка значений для кнопки Medium
    public void SetMediumSetting()
    {
        Height = 32;
        Weight = 32;
        Zoom = 6f;
        MinSpawnValue = 3;
        MaxSpawnValue = 7;
    }

    //Установка значений для кнопки Big
    public void SetBigSetting()
    {
        Height = 60;
        Weight = 60;
        Zoom = 9f;
        MinSpawnValue = 3;
        MaxSpawnValue = 7;
    }
    /*
     * Можно добавить больше методов и кнопок для более тонкой настройки
     * 
     * Также сюда можно добавить массив тайлов для генерации разных карт, с разным ландшафтом и типом местностей
     */
}
