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
using DataMorphose.Model.Meta;

namespace DataMorphose.Test
{
  [TestFixture]
  public class DeduceConstraints : BaseTestFixture
  {
    public DeduceConstraints() {
      filePath = Path.Combine(filePath, "DemoDB");
      filePath = Path.Combine(filePath, "Text");
    }

    [Test]
    public void DeduceColumnConstraints() {
      CSVImporter importer = new CSVImporter(/*isFirstRowHeader*/ true);
      Table table = importer.importFromFile(Path.Combine(filePath, "Categories.txt"));
      ColumnConstraints columnConstraints = new ColumnConstraints();
      columnConstraints.DeducePrimaryKey(table);
      // We know that the first column Contains is a primary key
      bool isPrimaryKey = 
        table.Columns[0].Meta.Constraints.ContainsKey(ConstraintKind.PrimaryKey);
      Assert.IsTrue(isPrimaryKey, "Column 0 not primary key!");
    }
  }
}

