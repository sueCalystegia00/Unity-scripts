using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordSlider : MonoBehaviour
{
    [SerializeField] private GameObject CSVReader;  // CSV読み込み用モジュール
    [SerializeField] private GameObject Camera;     // カメラオブジェクト
    [SerializeField] private Text TimeText;         // 画面UI上のテキスト，タイムスタンプを表示する
    [SerializeField] private LineRenderer lRend;    // 視線描画用レンダラー
    [SerializeField] private Slider slider;         // シークエンスバー

    // csvデータを格納するリスト
    public static List<string[]> gazeDatas = new List<string[]>();
    public static List<string[]> pupilDatas = new List<string[]>();

    // 行数カウンター
    public static int g_lcount = 0;
    public static int p_lcount = 0;

    // csvから読み込んだ各眼球データ
    public static int initTime, time;   // タイムスタンプ
    public static Vector3 gazeOriginL, gazeOriginR, gazeDirL, gazeDirR, c_pos, c_ang;   // 視線の原点とベクトル，カメラ位置と向き

    public int LengthOfRay = 10;    // 視線ラインの長さ

    // Start is called before the first frame update
    void Start()
    {
        CSVReader.GetComponent<CSVReader>().Read(); // csv読み込みモジュールの起動
        slider.maxValue = g_lcount;                 // シークエンスバーの最大値設定
        slider.minValue = slider.value = 1.0f;      // シークエンスバーの最小値と現在値の設定
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (slider.value < g_lcount)    // ←修正予定
        {
            // シークエンスバーの位置(=タイムスタンプ)に合わせて各データを代入，実験時の様子を再現
            int fcount = (int)slider.value;
            int.TryParse(gazeDatas[fcount][0], out time);
            gazeOriginL = new Vector3(float.Parse(gazeDatas[fcount][1]), float.Parse(gazeDatas[fcount][2]), float.Parse(gazeDatas[fcount][3]));
            gazeOriginR = new Vector3(float.Parse(gazeDatas[fcount][4]), float.Parse(gazeDatas[fcount][5]), float.Parse(gazeDatas[fcount][6]));
            gazeDirL = new Vector3(float.Parse(gazeDatas[fcount][7]), float.Parse(gazeDatas[fcount][8]), float.Parse(gazeDatas[fcount][9]));
            gazeDirR = new Vector3(float.Parse(gazeDatas[fcount][10]), float.Parse(gazeDatas[fcount][11]), float.Parse(gazeDatas[fcount][12]));
            c_pos = new Vector3(float.Parse(gazeDatas[fcount][13]), float.Parse(gazeDatas[fcount][14]), float.Parse(gazeDatas[fcount][15]));
            c_ang = new Vector3(float.Parse(gazeDatas[fcount][16]), float.Parse(gazeDatas[fcount][17]), float.Parse(gazeDatas[fcount][18]));

        }
        slider.value++; // スライダーを毎フレーム１ずつ動かす

        TimeText.text = time.ToString();    // UI上のテキストにタイムスタンプを表示
        SetCamera();    // カメラ位置と向きを設定
        SetGazeRay();   // 視線を描画

    }

    void SetCamera()
    {
        Camera.transform.position = c_pos;          // カメラ位置の更新
        Camera.transform.localEulerAngles = c_ang;  // カメラ向きの更新
    }

    void SetGazeRay()
    {
        Vector3 rayOrigin = c_pos + gazeOriginR;    // 視線原点の設定
        Vector3 tGazeDirection = Camera.transform.TransformDirection(gazeDirR); // 視線ベクトルの設定
        lRend.SetPosition(0, rayOrigin);    // 視線ラインの始点設定
        lRend.SetPosition(1, rayOrigin + tGazeDirection * LengthOfRay); // 視線ラインの終点設定
    }
}
