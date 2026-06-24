using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerHUD;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHUD = transform.Find("playerHUD").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
