using UnityEngine;

public class MyCostomPlayer : MonoBehaviour
{
    private MyGameInputs _input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = GetComponent<MyGameInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
