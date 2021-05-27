using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class GrassPhysics : MonoBehaviour
{
    [SerializeField] private float bendForceOnExit = -0.1f;
    [SerializeField] private bool  windIsEnabled;
    [SerializeField] private float baseWindForce = 0f;
    [SerializeField] private float windPeriod = 0f;
    [SerializeField] private float windOffset;
    [SerializeField] private float windForceMultiplier = 0f;
    [SerializeField] private float bendFactor = 0.5f;


    private bool  isBending;
    private bool  isRebounding;

    private float exitOffset;
    private float enterOffset;


    private float colliderHalfWidth;

    [SerializeField] private Spring m_spring = new Spring();
    private Mesh meshCache;
    private Transform transformCache;
    private Collider2D colliderCache;

    private void Awake()
    {
        colliderCache = GetComponent<Collider2D>();
        colliderCache.isTrigger = true;
        colliderHalfWidth = colliderCache.bounds.size.x / 2f;

        transformCache = transform;
        meshCache = GetComponent<MeshFilter>().mesh;
    }

    private void OnDestroy()
    {
        Destroy(meshCache);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Player charMovement = col.GetComponent<Player>();
        if (charMovement != null)
        {
            enterOffset = col.transform.position.x - transform.position.x;

            if (col.GetComponent<Rigidbody2D>().velocity.y < -3f)
            {
                if (col.transform.position.x < transformCache.position.x)
                    m_spring.ApplyAdditiveForce(-bendForceOnExit);
                else
                    m_spring.ApplyAdditiveForce(bendForceOnExit);

                isRebounding = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponent<Player>() != null)
        {
            float offset = col.transform.position.x - transform.position.x;
            if (isBending || Mathf.Sign(enterOffset) != Mathf.Sign(offset))
            {
                isRebounding = false;
                isBending = true;

                float radius = colliderHalfWidth + colliderCache.bounds.size.x * 0.5f;
                exitOffset = Map(offset, -radius, radius, -1f, 1f);

                exitOffset *= 0.9f;

                SetHorizontalOffset(exitOffset);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<Player>() != null)
        {
            if (isBending)
            {
                m_spring.ApplyForceStartingAtPosition(bendForceOnExit * Mathf.Sign(exitOffset), exitOffset);
            }

            isBending = false;
            isRebounding = true;
        }
    }

    private void FixedUpdate()
    {

        if (windIsEnabled && !isBending)
        {
            var windForce = baseWindForce + Mathf.Pow(Mathf.Sin(Time.time * windPeriod + windOffset) * 0.7f + 0.05f, 4) * 0.05f * windForceMultiplier;
            m_spring.ApplyAdditiveForce(windForce);

            if (!isRebounding)
            {
                SetHorizontalOffset(m_spring.Simulate()); 
            }
        }
        if (isRebounding)
        {
            SetHorizontalOffset(m_spring.Simulate());

            if (Mathf.Abs(m_spring.Velocity) < 0.000005f)
            {
                SetHorizontalOffset(0f);
                isRebounding = false;
            }
        }
    }

    private void SetHorizontalOffset(float offset)
    {
        var verts = meshCache.vertices;
        verts[2].x = -0.5f + offset * bendFactor / transformCache.localScale.x;
        verts[3].x = 0.5f + offset * bendFactor / transformCache.localScale.x;

        meshCache.vertices = verts;
    }

    public static float Map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
}