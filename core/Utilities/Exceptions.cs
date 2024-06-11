//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Cartheur.Animals.Utilities
{
    class PinAlreadyExportedException : Exception
    {
        public PinAlreadyExportedException()
            : base()
        { }

        public PinAlreadyExportedException(string message)
            : base(message)
        { }
    }
}
