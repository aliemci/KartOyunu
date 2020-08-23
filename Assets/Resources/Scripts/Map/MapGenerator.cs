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

    private int marketCount;
    private int rivalCount;

    [Header("Debug")]
    public List<hexagon> hexagons = new List<hexagon>();

    
    private float[,] fallOffMap;
    private float[,] noiseMap;

    private List<int> randomSpawnPoints;
    public List<MapObject> objectsOnMap = new List<MapObject>();

    private Map loadedMap;

    // FONKSIYONLAR --------------------------------

    public static List<int> shuffleArray(List<int> arr)
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

        loadedMap = SaveSystem.load_map();

        seed = loadedMap.mapSeed;
        marketCount = loadedMap.marketCount;
        rivalCount = loadedMap.rivalCount;

        //Üretim Safhası ↓

        generate_hexagons();

        //Eğer kayıtlı dosya yoksa
        if (!loadedMap.isLoaded)
        {
            randomSpawnPoints = shuffleArray(Enumerable.Range(0,hexagons.Count).ToList());
            loadedMap.isLoaded = true;

            generate_player();

            for (int i = 0; i < marketCount; i++)
                generate_NPC(NPCTypes.market);
            for (int i = 0; i < rivalCount; i++)
                generate_NPC(NPCTypes.rival);

            Debug.Log(objectsOnMap);

        }
        else
        {
            objectsOnMap = loadedMap.objsOnMap;

            Debug.Log(objectsOnMap.Capacity);

            foreach (MapObject item in objectsOnMap)
            {
                Debug.Log(item.name);
                if (item.name.Contains("Player"))
                    generate_player(item.index);
                else if (item.name.Contains("Market"))
                    generate_NPC(NPCTypes.market, item.index);
                else if (item.name.Contains("Rival"))
                    generate_NPC(NPCTypes.rival, item.index);
            }
            
        }

        loadedMap.objsOnMap = objectsOnMap;

        SaveSystem.save_map(loadedMap);
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


    public void generate_player(int randomSpawnIndex = -1)
    {
        bool should_saved = false;

        if(randomSpawnIndex == -1)
        {
            randomSpawnIndex = randomSpawnPoints[0];
            randomSpawnPoints.RemoveAt(0);
            should_saved = true;
        }

        playerObject = Instantiate(playerObject);

        playerObject.name = "Player";

        playerObject.transform.position = hexagons[randomSpawnIndex].hexObj.transform.position + new Vector3(0f,0f,-1f);

        playerObject.GetComponent<PlayerMovement>().parentHex = hexagons[randomSpawnIndex];

        if(should_saved)
            objectsOnMap.Add(new MapObject(playerObject.name, randomSpawnIndex));

        //Kamerayı oyuncunun üstüne getiriyor
        GameObject.Find("Main Camera").transform.position = playerObject.transform.position + new Vector3(0f,0f,-10f);


    }


    public void generate_NPC(NPCTypes npc, int randomSpawnIndex=-1)
    {
        bool should_saved = false;
        
        if (randomSpawnIndex == -1)
        {
            randomSpawnIndex = randomSpawnPoints[0];
            randomSpawnPoints.RemoveAt(0);
            should_saved = true;
        }

        if (npc == NPCTypes.market)
        {
            marketObject = Instantiate(marketObject);

            marketObject.name = "Market";

            marketObject.transform.position = hexagons[randomSpawnIndex].hexObj.transform.position + new Vector3(0f, 0f, -1f);

            marketObject.GetComponent<NPCBehaviour>().parentHex = hexagons[randomSpawnIndex];

            if (should_saved)
                objectsOnMap.Add(new MapObject(marketObject.name, randomSpawnIndex));
        }

        else if(npc == NPCTypes.rival)
        {
            rivalObject = Instantiate(rivalObject);

            rivalObject.name = "Rival " + randomSpawnIndex;

            rivalObject.transform.position = hexagons[randomSpawnIndex].hexObj.transform.position + new Vector3(0f, 0f, -1f);

            rivalObject.GetComponent<NPCBehaviour>().parentHex = hexagons[randomSpawnIndex];
            
            if (should_saved)
                objectsOnMap.Add(new MapObject(rivalObject.name, randomSpawnIndex));
        }


    }


    public void OnDestroy()
    {
        Debug.Log("OnDestroy Çalıştı!");

        MapObject playerMO = new MapObject("Player", hexagons.IndexOf(playerObject.GetComponent<PlayerMovement>().parentHex));

        mapUpdate();

        loadedMap.objsOnMap = objectsOnMap;

        loadedMap.objsOnMap[0] = playerMO;

        SaveSystem.save_map(loadedMap);
    }

    public void mapUpdate()
    {
        foreach(MapObject m in objectsOnMap)
        {
            if (!GameObject.Find(m.name))
                objectsOnMap.Remove(m);
        }
    }


}




// YAPILAR --------------------------------------

[System.Serializable]
public struct Map
{
    //[SerializeField]
    public int mapSeed;

    public int marketCount;

    public int rivalCount;

    public List<MapObject> objsOnMap;

    public bool isLoaded;
}

[System.Serializable]
public struct MapObject
{
    public string name;
    public int index;

    public MapObject(string objName, int objIndex)
    {
        name = objName;
        index = objIndex;
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

