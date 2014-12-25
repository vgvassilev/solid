/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.ComponentModel;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors 
{
  using Gtk = global::Gtk;

  [InspectorEditorType(typeof(byte))]
  [InspectorEditorType(typeof(sbyte))]
  [InspectorEditorType(typeof(Int16))]
  [InspectorEditorType(typeof(UInt16))]
  [InspectorEditorType(typeof(Int32))]
  [InspectorEditorType(typeof(UInt32))]
  [InspectorEditorType(typeof(Int64))]
  [InspectorEditorType(typeof(UInt64))]
  [InspectorEditorType(typeof(Decimal))]
  public class IntRangeEditor : Gtk.SpinButton, IInspectorEditor
  {
    Type propType;
    
    public IntRangeEditor() : base(0, 0, 1.0)
    {
      this.HasFrame = false;
    }
    
    public void Initialize(EditSession session)
    {
      propType = session.Inspector.PropertyType;
      
      double min, max;
      
      switch(Type.GetTypeCode(propType)) {
        case TypeCode.Int16:
          min =(double) Int16.MinValue;
          max =(double) Int16.MaxValue;
          break;
        case TypeCode.UInt16:
          min =(double) UInt16.MinValue;
          max =(double) UInt16.MaxValue;
          break;
        case TypeCode.Int32:
          min =(double) Int32.MinValue;
          max =(double) Int32.MaxValue;
          break;
        case TypeCode.UInt32:
          min =(double) UInt32.MinValue;
          max =(double) UInt32.MaxValue;
          break;
        case TypeCode.Int64:
          min =(double) Int64.MinValue;
          max =(double) Int64.MaxValue;
          break;
        case TypeCode.UInt64:
          min =(double) UInt64.MinValue;
          max =(double) UInt64.MaxValue;
          break;
        case TypeCode.Byte:
          min =(double) Byte.MinValue;
          max =(double) Byte.MaxValue;
          break;
        case TypeCode.SByte:
          min =(double) SByte.MinValue;
          max =(double) SByte.MaxValue;
          break;
        default:
          throw new ApplicationException("IntRange editor does not support editing values of type " + session.Inspector.PropertyType);
      }
      
      SetRange(min, max);
    }
    
    object IInspectorEditor.Value {
      get { return Convert.ChangeType(base.Value, propType); }
      set { base.Value =(double) Convert.ChangeType(value, typeof(double)); }
    }
  }
}
