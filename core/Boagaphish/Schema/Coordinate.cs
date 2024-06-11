//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Runtime.InteropServices;

namespace Boagaphish.Schema
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Coordinate
    {
        public static int X
        {
            get;
            set;
        }

        public static int Y
        {
            get;
            set;
        }
    }
}
