//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Collections.Generic;

namespace Boagaphish.Schema
{
    public class NextSchema : BaseSchema
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

        public NextSchema(string name, int width, int height, int x, int y, List<object> cellNames)
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
            string[] array = new string[81]
            {
                "Activate 1 it (push)",
                "Activate 2 it (pull)",
                "Deactivate it (stop)",
                "Approach it (come)",
                "Retreat from it (run)",
                "Get it (get)",
                "Drop all (drop)",
                "Say what you need (think)",
                "Rest (sleep)",
                "Travel west (left)",
                "Travel east (right)",
                "Verb12",
                "Verb13",
                "Verb14",
                "Verb15",
                "I've been patted",
                "I've been hit",
                "I've bumped into a wall",
                "I am near a wall",
                "I am in a vehicle",
                "User has spoken",
                "Own kind has spoken",
                "Audible event",
                "Visible event",
                "It is approaching",
                "It is retreating",
                "It is near me",
                "It is active",
                "It is an object",
                "sense20",
                "sense21",
                "sense22",
                "sense23",
                "sense24",
                "sense25",
                "sense26",
                "sense27",
                "sense28",
                "sense29",
                "sense30",
                "sense31",
                "Myself",
                "Hand",
                "Call",
                "Water",
                "Plant",
                "Egg",
                "Food",
                "Drink",
                "Vendor",
                "Music",
                "Animal",
                "Fire",
                "Shower",
                "Toy",
                "Bigtoy",
                "Weed",
                "Word32",
                "Word33",
                "Word34",
                "Word35",
                "Word36",
                "Word37",
                "Word38",
                "Word39",
                "Word40",
                "Word41",
                "Mover",
                "Lift",
                "Computer",
                "Fun",
                "Bang",
                "Word47",
                "Word48",
                "Word49",
                "Word50",
                "Word51",
                "Me",
                "Grendle",
                "Word54",
                "Word55"
            };
            string[] array2 = array;
            foreach (string item in array2)
            {
                CellNames.Add(item);
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
