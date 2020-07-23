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

    public class GetEyeDataModule : MonoBehaviour
    {
        private static EyeData_v2 eyeData = new EyeData_v2(); // The Moudle of getting eyedata measured by VIVE PRO EYE 
        private bool eye_callback_registered = false;         // To use the callback function called by 120Hz

        private static int initTime = 0;
        public static int timeStamp;
        public static float pupilDiameterLeft, pupilDiameterRight;
        public static Vector3 gazeOriginLeft, gazeOriginRight;
        public static Vector3 gazeDirectionLeft, gazeDirectionRight;

        readonly static object DebugWriter = new object();

        /// <summary>
        /// Use this for initialization
        /// </summary>
        void Start()
        {
            // Whether to enable anipal's Eye module
            if (!SRanipal_Eye_Framework.Instance.EnableEye) return;
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            // check the status of the anipal engine before getting eye data
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

            // the spells to use a callback function to get the measurement data at 120fps
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
            // Gets data from anipal's Eye module
            eyeData = eye_data;

            // The time when the frame was capturing. in millisecond.
            timeStamp = eyeData.timestamp;

            // The diameter of the pupil in milli meter
            pupilDiameterLeft = eyeData.verbose_data.left.pupil_diameter_mm;
            pupilDiameterRight = eyeData.verbose_data.right.pupil_diameter_mm;

            SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out gazeOriginLeft, out gazeDirectionLeft, eyeData);
            SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out gazeOriginRight, out gazeDirectionRight, eyeData);

            /*
            // The point in the eye from which the gaze ray originates in meter miles.(right-handed coordinate system)
            gazeOriginLeft = eyeData.verbose_data.left.gaze_origin_mm;
            gazeOriginRight = eyeData.verbose_data.right.gaze_origin_mm;

            // The normalized gaze direction of the eye in [0,1].(right-handed coordinate system)
            gazeDirectionLeft = eyeData.verbose_data.left.gaze_direction_normalized;
            gazeDirectionRight = eyeData.verbose_data.right.gaze_direction_normalized;
            */

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