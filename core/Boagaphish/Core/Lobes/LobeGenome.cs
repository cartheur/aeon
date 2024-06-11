//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using Boagaphish.Genetic;

namespace Boagaphish.Core.Lobes
{
    public class LobeGenome
    {
        protected int LobeId
        {
            get;
            set;
        }

        protected bool On
        {
            get;
            set;
        }

        public LobeGenome(int lobid, GeneHeader geneHeader)
        {
            LobeId = lobid;
            if (geneHeader == GeneHeader.Embryo)
            {
                On = true;
            }
        }
    }
}
