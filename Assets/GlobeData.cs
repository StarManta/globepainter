using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeData : MonoBehaviour {
	public static GlobeData main {
		get {
			return _main;
		}
	}
	private static GlobeData _main;
	void Awake() {
		_main = this;
	}

	public MeshFilter meshFilter {
		get {
			if (_meshFilter == null) {
				_meshFilter = GetComponent<MeshFilter>();
			}
			if (_meshFilter == null) {
				_meshFilter = gameObject.AddComponent<MeshFilter>();
			}
			return _meshFilter;
		}
	}
	private MeshFilter _meshFilter;
	public MeshCollider meshCollider {
		get {
			if (_meshCollider == null) _meshCollider = GetComponent<MeshCollider>();
			if (_meshCollider == null) _meshCollider = gameObject.AddComponent<MeshCollider>();
			return _meshCollider;
		}
	}
	private MeshCollider _meshCollider;
	public MeshRenderer meshRenderer {
		get {
			if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
			return _meshRenderer;
		}
	}
	private MeshRenderer _meshRenderer;

	public int segmentsPerFaceEdge = 2;

	[ContextMenu("Generate")]
	public void Generate() {
		// init mesh arrays
		int vertsPerFace = GlobeFace.GetVertCount(segmentsPerFaceEdge);
		int trisPerFace = GlobeFace.GetTriArraySize(segmentsPerFaceEdge);
		Vector3[] verts = new Vector3[vertsPerFace * 20];
		Vector2[] uv = new Vector2[vertsPerFace * 20];
		int[] tris = new int[trisPerFace * 20];

		GlobeFace.InitFaceArray();

		//init some useful constants
		float sin30 = Mathf.Sin(Mathf.PI / 6f);
		float sin60 = Mathf.Sin(Mathf.PI / 3f);
		float middleRadius = sin60;
		float middleHeight = sin30;
		float poleHeight = 1f;
		float radsPerX = 2f * Mathf.PI / 5f;

		//loop for each strip
		for (int x=0;x<5;x++)
		{
			int nextX = (x + 1) % 5;
			int prevX = (x + 4) % 5;
			GlobeFace southPoleFace = new GlobeFace(x * 4 + 0, x * radsPerX,
				new Vector3(0f, -poleHeight, 0f),	new Vector3(0f, -middleHeight, 0f),	new Vector3(1f, -middleHeight, 0f),
				new Vector2(sin30 + x, 0f),			new Vector2(0f+x, sin60),			new Vector2(1f+x, sin60),
				prevX * 4 + 0,						x * 4 + 1,							nextX * 4 + 0			);
			GlobeFace southMidFace  = new GlobeFace(x * 4 + 1, x * radsPerX,
				new Vector3(0f, -middleHeight, 0f),	new Vector3(0.5f, middleHeight, 0f),new Vector3(1f, -middleHeight, 0f),
				new Vector2(0f + x, sin60),			new Vector2(sin30 + x, 2f * sin60),	new Vector2(1f + x, sin60),
				prevX * 4 + 2,						x * 4 + 2,							x * 4 + 0				);
			GlobeFace northMidFace  = new GlobeFace(x * 4 + 2, x * radsPerX,
				new Vector3(1f, -middleHeight, 0f),	new Vector3(0.5f, middleHeight, 0f),new Vector3(1.5f, middleHeight, 0f),
				new Vector2(1f + x, sin60),			new Vector2(sin30 + x, 2f * sin60),	new Vector2(1f + sin30 + x, 2f * sin60),
				x * 4 + 1,						x * 4 + 3,							nextX * 4 + 1			);
			GlobeFace northPoleFace = new GlobeFace(x * 4 + 3, x * radsPerX,
				new Vector3(1.5f, middleHeight, 0f),		new Vector3(0.5f, middleHeight, 0f),new Vector3(0f, poleHeight, 0f),
				new Vector2(sin30 + 1f + x, 2f * sin60),	new Vector2(sin30 + x, 2f*sin60),	new Vector2(2f * sin30 + x, 3f*sin60),
				prevX * 4 + 2,						prevX * 4 + 3,							nextX * 4 + 3			);
		}

		for (int f=0;f<GlobeFace.allFaces.Length;f++) {
			int vertStart = f * vertsPerFace;
			int triStart = f * trisPerFace;
			GlobeFace.allFaces[f].Generate(segmentsPerFaceEdge, vertStart, triStart, verts, uv, tris);
			GlobeFace.allFaces[f].BuildNonconnectedNeighborsList();
		}

		Spherize(verts, 1f);

		if (meshFilter.sharedMesh == null) meshFilter.sharedMesh = new Mesh();
		meshFilter.sharedMesh.Clear();
		meshFilter.sharedMesh.vertices = verts;
		meshFilter.sharedMesh.uv = uv;
		meshFilter.sharedMesh.triangles = tris;
		meshFilter.sharedMesh.RecalculateNormals();
		meshFilter.sharedMesh.RecalculateBounds();
		meshFilter.sharedMesh.UploadMeshData(false);
		meshCollider.sharedMesh = meshFilter.sharedMesh;

	}

	void Spherize(Vector3[] verts, float radius) {
		for (int v=0;v<verts.Length;v++) {
			verts[v] = verts[v].normalized * radius;
		}
	}

	public Texture myTex;
	public void SetTexture(Texture tex) {
		meshRenderer.material.mainTexture = tex;
		myTex=tex;
	}

	public Vector3 GetWorldSpaceCoords(Vector2 uvCoords) {
		for (int f=0;f<GlobeFace.allFaces.Length;f++) {
			if (GlobeFace.allFaces[f].ContainsUVPoint(uvCoords)) {
				return transform.TransformPoint(GlobeFace.allFaces[f].GetLocalSpaceCoords(uvCoords));
			}
		}
		//return center of the world if not contained in any face
		return transform.position;
	}
}

