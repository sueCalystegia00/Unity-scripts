using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class GetEyeDataModule : MonoBehaviour
    {
        private static EyeData_v2 eyeData = new EyeData_v2(); // VIVE PRO EYE から眼球データを受け取るモジュール
        private bool eye_callback_registered = false;         // 120Hzで計測するためのコールバック関数が利用可能か
        readonly static object DebugWriter = new object();      // Lockステートメント用オブジェクト
        public static int timeStamp;    // VIVE内部の時間(ミリ秒)
        public static float pupilDiameterLeft, pupilDiameterRight;  // 瞳孔径
        public static Vector3 gazeOriginLeft, gazeOriginRight, gazeOriginCombine;      // 空間における眼球位置
        public static Vector3 gazeDirectionLeft, gazeDirectionRight, gazeDirectionCombine;    // 視線ベクトル

        

        /// <summary>
        /// Use this for initialization
        /// </summary>
        void Start()
        {
            // 視線計測プログラムの状態確認
            if (!SRanipal_Eye_Framework.Instance.EnableEye) return;
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            // 視線計測の前にモジュールの状態を確認
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

            // コールバックを呼び出すためのおまじない
            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            {
                SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = true;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
        }

        /// <summary>
        /// It's called at 120 fps to get more accurate data
        /// </summary>
        private static void EyeCallback(ref EyeData_v2 eye_data)
        {
            // anipalモジュールから眼球データを取得
            eyeData = eye_data;

            // 時間を取得(ミリ秒)
            timeStamp = eyeData.timestamp;

            // 瞳孔径の値を取得(ミリメートル)
            pupilDiameterLeft = eyeData.verbose_data.left.pupil_diameter_mm;
            pupilDiameterRight = eyeData.verbose_data.right.pupil_diameter_mm;

            // 視線の原点とベクトルを取得(3次元座標，正規化ベクトル)
            //SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out gazeOriginLeft, out gazeDirectionLeft, eyeData);
            //SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out gazeOriginRight, out gazeDirectionRight, eyeData);
            SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out gazeOriginCombine, out gazeDirectionCombine, eyeData);


            /*
            // The point in the eye from which the gaze ray originates in meter miles.(right-handed coordinate system)
            gazeOriginLeft = eyeData.verbose_data.left.gaze_origin_mm;
            gazeOriginRight = eyeData.verbose_data.right.gaze_origin_mm;

            // The normalized gaze direction of the eye in [0,1].(right-handed coordinate system)
            gazeDirectionLeft = eyeData.verbose_data.left.gaze_direction_normalized;
            gazeDirectionRight = eyeData.verbose_data.right.gaze_direction_normalized;
            */

            // CSV書き出し用モジュールの呼び出し
            lock (DebugWriter)
            {
                CSVWriter.Write();
            }
        }

        /*
        /// <summary>
        /// Terminates an anipal module
        /// </summary>
        private void Release()
        {
          if (eye_callback_registered == true)
          {
              SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
              eye_callback_registered = false;
          }
        }
        */

    }
}