//
// Copyright 2003 - 2025, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact m.e. at: cartheur@pm.me.
//
using System.Reflection;

namespace Aeon.Library;

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
    /// <param name="assemblies">All the assemblies the presence knows about.</param>
    /// <returns>The instantiated class.</returns>
    public AeonHandler Instantiate(Dictionary<string, Assembly> assemblies)
    {
        if (assemblies.ContainsKey(AssemblyName))
        {
            Assembly tagDll = assemblies[AssemblyName];
            Type[] tagDllTypes = tagDll.GetTypes();
            return (AeonHandler)tagDll.CreateInstance(ClassName);
        }
        return null;
    }
}
