using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RecordSlider : MonoBehaviour
{
    [SerializeField] private GameObject CSVReader;  // CSV読み込み用モジュール
    [SerializeField] private Slider slider;         // シークエンスバー
    [SerializeField] private GameObject Camera;     // カメラオブジェクト
    [SerializeField] private Text TimeText;         // 画面UI上のテキスト，タイムスタンプを表示する
    [SerializeField] private GameObject gazeArea;   // 視線描画用球体(グレーパネルくり抜き)
    //[SerializeField] private LineRenderer lRend;    // 視線描画用レンダラー
    [SerializeField] private VideoPlayer vPlayer;

    // csvデータを格納するリスト
    public static List<string[]> gazeDatas = new List<string[]>();
    public static List<string[]> pupilDatas = new List<string[]>();

    // 行数カウンター
    public static int g_lcount = 0;
    public static int p_lcount = 0;

    // csvから読み込んだ各眼球データ
    public static double initTime, time;   // タイムスタンプ
    public static Vector3 gazeOriginL, gazeOriginR, gazeOriginC;    //視線の原点(眼球位置)
    public static Vector3 gazeDirL, gazeDirR, gazeDirC;             //視線のベクトル
    public static Vector3 c_pos, c_ang;   //カメラ位置と向き

    public int LengthOfRay = 10;    // 視線ラインの長さ

    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        CSVReader.GetComponent<CSVReader>().Read(); // csv読み込みモジュールの起動
        g_lcount = gazeDatas.Count;                 // listの行数取得
        initTime = double.Parse(gazeDatas[1][0]);      // 最初のタイムスタンプの値を取得
        slider.maxValue = g_lcount;                 // シークエンスバーの最大値設定
        slider.minValue = slider.value = 1.0f;      // シークエンスバーの最小値と現在値の設定
        
        //Debug.Log(vPlayer.clip.length);
    }
    public void click(){
      if(playing)
      {
        playing = false;
      }
      else
      {
        playing = true;
      }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (slider.value < g_lcount && playing)    // ←修正予定
        {
            // シークエンスバーの位置(=タイムスタンプ)に合わせて各データを代入，実験時の様子を再現
            int fcount = (int)slider.value;
            time = (double.Parse(gazeDatas[fcount][0]) - initTime)/1000;
            gazeOriginC = new Vector3(float.Parse(gazeDatas[fcount][1]), float.Parse(gazeDatas[fcount][2]), float.Parse(gazeDatas[fcount][3]));
            gazeDirC = new Vector3(float.Parse(gazeDatas[fcount][4]), float.Parse(gazeDatas[fcount][5]), float.Parse(gazeDatas[fcount][6]));
            c_pos = new Vector3(float.Parse(gazeDatas[fcount][7]), float.Parse(gazeDatas[fcount][8]), float.Parse(gazeDatas[fcount][9]));
            c_ang = new Vector3(float.Parse(gazeDatas[fcount][10]), float.Parse(gazeDatas[fcount][11]), float.Parse(gazeDatas[fcount][12]));
            //vPlayer.time = float.Parse(gazeDatas[fcount][13]);

            slider.value++; // スライダーを毎フレーム１ずつ動かす
        }
        

        vPlayer.time = time;
        TimeText.text = time.ToString();    // UI上のテキストにタイムスタンプを表示
        SetCamera();    // カメラ位置と向きを設定
        SetGazeArea();    // 視線を円エリアで描画
        //SetGazeRay();   // 視線を光線で描画
    }

    void SetCamera()
    {
        Camera.transform.position = c_pos;          // カメラ位置の更新
        Camera.transform.localEulerAngles = c_ang;  // カメラ向きの更新
    }

    void SetGazeArea()
    {
        Vector3 rayOrigin = c_pos + gazeOriginC;    // 視線原点の設定
        Vector3 tGazeDirection = Camera.transform.TransformDirection(gazeDirC); // 視線ベクトルの設定
        Ray gaze = new Ray(rayOrigin, tGazeDirection);  // Ray型にまとめる
        RaycastHit hit;   //Rayが当たったオブジェクトの情報を入れる
        Debug.DrawLine (gaze.origin, gaze.direction * 100, Color.red);
        
        if(Physics.Raycast(gaze,out hit))
        {
          Debug.Log(hit);
          gazeArea.transform.position = hit.point;
        }

    }
    /*
    void SetGazeRay()
    {
        Vector3 rayOrigin = c_pos + gazeOriginC;    // 視線原点の設定
        Vector3 tGazeDirection = Camera.transform.TransformDirection(gazeDirC); // 視線ベクトルの設定
        lRend.SetPosition(0, rayOrigin);    // 視線ラインの始点設定
        lRend.SetPosition(1, rayOrigin + tGazeDirection * LengthOfRay); // 視線ラインの終点設定
    }
    */
}