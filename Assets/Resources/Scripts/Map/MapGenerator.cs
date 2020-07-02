using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    public Transform parentObj;

    [Header("Debug")]
    public List<hexagon> hexagons = new List<hexagon>();

    [Header("Generate Player")]
    public GameObject playerObj;

    private float[,] fallOffMap;
    private float[,] noiseMap;

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

        seed = Random.Range(0, 1000);

        generate_hexagons();
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
                    GameObject createdHexagon = Instantiate(hexagonObject, parentObj);

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
        parentObj.position = hexagons[Mathf.CeilToInt(hexagons.Count / 2)].hexObj.transform.position * -1;

        //Bir sonraki aşama

    }

    //Artık kullanılmayacak ↓
    public void generate_player()
    {
        int randomSpawnIndex = Random.Range(0, hexagons.Count - 1);

        //playerObj = Instantiate(playerObj, hexagons[randomSpawnIndex].hexObj.transform);
        playerObj = Instantiate(playerObj);

        playerObj.name = "Player";

        playerObj.transform.position = hexagons[randomSpawnIndex].hexObj.transform.position + new Vector3(0f,0f,-1f);

        playerObj.GetComponent<PlayerMovement>().parentHex = hexagons[randomSpawnIndex];

        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = false;
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
    }

    public void generate_NPC()
    {

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

