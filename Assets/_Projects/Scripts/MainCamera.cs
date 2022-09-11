using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneWeekGamejam.Charge
{
    /// <summary>
    /// orthographicでカメラのサイズが変更されない事が前提
    /// </summary>
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] Camera _camera = null;
		Vector3[] _squarePositions = new Vector3[4];


        List<Vector3> _debugPos = new List<Vector3>();

		void Start()
		{
			var bottomLeft = _camera.ViewportToWorldPoint(Vector3.zero);
			var topRight = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));
			_squarePositions = new Vector3[]
			{
				bottomLeft,
				new Vector3(bottomLeft.x,topRight.y),
				topRight,
				new Vector3(topRight.x,bottomLeft.y)
			};
		}

		private void Update()
		{
			if (!Keyboard.current.spaceKey.wasPressedThisFrame) { return; }
            _debugPos.Clear();

            for(var i = 0; i < 300; i++)
			{
                _debugPos.Add(GetRandomPositionScreenOut(100.0f));
			}
		}

		private void OnDrawGizmos()
		{
            foreach (var p in _debugPos)
            {
                Gizmos.DrawSphere(p, 40.0f);
            }
		}

		public Vector3 GetRandomPositionScreenOut(float offsetRad)
		{
			var squarePos = GetSquarePos(offsetRad);
			var length = squarePos[2] - squarePos[0];
			var ran = Random.Range(0.0f, 2.0f);
			var startIdx = (ran % 1.0f <= length.x / (length.x + length.y)) ? 1 : 0;
			startIdx += (ran > 1.0f) ? 0 : 2;
			var nextIdx = (startIdx + 1) % 4;
			var p = Vector3.Lerp(squarePos[startIdx], squarePos[nextIdx], Random.value);
			p.z = 0.0f;
			return p;
		}

		public bool IsScreenOut(Vector3 pos, float offsetRad)
		{
			var squarePos = GetSquarePos(offsetRad);
			return
				pos.x < squarePos[0].x ||
				pos.y < squarePos[0].y ||
				pos.x > squarePos[2].x ||
				pos.y > squarePos[2].y;
		}

		Vector3[] GetSquarePos(float offsetRad)
		{
			var squarePos = (Vector3[])_squarePositions.Clone();
			squarePos[0] += new Vector3(-offsetRad, -offsetRad) + _camera.transform.position;
			squarePos[1] += new Vector3(-offsetRad, offsetRad) + _camera.transform.position;
			squarePos[2] += new Vector3(offsetRad, offsetRad) + _camera.transform.position;
			squarePos[3] += new Vector3(offsetRad, -offsetRad) + _camera.transform.position;
			return squarePos;
		}
	}
}