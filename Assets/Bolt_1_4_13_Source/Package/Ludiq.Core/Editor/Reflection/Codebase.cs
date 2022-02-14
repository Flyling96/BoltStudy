using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Ludiq
{
	public static class Codebase
	{
		static Codebase()
		{
			using (ProfilingUtility.SampleBlock("Codebase initialization"))
			{
				_assemblies = new List<Assembly>();
				_runtimeAssemblies = new List<Assembly>();
				_editorAssemblies = new List<Assembly>();
				_ludiqAssemblies = new List<Assembly>();
				_ludiqRuntimeAssemblies = new List<Assembly>();
				_ludiqEditorAssemblies = new List<Assembly>();

				_types = new List<Type>();
				_runtimeTypes = new List<Type>();
				_editorTypes = new List<Type>();
				_ludiqTypes = new List<Type>();
				_ludiqRuntimeTypes = new List<Type>();
				_ludiqEditorTypes = new List<Type>();

				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					try
					{
#if NET_4_6
						if (assembly.IsDynamic)
						{
							continue;
						}
#endif

						_assemblies.Add(assembly);

						var isRuntimeAssembly = IsRuntimeAssembly(assembly);
						var isEditorAssembly = IsEditorDependentAssembly(assembly);
						var isLudiqAssembly = IsLudiqRuntimeDependentAssembly(assembly) || IsLudiqEditorDependentAssembly(assembly);
						var isLudiqEditorAssembly = IsLudiqEditorDependentAssembly(assembly);
						var isLudiqRuntimeAssembly = IsLudiqRuntimeDependentAssembly(assembly) && !IsLudiqEditorDependentAssembly(assembly);

						if (isRuntimeAssembly)
						{
							_runtimeAssemblies.Add(assembly);
						}

						if (isEditorAssembly)
						{
							_editorAssemblies.Add(assembly);
						}

						if (isLudiqAssembly)
						{
							_ludiqAssemblies.Add(assembly);
						}

						if (isLudiqEditorAssembly)
						{
							_ludiqEditorAssemblies.Add(assembly);
						}

						if (isLudiqRuntimeAssembly)
						{
							_ludiqRuntimeAssemblies.Add(assembly);
						}

						foreach (var type in assembly.GetTypesSafely())
						{
							_types.Add(type);

							RuntimeCodebase.PrewarmTypeDeserialization(type);

							if (isRuntimeAssembly)
							{
								_runtimeTypes.Add(type);
							}

							if (isEditorAssembly)
							{
								_editorTypes.Add(type);
							}

							if (isLudiqAssembly)
							{
								_ludiqTypes.Add(type);
							}

							if (isLudiqEditorAssembly)
							{
								_ludiqEditorTypes.Add(type);
							}

							if (isLudiqRuntimeAssembly || IsCustomType(assembly,type))
							{
								_ludiqRuntimeTypes.Add(type);
							}
						}
					}
					catch (Exception ex)
					{
						Debug.LogWarning($"Failed to analyze assembly '{assembly}':\n{ex}");
					}
				}

				assemblies = _assemblies.AsReadOnly();
				runtimeAssemblies = _runtimeAssemblies.AsReadOnly();
				editorAssemblies = _editorAssemblies.AsReadOnly();
				ludiqAssemblies = _ludiqAssemblies.AsReadOnly();
				ludiqRuntimeAssemblies = _ludiqRuntimeAssemblies.AsReadOnly();
				ludiqEditorAssemblies = _ludiqEditorAssemblies.AsReadOnly();

				types = _types.AsReadOnly();
				runtimeTypes = _runtimeTypes.AsReadOnly();
				editorTypes = _editorTypes.AsReadOnly();
				ludiqTypes = _ludiqTypes.AsReadOnly();
				ludiqRuntimeTypes = _ludiqRuntimeTypes.AsReadOnly();
				ludiqEditorTypes = _ludiqEditorTypes.AsReadOnly();
			}
		}

		//private static IEnumerable<string> CustomTypeNames
		//{
		//	get
  //          {
		//		yield return "ITransformFuncUnit";
		//		yield return "CustomSuperUnit";
		//		yield return "SceneObjectUnit";
		//	}
  //      }

		private static bool IsCustomType(Assembly assembly, Type type)
        {
			if(!IsRuntimeAssembly(assembly))
            {
				return false;
            }

			if(type.Namespace != "Bolt.Extend")
            {
				return false;
            }

			if(type.GetAttribute<CustomRutimeTypeAttribute>()!=null)
            {
				return true;
            }

			return false;
        }

		private static readonly List<Assembly> _assemblies;
		private static readonly List<Assembly> _runtimeAssemblies;
		private static readonly List<Assembly> _editorAssemblies;
		private static readonly List<Assembly> _ludiqAssemblies;
		private static readonly List<Assembly> _ludiqRuntimeAssemblies;
		private static readonly List<Assembly> _ludiqEditorAssemblies;
		private static readonly List<Type> _types;
		private static readonly List<Type> _runtimeTypes;
		private static readonly List<Type> _editorTypes;
		private static readonly List<Type> _ludiqTypes;
		private static readonly List<Type> _ludiqRuntimeTypes;
		private static readonly List<Type> _ludiqEditorTypes;
		private static List<Assembly> _settingsAssemblies;
		private static List<Type> _settingsAssembliesTypes;
		private static List<Type> _settingsTypes;
		
		#region Serialization
		
		public static string SerializeType(Type type)
		{
			return RuntimeCodebase.SerializeType(type);
		}

		public static bool TryDeserializeType(string typeName, out Type type)
		{
			return RuntimeCodebase.TryDeserializeType(typeName, out type);
		}

		public static Type DeserializeType(string typeName)
		{
			return RuntimeCodebase.DeserializeType(typeName);
		}
		
		private const char memberDataSeparator = ';';

		public static string SerializeMember(Member member)
		{
			// Format:  "targetType;name;paramType1;paramType2"
			// Example: "UnityEngine.Transform;rotate;UnityEngine.Vector3"

			var sb = new StringBuilder();
			sb.Append(SerializeType(member.targetType));
			sb.Append(memberDataSeparator);
			sb.Append(member.name);

			if (member.parameterTypes != null)
			{
				sb.Append(memberDataSeparator);

				for (int i = 0; i < member.parameterTypes.Length; i++)
				{
					sb.Append(SerializeType(member.parameterTypes[i]));

					if (i < member.parameterTypes.Length - 1)
					{
						sb.Append(memberDataSeparator);
					}
				}
			}

			return sb.ToString();
		}
		
		public static Member DeserializeMember(string memberData)
		{
			try
			{
				Ensure.That(nameof(memberData)).IsNotNullOrEmpty(memberData);

				var parts = memberData.Split(memberDataSeparator);

				if (parts.Length < 2)
				{
					throw new SerializationException("Malformed member data string.");
				}

				var targetType = DeserializeType(parts[0]);
				var name = parts[1];
				Type[] parameterTypes;

				if (parts.Length == 2)
				{
					parameterTypes = null;
				}
				else if (parts.Length == 3 && string.IsNullOrEmpty(parts[2]))
				{
					parameterTypes = Empty<Type>.array;
				}
				else
				{
					parameterTypes = new Type[parts.Length - 2];

					for (int i = 2; i < parts.Length; i++)
					{
						parameterTypes[i - 2] = DeserializeType(parts[i]);
					}
				}

				return new Member(targetType, name, parameterTypes);
			}
			catch (Exception ex)
			{
				throw new SerializationException($"Unable to find member: '{memberData}'.", ex);
			}
		}

		#endregion

		public static event Action settingsChanged;

		// NETUP: IReadOnlyCollection

		public static ReadOnlyCollection<Assembly> assemblies { get; private set; }

		public static ReadOnlyCollection<Assembly> runtimeAssemblies { get; private set; }

		public static ReadOnlyCollection<Assembly> editorAssemblies { get; private set; }

		public static ReadOnlyCollection<Assembly> ludiqAssemblies { get; private set; }

		public static ReadOnlyCollection<Assembly> ludiqRuntimeAssemblies { get; private set; }

		public static ReadOnlyCollection<Assembly> ludiqEditorAssemblies { get; private set; }

		public static ReadOnlyCollection<Assembly> settingsAssemblies { get; private set; }

		public static ReadOnlyCollection<Type> types { get; private set; }

		public static ReadOnlyCollection<Type> runtimeTypes { get; private set; }

		public static ReadOnlyCollection<Type> editorTypes { get; private set; }

		public static ReadOnlyCollection<Type> ludiqTypes { get; private set; }

		public static ReadOnlyCollection<Type> ludiqRuntimeTypes { get; private set; }

		public static ReadOnlyCollection<Type> ludiqEditorTypes { get; private set; }

		public static ReadOnlyCollection<Type> settingsAssembliesTypes { get; private set; }

		public static ReadOnlyCollection<Type> settingsTypes { get; private set; }

		public static ReadOnlyCollection<Type> GetTypeSet(TypeSet typeSet)
		{
			switch (typeSet)
			{
				case TypeSet.AllTypes:
					return types;
				case TypeSet.RuntimeTypes:
					return runtimeTypes;
				case TypeSet.SettingsTypes:
					return settingsTypes;
				case TypeSet.SettingsAssembliesTypes:
					return settingsAssembliesTypes;
				default:
					throw new UnexpectedEnumValueException<TypeSet>(typeSet);
			}
		}

		public static ReadOnlyCollection<Type> GetTypeSetFromAttribute(IAttributeProvider attributeProvider, TypeSet fallback = TypeSet.SettingsTypes)
		{
			return GetTypeSet(attributeProvider.GetAttribute<TypeSetAttribute>()?.typeSet ?? fallback);
		}

		private static bool IsEditorAssembly(AssemblyName assemblyName)
		{
			var name = assemblyName.Name;

			return
				name == "Assembly-CSharp-Editor" ||
				name == "Assembly-CSharp-Editor-firstpass" ||
				name == "UnityEditor" ||
				name == "UnityEditor.CoreModule";
		}

		private static bool IsUserAssembly(AssemblyName assemblyName)
		{
			var name = assemblyName.Name;
			
			return
				name == "Assembly-CSharp" ||
				name == "Assembly-CSharp-firstpass";
		}

		public static bool IsUserAssembly(Assembly assembly)
		{
			return IsUserAssembly(assembly.GetName());
		}

		private static bool IsEditorAssembly(Assembly assembly)
		{
			if (Attribute.IsDefined(assembly, typeof(AssemblyIsEditorAssembly)))
			{
				return true;
			}

			return IsEditorAssembly(assembly.GetName());
		}

		private static bool IsRuntimeAssembly(Assembly assembly)
		{
			// User assemblies refer to the editor when they include
			// a using UnityEditor / #if UNITY_EDITOR, but they should still
			// be considered runtime.
			return IsUserAssembly(assembly) || !IsEditorDependentAssembly(assembly);
		}

		private static bool IsEditorDependentAssembly(Assembly assembly)
		{
			if (IsEditorAssembly(assembly))
			{
				return true;
			}

			foreach (var dependency in assembly.GetReferencedAssemblies())
			{
				if (IsEditorAssembly(dependency))
				{
					return true;
				}
			}

			return false;
		}
		
		private static bool IsLudiqRuntimeDependentAssembly(Assembly assembly)
		{
			if (assembly.GetName().Name == "Ludiq.Core.Runtime")
			{
				return true;
			}

			foreach (var dependency in assembly.GetReferencedAssemblies())
			{
				if (dependency.Name == "Ludiq.Core.Runtime")
				{
					return true;
				}
			}

			return false;
		}

		private static bool IsLudiqEditorDependentAssembly(Assembly assembly)
		{
			if (assembly.GetName().Name == "Ludiq.Core.Editor")
			{
				return true;
			}

			foreach (var dependency in assembly.GetReferencedAssemblies())
			{
				if (dependency.Name == "Ludiq.Core.Editor")
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsEditorType(Type type)
		{
			var rootNamespace = type.RootNamespace();

			return IsEditorAssembly(type.Assembly) ||
				   rootNamespace == "UnityEditor" ||
				   rootNamespace == "UnityEditorInternal";
		}

		public static bool IsInternalType(Type type)
		{
			var rootNamespace = type.RootNamespace();

			return rootNamespace == "UnityEngineInternal" ||
				   rootNamespace == "UnityEditorInternal";
		}

		public static bool IsRuntimeType(Type type)
		{
			return !IsEditorType(type) && !IsInternalType(type);
		}

		private static string RootNamespace(this Type type)
		{
			return type.Namespace?.PartBefore('.');
		}

		public static void UpdateSettings()
		{
			using (ProfilingUtility.SampleBlock("Codebase settings update"))
			{
				_settingsAssemblies = new List<Assembly>();
				_settingsAssembliesTypes = new List<Type>();
				_settingsTypes = new List<Type>();

				foreach (var assembly in _assemblies)
				{
					var couldHaveIncludeInSettingsAttribute = ludiqAssemblies.Contains(assembly);

					// It's important not to provide types outside the settings assemblies,
					// because only those assemblies will be added to the linker to preserve stripping.
					if (IncludeInSettings(assembly))
					{
						_settingsAssemblies.Add(assembly);

						foreach (var type in assembly.GetTypesSafely())
						{
							// Apparently void can be returned somehow:
							// http://support.ludiq.io/topics/483
							if (type == typeof(void))
							{
								continue;
							}

							_settingsAssembliesTypes.Add(type);

							// For optimization, we bypass [IncludeInSettings] for assemblies
							// that could logically never have it.
							if (IncludeInSettings(type, couldHaveIncludeInSettingsAttribute))
							{
								_settingsTypes.Add(type);
							}
						}
					}
				}

				settingsAssemblies = _settingsAssemblies.AsReadOnly();
				settingsAssembliesTypes = _settingsAssembliesTypes.AsReadOnly();
				settingsTypes = _settingsTypes.AsReadOnly();

				settingsChanged?.Invoke();
			}
		}

		private static bool IncludeInSettings(Assembly a)
		{
			return LudiqCore.Configuration.assemblyOptions.Any(assemblyName => assemblyName == a.GetName().Name);
		}
		
		private static bool IncludeInSettings(Type t, bool couldHaveAttribute)
		{
			// User-defined settings types
			// TODO OPTIM with temporary hash set. Contains is O(n)
			if (LudiqCore.Configuration.typeOptions.Contains(t))
			{
				return true;
			}

            if(t.Name == "SceneObject") {
                return true;
            }
			
			var attributeInclude = couldHaveAttribute ? t.GetAttribute<IncludeInSettingsAttribute>()?.include : null;

			// Include non-runtime, non-internal enum and class deriving from UnityObject
			if ((attributeInclude ?? true) && (t.IsEnum || typeof(UnityObject).IsAssignableFrom(t)))
			{
				return !IsEditorType(t) && !IsInternalType(t);
			}

			return attributeInclude ?? false;
		}

		public static CodebaseSubset Subset(IEnumerable<Type> types, MemberFilter memberFilter, TypeFilter memberTypeFilter = null)
		{
			return CodebaseSubset.Get(types, memberFilter, memberTypeFilter);
		}

		public static CodebaseSubset Subset(IEnumerable<Type> typeSet, TypeFilter typeFilter, MemberFilter memberFilter, TypeFilter memberTypeFilter = null)
		{
			return CodebaseSubset.Get(typeSet, typeFilter, memberFilter, memberTypeFilter);
		}
	}
}