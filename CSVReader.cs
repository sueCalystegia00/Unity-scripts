using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CSVReader : MonoBehaviour
{
    private string gazeFilePath;
    private string pupilFilePath;
    [SerializeField] private RecordSlider recordslider;

    public void Read()
    {
        // ファイル保存場所の設定. dataPathでAssetsまでのパスを取れる
        gazeFilePath = Application.dataPath + "/Resorces/gazeData.csv";
        pupilFilePath = Application.dataPath + "/Resorces/pupilData.csv";

        FileStream g_fileStream = File.Open(gazeFilePath, FileMode.Open, FileAccess.Read);
        using (StreamReader streamReader = new StreamReader(g_fileStream))
        {
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                RecordSlider.gazeDatas.Add(line.Split(','));
                RecordSlider.g_lcount++;
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