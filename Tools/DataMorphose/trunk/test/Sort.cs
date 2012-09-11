/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using DataMorphose.Actions;
using DataMorphose.Import;
using DataMorphose.Model;

namespace DataMorphose.Test {

  [TestFixture]
  public class Sort : BaseTestFixture {

    public Sort () {
      filePath = Path.Combine(filePath, "DemoDB");
      filePath = Path.Combine(filePath, "Text");
    }

    [Test]
    public void SortByColumnAction() {
      CSVImporter importer = new CSVImporter(/*isFirstRowHeader*/ true);
      Table Categories = importer.importFromFile(Path.Combine(filePath, "Categories.txt"));
      // Note that only the first and the last records are out of order. Thus we can
      // simply change them. A lot quicker at that point of development.
      string expectedAfterUndo = Categories.ToString();
      List<string> expectedList = new List<string>(expectedAfterUndo.Split('\n'));
      string lastRec = expectedList[8];
      expectedList[8] = expectedList[1];
      expectedList[1] = lastRec;
      string expected = String.Join("\n", expectedList);

      SortByColumnAction sort = new SortByColumnAction(Categories, /*colIndex*/0);
      sort.Redo();
      Assert.IsTrue(Categories.ToString() == expected.ToString(), "Sort not working.");
      // Now revert the action:
      sort.Undo();
      Assert.IsTrue(Categories.ToString() == expectedAfterUndo, "Undo Sort not working.");
    }
  }
}
