using UnityEngine;


// TextLoader クラス：JSONファイルを読み込んで、テキストデータを Unity に取り込む
public class TextLoader : MonoBehaviour
{
    // 読み込む JSON ファイル名（Resources フォルダ内に配置）
    const string textAssetName = "TextPrototype";

    // 読み込んだデータを格納する変数
    public TextData textData;

    void Awake()
    {
        // Resources フォルダから JSON ファイルを読み込む
        TextAsset jsonFile = Resources.Load<TextAsset>(textAssetName);

        // JsonUtility は配列の読み込みにラップが必要なので、"lines" というキーで囲む
        string wrappedJson = "{\"lines\":" + jsonFile.text + "}";

        // JSON を TextData 型に変換（lines 配列が含まれる構造）
        textData = JsonUtility.FromJson<TextData>(wrappedJson);
    }
}
