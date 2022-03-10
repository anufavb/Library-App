using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;


public class databasetest : MonoBehaviour
{
	// If not found on android will create Tables and database



	private static string dbPath;
	private Text nameField, varNameField, varOtherField;
	public LibraryManager libraryManager;
	//string[] empty = { "JJ", "Sam B", "Anth M" };
	public Text outText;

    private void Awake()
    {
		dbPath = "URI=file:" + Application.persistentDataPath + "/PersonalLibDB.db";
	}

    private void Start()
	{
		//dbPath = "URI=file:" + Application.persistentDataPath + "/PersonalLibDB.db";
		
		///dbPath = "jar:file://" + Application.persistentDataPath + "/PersonalLibDB.db";
		///
		///Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
		///                     Application.dataPath + "!/assets/Employers");
		///
		///
		///
		///    // #UNITY_ANDROID
		///    WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/Employers.s3db");
		///    while (!loadDB.isDone) { }
		///    // then save to Application.persistentDataPath
		///    File.WriteAllBytes(filepath, loadDB.bytes);
		///
		/////dbPath = "URI=file:" + Application.dataPath + "/exampleDatabase.db";
		//PrintDatabasePath();
		CreateSchema();
		//Debug.Log("1!");

        /*for (int i=0; i<25; i++)
        {
			InsertBook("GB McClellan", "not my book", "", 212, "Science", empty);
			InsertBook("GG Meade", "my book", "1st", 902, "Maths", empty);
			InsertBook("AS Damean", "your book", "1st", 902, "", empty);
        }*/
		
		//DeleteSchemaValues();
		//DeleteSchemaValues("Books", "", "");
		//DeleteSchemaValues("BookAuthors", "", "");
		
		//Debug.Log("2!");
		//InsertBook("GG Meade", "my book", "1st", 902, "Maths", empty);
		////Debug.Log("3!");
		////empty = new string[] { "George L", "Jon F" };
		//InsertBook("GB McClellan", "not my book", "", 212, "Science", empty);
		////Debug.Log("4!");
		////empty = new string[] { "James G" };
		//InsertBook("AS Damean", "your book", "1st", 902, "", empty);
		//Debug.Log("5!");
		//InsertBook("", 4783);
		//InsertBook("US Grant", 4242);
		//InsertBook("", 107);
		//AlterSchema("high_score", "MiddleIni", "TEXT");
		//SetBookList(10);
		//GetBookList();
		//Debug.Log("6!");
		//Debug.Log(dbPath);
	}

	public void PrintDatabasePath()
    {
		outText.text = "dbPah: " + dbPath;
		Debug.Log(dbPath);
	}

	private void CreateSchema()
	{
		CreateSchemaTableTime();
		//CreateLibraryTable();
		//CreateAuthorTable();
	}

