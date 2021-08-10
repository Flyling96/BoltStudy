using Bolt;
using Ludiq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float duration = 2f;

    [SerializeField]
    private float time;

    void Start()
    {
        
    }

    void Update()
    {
        if(name != "Actor") {
            time += Time.deltaTime;
            if(time > duration) {
                Destroy(gameObject);
            }
        }
    }

}
