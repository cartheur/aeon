//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Collections.Generic;

namespace Boagaphish.Schema
{
    public class BaseSchema
    {
        public virtual List<object> CellNames
        {
            get;
            set;
        }

        public virtual object[,] Grid
        {
            get;
            set;
        }

        public virtual string SchemaName
        {
            get;
            set;
        }

        public virtual int SchemaNumber
        {
            get;
            set;
        }

        public BaseSchema(string name, int width, int height, int x, int y, List<object> cellNames)
        {
            Coordinate.X = x;
            Coordinate.Y = y;
            SchemaVector schemaVector = new SchemaVector(name, width, height, x, y, cellNames);
            Grid = schemaVector.CellGrid;
            CellListProperties();
            SchemaNumber = CellNames.Count;
            SchemaName = name;
        }

        public virtual List<object> CellListProperties()
        {
            CellNames = new List<object>();
            string[] array = new string[1]
            {
                "blank"
            };
            string[] array2 = array;
            foreach (string item in array2)
            {
                CellNames.Add(item);
            }
            return CellNames;
        }

        public virtual object GetCellDescription(int cell)
        {
            return CellNames[cell];
        }

        public virtual void ParseCellData()
        {
            int count = CellNames.Count;
            for (int i = 0; i < count; i++)
            {
            }
        }
    }
}
