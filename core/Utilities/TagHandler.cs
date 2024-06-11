//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cartheur.Animals.Utilities
{
    /// <summary>
    /// Encapsulates information about a custom tag class.
    /// </summary>
    public class TagHandler
    {
        /// <summary>
        /// The assembly this class is found in.
        /// </summary>
        public string AssemblyName;
        /// <summary>
        /// The class name for the assembly.
        /// </summary>
        public string ClassName;
        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string TagName;
        /// <summary>
        /// Provides an instantiation of the class represented by this tag handler.
        /// </summary>
        /// <param name="assemblies">All the assemblies the bot knows about.</param>
        /// <returns>The instantiated class.</returns>
        public AeonTagHandler Instantiate(Dictionary<string, Assembly> assemblies)
        {
            if (assemblies.ContainsKey(AssemblyName))
            {
                Assembly tagDll = assemblies[AssemblyName]; 
                Type[] tagDllTypes = tagDll.GetTypes();
                return (AeonTagHandler)tagDll.CreateInstance(ClassName);
            }
            return null;
        }
    }
}
