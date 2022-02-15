//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bolt
{
	using UnityEngine;
	using Ludiq;
	
	
	public partial class CreateStruct
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			string typeName_0 = RuntimeCodebase.SerializeType(type);
			writer.Write(typeName_0);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			string typeName_0 = reader.ReadString();
			if (RuntimeCodebase.TryDeserializeType(typeName_0, out var _type_1))
			{
				type = _type_1;
			}
			else
			{
				Debug.LogError("Deserialize Fail type : " + typeName_0);
			}
		}
	}
	
	public partial class Expose
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			string typeName_0 = RuntimeCodebase.SerializeType(type);
			writer.Write(typeName_0);
			writer.Write(instance);
			writer.Write(@static);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			string typeName_0 = reader.ReadString();
			if (RuntimeCodebase.TryDeserializeType(typeName_0, out var _type_1))
			{
				type = _type_1;
			}
			else
			{
				Debug.LogError("Deserialize Fail type : " + typeName_0);
			}
			instance = reader.ReadBoolean();
			@static = reader.ReadBoolean();
		}
	}
	
	public partial class InvokeMember
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(chainable);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			chainable = reader.ReadBoolean();
		}
	}
	
	public partial class MemberUnit
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
		}
	}
	
	public partial class SetMember
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(chainable);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			chainable = reader.ReadBoolean();
		}
	}
	
	public partial class ForEach
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(dictionary);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			dictionary = reader.ReadBoolean();
		}
	}
	
	public partial class SelectOnEnum
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			string typeName_0 = RuntimeCodebase.SerializeType(enumType);
			writer.Write(typeName_0);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			string typeName_0 = reader.ReadString();
			if (RuntimeCodebase.TryDeserializeType(typeName_0, out var _type_1))
			{
				enumType = _type_1;
			}
			else
			{
				Debug.LogError("Deserialize Fail type : " + typeName_0);
			}
		}
	}
	
	public partial class SelectOnFlow
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(_branchCount);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			_branchCount = reader.ReadInt32();
		}
	}
	
	public partial class SelectOnInteger
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			int _count_0 = options.Count;
			writer.Write(_count_0);
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				writer.Write(options[i_1]);
			}
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			int _count_0 = 0;
			_count_0 = reader.ReadInt32();
			options = new System.Collections.Generic.List<int>();
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				int _temp_2;
				_temp_2 = reader.ReadInt32();
				options.Add(_temp_2);
			}
		}
	}
	
	public partial class SelectOnString
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(ignoreCase);
			int _count_0 = options.Count;
			writer.Write(_count_0);
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				writer.Write(options[i_1]);
			}
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			ignoreCase = reader.ReadBoolean();
			int _count_0 = 0;
			_count_0 = reader.ReadInt32();
			options = new System.Collections.Generic.List<string>();
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				string _temp_2;
				_temp_2 = reader.ReadString();
				options.Add(_temp_2);
			}
		}
	}
	
	public partial class Sequence
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(_outputCount);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			_outputCount = reader.ReadInt32();
		}
	}
	
	public partial class SwitchOnEnum
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			string typeName_0 = RuntimeCodebase.SerializeType(enumType);
			writer.Write(typeName_0);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			string typeName_0 = reader.ReadString();
			if (RuntimeCodebase.TryDeserializeType(typeName_0, out var _type_1))
			{
				enumType = _type_1;
			}
			else
			{
				Debug.LogError("Deserialize Fail type : " + typeName_0);
			}
		}
	}
	
	public partial class SwitchOnInteger
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			int _count_0 = options.Count;
			writer.Write(_count_0);
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				writer.Write(options[i_1]);
			}
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			int _count_0 = 0;
			_count_0 = reader.ReadInt32();
			options = new System.Collections.Generic.List<int>();
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				int _temp_2;
				_temp_2 = reader.ReadInt32();
				options.Add(_temp_2);
			}
		}
	}
	
	public partial class SwitchOnString
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(ignoreCase);
			int _count_0 = options.Count;
			writer.Write(_count_0);
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				writer.Write(options[i_1]);
			}
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			ignoreCase = reader.ReadBoolean();
			int _count_0 = 0;
			_count_0 = reader.ReadInt32();
			options = new System.Collections.Generic.List<string>();
			for (int i_1 = 0; (i_1 < _count_0); i_1 = (i_1 + 1))
			{
				string _temp_2;
				_temp_2 = reader.ReadString();
				options.Add(_temp_2);
			}
		}
	}
	
	public partial class Throw
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(custom);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			custom = reader.ReadBoolean();
		}
	}
	
	public partial class ToggleFlow
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(startOn);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			startOn = reader.ReadBoolean();
		}
	}
	
	public partial class ToggleValue
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(startOn);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			startOn = reader.ReadBoolean();
		}
	}
	
	public partial class TryCatch
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			string typeName_0 = RuntimeCodebase.SerializeType(exceptionType);
			writer.Write(typeName_0);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			string typeName_0 = reader.ReadString();
			if (RuntimeCodebase.TryDeserializeType(typeName_0, out var _type_1))
			{
				exceptionType = _type_1;
			}
			else
			{
				Debug.LogError("Deserialize Fail type : " + typeName_0);
			}
		}
	}
	
	public partial class CustomEvent
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(_argumentCount);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			_argumentCount = reader.ReadInt32();
		}
	}
	
	public partial class EventUnit<TArgs>
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(coroutine);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			coroutine = reader.ReadBoolean();
		}
	}
	
	public partial class TriggerCustomEvent
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(_argumentCount);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			_argumentCount = reader.ReadInt32();
		}
	}
	
	public partial class Formula
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(cacheArguments);
			writer.Write(_formula);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			cacheArguments = reader.ReadBoolean();
			_formula = reader.ReadString();
		}
	}
	
	public partial class Literal
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			string typeName_0 = RuntimeCodebase.SerializeType(type);
			writer.Write(typeName_0);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			string typeName_0 = reader.ReadString();
			if (RuntimeCodebase.TryDeserializeType(typeName_0, out var _type_1))
			{
				type = _type_1;
			}
			else
			{
				Debug.LogError("Deserialize Fail type : " + typeName_0);
			}
		}
	}
	
	public partial class BinaryComparisonUnit
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(numeric);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			numeric = reader.ReadBoolean();
		}
	}
	
	public partial class Comparison
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(numeric);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			numeric = reader.ReadBoolean();
		}
	}
	
	public partial class MoveTowards<T>
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(perSecond);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			perSecond = reader.ReadBoolean();
		}
	}
	
	public partial class ScalarRound
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(((int)(rounding)));
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			rounding = ((Bolt.Round<float, int>.Rounding)(reader.ReadInt32()));
		}
	}
	
	public partial class Vector2Round
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(((int)(rounding)));
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			rounding = ((Bolt.Round<UnityEngine.Vector2, UnityEngine.Vector2>.Rounding)(reader.ReadInt32()));
		}
	}
	
	public partial class Vector3Round
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(((int)(rounding)));
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			rounding = ((Bolt.Round<UnityEngine.Vector3, UnityEngine.Vector3>.Rounding)(reader.ReadInt32()));
		}
	}
	
	public partial class Vector4Round
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(((int)(rounding)));
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			rounding = ((Bolt.Round<UnityEngine.Vector4, UnityEngine.Vector4>.Rounding)(reader.ReadInt32()));
		}
	}
	
	public partial class WaitForFlow
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(resetOnExit);
			writer.Write(_inputCount);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			resetOnExit = reader.ReadBoolean();
			_inputCount = reader.ReadInt32();
		}
	}
	
	public partial class GetVariable
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(specifyFallback);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			specifyFallback = reader.ReadBoolean();
		}
	}
	
	public partial class UnifiedVariableUnit
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(((int)(kind)));
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			kind = ((Bolt.VariableKind)(reader.ReadInt32()));
		}
	}
	
	public partial class MultiInputUnit<T>
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(_inputCount);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			_inputCount = reader.ReadInt32();
		}
	}
	
	public partial class SuperUnit
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
		}
	}
}
namespace Bolt.Extend
{
	using UnityEngine;
	using Ludiq;
	
	
	public partial class CustomSuperUnit
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(VariableName);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			VariableName = reader.ReadString();
		}
	}
	
	public partial class FunctionSuperUnit
	{
		
		public override void BinarySerialize(System.IO.BinaryWriter writer)
		{
			base.BinarySerialize(writer);
			writer.Write(functionName);
		}
		
		public override void BinaryDeserialize(System.IO.BinaryReader reader)
		{
			base.BinaryDeserialize(reader);
			functionName = reader.ReadString();
		}
	}
}
