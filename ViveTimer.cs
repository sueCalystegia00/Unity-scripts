using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.IO;

namespace ViveSR.anipal.Eye
{
    public class ViveTimer : MonoBehaviour
    {
        private bool isTesting = false;     // 実験中かプレイバック中か
        public static float preVTime = 0;   // 1フレーム前のタイムスタンプ
        public static float deltaVTime;     // フレーム間のタイムスタンプの差

        // Start is called before the first frame update
        void Start()
        {
            if (GameObject.Find("SRanipal Eye Framework")) isTesting = true;    // 実験中かプレイバック中か判定
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isTesting)  // 実験中
            {
                deltaVTime = ((float)GetEyeDataModule.timeStamp - preVTime)/1000;
                preVTime = (float)GetEyeDataModule.timeStamp;
            }
            else    // プレイバック中
            {
                deltaVTime = ((float)RecordSlider.time - preVTime) / 1000;
                preVTime = (float)RecordSlider.time;
            }

            //Debug.Log(deltaVTime);
        }
    }
}
