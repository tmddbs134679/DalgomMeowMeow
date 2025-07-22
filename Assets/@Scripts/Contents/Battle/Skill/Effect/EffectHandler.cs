using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static EffectHandler Instance { get; private set; }
    public EffectManager[] effectManagers;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        effectManagers = GetComponentsInChildren<EffectManager>();
    }
    


    void Start()
    {
        
    }

    
}
