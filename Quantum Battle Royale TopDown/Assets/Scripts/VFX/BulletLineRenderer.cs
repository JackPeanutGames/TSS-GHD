using UnityEngine;

public class BulletLineRenderer : MonoBehaviour
{
	public float Lenght = 1;

	private LineRenderer _lineRenderer;
	private Vector3 _lastPos;
	
	private void Start()
	{
    _lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.SetPosition(0, transform.position);
		_lineRenderer.SetPosition(1, transform.position);
		_lastPos = transform.position;
	}

	private void Update()
	{
		var direction = Vector3.Normalize(transform.position - _lastPos);

		_lineRenderer.SetPosition(0, transform.position + direction / Lenght);
		_lineRenderer.SetPosition(1, transform.position + direction / Lenght - direction * Lenght);

		_lastPos = transform.position;
	}
}