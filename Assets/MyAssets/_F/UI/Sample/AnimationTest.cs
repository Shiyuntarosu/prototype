using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator myAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            myAnimator.SetTrigger("Highlighted");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            myAnimator.SetTrigger("Normal");
        }
    }
}
