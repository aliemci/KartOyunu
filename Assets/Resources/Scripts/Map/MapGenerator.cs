using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Specifications")]
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;
    [HideInInspector]
    public bool autoUpdate;

    public TerrainType[] regions;

    [Header("Prefabs")]
    public GameObject hexagonObject;
    public GameObject playerObject, marketObject, rivalObject;
    public Transform parentObject;

    [Header("Debug")]
    public List<hexagon> hexagons = new List<hexagon>();


    private float[,] fallOffMap;
    private float[,] noiseMap;

    private List<int> randomSpawnPoints;


    private List<int> shuffleArray(List<int> arr)
    {
        List<int> res = arr;
        var count = arr.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = res[i];
            res[i] = res[r];
            res[r] = tmp;
        }
        return res;
    }

    public Material[] generate_map()
    {
        noiseMap = Noise.generate_noise_map(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Material[] materialSet = new Material[mapWidth * mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                //Ada görünümü alsın diye, çevresindeki yükseklikler sıfırlanıyor.
                noiseMap[x, y] = noiseMap[x, y] - fallOffMap[x, y];

                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        materialSet[y * mapWidth + x] = regions[i].material;
                        break;
                    }
                }
            }
        }

        return materialSet;
    }

    void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if (mapHeight < 1)
            mapHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }

    public void Awake()
    {
        fallOffMap = FalloffGenerator.generate_falloff_map(mapWidth, mapHeight);

        //seed = Random.Range(0, 1000);

        //Üretim Safhası ↓

        generate_hexagons();

        randomSpawnPoints = shuffleArray(Enumerable.Range(0,hexagons.Count).ToList());

        generate_player();

        generate_NPC();
    }

    public void generate_hexagons()
    {
        float yOffset = 1.5f;
        float xOffset = 0.866f;

        float heightScale = 20f;

        Material[] materialSet = generate_map();

        for (int x = 0; x < mapWidth; x++)
        {
            List<hexagon> tempHexagons = new List<hexagon>();

            for (int y = 0; y < mapHeight; y++)
            {
                if (noiseMap[x, y] > regions[1].height)
                {
                    GameObject createdHexagon = Instantiate(hexagonObject, parentObject);

                    createdHexagon.name = x + " " + y;

                    float heightOfHex = noiseMap[x, y] * heightScale;
                    if (heightOfHex < 0)
                        heightOfHex = 0.01f;

                    createdHexagon.transform.localScale = new Vector3(1f, 1f, 1f);
                    createdHexagon.transform.localRotation = Quaternion.Euler(0f,180f,0f);

                    createdHexagon.GetComponent<MeshRenderer>().material = materialSet[y * mapWidth + x];


                    //6gen offsetleri
                    if (y % 2 == 0)
                        createdHexagon.transform.localPosition = new Vector3((2 * x) * xOffset, y * yOffset, 0f);
                    else
                        createdHexagon.transform.localPosition = new Vector3((2 * x + 1) * xOffset, y * yOffset, 0f);

                    createdHexagon.GetComponent<Hexagon>().ownHexagon = new hexagon(createdHexagon, new Vector2(x, y), createdHexagon.transform.position);

                    //createdHexagon.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = x + " " + y;

                    //Listeye ekliyor
                    hexagons.Add(createdHexagon.GetComponent<Hexagon>().ownHexagon);
                }

            }

        }

        //Haritayı ortalamak için
        parentObject.position = hexagons[Mathf.CeilToInt(hexagons.Count / 2)].hexObj.transform.position * -1;

        
    }


    public void generate_player()
    {
        int randomSpawnIndex = randomSpawnPoints[0];

        randomSpawnPoints.RemoveAt(0);

        //playerObj = Instantiate(playerObj, hexagons[randomSpawnIndex].hexObj.transform);
        playerObject = Instantiate(playerObject);

        playerObject.name = "Player";

        playerObject.transform.position = hexagons[randomSpawnIndex].hexObj.transform.position + new Vector3(0f,0f,-1f);

        playerObject.GetComponent<PlayerMovement>().parentHex = hexagons[randomSpawnIndex];

        //Kamerayı oyuncunun üstüne getiriyor
        GameObject.Find("Main Camera").transform.position = playerObject.transform.position + new Vector3(0f,0f,-10f);


    }


    public void generate_NPC()
    {
        int randomSpawnIndex = randomSpawnPoints[0];

        randomSpawnPoints.RemoveAt(0);

        marketObject = Instantiate(marketObject);

        marketObject.name = "Market";

        marketObject.transform.position = hexagons[randomSpawnIndex].hexObj.transform.position + new Vector3(0f, 0f, -1f);

        marketObject.GetComponent<NPCBehaviour>().parentHex = hexagons[randomSpawnIndex];

        for (int i = 0; i < 3; i++)
        {
            randomSpawnIndex = randomSpawnPoints[0];

            randomSpawnPoints.RemoveAt(0);

            rivalObject = Instantiate(rivalObject);

            rivalObject.name = "Rival " + (i + 1);

            rivalObject.transform.position = hexagons[randomSpawnIndex].hexObj.transform.position + new Vector3(0f, 0f, -1f);

            rivalObject.GetComponent<NPCBehaviour>().parentHex = hexagons[randomSpawnIndex];

        }


    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Material material;
}

[System.Serializable]
public struct hexagon
{

    public GameObject hexObj;
    public Vector2 coordinates;
    public Vector2 position;

    public hexagon(GameObject obj, Vector2 coord, Vector2 pos)
    {
        hexObj = obj;
        coordinates = coord;
        position = pos;
    }

}

