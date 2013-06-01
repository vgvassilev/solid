/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using DataMorphose.Model;
using DataMorphose.Plugins.ImportExport.Import;

using System;
using System.IO;

using NUnit;
using NUnit.Framework;

namespace DataMorphose.Plugins.ImportExport.Test
{

  [TestFixture]
  public class CSVImport 
  {
    private string filePath = Path.Combine(SolidOpt.BuildInformation.BuildInfo.SourceDir,
                                           "plugins", "ImportExport", "test", "DemoDB", "Text");
    private CSVImporter importer = new CSVImporter(/*firstRawIsHeader*/true);

    public CSVImport() {
    }

    [Test]
    public void CheckCategories() {
      Console.WriteLine(filePath);
      Table Categories = importer.importFromFile(Path.Combine(filePath, "Categories.txt"));
      CheckTable(Categories, "Categories", 8, 4);
    }

    [Test]
    public void CheckCustomers() {
      Table Customers = importer.importFromFile(Path.Combine(filePath, "Customers.txt"));
      CheckTable(Customers, "Customers", 86, 11);
    }

    [Test]
    public void CheckEmployees() {
      Table Employees = importer.importFromFile(Path.Combine(filePath, "Employees.txt"));
      CheckTable(Employees, "Employees", 9, 17);
    }

    [Test]
    public void CheckOrderDetails() {
      Table OrderDetails = importer.importFromFile(Path.Combine(filePath, "OrderDetails.txt"));
      CheckTable(OrderDetails, "OrderDetails", 400, 5);
    }

    [Test]
    public void CheckOrders() {
      Table Orders = importer.importFromFile(Path.Combine(filePath, "Orders.txt"));
      CheckTable(Orders, "Orders", 500, 14);
    }

    [Test]
    public void CheckProducts() {
      Table Products = importer.importFromFile(Path.Combine(filePath, "Products.txt"));
      CheckTable(Products, "Products", 77, 10);
    }

    [Test]
    public void CheckShippers() {
      Table Shippers = importer.importFromFile(Path.Combine(filePath, "Shippers.txt"));
      CheckTable(Shippers, "Shippers", 3, 3);
    }

    [Test]
    public void CheckSuppliers() {
      Table Suppliers = importer.importFromFile(Path.Combine(filePath, "Suppliers.txt"));
      CheckTable(Suppliers, "Suppliers", 38, 12);
    }

    private void CheckTable(Table table, string expectedName, int expectedRows,
                            int expectedColumns) {

      Assert.IsNotNull(table, "Table Customers must not be Null");
      Assert.IsTrue(table.Name == expectedName, "Table name different than expected");
      Assert.IsTrue(table.Columns.Count == expectedColumns,
                    "Columns count different from expected");
      Assert.IsTrue(table.Columns[0].Values.Count == expectedRows, "Row count different from expected");
    }
  }
}

