//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Runtime.InteropServices;

namespace Boagaphish.Core.Animals
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Cell
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct Coordinate
        {
            public static int X0
            {
                get;
                set;
            }

            public static int Y0
            {
                get;
                set;
            }

            public static int X1
            {
                get;
                set;
            }

            public static int Y1
            {
                get;
                set;
            }

            public static int T1
            {
                get;
                set;
            }

            public static int T2
            {
                get;
                set;
            }
        }
    }
}
