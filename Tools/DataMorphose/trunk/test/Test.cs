// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using DataMorphose.Model;

using System;
using System.IO;

using NUnit.Framework;

namespace DataMorphose.Test
{
  class ColLexer {
    private string line = null;
    private int curLineIndex = 0;
    public ColLexer(string line) {
      this.line = line;
    }

    public string Lex() {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      while (curLineIndex < line.Length) {
        if (line[curLineIndex] != '|')
          sb.Append(line[curLineIndex]);
        else {
          curLineIndex++;
          return sb.ToString().Trim();
        }
        curLineIndex++;
      }
      return (sb.Length == 0) ? null : sb.ToString().Trim();
    }

  }


  [TestFixture()]
  public class Test
  {
    private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");

    public Test() {
      filePath = Path.Combine(filePath, "..");
      filePath = Path.Combine(filePath, "..");
      filePath = Path.Combine(filePath, "test");
      filePath = Path.Combine(filePath, "DemoDB");
      filePath = Path.Combine(filePath, "Text");
    }

    private void importDBFromFiles() {
      //Database db = new Database("Test DB");

    }

    private Table importFromFile(string file) {
      if (File.Exists(file)) {
        Table table = new Table(Path.GetFileNameWithoutExtension(file));
        StreamReader reader = new StreamReader(file);
        // BufferedStream bs = new BufferedStream(new FileStream(file));
        // The first row contains the column names.
        ColLexer lexer = new ColLexer(reader.ReadLine());
        string colValue;
        Row header = new Row();

        while((colValue = lexer.Lex()) != null)
          header.Columns.Add(new Column(colValue));
        table.Header = header;

        Row row = null;
        Column col = null;
        string s;
        while((s = reader.ReadLine()) != null) {
          lexer = new ColLexer(s);
          row = new Row();
          while((colValue = lexer.Lex()) != null) {
            col = new Column("");
            col.Value = colValue;
            row.Columns.Add(col);
          }
          table.Rows.Add(row);
        }
        return table;
      }
      return null;
    }

    [Test()]
    public void CheckCategories() {
      Table Categories = importFromFile(Path.Combine(filePath, "Categories.txt"));
      CheckTable(Categories, "Categories", 8, 4);
    }

    [Test()]
    public void CheckCustomers() {
      Table Customers = importFromFile(Path.Combine(filePath, "Customers.txt"));
      CheckTable(Customers, "Customers", 95, 11);
    }

    [Test()]
    public void CheckEmployees() {
      Table Employees = importFromFile(Path.Combine(filePath, "Employees.txt"));
      CheckTable(Employees, "Employees", 12, 17);
    }

    [Test()]
    public void CheckOrderDetails() {
      Table OrderDetails = importFromFile(Path.Combine(filePath, "OrderDetails.txt"));
      CheckTable(OrderDetails, "OrderDetails", 400, 5);
    }

    [Test()]
    public void CheckOrders() {
      Table Orders = importFromFile(Path.Combine(filePath, "Orders.txt"));
      CheckTable(Orders, "Orders", 529, 14);
    }

    [Test()]
    public void CheckProducts() {
      Table Products = importFromFile(Path.Combine(filePath, "Products.txt"));
      CheckTable(Products, "Products", 77, 10);
    }

    [Test()]
    public void CheckShippers() {
      Table Shippers = importFromFile(Path.Combine(filePath, "Shippers.txt"));
      CheckTable(Shippers, "Shippers", 3, 3);
    }

    [Test()]
    public void CheckSuppliers() {
      Table Suppliers = importFromFile(Path.Combine(filePath, "Suppliers.txt"));
      CheckTable(Suppliers, "Suppliers", 38, 12);
    }

    private void CheckTable(Table table, string expectedName, int expectedRows,
                            int expectedColumns) {

      Assert.IsNotNull(table, "Table Customers must not be Null");
      Assert.IsTrue(table.Name == expectedName, "Table name different than expected");
      Assert.IsTrue(table.Header.Columns.Count == expectedColumns,
                    "Columns count different from expected");
      Assert.IsTrue(table.Rows.Count == expectedRows, "Row count different from expected");
    }
  }
}

