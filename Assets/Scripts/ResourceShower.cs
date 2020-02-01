using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceShower : MonoBehaviour
{
    public bool         emit = true;
    public float        radius = 10.0f;
    public float        rate = 5.0f;
    public float        trailTime = 0.1f;
    public float        trailWidth = 0.5f;
    public Color        color;
    public Material     material;
    public Transform    targetPos;
    public float        speed;
    public float        rotationSpeed = 1.0f;
    public float        zOffset = -1.0f;

    float accum;

    class Mote
    {
        public float           birthTime;
        public Vector3         targetPos;
        public Vector3         velocity;
        public TrailRenderer   trail;
    };

    List<Mote> motes;

    void Start()
    {
        accum = 0.0f;
        motes = new List<Mote>();
    }

    void Update()
    {
        if (emit)
        {
            accum += Time.deltaTime;

            int n = Mathf.FloorToInt(accum * rate);
            accum -= (n / rate);

            for (int i = 0; i < n; i++)
            {
                Vector2 pt = radius * Random.insideUnitCircle + transform.position.xy();

                SpawnParticle(pt, targetPos.position, color);
            }
        }

        foreach (var mote in motes)
        {
            Vector3 toTarget = mote.targetPos - mote.trail.transform.position;
            if ((Vector3.SqrMagnitude(toTarget) < 4.0f) || ((Time.unscaledTime - mote.birthTime) > 2.0f))
            {
                mote.trail.emitting = false;
                Destroy(mote.trail.gameObject, trailTime * 1.5f);
                mote.trail = null;
            }
            else
            {
                Vector3 v = mote.velocity;

                v = Vector3.RotateTowards(v, toTarget, Mathf.Deg2Rad * Time.deltaTime * rotationSpeed, speed);
                v = v.normalized * speed;
                mote.velocity = v;
                mote.trail.transform.position += v * Time.deltaTime;
            }
        }

        motes.RemoveAll((m) => m.trail == null);
    }

    void SpawnParticle(Vector2 pos, Vector2 targetPos, Color c)
    {
        Vector3 p = pos;
        p.z = transform.position.z + zOffset;

        Gradient g = new Gradient();
        g.FromColor(c);

        GameObject go = new GameObject();
        go.name = "ResourceMote";
        go.transform.position = p;

        TrailRenderer tr = go.AddComponent<TrailRenderer>();
        tr.material = material;
        tr.time = trailTime;
        tr.colorGradient = g;
        tr.widthMultiplier = trailWidth;

        Mote mote = new Mote();
        mote.birthTime = Time.unscaledTime;
        mote.targetPos = targetPos;
        mote.velocity = Random.insideUnitCircle.normalized * speed;
        mote.trail = tr;

        motes.Add(mote);
    }

    public void SpawnParticle(Color c)
    {
        Vector2 pt = radius * Random.insideUnitCircle + transform.position.xy();

        SpawnParticle(targetPos.position, pt, c);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
