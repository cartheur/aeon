//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Cartheur.Animals.Utilities
{
    /// <summary>
    /// A custom attribute to be applied to all custom tags in external late-binding libraries.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomTagAttribute : Attribute
    {
    }
}
