﻿// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
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

#if !SILVERLIGHT
namespace Castle.Components.DictionaryAdapter.Xml
{
	using System;
	using System.Collections.Generic;
	using System.Xml;
	using System.Xml.Serialization;
	using System.Xml.XPath;

	public static class XmlExtensions
	{
		public static XPathNavigator CreateNavigatorSafe(this IXPathNavigable source)
		{
            if (source == null)
                throw new ArgumentNullException("source");

			return source.CreateNavigator();
		}

		public static bool HasNameLike(this XPathNavigator node, string localName)
		{
			return Comparer.Equals(localName, node.LocalName);
		}

		public static bool HasNameLike(this XPathNavigator node, string localName, string namespaceUri)
		{
			return Comparer.Equals(localName, node.LocalName)
				&& (namespaceUri == null || Comparer.Equals(namespaceUri, node.NamespaceURI));
		}

		public static bool HasXsiType(this XPathNavigator node, string type)
		{
			return type == node.GetXsiType();
		}

		public static string GetXsiType(this XPathNavigator node)
		{
			if (!node.MoveToAttribute("type", XsiNamespaceUri))
				return null;

			var value = node.Value;
			node.MoveToParent();
			return value == string.Empty ? null : value;
		}

		public static void SetXsiType(this XPathNavigator node, string type)
		{
			node.CreateAttribute(XsiPrefix, "type", XsiNamespaceUri, type);
		}

		public static XmlMetadata GetXmlMeta(this DictionaryAdapterMeta meta)
		{
			return (XmlMetadata) meta.ExtendedProperties[XmlMetaKey];
		}

		public static void SetXmlMeta(this DictionaryAdapterMeta meta, XmlMetadata xmlMeta)
		{
			meta.ExtendedProperties[XmlMetaKey] = xmlMeta;
		}

		public static bool HasXmlMeta(this DictionaryAdapterMeta meta)
		{
			return meta.ExtendedProperties.Contains(XmlMetaKey);
		}

		public static XmlAccessor GetAccessor(this PropertyDescriptor property)
		{
			return (XmlAccessor) property.ExtendedProperties[XmlAccessorKey];
		}

		public static void SetAccessor(this PropertyDescriptor property, XmlAccessor accessor)
		{
			property.ExtendedProperties[XmlAccessorKey] = accessor;
		}

		public static bool HasAccessor(this PropertyDescriptor property)
		{
			return property.ExtendedProperties.Contains(XmlAccessorKey);
		}

		public static string GetLocalName(this Type type)
		{
			string name;
			if (XsdTypes.TryGetValue(type, out name))
				return name;

			name = type.Name;
			return type.IsInterface && name.IsInterfaceName()
				? name.Substring(1)
				: name;
		}

		internal static bool IsSimpleType(this Type type)
		{
			return XsdTypes.ContainsKey(type);
		}

		internal static bool IsCustomSerializable(this Type type)
		{
			return typeof(IXmlSerializable).IsAssignableFrom(type);
		}

		private static bool IsInterfaceName(this string name)
		{
			return name.Length > 1
				&& name[0] == 'I'
				&& char.IsUpper(name, 1);
		}

		private const string
			XmlAccessorKey = "XmlAccessor",
			XmlMetaKey     = "XmlMeta";

		private static readonly StringComparer
			Comparer = StringComparer.OrdinalIgnoreCase;

		public const string
			XmlnsPrefix       = "xmlns",
			XmlnsNamespaceUri = "http://www.w3.org/2000/xmlns/",
			XsiPrefix         = "xsi",
			XsiNamespaceUri   = "http://www.w3.org/2001/XMLSchema-instance",
			WsdlPrefix        = "wsdl", // For Guid
			WsdlNamespaceUri  = "http://microsoft.com/wsdl/types/"; // For Guid

		internal static readonly Dictionary<Type, string>
			XsdTypes = new Dictionary<Type,string>
		{
			{ typeof(object),           "anyType"       },
			{ typeof(string),           "string"        },
			{ typeof(bool),             "boolean"       },
			{ typeof(sbyte),            "byte"          },
			{ typeof(byte),             "unsignedByte"  },
			{ typeof(short),            "short"         },
			{ typeof(ushort),           "unsignedShort" },
			{ typeof(int),              "int"           },
			{ typeof(uint),             "unsignedInt"   },
			{ typeof(long),             "long"          },
			{ typeof(ulong),            "unsignedLong"  },
			{ typeof(float),            "float"         },
			{ typeof(double),           "double"        },
			{ typeof(decimal),          "decimal"       },
			{ typeof(Guid),             "guid"          },
			{ typeof(DateTime),         "dateTime"      },
			{ typeof(TimeSpan),         "duration"      },
			{ typeof(byte[]),           "base64Binary"  },
			{ typeof(Uri),              "anyURI"        },
			{ typeof(XmlQualifiedName), "QName"         }
		};
	}
}
#endif