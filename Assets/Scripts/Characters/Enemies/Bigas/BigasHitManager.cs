using UnityEngine;

public class BigasHitManager : HitManager
{
    private BigasManager _bigasManager;
    void Start()
    {
        _bigasManager = GetComponent<BigasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
