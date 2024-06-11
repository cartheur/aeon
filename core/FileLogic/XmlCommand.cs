//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using Cartheur.Animals.Utilities;
using System;
using System.Data;

namespace Cartheur.Animals.FileLogic
{
    /// <summary>
    /// The xms file type.
    /// </summary>
    public enum XmsFileType
    {
        /// <summary>
        /// The category file type.
        /// </summary>
        Category,
        /// <summary>
        /// The command file type.
        /// </summary>
        Command
    }

    /// <summary>
    /// The xml command storage object.
    /// </summary>
	public static class XmlCommand
	{
		private static readonly DataSet ReadonlyDataSet = new DataSet();
		private static DataView _dataView = new DataView();
        private static string _filepath;
        static string _filename;
        /// <summary>
        /// Saves the specified file at the path.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="fileType">Type of the file.</param>
        public static void Save(string filepath, XmsFileType fileType)
		{
            switch (fileType)
            {
                case XmsFileType.Category:
                    _filename = @"\Category.xml";
                    break;
                case XmsFileType.Command:
                    _filename = @"\Command.xml";
                    break;
            }
            _filepath = filepath + _filename;
			ReadonlyDataSet.WriteXml(filepath, XmlWriteMode.WriteSchema);
		}
        /// <summary>
        /// Inserts the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="fileType">Type of the file.</param>
		public static void Insert(string categoryID, string categoryName, XmsFileType fileType)
		{
			DataRow dataRow = _dataView.Table.NewRow();
			dataRow[0] = categoryID;
			dataRow[1] = categoryName;
			_dataView.Table.Rows.Add(dataRow);
			Save(_filepath, fileType);
		}
        /// <summary>
        /// Updates the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="fileType">Type of the file.</param>
		public static void Update(string categoryID, string categoryName, XmsFileType fileType)
		{
			DataRow dataRow = Select(categoryID);
			dataRow[1] = categoryName;
			Save(_filepath, fileType);
		}
        /// <summary>
        /// Deletes the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <param name="fileType">Type of the file.</param>
		public static void Delete(string categoryID, XmsFileType fileType)
		{
			_dataView.RowFilter = "commandID='" + categoryID + "'";
			_dataView.Sort = "commandID";
			_dataView.Delete(0);
			_dataView.RowFilter = "";
			Save(_filepath, fileType);
		}
        /// <summary>
        /// Selects the specified category identifier.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <returns></returns>
		public static DataRow Select(string categoryID)
		{
			_dataView.RowFilter = "commandID='" + categoryID + "'";
			_dataView.Sort = "commandID";
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
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
		public static DataView SelectAll(string filepath, XmsFileType fileType)
		{
            switch (fileType)
            {
                case XmsFileType.Category:
                    _filename = @"\Category.xml";
                    break;
                case XmsFileType.Command:
                    _filename = @"\Command.xml";
                    break;
            }
            _filepath = filepath + _filename;
            ReadonlyDataSet.Clear();
            try
            {
                ReadonlyDataSet.ReadXml(_filepath, XmlReadMode.ReadSchema);
            }
			catch(Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.Xms);
            }
			_dataView = ReadonlyDataSet.Tables[0].DefaultView;
			return _dataView;
		}
	}
}
