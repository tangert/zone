
///// <summary>
///// Hour particle.
///// </summary>
///// 

//public class HourParticle: ParticleBase
//{

//    public float timeMultiplier = 1 / 3600;
//    public HourParticle(GameObject proto, Vector3 pos) : base(proto, pos)
//    {

//    }

//    override public void move(DynamicContext dyn, float gravity)
//    {
//        //gravityDirection = dyn.vi;
//        Vector3 acceleration = gravityDirection * gravity;
//        velocity += acceleration * dyn.timeStep * timeMultiplier + Vector3.up * (Random.value * 2.0f - 1.0f);
//        position += velocity * dyn.timeStep;
//        this.setPosition(position);
//    }
//}

///// <summary>
///// Minute particle.
///// </summary>
//public class MinuteParticle : ParticleBase
//{
//    public float timeMultiplier = 1 / 60;

//    public MinuteParticle(GameObject proto, Vector3 pos) : base(proto, pos)
//    {

//    }

//    override public void move(DynamicContext dyn, float gravity)
//    {
//        //gravityDirection = dyn.viewerFront;
//        Vector3 acceleration = gravityDirection * gravity;
//        velocity += acceleration * dyn.timeStep * timeMultiplier + Vector3.up * (Random.value * 2.0f - 1.0f);
//        position += velocity * dyn.timeStep;
//        this.setPosition(position);
//    }
//}

///// <summary>
///// Second particle.
///// </summary>
//public class SecondParticle : ParticleBase
//{
//    public float timeMultiplier = 1;

//    public SecondParticle(GameObject proto, Vector3 pos) : base(proto, pos)
//    {

//    }

//    override public void move(DynamicContext dyn, float gravity)
//    {
//        //gravityDirection = dyn.viewerFront;
//        Vector3 acceleration = gravityDirection * gravity;
//        velocity += acceleration * dyn.timeStep * timeMultiplier + Vector3.up * (Random.value * 2.0f - 1.0f);
//        position += velocity * dyn.timeStep;
//        this.setPosition(position);
//    }
//}


//// Pan's types
///// <summary>
///// Particle rain type.
///// </summary>
//public class ParticleRainType : ParticleBase {

//    public ParticleRainType(GameObject proto, Vector3 pos):base(proto, pos)
//    {
        
//    }
//}


//public class ParticleJumpy : ParticleBase
//{
//    public  float randomness = 0.1f;

//    public ParticleJumpy(GameObject proto, Vector3 pos) : base(proto, pos)
//    {
//       gravityDirection = Vector3.right;
//        //obj.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
//    }

    

//   override public void move(DynamicContext dyn, float gravity)
//    {
//        gravityDirection = dyn.viewerFront;
//        Vector3 acceleration = gravityDirection * gravity;
//        velocity += acceleration * dyn.timeStep + Vector3.up * (Random.value*2.0f-1.0f)*randomness;
//        position += velocity * dyn.timeStep;
//        this.setPosition(position);
//    }
//}