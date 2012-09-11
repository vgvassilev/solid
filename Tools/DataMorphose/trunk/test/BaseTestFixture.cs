/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

namespace DataMorphose.Test {
  public class BaseTestFixture {
    protected string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
    public BaseTestFixture() {
      filePath = Path.Combine(filePath, "..");
      filePath = Path.Combine(filePath, "..");
      filePath = Path.Combine(filePath, "test");
    }
  }
}
