//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.Core.Lobes
{
    public class LobeProperties
    {
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public double Width
        {
            get;
            set;
        }

        public double Height
        {
            get;
            set;
        }

        public bool Overlap
        {
            get;
            set;
        }

        public LobeProperties()
        {
            Overlap = false;
        }

        public int NeuronsCount()
        {
            return X * Y;
        }
    }
}
