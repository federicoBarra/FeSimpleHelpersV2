using UnityEngine;

namespace FeSimpleHelpers.ProcGen
{
	[RequireComponent(typeof(MeshFilter))]
	public class ConeMeshGeneration : MonoBehaviour
	{
		public float angle = 45.0f;
		public float radius = 5.0f;
		public int segments = 360;

		private MeshFilter meshFilter;
		private Mesh coneMesh;

		void Awake()
		{
			meshFilter = GetComponent<MeshFilter>();
			coneMesh = new Mesh();
			meshFilter.mesh = coneMesh;
			GenerateConeMesh();
		}

		public void Regenerate(float _angle, float _radius, int _segments)
		{
			angle = _angle;

			if (_radius > 0)
				radius = _radius;

			if (_segments > 0)
				segments = _segments;

			GenerateConeMesh();
		}

		void GenerateConeMesh()
		{
			coneMesh.Clear();

			Vector3[] vertices = new Vector3[segments + 2];
			Vector2[] uv = new Vector2[vertices.Length];
			int[] triangles = new int[segments * 3];

			float startAngle = 90 + -(angle / 2);

			vertices[0] = Vector3.zero; // Apex of the cone
			uv[0] = new Vector2(0.5f, 0);

			for (int i = 0; i <= segments; i++)
			{
				float t = i / (float)segments;
				float theta = startAngle + angle * t;
				theta *= Mathf.Deg2Rad;
				float x = radius * Mathf.Cos(theta);
				float y = radius * Mathf.Sin(theta);
				vertices[i + 1] = new Vector3(x, 0, y);
				uv[i + 1] = new Vector2(t, 1);

				if (i < segments)
				{
					triangles[i * 3] = 0;
					triangles[i * 3 + 1] = i + 2;
					triangles[i * 3 + 2] = i + 1;
				}
			}

			coneMesh.vertices = vertices;
			coneMesh.uv = uv;
			coneMesh.triangles = triangles;

			coneMesh.RecalculateNormals();
			coneMesh.RecalculateBounds();
		}
	}
}