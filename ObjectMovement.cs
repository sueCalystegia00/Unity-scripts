using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.IO;

namespace ViveSR.anipal.Eye
{
    public class ObjectMovement : MonoBehaviour
    {
        public Vector3 LengthAndDirection = new Vector3(5, 0, 0);

        private Vector3 _startPosition;

        void Start()
        {
            _startPosition = transform.position;
        }

        void Update()
        {
            var offset = Mathf.Sin(ViveTimer.preVTime / 1000);
            transform.position = _startPosition + LengthAndDirection * offset;
        }
    }
}