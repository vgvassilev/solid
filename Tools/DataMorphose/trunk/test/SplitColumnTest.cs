/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.IO;
using NUnit.Framework;

using DataMorphose.Import;
using DataMorphose.Model;
using DataMorphose.Transform;

namespace DataMorphose.Test
{
  [TestFixture]
  public class SplitColumnTest : BaseTestFixture
  {
    public SplitColumnTest() {
      filePath = Path.Combine(filePath, "DemoDB");
      filePath = Path.Combine(filePath, "Text");
    }

    [Test]
    public void SplitName() {
      CSVImporter importer = new CSVImporter(/*isFirstRowHeader*/ true);

      Table Customers = importer.importFromFile(Path.Combine(filePath, "Customers.txt"));

      // Split the column CustomerName into First/LastName
      SplitColumn sc = new SplitColumn(Customers, Customers.Columns[2], /*delimiter*/' ');
      sc.Redo();

      Assert.IsTrue(Customers.Columns.Count == 12, "Column count is wrong");
    }
  }
}

