using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/*
    Json
1. string���� �о�� ������ Ű�� ���� - Database ����IP, ��й�ȣ
2. �ܺ� dll�� ����Ͽ��� Ŭ���� �����̰ų� ������ �����̳� ���Ŀ� �����͸� ����ȭ�� ������ȭ�� ���� �����͸� ���� ������ �´�.

*/

public class MapData_Load : MonoBehaviour
{
    public MapData Load(string Filename)
    {
        if(!Filename.Contains(".json"))
        {
            Filename += ".json";
        }

        Filename = Path.Combine(Application.streamingAssetsPath, Filename);
        string ReadData = File.ReadAllText(Filename);
        MapData mapData = new MapData();

        // �� ����ȭ
        mapData = JsonConvert.DeserializeObject<MapData>(ReadData);
        return mapData;
    }
}
