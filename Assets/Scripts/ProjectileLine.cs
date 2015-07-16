using UnityEngine;
using System.Collections;


using System.Collections.Generic;

public class ProjectileLine : MonoBehaviour {

	public static ProjectileLine S; 


	public float minDist = 0.1f;
	private LineRenderer line;
	private GameObject _poi;
	private Vector3 lastPoint;
	private int pointsCount;

	void Awake() {
		S = this; 

		line = GetComponent<LineRenderer>();

		Color c1 = Color.white;
		Color c2 = Color.blue;
		line.SetColors(c1,c2);
		pointsCount = 0;

		line.enabled = false;
	}


	public GameObject poi {
		get {
			return _poi;
		}
		set {
			_poi = value;
			if(_poi != null) {

				line.enabled = false;
				pointsCount = 0;
				line.SetVertexCount(0);
			}
		}
	}

	void FixedUpdate() {
		if(poi == null) {

			if(FollowCam.S.poi != null) {
				if(FollowCam.S.poi.tag == "Projectile") {
					poi = FollowCam.S.poi;
				} else {
					return; 
				}
			} else {
				return; 
			}
		}


		AddPoint();
		if(poi.GetComponent<Rigidbody>().IsSleeping()){

			poi = null;
		}
	}

	public void AddPoint(){
		Vector3 pt = _poi.transform.position;

		if(pointsCount > 0 && (pt - lastPoint).magnitude < minDist) {
			return;
		}

		if(pointsCount == 0){

			line.SetVertexCount(1);
			line.SetPosition(0, pt);

			pointsCount += 1;
			line.enabled = true;
		} else {

			pointsCount++;
			line.SetVertexCount(pointsCount);
			line.SetPosition(pointsCount - 1, pt);
		}

		lastPoint = pt;
	}

	public void Clear(){
		_poi = null;
		line.enabled = false;
		pointsCount = 0;
		line.SetVertexCount(0);
	}
}
