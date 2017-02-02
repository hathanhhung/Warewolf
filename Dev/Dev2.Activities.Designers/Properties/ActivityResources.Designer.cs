/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2017 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/


//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Dev2.Activities.Properties {
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode()]
    [CompilerGenerated()]
    internal class ActivityResources {
        
        private static ResourceManager resourceMan;
        
        private static CultureInfo resourceCulture;
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ActivityResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("Dev2.Activities.Properties.ActivityResources", typeof(ActivityResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one input mapping must be provided..
        /// </summary>
        internal static string AtLeastOneMappingMsg {
            get {
                return ResourceManager.GetString("AtLeastOneMappingMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Batch size must be a number greater than or equal to zero..
        /// </summary>
        internal static string BatchsizeMustBeNumberMsg {
            get {
                return ResourceManager.GetString("BatchsizeMustBeNumberMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A database must be selected..
        /// </summary>
        internal static string DatabaseMustBeSelectedMsg {
            get {
                return ResourceManager.GetString("DatabaseMustBeSelectedMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is an IDENTITY. You may not enter a mapping when the Keep Identity option is disabled..
        /// </summary>
        internal static string IdentityWithKeepOptionDisabledMsg {
            get {
                return ResourceManager.GetString("IdentityWithKeepOptionDisabledMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is an IDENTITY. You must enter a mapping when the Keep Identity option is enabled..
        /// </summary>
        internal static string IdentityWithKeepOptionEnabledMsg {
            get {
                return ResourceManager.GetString("IdentityWithKeepOptionEnabledMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} does not allow NULL..
        /// </summary>
        internal static string NotNullableMsg {
            get {
                return ResourceManager.GetString("NotNullableMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A table must be selected..
        /// </summary>
        internal static string TableMustBeSelectedMsg {
            get {
                return ResourceManager.GetString("TableMustBeSelectedMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timeout must be a number greater than or equal to zero..
        /// </summary>
        internal static string TimeoutMustBeNumberMsg {
            get {
                return ResourceManager.GetString("TimeoutMustBeNumberMsg", resourceCulture);
            }
        }
    }
}