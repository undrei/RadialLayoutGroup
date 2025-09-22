using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
[ExecuteAlways]
public class RadialMenuPointer : MaskableGraphic
{
	[SerializeField] 
	public int segments = 10;
	[SerializeField] 
	public float radius = 100f;
	[SerializeField] 
	public float angle = 90f;
	[SerializeField] 
	public float targetAngle = 0f;
	
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();

		Vector2 center = Vector2.zero;
		
		UIVertex centerVertex = UIVertex.simpleVert;
		centerVertex.color = color;
		centerVertex.position = center;
		vh.AddVert(centerVertex);
		
		float angleStep = angle / segments;
		float start = targetAngle - angle / 2f;
		float currentAngle = start;

		List<UIVertex> points = new List<UIVertex>();

		for (int i = 0; i <= segments; i++)
		{
			float rad = Mathf.Deg2Rad * currentAngle;
			Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

			UIVertex vertex = UIVertex.simpleVert;
			vertex.color = color;
			vertex.position = pos;
			vh.AddVert(vertex);

			currentAngle += angleStep;
		}
		
		for (int i = 1; i <= segments; i++)
		{
			vh.AddTriangle(0, i, i + 1);
		}
	}

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate();
		SetVerticesDirty();
	}
#endif
	
	public void SetPosition(float targetAngle)
	{
		this.targetAngle = targetAngle;
		SetVerticesDirty();
	}
}
