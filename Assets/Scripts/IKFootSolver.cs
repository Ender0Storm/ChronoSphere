using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField]
    private Transform ikBoneTransform;
    [SerializeField]
    private Rigidbody referenceRB;
    [SerializeField]
    private List<IKFootSolver> otherLegsScripts;

    [Header("Walk parameters")]
    [SerializeField]
    private float distanceToMove;
    [SerializeField]
    private float maxDistanceBeforeMove;
    [SerializeField]
    private float walkHeight;
    
    [Header("Animation Adjustements")]
    [SerializeField]
    [Range(0, 0.99f)]
    private float startOffset;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float maxHeight;

    [Header("Layers")]
    [SerializeField]
    private LayerMask terrainLayer;

    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    
    private float lerp;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = ikBoneTransform.position + (Vector3.right * startOffset * maxDistanceBeforeMove);
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Looks for a new foothold if we walk too far from it's current position
        if (Vector3.Distance(targetPosition, ikBoneTransform.position) > maxDistanceBeforeMove && CanWalk())
        {
            lerp = 0;
            oldPosition = newPosition;
            Vector3 direction = referenceRB.velocity.sqrMagnitude > 0 ? referenceRB.velocity.normalized : (ikBoneTransform.position - targetPosition).normalized;
            targetPosition = direction * distanceToMove + ikBoneTransform.position;
        }

        // Raycast to see if there is ground and find a point on the ground if there is
        Ray ray = new(targetPosition + (Vector3.up * maxHeight/2), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, maxHeight, terrainLayer.value))
        {
            if (info.point != null)
            {
                newPosition = info.point;
            }
        }

        // Lerp for leg position during a walk animation
        if (lerp < 1)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * walkHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * walkSpeed * referenceRB.velocity.magnitude;
        }
        else
        {
            currentPosition = newPosition;
        }

        transform.position = currentPosition;
    }

    private bool CanWalk()
    {
        // Checks if other foots are currently moving
        foreach (IKFootSolver legScript in otherLegsScripts)
        {
            if (legScript.Walking()) return false; 
        }
        return true;
    }

    public bool Walking()
    {
        return lerp < 1;
    }
}
