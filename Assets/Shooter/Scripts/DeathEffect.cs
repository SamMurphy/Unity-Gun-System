using UnityEngine;
using System.Collections;


public class DeathEffect : MonoBehaviour
{

    void Start()
    {
        Object.Destroy(gameObject, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
