using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/*
    Json
1. string으로 읽어온 데이터 키로 접근 - Database 접속IP, 비밀번호
2. 외부 dll을 사용하여서 클래스 형식이거나 데이터 컨테이너 형식에 데이터를 직렬화와 역직렬화를 통해 데이터를 쉽게 가지고 온다.

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

        // 역 직렬화
        mapData = JsonConvert.DeserializeObject<MapData>(ReadData);
        return mapData;
    }
}
