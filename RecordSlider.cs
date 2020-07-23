using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordSlider : MonoBehaviour
{
    [SerializeField] private GameObject CSVReader;
    [SerializeField] private GameObject Camera;
    [SerializeField] private Text TimeText;
    [SerializeField] private LineRenderer lRend;
    [SerializeField] private Slider slider;

    // csvデータを格納するリスト
    public static List<string[]> gazeDatas = new List<string[]>();
    public static List<string[]> pupilDatas = new List<string[]>();

    // 行数カウンター
    public static int g_lcount = 0;
    public static int p_lcount = 0;

    public static int initTime, time;
    public static Vector3 gazeOriginL, gazeOriginR, gazeDirL, gazeDirR, c_pos, c_ang;
    
    public int LengthOfRay = 10;
    // Start is called before the first frame update
    void Start()
    {
        CSVReader.GetComponent<CSVReader>().Read();
        slider.maxValue = g_lcount;
        slider.minValue = slider.value = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (slider.value < g_lcount)
        {
            int fcount = (int)slider.value;
            int.TryParse(gazeDatas[fcount][0], out time);
            gazeOriginL = new Vector3(float.Parse(gazeDatas[fcount][1]), float.Parse(gazeDatas[fcount][2]), float.Parse(gazeDatas[fcount][3]));
            gazeOriginR = new Vector3(float.Parse(gazeDatas[fcount][4]), float.Parse(gazeDatas[fcount][5]), float.Parse(gazeDatas[fcount][6]));
            gazeDirL = new Vector3(float.Parse(gazeDatas[fcount][7]), float.Parse(gazeDatas[fcount][8]), float.Parse(gazeDatas[fcount][9]));
            gazeDirR = new Vector3(float.Parse(gazeDatas[fcount][10]), float.Parse(gazeDatas[fcount][11]), float.Parse(gazeDatas[fcount][12]));
            c_pos = new Vector3(float.Parse(gazeDatas[fcount][13]), float.Parse(gazeDatas[fcount][14]), float.Parse(gazeDatas[fcount][15]));
            c_ang = new Vector3(float.Parse(gazeDatas[fcount][16]), float.Parse(gazeDatas[fcount][17]), float.Parse(gazeDatas[fcount][18]));

        }
        slider.value++;

        TimeText.text = time.ToString();
        SetCamera();
        SetGazeRay();

    }

    void SetCamera()
    {
        Camera.transform.position = c_pos;
        Camera.transform.localEulerAngles = c_ang;
    }

    void SetGazeRay()
    {
        Vector3 rayOrigin = c_pos + gazeOriginR;
        Vector3 tGazeDirection = Camera.transform.TransformDirection(gazeDirR);
        lRend.SetPosition(0, rayOrigin);
        lRend.SetPosition(1, rayOrigin + tGazeDirection * LengthOfRay);
    }
}
