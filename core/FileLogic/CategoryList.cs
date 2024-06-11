//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections;
using System.Data;

namespace Cartheur.Animals.FileLogic
{
    /// <summary>
    /// The category list storage object.
    /// </summary>
	public class CategoryList
	{
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
        /// <returns></returns>
		public static Category GetCategory(string categoryID)
		{
			DataRow dataRow = XmlCategory.Select(categoryID);
			Category result = null;
			if (dataRow != null)
			{
				Category category = new Category();
				category.CategoryID = ((dataRow[0] != DBNull.Value) ? dataRow[0].ToString() : string.Empty);
				category.CategoryName = ((dataRow[1] != DBNull.Value) ? dataRow[1].ToString() : string.Empty);
				result = category;
			}
			return result;
		}
        /// <summary>
        /// Gets the category list.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
		public static IList GetCategoryList(string filepath)
		{
			return XmlCategory.SelectAll(filepath);
		}
        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="cat">The cat.</param>
		public static void UpdateCategory(Category cat)
		{
			XmlCategory.Update(cat.CategoryID, cat.CategoryName);
		}
        /// <summary>
        /// Inserts the category.
        /// </summary>
        /// <param name="cat">The cat.</param>
		public static void InsertCategory(Category cat)
		{
			XmlCategory.Insert(cat.CategoryID, cat.CategoryName);
		}
        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="categoryID">The category identifier.</param>
		public static void DeleteCategory(string categoryID)
		{
			XmlCategory.Delete(categoryID);
		}
	}
}
