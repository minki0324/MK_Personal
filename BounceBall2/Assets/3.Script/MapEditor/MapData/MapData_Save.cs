using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class MapData_Save : MonoBehaviour
{
    [SerializeField] private TileMap2D tilemap;
    [SerializeField] InputField Name_inputfield;

    private void Awake()
    {
        Name_inputfield.text = "Noname.json";
    }
    public void Save()
    {
        MapData data = tilemap.GetMapData();
        string filename = Name_inputfield.text;

        if (filename.Contains(".json")) ; // json문구가 포함되지 않았다면 ?
        {
            filename += ".json";
        }

        filename = Path.Combine("MapData/", filename);
        string tojson = JsonConvert.SerializeObject(data, Formatting.Indented);

        File.WriteAllText(filename, tojson);
    }
}
