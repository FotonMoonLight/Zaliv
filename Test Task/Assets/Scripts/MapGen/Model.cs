using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    //������� �����
    public int Height;
    public int Weight;

    // ������ ���� ������ ��������� ������� (���� ��������� �� �����, ������ ������� �� �����
    public int MinSpawnValue;
    public int MaxSpawnValue;

    // ������ ������������ ����� (��� ������ �������� ��� ������ ������������ ���������)
    public float Zoom;

    //��������� �������� ��� ������ Small
    public void SetSmallSetting()
    {
        Height = 16;
        Weight = 16;
        Zoom = 5f;
        MinSpawnValue = 3;
        MaxSpawnValue = 7;
    }

    // ��������� �������� ��� ������ Medium
    public void SetMediumSetting()
    {
        Height = 32;
        Weight = 32;
        Zoom = 6f;
        MinSpawnValue = 3;
        MaxSpawnValue = 7;
    }

    //��������� �������� ��� ������ Big
    public void SetBigSetting()
    {
        Height = 60;
        Weight = 60;
        Zoom = 9f;
        MinSpawnValue = 3;
        MaxSpawnValue = 7;
    }
    /*
     * ����� �������� ������ ������� � ������ ��� ����� ������ ���������
     * 
     * ����� ���� ����� �������� ������ ������ ��� ��������� ������ ����, � ������ ���������� � ����� ����������
     */
}