public class GlobeFace {
	public static GlobeFace[] allFaces;
	public static void InitFaceArray() {
		allFaces = new GlobeFace[20];
	}
	public GlobeFace(int index, float radsToRotate,
		Vector3 p1, Vector3 p2, Vector3 p3, 
		Vector2 u1, Vector2 u2, Vector2 u3, 
		int n1, int n2, int n3) {

		this.index = index;
		if (allFaces == null) {
			throw new System.Exception("Face Array must be initialized before constructing faces!");
		}
		if (index < 0 || index >= allFaces.Length) {
			throw new System.IndexOutOfRangeException(string.Format("Index {0} is out of range of face array of size {1}", index, allFaces.Length) );
		}
		if (allFaces[index] != null) {
			Debug.LogWarningFormat("Face at index {0} is not null. We'll continue, but this is probably a bad sign.");
		}
		allFaces[index] = this;


		cornerVertices = new Vector3[]{p1, p2, p3};
		float radsPerX = 2f * Mathf.PI / 5f;
		for (int v=0;v<cornerVertices.Length;v++) {
			if (Mathf.Abs(cornerVertices[v].y) < 1f)
			{
				float radsAround = (cornerVertices[v].x) * radsPerX + radsToRotate;
				cornerVertices[v] = new Vector3(Mathf.Cos(radsAround),cornerVertices[v].y,Mathf.Sin(radsAround));
			}
		}
		Vector2 mapCoordSizeTotal = new Vector2(5f + Mathf.Sin(Mathf.PI/6f), 3f * Mathf.Sin(Mathf.PI/3f));
		Vector2 uvSizeMult = new Vector2(1f/mapCoordSizeTotal.x, 2f/mapCoordSizeTotal.x);
		cornerUV = new Vector2[]{Vector2.Scale(u1, uvSizeMult), Vector2.Scale(u2, uvSizeMult), Vector2.Scale(u3, uvSizeMult)};
		neighborIndices = new int[]{n1, n2, n3};
	}
	public int index;
	public Vector3[] cornerVertices;
	public Vector2[] cornerUV;
	public int[] neighborIndices;


