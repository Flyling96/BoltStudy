using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bolt;
using Ludiq;

namespace CustomBolt
{
    public enum SceneObjectType
    {
        None,
        Level,
        Actor,
        RecordPoint,
    }


    [Serializable]
    public abstract class SceneObject
    {
        public abstract SceneObjectType m_Type { get; }

        [SerializeField]
        protected GameObject m_GameObject;

        public Vector3 Position
        {
            get
            {
                return m_GameObject.transform.position;
            }
            set
            {
                m_GameObject.transform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return m_GameObject.transform.rotation;
            }
            set
            {
                m_GameObject.transform.rotation = value;
            }
        }

        public SceneObject(GameObject gameObject)
        {
            m_GameObject = gameObject;
        }

    }

    public class Level:SceneObject
    {
        public override SceneObjectType m_Type => SceneObjectType.Level;

        public Level(GameObject gameObject) : base(gameObject) { }

    }

    [Serializable]
    public class Actor:SceneObject
    {
        public override SceneObjectType m_Type => SceneObjectType.Actor;

        public Actor(GameObject gameObject) : base(gameObject)
        {
            Init();
        }

        public void Init()
        {
            m_HP = m_MaxHp;
        }

        [SerializeField]
        private int m_MaxHp = 300;
        [SerializeField]
        private int m_HP;

        public int HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                m_HP = value;
                if(m_HP < 0)
                {
                    OnDead();
                }
            }
        }

        public void OnDead()
        {
            //CustomEvent.Trigger();
        }
    }

    [Serializable]
    public class RecordPoint : SceneObject
    {
        public override SceneObjectType m_Type => SceneObjectType.RecordPoint;

        public RecordPoint(GameObject gameObject) : base(gameObject) { }

        [SerializeField]
        private Vector3 m_RebirthPoint;

        public Vector3 RebirthPoint
        {
            get
            {
                return m_RebirthPoint;
            }
            set
            {
                m_RebirthPoint = value;
            }
        }

        public void RebirthActor(Actor actor)
        {
            actor.Position = Position + m_RebirthPoint;
            actor.Init();
        }
    }


}
