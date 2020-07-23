using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.IO;

namespace ViveSR.anipal.Eye
{

    ///<summary>
    /// This source code is based on SRanipal_Unity_SDK version 1.1.0.1.
    /// This class collects eye movement data measured by VIVE-PRO-EYE.
    ///</summary>

    public class CSVWriter : MonoBehaviour
    {
        [SerializeField] private GameObject Camera;
        [SerializeField] private LineRenderer lRend;
        public int LengthOfRay = 10;

        private static StreamWriter writer90hz;
        private static StreamWriter writer120hz;
        private string gazeFilePath;
        private string pupilFilePath;
        private string gazeDataLabels = "timestamp" + "," +
                                        "gazeOriginL.X" + "," + "gazeOriginL.Y" + "," + "gazeOriginL.Z" + "," +
                                        "gazeOriginR.X" + "," + "gazeOriginR.Y" + "," + "gazeOriginR.Z" + "," +
                                        "gazeDir_L.X" + "," + "gazeDir_L.Y" + "," + "gazeDir_L.Z" + "," +
                                        "gazeDir_R.X" + "," + "gazeDir_R.Y" + "," + "gazeDir_R.Z" + "," +
                                        "cameraPos.X" + "," + "cameraPos.Y" + "," + "cameraPos.Z" + "," +
                                        "cameraAng.X" + "," + "cameraAng.Y" + "," + "cameraAng.Z";
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
                                GetEyeDataModule.gazeOriginLeft.x + "," + GetEyeDataModule.gazeOriginLeft.y + "," + GetEyeDataModule.gazeOriginLeft.z + "," +
                                GetEyeDataModule.gazeOriginRight.x + "," + GetEyeDataModule.gazeOriginRight.y + "," + GetEyeDataModule.gazeOriginRight.z + "," +
                                GetEyeDataModule.gazeDirectionLeft.x + "," + GetEyeDataModule.gazeDirectionLeft.y + "," + GetEyeDataModule.gazeDirectionLeft.z + "," +
                                GetEyeDataModule.gazeDirectionRight.x + "," + GetEyeDataModule.gazeDirectionRight.y + "," + GetEyeDataModule.gazeDirectionRight.z + "," +
                                Camera.transform.position.x + "," + Camera.transform.position.y + "," + Camera.transform.position.z + "," +
                                Camera.transform.localEulerAngles.x + "," + Camera.transform.localEulerAngles.y + "," + Camera.transform.localEulerAngles.z);
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

        void SetGazeRay()
        {
            Vector3 rayOrigin = Camera.transform.position + GetEyeDataModule.gazeOriginRight;
            Vector3 tGazeDirection = Camera.transform.TransformDirection(GetEyeDataModule.gazeDirectionRight);
            lRend.SetPosition(0, rayOrigin);
            lRend.SetPosition(1, rayOrigin + tGazeDirection * LengthOfRay);

        }
    }
}