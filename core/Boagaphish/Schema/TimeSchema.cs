//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Collections.Generic;

namespace Boagaphish.Schema
{
    public class TimeSchema : BaseSchema
    {
        public new List<object> CellNames
        {
            get;
            set;
        }

        public override object[,] Grid
        {
            get;
            set;
        }

        public new int SchemaNumber
        {
            get;
            set;
        }

        public override string SchemaName
        {
            get;
            set;
        }

        public TimeSchema(string name, int width, int height, int x, int y, List<object> cellNames)
            : base(name, width, height, x, y, cellNames)
        {
            SchemaVector schemaVector = new SchemaVector(name, width, height, x, y, cellNames);
            Grid = schemaVector.CellGrid;
            CellListProperties();
            SchemaNumber = CellNames.Count;
            SchemaName = name;
        }

        public new List<object> CellListProperties()
        {
            CellNames = new List<object>();
            double[] array = new double[1];
            double[] array2 = array;
            foreach (double num in array2)
            {
                CellNames.Add(num);
            }
            return CellNames;
        }

        public new object GetCellDescription(int cell)
        {
            return CellNames[cell];
        }

        public new void ParseCellData()
        {
            int count = CellNames.Count;
            for (int i = 0; i < count; i++)
            {
            }
        }
    }
}