	public static void DropTablesTimeSchema()
    {
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "DROP TABLE IF EXISTS TablesTime;";
				var result = cmd.ExecuteNonQuery();
				Debug.Log("drop schema: " + result);
			}
		}
	}

	public static void CreateSchemaTableTime()
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "CREATE TABLE IF NOT EXISTS TablesTime" +
									"( 'TableName' TEXT NOT NULL UNIQUE, " +
									"'DateModified' datetime NOT NULL, " +
									"'Rows' INTEGER NOT NULL DEFAULT 2 CHECK('Rows'>0), " +
									"'Columns'	INTEGER NOT NULL DEFAULT 2 CHECK('Columns'>0), " +
									"PRIMARY KEY('TableName') UNIQUE('TableName') ON CONFLICT REPLACE);";
				var result = cmd.ExecuteNonQuery();
				Debug.Log("create schema: " + result);
			}
		}
	}

	public static bool InsertNewTableTime(string tableName, Vector2Int rowCol, SqliteConnection conn, SqliteCommand cmd)
	{
		try
		{
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = $"INSERT INTO TablesTime(TableName, DateModified, Rows, Columns) " +
													$"VALUES('{tableName}', CURRENT_TIMESTAMP, {rowCol.x}, {rowCol.y});";
				//+$" ON CONFLICT(TableName) DO UPDATE SET DateModified = CURRENT_TIMESTAMP, Rows = {rowCol.x}, Columns = {rowCol.y};";
			var result = cmd.ExecuteNonQuery();
			Debug.Log("insert tabletime: " + result);
		}
		catch (Exception ex)
		{
			DebugText.outText += $"\nexception Text: \n{ex.Message}";
		}
		return true;
	}
	
	public static bool InsertNewTableTime(string tableName)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				try
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = $"INSERT INTO TablesTime(TableName, DateModified) VALUES('{tableName}', CURRENT_TIMESTAMP);";
					//+ $" ON CONFLICT(TableName) DO UPDATE SET DateModified = CURRENT_TIMESTAMP;";
					DebugText.outText += $"cmd Text: \n{cmd.CommandText.ToString()}\n";
					var result = cmd.ExecuteNonQuery();
					Debug.Log("insert tabletime: " + result);
					DebugText.outText += $"result: {result}\n";
				}
				catch (Exception ex)
                {
					DebugText.outText += $"\nexception Text: \n{ex.Message}";
                }
				return true;
			}
		}
	}

	public static void UpdateTableTime(string tableName)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = $"UPDATE TablesTime Set DateModified = CURRENT_TIMESTAMP WHERE TableName = '{tableName}';";
				var result = cmd.ExecuteNonQuery();
				Debug.Log("query: " + cmd.CommandText.ToString());
				Debug.Log("update tabletime: " + result);
			}
		}
	}

	public static void UpdateTableTimeName(string tableName, string oldTableName)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = $"UPDATE TablesTime Set TableName = '{tableName}' WHERE TableName = '{oldTableName}';";
				Debug.Log("query: " + cmd.CommandText.ToString());
				var result = cmd.ExecuteNonQuery();
				Debug.Log("update tabletime name: " + result);
				cmd.CommandText = $"ALTER TABLE '{oldTableName}' RENAME TO '{tableName}';";
				Debug.Log("query: " + cmd.CommandText.ToString());
				result = cmd.ExecuteNonQuery();
				Debug.Log("update table name: " + result);
			}
		}
		UpdateTableTime(tableName);
	}

	public static List<Tuple<string, Vector2Int>> GetShelves()
	{
		List<Tuple<string, Vector2Int>> shelves = new List<Tuple<string, Vector2Int>>();
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT TableName, Rows, Columns FROM TablesTime ORDER BY DateModified DESC;";
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					var shelf = reader.GetString(0);
					var shelfRow= reader.GetInt32(1);
					var shelfCol = reader.GetInt32(2);
					shelves.Add(Tuple.Create(shelf, new Vector2Int(shelfRow, shelfCol)));
					Debug.Log(shelf);
				}
			}
		}
		return shelves;
	}

	public static void CreateLibraryTable(string tableName, Vector2Int rowCol)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = $"CREATE TABLE IF NOT EXISTS '{tableName}' ( " +
								  "  'ID' INTEGER NOT NULL, " +
								  "  'Title' TEXT NOT NULL, " +
								  "  'ISBN' INTEGER NOT NULL, " +
								  "  'Subtitles' TEXT, " +
								  "  'Edition' TEXT, " +
								  "  'Pages' INTEGER, " +
								  "  'Section' TEXT, " +
								  "  'Row' INTEGER NOT NULL, " +
								  "  'Column' INTEGER NOT NULL, " +
								  "  'PositionX' FLOAT NOT NULL, " +
								  "  'PositionY' FLOAT NOT NULL, " +
								  "  'Width' FLOAT, " +
								  "  'Height' FLOAT, " +
								  "	 PRIMARY KEY (ID)" +
								  //",FOREIGN KEY(TypeId) REFERENCES Types(TypeId)" + 
								  ");";
				var result = cmd.ExecuteNonQuery();
				Debug.Log("create schema: " + result);
				InsertNewTableTime(tableName, rowCol, conn, cmd);
			}
		}
	}

	private void CreateAuthorTable()
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'BookAuthors' ( " +
								  "  'ID' INTEGER NOT NULL, " +
								  "  'AuthorName' TEXT NOT NULL, " +
								  "	 PRIMARY KEY (AuthorName, ID)," +
								  " FOREIGN KEY(ID) REFERENCES Books(ID)" +
								  "  ON DELETE CASCADE " +
								  "  ON UPDATE CASCADE " +
								  ");";

				/*cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'BookAuthors' ( " +
								  "  'ID' INTEGER NOT NULL, " +
								  "  'AuthorName' TEXT NOT NULL, " +
								  "	 PRIMARY KEY (AuthorName)," +
								  " FOREIGN KEY(ID) REFERENCES Books(ID)" + 
								  "  ON DELETE CASCADE " + 
								  "  ON UPDATE CASCADE " + 
								  ");";*/

				var result = cmd.ExecuteNonQuery();
				Debug.Log("create schema: " + result);
			}
		}
	}

	public void AlterSchema()
	{
		AlterSchema(nameField.text, varNameField.text, varOtherField.text);

	}

	private void AlterSchema(String schemaName, String varName, String varType)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			try
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "ALTER TABLE '" + schemaName + "' " +
									  "ADD COLUMN " + varName + " " + varType + ";";

					var result = cmd.ExecuteNonQuery();
					Debug.Log("alter schema: " + result);
				}
			}
			catch (SqliteException e)
			{
				Debug.Log("Error! " + e.Message);
			}
		}
	}
	
	public static void UpdateBook(int id, string title, string isbn, string subtitles, string edition, int pages, string section,
							string[] authors, int row, int column, float positionX, float positionY, float width, float height)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			try
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = $"UPDATE '{LibraryManager.curShelfName}' SET Title = @Title, ISBN = @ISBN, Subtitles = @Subtitles, Edition = @Edition, Pages = @Pages, Section = @Section, " +
                        "Row = @Row, Column = @Column, PositionX = @PositionX, PositionY = @PositionY, Width = @Width, Height = @Height WHERE ID = " + id;
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Title",
						Value = title
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "ISBN",
						Value = isbn
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Subtitles",
						Value = subtitles
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Edition",
						Value = edition
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Pages",
						Value = pages
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Section",
						Value = section
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Row",
						Value = row
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Column",
						Value = column
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "PositionX",
						Value = positionX
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "PositionY",
						Value = positionY
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Width",
						Value = width
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "Height",
						Value = height
					});

					var result = cmd.ExecuteNonQuery();
					Debug.Log($"update book[{id}] ({title}): " + result);
				}
			}
			catch (SqliteException e)
			{
				Debug.Log("Error! " + e.Message);
			}
		}
	}
	public static void UpdateBookPos(int id, float positionX, float positionY)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			try
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = $"UPDATE '{LibraryManager.curShelfName}' SET PositionX = @PositionX, PositionY = @PositionY WHERE ID = " + id;
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "PositionX",
						Value = positionX
					});
					cmd.Parameters.Add(new SqliteParameter
					{
						ParameterName = "PositionY",
						Value = positionY
					});
					
					var result = cmd.ExecuteNonQuery();
					Debug.Log($"update bookPos[{id}] : " + result);
				}
			}
			catch (SqliteException e)
			{
				Debug.Log("Error! " + e.Message);
			}
		}
	}

	public static List<string> GetSchemaList()
	{
		List<string> schemaList = new List<string>();
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name != 'TablesTime' ORDER BY name;";
				//cmd.CommandText = "SELECT * FROM sqlite_master WHERE type='table' ORDER BY name;;";
				//cmd.CommandText = "PRAGMA table_info(Books);";

				Debug.Log("listing books (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					schemaList.Add(reader.GetString(0));
					///string text = ""; int i = 0;
					///while (reader[i].GetType() != typeof(DBNull))
					///{
					///	text += reader.GetString(i);
					///	text += reader.GetValue(i) + " | ";
					///	//text += reader.GetValue(i).ToString();
					///	i++;
					///}
					//Debug.Log(text);
				}
				Debug.Log("listing books (end)");
			}
		}
		return schemaList;
	}
	
	public static List<int> SearchBooks(string searchQuery, bool exactText)
	{
		List<int> searchList = new List<int>();
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				if(exactText)
					cmd.CommandText = $"SELECT ID FROM '{LibraryManager.curShelfName}' WHERE Title LIKE '{searchQuery}%';";
				else
					cmd.CommandText = $"SELECT ID FROM '{LibraryManager.curShelfName}' WHERE Title LIKE '%{searchQuery}%' " +
                        $"ORDER BY(CASE WHEN Title = '{searchQuery}' THEN 1 WHEN Title LIKE '{searchQuery}%' THEN 2 ELSE 3 END), Title;";
				Debug.Log($"search command: {cmd.CommandText}");
				Debug.Log("searching books (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					searchList.Add(reader.GetInt32(0));
				}
				Debug.Log("searching books (end)");
			}
		}
		return searchList;
	}
	
	/*public static List<int> SearchBooks(string searchQuery)
	{
		List<int> searchList = new List<int>();
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = $"SELECT ID FROM 'Testing Shelf' WHERE Title LIKE '{searchQuery}%';";
				Debug.Log($"search command: {cmd.CommandText.ToString()}");
				Debug.Log("searching books (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					searchList.Add(reader.GetInt32(0));
				}
				Debug.Log("searching books (end)");
			}
		}
		return searchList;
	}*/

	public void DeleteSchemaValues()
	{
		//DeleteSchemaValues(nameField.text, varNameField.text, varOtherField.text);
		//DeleteSchemaValues("BookAuthors", varNameField.text, varOtherField.text);
	}

	private void DeleteSchemaValues(String schemaName, String varName, String varEquals)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			try
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "DELETE FROM '" + schemaName + "' " +
									((varName.Equals("") || varEquals.Equals("")) ?
									 "" : "WHERE " + varName + " = " + varEquals) + ";";

					var result = cmd.ExecuteNonQuery();
					Debug.Log("delete schema: " + result);
				}
			}
			catch (SqliteException e)
			{
				Debug.Log("Error! " + e.ToString());
			}
		}
	}

	public void InsertBook()
	{
		//InsertBook(varNameField.text, int.Parse(varOtherField.text));

	}
	public static int InsertBook(string title, string isbn, string subtitles, string edition, int pages, string section, 
							string[] authors, int row, int column, float positionX, float positionY, float width, float height)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "INSERT INTO Books (Title, ISBN, Subtitles, Edition, Pages, Section, Row, Column, PositionX, PositionY, Width, Height) " +
								  "VALUES (@Title, @ISBN, @Subtitles, @Edition, @Pages, @Section, @Row, @Column, @PositionX, @PositionY, @Width, @Height);";

				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Title",
					Value = title
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "ISBN",
					Value = isbn
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Subtitles",
					Value = subtitles
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Edition",
					Value = edition
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Pages",
					Value = pages
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Section",
					Value = section
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Row",
					Value = row
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Column",
					Value = column
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "PositionX",
					Value = positionX
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "PositionY",
					Value = positionY
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Width",
					Value = width
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Height",
					Value = height
				});
				Debug.Log("insert book command: " + cmd.CommandText.ToString());
				var result = cmd.ExecuteNonQuery();
				Debug.Log("insert book: " + result);
				return GetLastID(conn, cmd);
			}
		}
	}
	
	public static int GetLastID(SqliteConnection cpmm, SqliteCommand cmd)
	{
		cmd.CommandType = CommandType.Text;
		cmd.CommandText = "select last_insert_rowid()";
		Int64 LastRowID64 = (Int64)cmd.ExecuteScalar();
		int LastRowID = (int)LastRowID64;
		Debug.Log($"Getting LastRowID...");
		Debug.Log($"LastRowID: {LastRowID}");
		return LastRowID;
	}
	public static void LastID()
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "select last_insert_rowid()";
				Int64 LastRowID64 = (Int64)cmd.ExecuteScalar();
				int LastRowID = (int)LastRowID64;
				Debug.Log($"Getting LastRowID...");
				Debug.Log($"LastRowID: {LastRowID}");
            }
        }
	}

	public static void GetBookList()
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT * FROM Books;";
				//cmd.CommandText = "PRAGMA table_info(Books);";

				Debug.Log("listing books (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					///var id = reader.GetInt32(0);
					///var title = reader.GetString(1);
					///
					///var subtitles = "";
					///if (reader[2].GetType() != typeof(DBNull))
					///{
					///	subtitles = reader.GetString(2);
					///}
					///var edition = "";
					///if (reader[3].GetType() != typeof(DBNull))
					///{
					///	edition = reader.GetString(3);
					///}
					///
					///var pages = reader.GetInt32(4);
					///
					///var section = "";
					///if (reader[5].GetType() != typeof(DBNull))
					///{
					///	section = reader.GetString(5);
					///}
					///
					///var author = "";
					///if (reader[6].GetType() != typeof(DBNull))
					///{
					///	author = reader.GetString(6);
					///}
					///
					///var text = string.Format("id no #{0}: Title: {1} Subtitles: {2} Edition: {3} Pages: {4} Section: {5} Author: {6}.",
					///								id, title, subtitles, edition, pages, section, author);
					///string text = "[id no #" + id + "] Title: " + title + " Subtitles: " + subtitles + " Edition: "
					///		+ edition + " Pages: " + pages + " Section: " + section + " Author: " + author + ".";
					string text = ""; int i = 0;
					while (reader[i].GetType() != typeof(DBNull))
                    {
						//text += reader.GetString(i);
						text += reader.GetValue(i) + " | ";
						//text += reader.GetValue(i).ToString();
						i++;
                    }
					Debug.Log(text);
				}
				Debug.Log("listing books (end)");
			}
		}
	}
	
	public static int GetLengthOfRowCol(int row, int column)
    {
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT COUNT* FROM Books WHERE Row = @Row AND Column = @Col;";
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Row",
					Value = row
				});
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Col",
					Value = column
				});
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					var length = reader.GetInt32(0);
					return length;
				}
			}
		}
		return -1;
	}
	
	public static List<Book> GetBooksFromRowCol(int row, int column)
    {
		List<Book> books = new List<Book>();
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				//cmd.CommandText = "SELECT * FROM Books NATURAL JOIN BookAuthors DESC LIMIT @Count;";
				cmd.CommandText = $"SELECT * FROM '{LibraryManager.curShelfName}' WHERE Row = @Row AND Column = @Col;";

				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Row",
					Value = row
				});
				
				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Col",
					Value = column
				});

				//Debug.Log($"Row: {row} | Col: {column}");
				//Debug.Log(cmd.CommandText.ToString());
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					var id = reader.GetInt32(0);
					var title = reader.GetString(1);
					var isbn = reader.GetValue(2);

					var subtitles = "";
					if (reader[2].GetType() != typeof(DBNull))
					{
						subtitles = reader.GetString(3);
					}
					var edition = reader.GetValue(4);
					
					var pages = reader.GetInt32(5);

					var section = reader.GetValue(6);
					var posX = reader.GetFloat(9);
					var posY = reader.GetFloat(10);
					var width = reader.GetFloat(11);
					var height = reader.GetFloat(12);
					var author = "";
					///if (reader[6].GetType() != typeof(DBNull))
					///{
					///	author = reader.GetString(7);
					///}
					//Book book = new Book();
					Book book;//= gameObject.AddComponent<Book>();
					GameObject gO = new GameObject();//= gameObject.AddComponent<Book>();
					book = gO.AddComponent<Book>();
					book.SetBook(id, title, isbn.ToString(), subtitles, edition.ToString(), pages, section.ToString(), 
									author, row, column, posX, posY, width, height);
					books.Add(book);
					Destroy(gO);
					//var text = string.Format("id no #{0}: Title: {1} Subtitles: {2} Edition: {3} Pages: {4} Section: {5} Author: {6}.",
					//								id, title, subtitles, edition, pages, section, author);
					string text = "[id no #" + id + "] Title: " + title + " ISBN: " + isbn + " Subtitles: " + subtitles + " Edition: "
							+ edition + " Pages: " + pages + " Section: " + section + " Author: " + author + ".";
					Debug.Log(text);
				}
				//ddDebug.Log("scores (end)");
			}
		}
		return books;
	}


	public static void SetBookList(int limit)
	{
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				//cmd.CommandText = "SELECT * FROM Books NATURAL JOIN BookAuthors DESC LIMIT @Count;";
				cmd.CommandText = $"SELECT * FROM '{LibraryManager.curShelfName}' NATURAL JOIN BookAuthors;";

				cmd.Parameters.Add(new SqliteParameter
				{
					ParameterName = "Count",
					Value = limit
				});

				Debug.Log("scores (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					var id = reader.GetInt32(0);
					var title = reader.GetString(1);

					var subtitles = "";
					if (reader[2].GetType() != typeof(DBNull))
					{
						subtitles = reader.GetString(2);
					}
					var edition = "";
					if (reader[3].GetType() != typeof(DBNull))
					{
						edition = reader.GetString(3);
					}

					var pages = reader.GetInt32(4);

					var section = "";
					if (reader[5].GetType() != typeof(DBNull))
					{
						section = reader.GetString(5);
					}

					var author = "";
					if (reader[6].GetType() != typeof(DBNull))
					{
						author = reader.GetString(6);
					}

					//var text = string.Format("id no #{0}: Title: {1} Subtitles: {2} Edition: {3} Pages: {4} Section: {5} Author: {6}.",
					//								id, title, subtitles, edition, pages, section, author);
					string text = "[id no #" + id + "] Title: " + title + " Subtitles: " + subtitles + " Edition: "
							+ edition + " Pages: " + pages + " Section: " + section + " Author: " + author + ".";
					Debug.Log(text);
				}
				Debug.Log("scores (end)");
			}
		}
	}

	public static int GetBookLength()
    {
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				//cmd.CommandText = "SELECT * FROM Books NATURAL JOIN BookAuthors DESC LIMIT @Count;";
				cmd.CommandText = $"SELECT COUNT(*) FROM '{LibraryManager.curShelfName}'; "; ;
				//Debug.Log($"SQLite command: {cmd.CommandText.ToString()}");

				Debug.Log("counting (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					var id = reader.GetInt32(0);
					return id;
				}
			}
		}
		return -1;
    }
	public static void SetBookwIndex(ref Book book, int index)
	{
		//List<Book> bookList = new List<Book>();
		using (var conn = new SqliteConnection(dbPath))
		{
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				//cmd.CommandText = "SELECT * FROM Books NATURAL JOIN BookAuthors DESC LIMIT @Count;";
				cmd.CommandText = $"SELECT * FROM '{LibraryManager.curShelfName}' NATURAL JOIN BookAuthors " +
									" WHERE ID = " + ++index + "; ";;
				//Debug.Log($"SQLite command: {cmd.CommandText.ToString()}");

				//Debug.Log("scores (begin)");
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					var id = reader.GetInt32(0);
					var title = reader.GetString(1);

					var subtitles = "";
					if (reader[2].GetType() != typeof(DBNull))
					{
						subtitles = reader.GetString(2);
					}
					var edition = "";
					if (reader[3].GetType() != typeof(DBNull))
					{
						edition = reader.GetString(3);
					}

					var pages = reader.GetInt32(4);

					var section = "";
					if (reader[5].GetType() != typeof(DBNull))
					{
						section = reader.GetString(5);
					}

					var author = "";
					if (reader[6].GetType() != typeof(DBNull))
					{
						author = reader.GetString(6);
					}

					//Book newBook = new Book { ID = id, Title = title, Subtitles = subtitles, Edition = edition, Pages = pages, Section = section, Author = author };
					//bookList[--id].SetBook(id, title, subtitles, edition, pages, section, author);
					//bookList.Add(id, title, subtitles, edition, pages, section, author);
					//book.SetBook(id, title, subtitles, edition, pages, section, author);
					//var text = string.Format("id no #{0}: Title: {1} Subtitles: {2} Edition: {3} Pages: {4} Section: {5} Author: {6}.",
					//								id, title, subtitles, edition, pages, section, author);

					//string text = "[id no #" + id + "] Title: " + title + " Subtitles: " + subtitles + " Edition: "
					//		+ edition + " Pages: " + pages + " Section: " + section + " Author: " + author + ".";
					//Debug.Log(text);
				}
				//Debug.Log("scores (end)");
			}
		}
		//return bookList;
	}
}