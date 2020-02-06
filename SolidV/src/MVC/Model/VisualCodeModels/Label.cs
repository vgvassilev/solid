/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class Label : Shape
  {
    private string textLabel = "";
    public string TextLabel {
      get { return textLabel; }
      set { textLabel = value; }
    }

    public Label(Rectangle rectangle) : base(rectangle) { }
    public Label(Rectangle rectangle, string text) : base(rectangle) {
      this.TextLabel = text;
    }
    public Label(Shape shape, string text) : base(shape.Rectangle) {
      this.TextLabel = text;
    }
  }
}

