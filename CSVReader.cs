using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CSVReader : MonoBehaviour
{    
    private string gazeFilePath;    // 視線データのファイル保存場所
    private string pupilFilePath;   // 瞳孔径データのファイル保存場所
    [SerializeField] private RecordSlider recordslider; // プレイバック時のシークエンスバーの取得

    public void Read()
    {
        // ファイル保存場所の設定. dataPathでAssetsまでのパスを取れる
        gazeFilePath = Application.dataPath + "/Resorces/gazeData.csv";
        pupilFilePath = Application.dataPath + "/Resorces/pupilData.csv";

        // csvファイルを読み込む
        FileStream g_fileStream = File.Open(gazeFilePath, FileMode.Open, FileAccess.Read);
        using (StreamReader streamReader = new StreamReader(g_fileStream))
        {
            while (!streamReader.EndOfStream)   // 読み終えるまで続ける
            {
                string line = streamReader.ReadLine();          // 一行読んでlineに一時格納
                RecordSlider.gazeDatas.Add(line.Split(','));    // lineをカンマ区切りでリストに追加していく
                RecordSlider.g_lcount++;                        // リストに追加するたびに+1することでデータの行数を数える
            }
        }
        /*
        FileStream p_fileStream = File.Open(pupilFilePath, FileMode.Open, FileAccess.Read);
        using (StreamReader streamReader = new StreamReader(g_fileStream))
        {
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                RecordSlider.pupilDatas.Add(line.Split(','));
                RecordSlider.p_lcount++;
            }
        }
        RecordSlider.initTime = int.Parse(RecordSlider.gazeDatas[1][0]);
        */
    }

}