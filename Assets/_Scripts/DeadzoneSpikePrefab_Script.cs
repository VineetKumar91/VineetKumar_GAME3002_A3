using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DeadzoneSpikePrefab_Script : MonoBehaviour
{
    [SerializeField] private float m_fAngleValue;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, m_fAngleValue);
    }
}
