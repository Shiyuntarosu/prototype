using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// /テキストの表示に関するクラス　テキストを表示したい場合：PlayText関数
/// </summary>
public class TextAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private TextMeshProUGUI speakerName;

    // 外部から呼び出す用の関数
    public void PlayText(string text, string speaker, float speed)
    {
        StopAllCoroutines(); // 前の演出を止める
        StartCoroutine(ShowText(text, speaker, speed));
    }

    // タイピング風に文字を表示するコルーチン
    private IEnumerator ShowText(string text, string speaker, float speed)
    {
        logText.text = "";
        speakerName.text = speaker;

        foreach (char c in text)
        {
            logText.text += c;
            yield return new WaitForSeconds(speed);
        }
    }
}
