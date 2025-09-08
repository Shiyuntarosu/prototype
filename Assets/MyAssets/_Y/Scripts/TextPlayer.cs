using UnityEngine;

public class TextPlayer : MonoBehaviour
{
    [SerializeField] private TextLoader textLoader;

    private void Start()
    {
        ShowLine(textLoader.textData.lines[0]);
    }

    [SerializeField] private GameObject textAreaPrefab;

    public void ShowLine(TextLine line)
    {
        var instance = Instantiate(textAreaPrefab, transform);
        var animator = instance.GetComponent<TextAnimator>();
        animator.PlayText(line.text, line.speaker, line.textSpeed);
    }
}
