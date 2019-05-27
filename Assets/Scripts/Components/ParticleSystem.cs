using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem
{
    public List<ParticleBase> particles = new List<ParticleBase>();
    public DynamicContext context = new DynamicContext();

    public ParticleSystem(float size)
    {
        context.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(size, size, size));
    }

    public void move(float gravity)
    {
        foreach (ParticleBase p in particles)
        {
            if (!context.bounds.Contains(p.position))
            {
                p.reset(context);
            }
            p.move(context, gravity);

        }
    }

    // MARK: Helper functions
    public Vector3 getRandomPosition()
    {
        float x = Random.Range(context.bounds.min.x, context.bounds.max.x);
        float y = Random.Range(context.bounds.min.y, context.bounds.max.y);
        float z = Random.Range(context.bounds.min.z, context.bounds.max.z);
        Vector3 pos = new Vector3(x, y, z);
        return pos;
    }


    // Populate time affected
    public void populateTimeType(int count, TimeType type, GameObject proto)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = getRandomPosition();
            TimeParticle p = new TimeParticle(type, proto, pos);
            p.setPosition(pos);
            particles.Add(p);
        }
    }
}