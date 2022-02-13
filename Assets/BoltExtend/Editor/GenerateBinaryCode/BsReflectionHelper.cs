using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AutoBinary
{
    public static class BsReflectionHelper
    {
		private static BindingFlags DeclaredFlags =
			BindingFlags.NonPublic |
			BindingFlags.Public |
			BindingFlags.Instance |
			BindingFlags.Static |
			BindingFlags.DeclaredOnly;

		public static MethodInfo GetDeclaredMethod(this Type type, string methodName)
		{
			var methods = type.GetMethods(DeclaredFlags);

			for (var i = 0; i < methods.Length; ++i)
			{
				if (methods[i].Name == methodName)
				{
					return methods[i];
				}
			}

			return null;
		}

		public static MethodInfo GetFlattenedMethod(this Type type, string methodName)
		{
			while (type != null)
			{
				var methods = type.GetMethods(DeclaredFlags);

				for (var i = 0; i < methods.Length; ++i)
				{
					if (methods[i].Name == methodName)
					{
						return methods[i];
					}
				}

				type = type.BaseType;
			}

			return null;
		}
	}
}
