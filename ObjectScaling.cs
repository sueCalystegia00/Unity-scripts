using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class ObjectScaling : MonoBehaviour
    {
        public Vector3 MaximumScale = new Vector3(1, 1, 1);
        public Vector3 MinimumScale = new Vector3(.25f, .25f, .25f);

        void Update()
        {
            var offset = Mathf.Abs(Mathf.Sin(ViveTimer.preVTime/1000));
            transform.localScale = Vector3.Lerp(MinimumScale, MaximumScale, offset);
        }
    }

}