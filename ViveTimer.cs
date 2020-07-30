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
        private bool isTesting = false;
        public static float preVTime = 0;
        public static float deltaVTime;

        // Start is called before the first frame update
        void Start()
        {
            if (GameObject.Find("SRanipal Eye Framework")) isTesting = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isTesting)
            {
                deltaVTime = ((float)GetEyeDataModule.timeStamp - preVTime)/1000;
                preVTime = (float)GetEyeDataModule.timeStamp;
            }
            else
            {
                deltaVTime = ((float)RecordSlider.time - preVTime) / 1000;
                preVTime = (float)RecordSlider.time;
            }
            
        }
    }
}
