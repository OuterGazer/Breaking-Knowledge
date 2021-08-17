using UnityEngine;
using System.Collections;

public class DontGoThroughThings : MonoBehaviour
{
	// Careful when setting this to true - it might cause double
	// events to be fired - but it won't pass through the trigger
	public bool sendTriggerMessage = false;

	public LayerMask layerMask = -1; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 

	private float minimumExtent;
	private float partialExtent;
	private float sqrMinimumExtent;
	private Vector2 previousPosition;
	private Rigidbody2D myRigidbody2D;
	private Collider2D myCollider2D;

	//initialize values 
	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
		myCollider2D = GetComponent<Collider2D>();
		previousPosition = myRigidbody2D.position;
		minimumExtent = Mathf.Min(myCollider2D.bounds.extents.x, myCollider2D.bounds.extents.y);
		partialExtent = minimumExtent * (1.0f - skinWidth);
		sqrMinimumExtent = minimumExtent * minimumExtent;
	}

	void FixedUpdate()
	{
		//have we moved more than our minimum extent? 
		Vector2 movementThisStep = myRigidbody2D.position - previousPosition;
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;

		if (movementSqrMagnitude > sqrMinimumExtent)
		{
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			RaycastHit hitInfo;

			//check for obstructions we might have missed 
			if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
			{
				if (!hitInfo.collider)
					return;

				if (hitInfo.collider.isTrigger)
					hitInfo.collider.SendMessage("OnTriggerEnter", myCollider2D);

				if (!hitInfo.collider.isTrigger)
                {
					Vector2 hitInfoPoint = new Vector2(hitInfo.point.x, hitInfo.point.y);
					myRigidbody2D.position = hitInfoPoint - (movementThisStep / movementMagnitude) * partialExtent;
				}
			}
		}

		previousPosition = myRigidbody2D.position;
	}
}