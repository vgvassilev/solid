/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using NUnit.Framework;

using DataMorphose.Deduce;
using DataMorphose.Import;
using DataMorphose.Model;

namespace DataMorphose.Test {
  [TestFixture]
  public class DeduceType : BaseTestFixture {

    public DeduceType () {
      filePath = Path.Combine(filePath, "DemoDB");
      filePath = Path.Combine(filePath, "Text");
    }

    [Test]
    public void DeduceColumnType() {
      CSVImporter importer = new CSVImporter(/*isFirstRowHeader*/ true);
      Table Categories = importer.importFromFile(Path.Combine(filePath, "Categories.txt"));
      foreach (Column column in Categories.Columns)
        ColumnType.ResolveColumnType(column);
      // ID
      Assert.IsTrue(Categories.Columns[0].Meta.Type == typeof(Int32), "Types differ.");
      // CategoryName
      Assert.IsTrue(Categories.Columns[1].Meta.Type == typeof(string), "Types differ.");
      // Description
      Assert.IsTrue(Categories.Columns[2].Meta.Type == typeof(string), "Types differ.");
      // Picture
      Assert.IsTrue(Categories.Columns[3].Meta.Type == typeof(string), "Types differ.");
    }
  }
}

