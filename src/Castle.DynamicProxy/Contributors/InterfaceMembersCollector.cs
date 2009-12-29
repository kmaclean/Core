﻿// Copyright 2004-2009 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.DynamicProxy.Contributors
{
	using System;
	using System.Reflection;
	using Castle.DynamicProxy.Generators;

	public class InterfaceMembersCollector : MembersCollector
	{
		public InterfaceMembersCollector(Type @interface)
			: base(@interface)
		{

		}

		protected override MethodToGenerate GetMethodToGenerate(MethodInfo method, IProxyGenerationHook hook, bool isStandalone)
		{
			if (!IsAccessible(method))
			{
				return null;
			}

			var proxyable = AcceptMethod(method, false, hook);
			return new MethodToGenerate(method, method, isStandalone, proxyable, false);
		}

	}
}