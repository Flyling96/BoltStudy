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

		//[SerializeField]
		private byte[] m_Bytes = null;

		[SerializeField]
		[DoNotSerialize]
		private string m_BytesString = null;

		[SerializeField]
		[DoNotSerialize]
		private List<UnityEngine.Object> m_UnityObjects = new List<UnityEngine.Object>();

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
			using (var writer = new BinaryWriter(new MemoryStream()))
			{
				m_UnityObjects.Clear();
				BinaryManager.Instance.SerializeObject(writer, m_Value, m_UnityObjects);
				m_Bytes = new byte[(int)writer.BaseStream.Length];
				writer.BaseStream.Position = 0;
				writer.BaseStream.Read(m_Bytes, 0, (int)writer.BaseStream.Length);
				m_BytesString = System.Convert.ToBase64String(m_Bytes);
			}
		}

		public void OnAfterDeserialize()
        {
			if (!string.IsNullOrEmpty(m_BytesString))
			{
				ReadBytesToObject();
			}
			else if (!string.IsNullOrEmpty(m_TypeFullName))
			{
				if(RuntimeCodebase.TryDeserializeType(m_TypeFullName,out var type))
                {
					m_Type = type;	
                }
			}
		}

		private void ReadBytesToObject()
        {
			try
            {
				m_Bytes = System.Convert.FromBase64String(m_BytesString);
            }
			catch(Exception e)
            {
				Debug.LogError(e.ToString());
            }

			using (var reader = new BinaryReader(new MemoryStream(m_Bytes)))
			{
				if (m_UnityObjects != null && m_UnityObjects.Count > 0)
				{
					BinaryManager.Instance.DeserializeObject(reader, ref m_Value, m_UnityObjects);
				}
				else
				{
					BinaryManager.Instance.DeserializeObject(reader, ref m_Value);
				}
			}
		}

	}
}