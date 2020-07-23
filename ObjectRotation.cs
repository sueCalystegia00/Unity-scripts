using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace ViveSR.anipal.Eye
{
    public class ObjectRotation : MonoBehaviour
    {
        public Vector3 LengthAndDirection = new Vector3(5, 5, 0);

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(LengthAndDirection * ViveTimer.deltaVTime);
            
        }
    }

}
