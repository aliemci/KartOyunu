using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField]
    public static GameObject damageGO = Resources.Load<GameObject>("Prefabs/Damage");


    public static void CreateDamageIndicator(Vector3 position, int damage)
    {
        Transform canvasTransform = GameObject.Find("Canvas").transform;

        GameObject indicatorGO = Instantiate(damageGO, position, Quaternion.identity, canvasTransform);

        indicatorGO.GetComponent<TextMeshProUGUI>().text = (-1 * damage).ToString();

    }

}
