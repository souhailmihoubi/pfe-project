using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace player
{
    public class PlayerScript : MonoBehaviourPunCallbacks
    {
        [HideInInspector]
        public InputStr Input;
        public struct InputStr
        {
            public float LookX;
            public float LookZ;
            public float RunX;
            public float RunZ;
        }

        public float Speed = 10f;

        protected Rigidbody Rigidbody;
        protected Quaternion LookRotation;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
            var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);

            Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);
        }
    }
}


