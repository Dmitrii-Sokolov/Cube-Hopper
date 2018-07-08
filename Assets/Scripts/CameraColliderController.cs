using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider Collider;

    [SerializeField]
    private Camera MainCamera;

	void Start ()
    {
        Collider.size = new Vector3(2f * MainCamera.orthographicSize * MainCamera.aspect, 2f * MainCamera.orthographicSize, 200f);
	}
}
