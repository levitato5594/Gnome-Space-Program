using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MeshTest : MeshInstance3D
{
	[Export] float samplerMeshSize;
	[Export] QuadMesh samplerMesh;
	[Export] FastNoiseLite noise;
	Godot.Collections.Array meshData;
	Godot.Collections.Array largeMeshData;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		meshData = Mesh.SurfaceGetArrays(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		updateMesh();
	}

	private void updateMesh()
	{
		samplerMesh.Size = new Vector2(samplerMeshSize,samplerMeshSize);
		largeMeshData = samplerMesh.SurfaceGetArrays(0);
		Material meshMaterial = Mesh.SurfaceGetMaterial(0);

		Vector3[] vertices = (Vector3[])meshData[(int)Mesh.ArrayType.Vertex];
		int[] indices = (int[])meshData[(int)Mesh.ArrayType.Index];

		Vector3[] largeVertices = (Vector3[])largeMeshData[(int)Mesh.ArrayType.Vertex];
		int[] largeIndices = (int[])largeMeshData[(int)Mesh.ArrayType.Index];

		Color[] colors = new Color[vertices.Length];

		//int distFromEdge = 0;
		int meshSubdivision = 8;//((QuadMesh)Mesh).SubdivideDepth;

		//int row = 0;
		//int column = 0;
		for (int i = 0; i < vertices.Length; i++)
		{
			//if (column > distFromEdge-1 && column < meshSubdivision+2-distFromEdge && row > distFromEdge-1 && row < meshSubdivision+2-distFromEdge)
			//{
				Vector3 vertex = vertices[i]+GlobalPosition;

				// Cast to float for compatibility with this existing code
				float height = (float)noise.GetNoise3D(vertex.X,vertex.Y,vertex.Z)+0.5f;
				vertices[i] = vertices[i]+(height*Vector3.Back/2f);



				Color color = new Color(height,height,height);//Color.FromHsv(i/(float)vertices.Length,1,1,1);
				colors[i] = color;
				//GD.Print(color);
			//}
			//row++;
			//if (row>=meshSubdivision+2)
			//{
			//	row = 0;
			//	column++;
			//}
		}

		for (int i = 0; i < largeVertices.Length; i++)
		{
			Vector3 vertex = largeVertices[i]+GlobalPosition;

			// Cast to float for compatiblity with this existing code
			float height = (float)noise.GetNoise3D(vertex.X,vertex.Y,vertex.Z)+0.5f;
			largeVertices[i] = largeVertices[i]+(height*Vector3.Back/2f);
		}

		//GD.Print(vertices.Length);
		Vector3[] largeNormals = MeshManipulation.calculateSmoothNormals(largeVertices, largeIndices);
		//GD.Print(largeNormals.Length);
		Vector3[] normals = FilterCenterVector3s(largeNormals, 2, meshSubdivision+4);
		//GD.Print(normals.Length);

		Godot.Collections.Array newMeshData = meshData.Duplicate();

		newMeshData[(int)Mesh.ArrayType.Color] = colors;
		newMeshData[(int)Mesh.ArrayType.Normal] = normals;
		newMeshData[(int)Mesh.ArrayType.Vertex] = vertices;
		//GD.Print(meshData[(int)Mesh.ArrayType.Color]);
		ArrayMesh newmesh = new();
		newmesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, newMeshData);
		newmesh.SurfaceSetMaterial(0, meshMaterial);
		Mesh = newmesh;
	}

	private static Vector3[] FilterCenterVector3s(Vector3[] original, int distFromEdge, int meshSubdivision)
	{
		List<Vector3> newList = [];
		int row = 0;
		int column = 0;
		for (int i = 0; i < original.Length; i++)
		{
			if (column > distFromEdge-1 && column < meshSubdivision+2-distFromEdge && row > distFromEdge-1 && row < meshSubdivision+2-distFromEdge)
			{
				newList.Add(original[i]);
			}
			row++;
			if (row>=meshSubdivision+2)
			{
				row = 0;
				column++;
			}
		}
		// transfer
		Vector3[] mewArray = new Vector3[newList.Count];
		for (int i = 0; i < newList.Count; i++)
		{
			mewArray[i] = newList[i];
		}
		newList.Clear();
		return mewArray;
	}
}
