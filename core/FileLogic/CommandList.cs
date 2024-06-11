//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections;
using System.Data;

namespace Cartheur.Animals.FileLogic
{
    /// <summary>
    /// The command list storage object.
    /// </summary>
	public class CommandList
	{
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <returns></returns>
		public static Command GetCommand(string commandID)
		{
			DataRow dataRow = XmlCommand.Select(commandID);
			Command result = null;
			if (dataRow != null)
			{
				Command command = new Command();
				command.CommandID = ((dataRow[0] != DBNull.Value) ? dataRow[0].ToString() : string.Empty);
				command.CommandName = ((dataRow[1] != DBNull.Value) ? dataRow[1].ToString() : string.Empty);
				result = command;
			}
			return result;
		}
        /// <summary>
        /// Gets the command list.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
		public static IList GetCommandList(string filepath)
		{
			return XmlCommand.SelectAll(filepath, XmsFileType.Command);
		}
        /// <summary>
        /// Updates the command.
        /// </summary>
        /// <param name="command">The command.</param>
		public static void UpdateCommand(Command command)
		{
			XmlCategory.Update(command.CommandID, command.CommandName);
		}
        /// <summary>
        /// Inserts the command.
        /// </summary>
        /// <param name="command">The command.</param>
		public static void InsertCommand(Command command)
		{
			XmlCategory.Insert(command.CommandID, command.CommandName);
		}
        /// <summary>
        /// Deletes the command.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
		public static void DeleteCommand(string commandID)
		{
			XmlCategory.Delete(commandID);
		}
	}
}
