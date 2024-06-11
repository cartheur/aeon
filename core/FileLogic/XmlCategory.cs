//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Data;

namespace Cartheur.Animals.FileLogic
{
    /// <summary>
    /// The xml category storage object.
    /// </summary>
	public static class XmlCategory
	{
		private static readonly DataSet ReadonlyDataSet = new DataSet();
		private static DataView _dataView = new DataView();
        private static string _filepath;
        /// <summary>
        /// Saves the specified filepath.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
		public static void Save(string filepath)
		{
            _filepath = filepath;
			ReadonlyDataSet.WriteXml(filepath, XmlWriteMode.WriteSchema);
		}
        /// <summary>
        /// Inserts the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <param name="categoryName">Name of the category.</param>
		public static void Insert(string categoryID, string categoryName)
		{
			DataRow dataRow = _dataView.Table.NewRow();
			dataRow[0] = categoryID;
			dataRow[1] = categoryName;
			_dataView.Table.Rows.Add(dataRow);
			Save(_filepath);
		}
        /// <summary>
        /// Updates the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <param name="categoryName">Name of the category.</param>
		public static void Update(string categoryID, string categoryName)
		{
			DataRow dataRow = Select(categoryID);
			dataRow[1] = categoryName;
			Save(_filepath);
		}
        /// <summary>
        /// Deletes the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
		public static void Delete(string categoryID)
		{
			_dataView.RowFilter = "categoryID='" + categoryID + "'";
			_dataView.Sort = "categoryID";
			_dataView.Delete(0);
			_dataView.RowFilter = "";
			Save(_filepath);
		}
        /// <summary>
        /// Selects the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <returns></returns>
		public static DataRow Select(string categoryID)
		{
			_dataView.RowFilter = "categoryID='" + categoryID + "'";
			_dataView.Sort = "categoryID";
			DataRow result = null;
			if (_dataView.Count > 0)
			{
				result = _dataView[0].Row;
			}
			_dataView.RowFilter = "";
			return result;
		}
        /// <summary>
        /// Selects all data views.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
		public static DataView SelectAll(string filepath)
		{
            _filepath = filepath;
            ReadonlyDataSet.Clear();
			ReadonlyDataSet.ReadXml(filepath, XmlReadMode.ReadSchema);
			_dataView = ReadonlyDataSet.Tables[0].DefaultView;
			return _dataView;
		}
	}
}
