// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data.Common {
    internal static class GreenMethods {

        private const string ExtensionAssemblyRef = "System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=" + AssemblyRef.EcmaPublicKey;

        // For performance, we should convert these calls to using DynamicMethod with a Delegate, or 
        // even better, friend assemblies if its possible; so far there's only one of these per
        // AppDomain, so we're OK.

        //------------------------------------------------------------------------------
        // Access to the DbProviderServices type
        private const string SystemDataCommonDbProviderServices_TypeName = "System.Data.Common.DbProviderServices, " + ExtensionAssemblyRef;
        internal static Type SystemDataCommonDbProviderServices_Type = Type.GetType(SystemDataCommonDbProviderServices_TypeName, false);
        
        //------------------------------------------------------------------------------
        // Access to the SqlProviderServices class singleton instance;
        private const string SystemDataSqlClientSqlProviderServices_TypeName = "System.Data.SqlClient.SQLProviderServices, " + ExtensionAssemblyRef;
        private static FieldInfo SystemDataSqlClientSqlProviderServices_Instance_FieldInfo;
        
        internal static object SystemDataSqlClientSqlProviderServices_Instance() {
            if (null == SystemDataSqlClientSqlProviderServices_Instance_FieldInfo) {
                Type t = Type.GetType(SystemDataSqlClientSqlProviderServices_TypeName, false);

                if (null != t) {
                    SystemDataSqlClientSqlProviderServices_Instance_FieldInfo = t.GetField("Instance", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                }
            }
            object result = SystemDataSqlClientSqlProviderServices_Instance_GetValue();
            return result;
        }

        [System.Security.Permissions.ReflectionPermission(System.Security.Permissions.SecurityAction.Assert, MemberAccess=true)]
        private static object SystemDataSqlClientSqlProviderServices_Instance_GetValue() {
            object result = null;
            if (null != SystemDataSqlClientSqlProviderServices_Instance_FieldInfo) {
                result = SystemDataSqlClientSqlProviderServices_Instance_FieldInfo.GetValue(null);
            }
            return result;
        }

    }
}