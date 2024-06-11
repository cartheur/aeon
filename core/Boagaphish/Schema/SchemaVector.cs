//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Collections.Generic;

namespace Boagaphish.Schema
{
    public class SchemaVector
    {
        public object[,] CellGrid
        {
            get;
            set;
        }

        public SchemaVector(string name, int width, int height, int x, int y, List<object> cellNames)
        {
            object[,] array = new object[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    foreach (object cellName in cellNames)
                    {
                        array[x + j, y + i] = cellName;
                    }
                }
            }
            CellGrid = array;
        }

        public SchemaVector(BaseSchema baseSchema)
        {
        }
    }
}
