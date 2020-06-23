using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public GameObject hexagonObject;
    public Transform parentObj;

    public List<hexagon> hexagons = new List<hexagon>();

    private float[,] fallOffMap;
    private float[,] noiseMap;

    public Material[] generate_map()
    {
        noiseMap = Noise.generate_noise_map(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Material[] textureMap = new Material[mapWidth * mapHeight];
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
                        textureMap[y * mapWidth + x] = regions[i].material;
                        break;
                    }
                }
            }
        }

        return textureMap;
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

        Material[] textureMap = generate_map();

        for (int x = 0; x < mapWidth; x++)
        {
            List<hexagon> tempHexagons = new List<hexagon>();

            for (int y = 0; y < mapHeight; y++)
            {
                GameObject createdHexagon = Instantiate(hexagonObject, parentObj);

                float heightOfHex = noiseMap[x, y] * heightScale;
                if (heightOfHex < 0)
                    heightOfHex = 0.01f;

                createdHexagon.transform.localScale = new Vector3(1f, 1f, heightOfHex);
                createdHexagon.transform.localRotation = Quaternion.identity;

                createdHexagon.GetComponent<MeshRenderer>().material = textureMap[y * mapWidth + x];

                if (y%2==0)
                    createdHexagon.transform.localPosition = new Vector3((2 * x) * xOffset, y * yOffset, 0f);
                else
                    createdHexagon.transform.localPosition = new Vector3((2 * x + 1) * xOffset, y * yOffset, 0f);

                if(noiseMap[x,y] > regions[1].height)
                {
                    hexagon temp = new hexagon(hexagonObject, new float[x,y]);

                    hexagons.Add(temp);
                }

            }

        }

        //Haritayı ortalamak için
        parentObj.position = hexagons[hexagons.Count / 2].hexObj.transform.position * -1;

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
    public float[,] coordinates;

    public hexagon(GameObject obj, float[,] coor)
    {
        hexObj = obj;
        coordinates = coor;
    }

}