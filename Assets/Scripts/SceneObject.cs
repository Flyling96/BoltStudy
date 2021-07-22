using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bolt;
using Ludiq;

namespace DragonSlay
{
    public interface ISceneObjectInterface { }

    public interface ITransform : ISceneObjectInterface
    {
        void Transform(Vector3 pos, Quaternion rot);
    }

    public interface IArea: ISceneObjectInterface
    {
        Area Area { get; }
    }


    public struct Area
    {
        public Vector3 m_Center;
        public float m_Radius;
    }


    public enum SceneObjectType
    {
        None,
        Level,
        Actor,
        RecordPoint,
    }

    public abstract class SceneObject: ITransform
    {
        public static ulong Current = 0;

        public abstract SceneObjectType m_Type { get; }

        private ulong m_Uid;

        public ulong Uid => m_Uid;

        private int m_Tid;

        public int Tid => m_Tid;

        public virtual void OnCreate(SceneObjectData data,Transform parent = null)
        {
            m_GameObject = new GameObject();
            m_GameObject.transform.SetParent(parent);
            m_Uid = (ulong)m_Type * 1000000 + Current++;
            m_GameObject.name = string.Format("{0}_{1}",m_Type.DisplayName(), m_Uid);
            m_Tid = data.m_Tid;
            Position = data.m_Position;
            Rotation = data.m_Rotation;
        }

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


        public void Transform(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }

    public class Level:SceneObject
    {
        public override SceneObjectType m_Type => SceneObjectType.Level;


    }

    public class Actor:SceneObject
    {
        public override SceneObjectType m_Type => SceneObjectType.Actor;


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

    public class RecordPoint : SceneObject
    {
        public override SceneObjectType m_Type => SceneObjectType.RecordPoint;

        public override void OnCreate(SceneObjectData data, Transform parent = null)
        {
            base.OnCreate(data, parent);
            RecordPointData recordPointData = data as RecordPointData;
            if(recordPointData != null)
            {
                m_RebirthPointOffset = recordPointData.m_RebirthPointOffset;
            }
        }

        private Vector3 m_RebirthPointOffset;

        public Vector3 RebirthPointOffset
        {
            get
            {
                return m_RebirthPointOffset;
            }
            set
            {
                m_RebirthPointOffset = value;
            }
        }

        public void RebirthActor(Actor actor)
        {
            actor.Position = Position + m_RebirthPointOffset;
            actor.Init();
        }
    }


}
