using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct CardTransform{
    public Vector3 pos;
    public quaternion rot;

    public CardTransform(Vector3 vector3, quaternion rot){
        this.pos = vector3;
        this.rot = rot;
    }
}
