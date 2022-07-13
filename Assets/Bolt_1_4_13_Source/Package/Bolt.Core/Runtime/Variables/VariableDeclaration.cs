using System;
using Ludiq;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Bolt
{
	[SerializationVersion("A")]
	[Serializable]
	public sealed class VariableDeclaration
	{
		[Obsolete(Serialization.ConstructorWarning)]
		public VariableDeclaration() { }

		public VariableDeclaration(string name, object value)
		{
			this.name = name;
			this.value = value;	
		}

		[SerializeField]
		private string m_Name;

		[SerializeField]
		private string m_TypeFullName;

		private Type m_Type;

		private object m_Value;

		[Serialize]
		public string name 
		{ 
			get => m_Name;
			private set => m_Name = value; 
		}

		[Serialize]
		public object value
		{
			get => m_Value;
			set
			{
				m_Value = value;
			}
		}

		public Type type
        {
			get => m_Type;
			set
            {
				m_Type = value;
            }
        }

		[SerializeField]
		private byte[] m_Bytes = null;

		[SerializeField]
		private GameObject m_GameObject = null;

		[SerializeField]
		private List<GameObject> m_GameObjects = null;

		public void OnBeforeSerialize()
        {
			m_TypeFullName = string.Empty;
			if (m_Value == null)
			{
				if (m_Type == typeof(string))
				{
					m_Value = string.Empty;
				}
				else
				{
					if (m_Type != null)
					{
						m_TypeFullName = RuntimeCodebase.SerializeType(m_Type);
					}
					return;
				}
			}

			WriteObjectToBytes();
		}

		private void WriteObjectToBytes()
        {
			if (m_Value is GameObject gameObject)
			{
				m_Bytes = null;
				m_GameObjects = null;
				m_GameObject = gameObject;
			}
			else if (m_Value is List<GameObject> gameObjects)
			{
				m_Bytes = null;
				m_GameObject = null;
				m_GameObjects = gameObjects;
			}
			else
			{
				m_GameObject = null;
				m_GameObjects = null;

				using (var writer = new BinaryWriter(new MemoryStream()))
				{
					BinaryManager.Instance.SerializeObject(writer, m_Value);
					m_Bytes = new byte[(int)writer.BaseStream.Length];
					writer.BaseStream.Position = 0;
					writer.BaseStream.Read(m_Bytes, 0, (int)writer.BaseStream.Length);
				}
			}

		}

		public void OnAfterDeserialize()
        {
			if (m_Bytes != null && m_Bytes.Length > 0)
			{
				ReadBytesToObject();
			}
			else if (m_GameObject != null)
            {
				m_Value = m_GameObject;
            }
			else if (!string.IsNullOrEmpty(m_TypeFullName))
			{
				if(RuntimeCodebase.TryDeserializeType(m_TypeFullName,out var type))
                {
					m_Type = type;	
                }
			}
			else if(m_GameObjects != null)
            {
				m_Value = m_GameObjects;
            }
		}

		private void ReadBytesToObject()
        {
			using (var reader = new BinaryReader(new MemoryStream(m_Bytes)))
			{
				BinaryManager.Instance.DeserializeObject(reader,ref m_Value);
			}
		}

	}
}