	public static int GetTriArraySize(int segments) {
		return 3 * segments * segments;
	}
	public static int GetVertCount(int segments) {
		int total=1;
		for (int s=0;s<segments;s++) {
			total += s+2;
		}
		return total;
	}
	public void Generate(int segments, int vertStart, int triStart, Vector3[] verts, Vector2[] uv, int[] tris) {
		int thisTriStart = triStart;
		int thisVertStart = vertStart;
		Vector3 vertDelta0to1 = (cornerVertices[1] - cornerVertices[0]) / segments;
		Vector3 vertDelta1to2 = (cornerVertices[2] - cornerVertices[1]) / segments;
		Vector2 uvDelta0to1 = (cornerUV[1] - cornerUV[0]) / segments;
		Vector2 uvDelta1to2 = (cornerUV[2] - cornerUV[1]) / segments;
		//x=0 and y=0 at cornerVertices[0]
		//y=max and x=0 at cornervertices[1]
		// shape: 
		//  0
		//  Δ
		// 1 2
		int[] thisRowVertIndices = new int[segments+1];
		int[] lastRowVertIndices = new int[segments+1];
		for (int y=0;y<segments+1;y++) {
			int vertsAtY = y+1;
			for (int x=0;x<vertsAtY;x++) {
				verts[thisVertStart] = cornerVertices[0] + vertDelta0to1 * y + vertDelta1to2 * x;
				uv[thisVertStart] = cornerUV[0] + uvDelta0to1 * y + uvDelta1to2 * x;
				thisRowVertIndices[x] = thisVertStart;
				thisVertStart++;
			}

			//triangles
			int trisAtWidth = -1 + y * 2;
			bool isFatBottom = true;
			for (int x=0;x<trisAtWidth;x++) {
				if (isFatBottom) {
					tris[thisTriStart+0] = lastRowVertIndices[x/2];
					tris[thisTriStart+1] = thisRowVertIndices[x/2];
					tris[thisTriStart+2] = thisRowVertIndices[x/2+1];
				}
				else {
					tris[thisTriStart+0] = lastRowVertIndices[x/2+1];
					tris[thisTriStart+1] = lastRowVertIndices[x/2];
					tris[thisTriStart+2] = thisRowVertIndices[x/2+1];
				}
				isFatBottom = !isFatBottom;
				thisTriStart += 3;
			}

			//now move "this" to "last". and do so in such a way that doesn't make garbage
			int[] swap = lastRowVertIndices;
			lastRowVertIndices = thisRowVertIndices;
			thisRowVertIndices = swap;
		}
	}

	public bool ContainsUVPoint(Vector2 uv) {
		// Compute vectors        
		Vector2 v0 = cornerUV[2] - cornerUV[0];
		Vector2 v1 = cornerUV[1] - cornerUV[0];
		Vector2 v2 = uv - cornerUV[0];

			// Compute dot products
		float dot00 = Vector2.Dot(v0, v0);
		float dot01 = Vector2.Dot(v0, v1);
		float dot02 = Vector2.Dot(v0, v2);
		float dot11 = Vector2.Dot(v1, v1);
		float dot12 = Vector2.Dot(v1, v2);

			// Compute barycentric coordinates
		float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
		float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
		float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

			// Check if point is in triangle
		return (u >= 0) && (v >= 0) && (u + v < 1);
	}
	public Vector3 GetLocalSpaceCoords(Vector2 uv) {
		// Compute vectors        
		Vector2 v0 = cornerUV[2] - cornerUV[0];
		Vector2 v1 = cornerUV[1] - cornerUV[0];
		Vector2 v2 = uv - cornerUV[0];

		// Compute dot products
		float dot00 = Vector2.Dot(v0, v0);
		float dot01 = Vector2.Dot(v0, v1);
		float dot02 = Vector2.Dot(v0, v2);
		float dot11 = Vector2.Dot(v1, v1);
		float dot12 = Vector2.Dot(v1, v2);

		// Compute barycentric coordinates
		float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
		float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
		float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
		return u * v0 + v * v1;
	}

	private List<FaceConnection> nonconnectedNeighbors;
	public void BuildNonconnectedNeighborsList() {
		nonconnectedNeighbors = new List<FaceConnection>();
		for (int n=0;n<neighborIndices.Length;n++) {
			List<int> sharedMyVerts = new List<int>();
			List<int> sharedTheirVerts = new List<int>();
			int sharedUVCount = 0;
			for (int v1=0;v1<3;v1++) {
				for (int v2=0;v2<3;v2++) {
					if (cornerVertices[v1] == allFaces[neighborIndices[n]].cornerVertices[v2]) {
						sharedMyVerts.Add(v1);
						sharedMyVerts.Add(v2);
						if (cornerUV[v1] == allFaces[neighborIndices[n]].cornerUV[v2]) {
							sharedUVCount++;
						}
					}
				}
			}
			if (sharedUVCount < 2) {
				FaceConnection fc = new FaceConnection();
				fc.connectedFace = allFaces[neighborIndices[n]];
				fc.connectedFaceIndex = neighborIndices[n];

			}
		}
	}
}

public class FaceConnection
{
	public int connectedFaceIndex;
	public GlobeFace connectedFace;
	public Vector3 translateMyTriToMatch;
	public float rotateMyTriToMatch;
}

