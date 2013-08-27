/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class ForceDirectedBlockAutoArraingment
  {
    private const double ATTRACTION_CONSTANT = 0.2;   // spring constant
    private const double REPULSION_CONSTANT = 10000;  // charge constant
    
    private const double DEFAULT_DAMPING = 0.6;
    private const int DEFAULT_SPRING_LENGTH = 200;
    private const int DEFAULT_MAX_ITERATIONS = 500;

    private List<Shape> shapes;
    public List<Shape> Shapes {
      get { return shapes; }
      set { shapes = value; }
    }

    /// <summary>
    /// Runs the force-directed layout algorithm on this Diagram, using the default parameters.
    /// </summary>
    /// 
    public void Arrange() {
      Arrange(DEFAULT_DAMPING, DEFAULT_SPRING_LENGTH, DEFAULT_MAX_ITERATIONS, true);
    }
    
    /// <summary>
    /// Runs the force-directed layout algorithm on this Diagram, offering the option of a random 
    /// or deterministic layout.
    /// </summary>
    /// <param name="deterministic">Whether to use a random or deterministic layout.</param>
    /// 
    public void Arrange(bool deterministic) {
      Arrange(DEFAULT_DAMPING, DEFAULT_SPRING_LENGTH, DEFAULT_MAX_ITERATIONS, deterministic);
    }
    
    /// <summary>
    /// Runs the force-directed layout algorithm on this Diagram, using the specified parameters.
    /// </summary>
    /// <param name="damping">
    /// Value between 0 and 1 that slows the motion of the nodes during layout.
    /// </param>
    /// <param name="springLength">
    /// Value in pixels representing the length of the imaginary springs that run along the 
    /// connectors.
    /// </param>
    /// <param name="maxIterations">
    /// Maximum number of iterations before the algorithm terminates.
    /// </param>
    /// <param name="deterministic">
    /// Whether to use a random or deterministic layout.
    /// </param>
    ///
    public void Arrange(double damping, int springLength, int maxIterations, bool deterministic) {
      // random starting positions can be made deterministic by seeding System.Random with a 
      // constant
      Random rnd = deterministic ? new Random(0) : new Random();
      
      // copy nodes into an array of metadata and randomise initial coordinates for each node
      NodeLayoutInfo[] layout = new NodeLayoutInfo[Shapes.Count];
      for (int i = 0; i < Shapes.Count; i++) {
        layout[i] = new NodeLayoutInfo(Shapes[i], new Vector(), new PointD(0, 0));
        layout[i].Node.Location = new PointD(rnd.Next(-50, 50), rnd.Next(-50, 50));
      }
      
      int stopCount = 0;
      int iterations = 0;
      
      while (true) {
        double totalDisplacement = 0;
        
        for (int i=0; i<layout.Length; i++) {
          NodeLayoutInfo current = layout[i];
          
          // express the node's current position as a vector, relative to the origin
          int distance = CalcDistance(new PointD(0, 0), current.Node.Location);
          Vector currentPosition 
            = new Vector(distance, GetBearingAngle(new PointD(0, 0), current.Node.Location));
          Vector netForce = new Vector(0, 0);
          
          // determine repulsion between nodes
          foreach (Shape other in Shapes) {
            if (other != current.Node)
              netForce += CalcRepulsionForce(current.Node, other);
          }
          
          // if it was a connected node
          if (current.Node is RelationShape) {
            RelationShape relationShape = current.Node as RelationShape;
            // determine attraction caused by connections
            foreach (Shape child in relationShape.Related) {
              netForce += CalcAttractionForce(relationShape, child, springLength);
            }
            
            foreach (Shape parent in Shapes) {
              if (parent is RelationShape) {
                RelationShape parentRelationShape = parent as RelationShape;
                if (relationShape.Related.Contains(current.Node)) 
                  netForce += CalcAttractionForce(current.Node, parentRelationShape, springLength);
              }
            }
          }
          // apply net force to node velocity
          current.Velocity = (current.Velocity + netForce) * damping;
          
          // apply velocity to node position
          current.NextPosition = (currentPosition + current.Velocity).ToPointD();
        }
        
        // move nodes to resultant positions (and calculate total displacement)
        for (int i = 0; i < layout.Length; i++) {
          NodeLayoutInfo current = layout[i];
          
          totalDisplacement += CalcDistance(current.Node.Location, current.NextPosition);
          current.Node.Location = current.NextPosition;
        }
        
        iterations++;
        if (totalDisplacement < 10) stopCount++;
        if (stopCount > 15) break;
        if (iterations > maxIterations) break;
      }
      
      // center the diagram around the origin
      Rectangle logicalBounds = GetDiagramBounds();
      PointD midPoint = new PointD(logicalBounds.X + (logicalBounds.Width / 2), 
                                   logicalBounds.Y + (logicalBounds.Height / 2));
      
      foreach (Shape node in Shapes) {
        node.Location = new PointD(Math.Abs(midPoint.X - node.Location.X), 
                                   Math.Abs(midPoint.Y - node.Location.Y));
      }
    }
    
    /// <summary>
    /// Calculates the distance between two points.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <returns>The pixel distance between the two points.</returns>
    /// 
    public static int CalcDistance(PointD a, PointD b) {
      double xDist = (a.X - b.X);
      double yDist = (a.Y - b.Y);
      return (int)Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
    }
    
    /// <summary>
    /// Calculates the bearing angle from one point to another.
    /// </summary>
    /// <param name="start">The node that the angle is measured from.</param>
    /// <param name="end">The node that creates the angle.</param>
    /// <returns>The bearing angle, in degrees.</returns>
    /// 
    private double GetBearingAngle(PointD start, PointD end) {
      PointD half = new PointD(start.X + ((end.X - start.X) / 2), start.Y + ((end.Y - start.Y) / 2));
      
      double diffX = (double)(half.X - start.X);
      double diffY = (double)(half.Y - start.Y);
      
      if (diffX == 0) diffX = 0.001;
      if (diffY == 0) diffY = 0.001;
      
      double angle;
      if (Math.Abs(diffX) > Math.Abs(diffY)) {
        angle = Math.Tanh(diffY / diffX) * (180.0 / Math.PI);
        if (((diffX < 0) && (diffY > 0)) || ((diffX < 0) && (diffY < 0))) angle += 180;
      }
      else {
        angle = Math.Tanh(diffX / diffY) * (180.0 / Math.PI);
        if (((diffY < 0) && (diffX > 0)) || ((diffY < 0) && (diffX < 0))) angle += 180;
        angle = (180 - (angle + 90));
      }
      
      return angle;
    }
    
    /// <summary>
    /// Calculates the attraction force between two connected nodes, using the specified spring
    /// length.
    /// </summary>
    /// <param name="x">The node that the force is acting on.</param>
    /// <param name="y">The node creating the force.</param>
    /// <param name="springLength">The length of the spring, in pixels.</param>
    /// <returns>A Vector representing the attraction force.</returns>
    /// 
    private Vector CalcAttractionForce(Shape x, Shape y, int springLength) {
      int proximity = Math.Max(CalcDistance(x.Location, y.Location), 1);
      
      // Hooke's Law: F = -kx
      double force = ATTRACTION_CONSTANT * Math.Max(proximity - springLength, 0);
      double angle = GetBearingAngle(x.Location, y.Location);
      
      return new Vector(force, angle);
    }
    
    /// <summary>
    /// Calculates the repulsion force between any two nodes in the diagram space.
    /// </summary>
    /// <param name="x">The node that the force is acting on.</param>
    /// <param name="y">The node creating the force.</param>
    /// <returns>A Vector representing the repulsion force.</returns>
    /// 
    private Vector CalcRepulsionForce(Shape x, Shape y) {
      int proximity = Math.Max(CalcDistance(x.Location, y.Location), 1);
      
      // Coulomb's Law: F = k(Qq/r^2)
      double force = -(REPULSION_CONSTANT / Math.Pow(proximity, 2));
      double angle = GetBearingAngle(x.Location, y.Location);
      
      return new Vector(force, angle);
    }
    
    /// <summary>
    /// Determines the logical bounds of the diagram. This is used to center and scale the diagram 
    /// when drawing.
    /// </summary>
    /// <returns>
    /// A System.Drawing.Rectangle that fits exactly around every node in the diagram.
    /// </returns>
    ///
    private Rectangle GetDiagramBounds() {
      double minX = Double.MaxValue, minY = Double.MaxValue;
      double maxX = Double.MinValue, maxY = Double.MinValue;
      foreach (Shape node in Shapes) {
        if (node.Location.X < minX)
          minX = node.Location.X;
        if (node.Location.X > maxX)
          maxX = node.Location.X;
        if (node.Location.Y < minY)
          minY = node.Location.Y;
        if (node.Location.Y > maxY)
          maxY = node.Location.Y;
      }
      
      return new Rectangle(minX, minY, maxX - minX, maxY - minY);
      //return Rectangle.FromLTRB(minX, minY, maxX, maxY);
    }
    
    
    /// <summary>
    /// Private inner class used to track the node's position and velocity during simulation.
    /// </summary>
    /// 
    private class NodeLayoutInfo {
      
      public Shape Node;     // reference to the node in the simulation
      public Vector Velocity;   // the node's current velocity, expressed in vector form
      public PointD NextPosition;  // the node's position after the next iteration
      
      /// <summary>
      /// Initialises a new instance of the Diagram.NodeLayoutInfo class, using the specified parameters.
      /// </summary>
      /// <param name="node"></param>
      /// <param name="velocity"></param>
      /// <param name="nextPosition"></param>
      /// 
      public NodeLayoutInfo(Shape node, Vector velocity, PointD nextPosition) {
        Node = node;
        Velocity = velocity;
        NextPosition = nextPosition;
      }
    }
    
    /// <summary>
    /// Represents a vector whose magnitude and direction are both expressed as System.Double. 
    /// Vector addition and scalar multiplication are supported.
    /// </summary>
    /// 
    public struct Vector {
      
      private double mMagnitude;
      private double mDirection;
      
      /// <summary>
      /// Gets or sets the magnitude of the vector.
      /// </summary>
      /// 
      public double Magnitude {
        get { return mMagnitude; }
        set { mMagnitude = value; }
      }
      /// <summary>
      /// Gets or sets the direction of the vector.
      /// </summary>
      /// 
      public double Direction {
        get { return mDirection; }
        set { mDirection = value; }
      }
      
      /// <summary>
      /// Initialises a new instance of the Vector class using the specified magnitude and direction. 
      /// Automatically simplifies the representation to ensure a positive magnitude and a sub-circular angle.
      /// </summary>
      /// <param name="magnitude">The magnitude of the vector.</param>
      /// <param name="direction">The direction of the vector, in degrees.</param>
      /// 
      public Vector(double magnitude, double direction) {
        mMagnitude = magnitude;
        mDirection = direction;
        
        if (mMagnitude < 0) {
          // resolve negative magnitude by reversing direction
          mMagnitude = -mMagnitude;
          mDirection = (180.0 + mDirection) % 360;
        }
        
        // resolve negative direction
        if (mDirection < 0) mDirection = (360.0 + mDirection);
      }
      
      /// <summary>
      /// Calculates the resultant sum of two vectors.
      /// </summary>
      /// <param name="a">The first operand.</param>
      /// <param name="b">The second operand.</param>
      /// <returns>The result of vector addition.</returns>
      /// 
      public static Vector operator +(Vector a, Vector b) {
        // break into x-y components
        double aX = a.Magnitude * Math.Cos((Math.PI / 180.0) * a.Direction);
        double aY = a.Magnitude * Math.Sin((Math.PI / 180.0) * a.Direction);
        
        double bX = b.Magnitude * Math.Cos((Math.PI / 180.0) * b.Direction);
        double bY = b.Magnitude * Math.Sin((Math.PI / 180.0) * b.Direction);
        
        // add x-y components
        aX += bX;
        aY += bY;
        
        // pythagorus' theorem to get resultant magnitude
        double magnitude = Math.Sqrt(Math.Pow(aX, 2) + Math.Pow(aY, 2));
        
        // calculate direction using inverse tangent
        double direction;
        if (magnitude == 0)
          direction = 0;
        else
          direction = (180.0 / Math.PI) * Math.Atan2(aY, aX);
        
        return new Vector(magnitude, direction);
      }
      
      /// <summary>
      /// Calculates the result of multiplication by a scalar value.
      /// </summary>
      /// <param name="vector">The Vector that forms the first operand.</param>
      /// <param name="multiplier">The System.Double that forms the second operand.</param>
      /// <returns>A Vector whose magnitude has been multiplied by the scalar value.</returns>
      /// 
      public static Vector operator *(Vector vector, double multiplier) {
        // only magnitude is affected by scalar multiplication
        return new Vector(vector.Magnitude * multiplier, vector.Direction);
      }
      
      /// <summary>
      /// Converts the vector into an X-Y coordinate representation.
      /// </summary>
      /// <returns>An X-Y coordinate representation of the Vector.</returns>
      public PointD ToPointD() {
        // break into x-y components
        double aX = mMagnitude * Math.Cos((Math.PI / 180.0) * mDirection);
        double aY = mMagnitude * Math.Sin((Math.PI / 180.0) * mDirection);
        
        return new PointD(aX, aY);
      }
      
      /// <summary>
      /// Returns a string representation of the vector.
      /// </summary>
      /// <returns>A System.String representing the vector.</returns>
      /// 
      public override string ToString() {
        return mMagnitude.ToString("N5") + " " + mDirection.ToString("N2") + "°";
      }
    }

    public ForceDirectedBlockAutoArraingment(ShapesModel model) {
      this.Shapes = model.Shapes;
      Arrange();
    }
  }
}

