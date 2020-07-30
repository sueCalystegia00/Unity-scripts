using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.IO;

namespace ViveSR.anipal.Eye
{
    public class CSVWriter : MonoBehaviour
    {
        [SerializeField] private GameObject Camera;     // カメラオブジェクト
        
        [SerializeField] private LineRenderer lRend;    // テスト用:視線確認用のLineのレンダラー
        public int LengthOfRay = 10;                    // テスト用:視線ラインの描画距離

        private static StreamWriter writer90hz;         // gazeData書き出し用
        private static StreamWriter writer120hz;        // pupilData書き出し用
        private string gazeFilePath;                    // 視線データ保存場所
        private string pupilFilePath;                   // 瞳孔データ保存場所
        // gazeData.csvの１行目，column名
        private string gazeDataLabels = "timestamp" + "," +
                                        //"gazeOriginL.X" + "," + "gazeOriginL.Y" + "," + "gazeOriginL.Z" + "," +
                                        //"gazeOriginR.X" + "," + "gazeOriginR.Y" + "," + "gazeOriginR.Z" + "," +
                                        "gazeOriginC.X" + "," + "gazeOriginC.Y" + "," + "gazeOriginC.Z" + "," +
                                        //"gazeDir_L.X" + "," + "gazeDir_L.Y" + "," + "gazeDir_L.Z" + "," +
                                        //"gazeDir_R.X" + "," + "gazeDir_R.Y" + "," + "gazeDir_R.Z" + "," +
                                        "gazeDir_C.X" + "," + "gazeDir_C.Y" + "," + "gazeDir_C.Z" + "," +
                                        "cameraPos.X" + "," + "cameraPos.Y" + "," + "cameraPos.Z" + "," +
                                        "cameraAng.X" + "," + "cameraAng.Y" + "," + "cameraAng.Z";
        // pupilData.csvの１行目，column名
        private string pupilDataLabels = "timestamp" + "," + "pupilDia_L" + "," + "pupilDia_R";

        /// <summary>
        /// Use this for initialization
        /// </summary>
        void Start()
        {
            // ファイル保存場所の設定. dataPathでAssetsまでのパスを取れる
            gazeFilePath = Application.dataPath + "/Resorces/gazeData.csv";
            pupilFilePath = Application.dataPath + "/Resorces/pupilData.csv";
            // StreamWriterの初期化
            writer90hz = new StreamWriter(gazeFilePath, true) { AutoFlush = true };
            writer120hz = new StreamWriter(pupilFilePath, true) { AutoFlush = true };
            // 1行目(データのラベル)の書き出し
            writer90hz.WriteLine(gazeDataLabels);
            writer120hz.WriteLine(pupilDataLabels);
            
        }

        void FixedUpdate()
        {

            // 視線データの書き出し
            writer90hz.WriteLine(GetEyeDataModule.timeStamp + "," +
                                //GetEyeDataModule.gazeOriginLeft.x + "," + GetEyeDataModule.gazeOriginLeft.y + "," + GetEyeDataModule.gazeOriginLeft.z + "," +
                                //GetEyeDataModule.gazeOriginRight.x + "," + GetEyeDataModule.gazeOriginRight.y + "," + GetEyeDataModule.gazeOriginRight.z + "," +
                                GetEyeDataModule.gazeOriginCombine.x + "," + GetEyeDataModule.gazeOriginCombine.y + "," + GetEyeDataModule.gazeOriginCombine.z + "," +
                                //GetEyeDataModule.gazeDirectionLeft.x + "," + GetEyeDataModule.gazeDirectionLeft.y + "," + GetEyeDataModule.gazeDirectionLeft.z + "," +
                                //GetEyeDataModule.gazeDirectionRight.x + "," + GetEyeDataModule.gazeDirectionRight.y + "," + GetEyeDataModule.gazeDirectionRight.z + "," +
                                GetEyeDataModule.gazeDirectionCombine.x + "," + GetEyeDataModule.gazeDirectionCombine.y + "," + GetEyeDataModule.gazeDirectionCombine.z + "," +
                                Camera.transform.position.x + "," + Camera.transform.position.y + "," + Camera.transform.position.z + "," +
                                Camera.transform.localEulerAngles.x + "," + Camera.transform.localEulerAngles.y + "," + Camera.transform.localEulerAngles.z);
            // テスト用: 視線をラインで描画する
            SetGazeRay();
        }

        // GetEyeDataModuleから呼び出される(120Hz更新)
        public static void Write()
        {
            // 瞳孔径の書き出し
            writer120hz.WriteLine(GetEyeDataModule.timeStamp + "," +
                                GetEyeDataModule.pupilDiameterLeft + "," + GetEyeDataModule.pupilDiameterRight);

            /*
            // 全データの書き出し
            streamwriter.WriteLine(GetEyeDataModule.timeStamp + "," + 
                                GetEyeDataModule.pupilDiameterLeft + "," + GetEyeDataModule.pupilDiameterRight + "," +
                                GetEyeDataModule.gazeOriginLeft.x + "," + GetEyeDataModule.gazeOriginLeft.y + "," + GetEyeDataModule.gazeOriginLeft.z + "," +
                                GetEyeDataModule.gazeOriginRight.x + "," + GetEyeDataModule.gazeOriginRight.y + "," + GetEyeDataModule.gazeOriginRight.z + "," +
                                GetEyeDataModule.gazeDirectionLeft.x + "," + GetEyeDataModule.gazeDirectionLeft.y + "," + GetEyeDataModule.gazeDirectionLeft.z + "," +
                                GetEyeDataModule.gazeDirectionRight.x + "," + GetEyeDataModule.gazeDirectionRight.y + "," + GetEyeDataModule.gazeDirectionRight.z + "," +
                                Camera.transform.position.x + "," + Camera.transform.position.y + "," + Camera.transform.position.z + "," +
                                Camera.transform.localEulerAngles.x + "," + Camera.transform.localEulerAngles.y + "," + Camera.transform.localEulerAngles.z);
            */

        }

        // テスト用: 視線の描画
        void SetGazeRay()
        {
            Vector3 rayOrigin = Camera.transform.position + GetEyeDataModule.gazeOriginCombine;
            Vector3 tGazeDirection = Camera.transform.TransformDirection(GetEyeDataModule.gazeDirectionCombine);
            lRend.SetPosition(0, rayOrigin);                                // 視線の始点設定
            lRend.SetPosition(1, rayOrigin + tGazeDirection * LengthOfRay); // 視線の終点設定

        }
    }
}