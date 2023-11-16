using Elements.Geometry;
using Elements.Geometry.Solids;
using Cameras;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Drawing;

namespace Elements
{
    public class ViewCamera : GeometricElement
    {
        [JsonProperty("Add Id")]
        public string AddId { get; set; }

        [JsonProperty("View Name")]
        public string ViewName { get; set; }

        [JsonProperty("Focal Point")]
        public Vector3 FocalPoint { get; set; }

        public ViewCamera(CamerasOverrideAddition add)
        {
            this.AddId = add.Id;
            this.Transform = add.Value.Transform;
            this.ViewName = add.Value.ViewName;
            this.FocalPoint = add.Value.Transform.Moved(new Vector3(0, -1, 0)).Origin;

            GenerateView();
            GenerateGeometry();
        }

        public bool Match(CamerasIdentity identity)
        {
            return identity.AddId == this.AddId;
        }

        public ViewCamera Update(CamerasOverride edit)
        {
            this.Transform = edit.Value.Transform;
            this.ViewName = edit.Value.ViewName;
            this.FocalPoint = edit.Value.Transform.Moved(new Vector3(0, -1, 0)).Origin;

            GenerateView();
            GenerateGeometry();

            return this;
        }

        public void GenerateView()
        {
            if (this.ViewName == "")
            {
                this.ViewName = AddId;
            }
            var angle = this.Transform.Inverted().OfPoint(this.FocalPoint).Unitized();
            var scope = new ViewScope()
            {
                BoundingBox = GetProperBounds(this.FocalPoint, this.Transform.Origin),
                Camera = new Camera(angle, null, CameraProjection.Perspective),
                ClipWithBoundingBox = false,
                LockRotation = true,
                Modal = true,
                Name = this.ViewName
            };
            this.AdditionalProperties["View Scope"] = scope;
        }

        private BBox3 GetProperBounds(Vector3 focalPoint, Vector3 origin)
        {
            // Calculate the desired radius
            double desiredRadius = focalPoint.DistanceTo(origin) / 2;

            // Multiply the desired radius by 1.25
            double adjustedRadius = desiredRadius * 1.25;

            // Calculate the direction vector from Origin to focalPoint
            Vector3 direction = (focalPoint - origin).Unitized();

            // Update the Origin to the new position
            Vector3 newOrigin = focalPoint - (direction * adjustedRadius);

            return new BBox3(new List<Vector3>() { focalPoint, newOrigin });
        }

        public void GenerateGeometry()
        {
            this.RepresentationInstances = new List<RepresentationInstance>();

            // foreach (var curveRep in CameraOutline(.25))
            // {
            //     this.RepresentationInstances.Add(new RepresentationInstance(curveRep, BuiltInMaterials.Edges));
            // }

            var contentElement = new ContentElement("https://github.com/jamesbradleym/Cameras/blob/main/dependencies/ViewScopeCamera.glb", new BBox3(), 1, Vector3.XAxis, null)
            {
                IsElementDefinition = true
            };

            contentElement.Transform.Scale(0.001);

            this.RepresentationInstances = new List<RepresentationInstance>
            {
                new(new ContentRepresentation(contentElement.GltfLocation, contentElement.BoundingBox), contentElement.Material)
            };
        }

        public List<CurveRepresentation> CameraOutline(double scale = 1.0)
        {
            var linework = new List<Line>() {
                new Line((0.875, -0.25, 0), (0.875, 0.25, 0)),
                new Line((0.875, 0.25, 0), (0.875, 0.25, 0.125)),
                new Line((0.875, 0.25, 0.125), (0.875, 0.3, 0.125)),
                new Line((0.875, 0.3, 0.125), (0.875, 0.3, 0.875)),
                new Line((0.875, 0.3, 0.875), (0.875, 0.25, 0.875)),
                new Line((0.875, 0.25, 0.875), (0.875, 0.25, 1.0)),
                new Line((0.875, 0.25, 1.0), (0.875, -0.25, 1.0)),
                new Line((0.875, -0.25, 1.0), (0.875, -0.25, 0.875)),
                new Line((0.875, -0.25, 0.875), (0.875, -0.3, 0.875)),
                new Line((0.875, -0.3, 0.875), (0.875, -0.3, 0.125)),
                new Line((0.875, -0.3, 0.125), (0.875, -0.25, 0.125)),
                new Line((0.875, -0.25, 0.125), (0.875, -0.25, 0)),


                new Line((-0.875, -0.25, 0), (-0.875, 0.25, 0)),
                new Line((-0.875, 0.25, 0), (-0.875, 0.25, 0.125)),
                new Line((-0.875, 0.25, 0.125), (-0.875, 0.3, 0.125)),
                new Line((-0.875, 0.3, 0.125), (-0.875, 0.3, 0.875)),
                new Line((-0.875, 0.3, 0.875), (-0.875, 0.25, 0.875)),
                new Line((-0.875, 0.25, 0.875), (-0.875, 0.25, 1.0)),
                new Line((-0.875, 0.25, 1.0), (-0.875, -0.25, 1.0)),
                new Line((-0.875, -0.25, 1.0), (-0.875, -0.25, 0.875)),
                new Line((-0.875, -0.25, 0.875), (-0.875, -0.3, 0.875)),
                new Line((-0.875, -0.3, 0.875), (-0.875, -0.3, 0.125)),
                new Line((-0.875, -0.3, 0.125), (-0.875, -0.25, 0.125)),
                new Line((-0.875, -0.25, 0.125), (-0.875, -0.25, 0)),

                new Line((0.875, -0.25, 0), (-0.875, -0.25, 0)),
                new Line((0.875, 0.25, 0), (-0.875, 0.25, 0)),
                new Line((0.875, 0.25, 0.125), (-0.875, 0.25, 0.125)),
                new Line((0.875, 0.3, 0.125), (-0.875, 0.3, 0.125)),
                new Line((0.875, 0.3, 0.875), (-0.875, 0.3, 0.875)),
                new Line((0.875, 0.25, 0.875), (-0.875, 0.25, 0.875)),
                new Line((0.875, -0.25, 0.875), (-0.875, -0.25, 0.875)),
                new Line((0.875, -0.3, 0.875), (-0.875, -0.3, 0.875)),
                new Line((0.875, -0.3, 0.125), (-0.875, -0.3, 0.125)),
                new Line((0.875, -0.25, 0.125), (-0.875, -0.25, 0.125)),

                new Line((0.875, -0.25, 1.0), (0.3, -0.25, 1.0)),
                new Line((0.3, -0.25, 1.0), (0.125, -0.25, 1.25)),
                new Line((0.125, -0.25, 1.25), (-0.175, -0.25, 1.25)),
                new Line((-0.175, -0.25, 1.25), (-0.425, -0.25, 1.0)),
                new Line((-0.425, -0.25, 1.0), (-0.875, -0.25, 1.0)),
                new Line((0.875, 0.25, 1.0), (0.3, 0.25, 1.0)),
                new Line((0.3, 0.25, 1.0), (0.125, 0.25, 1.25)),
                new Line((0.125, 0.25, 1.25), (-0.175, 0.25, 1.25)),
                new Line((-0.175, 0.25, 1.25), (-0.425, 0.25, 1.0)),
                new Line((-0.425, 0.25, 1.0), (-0.875, 0.25, 1.0)),

                new Line((0.3, 0.25, 1.0), (0.3, -0.25, 1.0)),
                new Line((0.125, 0.25, 1.25), (0.125, -0.25, 1.25)),
                new Line((-0.175, 0.25, 1.25), (-0.175, -0.25, 1.25)),
                new Line((-0.425, 0.25, 1.0), (-0.425, -0.25, 1.0)),


                new Line((0.038, -0.30, 0.875), (0.038, -0.334, 0.875)),
                new Line((-0.337, -0.30, 0.5), (-0.337, -0.334, 0.5)),
                new Line((0.038, -0.30, 0.125), (0.038, -0.334, 0.125)),
                new Line((0.413, -0.30, 0.5), (0.413, -0.334, 0.5)),

                new Line((0.038, -0.334, 0.85), (0.038, -0.609, 0.85)),
                new Line((-0.312, -0.334, 0.5), (-0.312, -0.609, 0.5)),
                new Line((0.038, -0.334, 0.15), (0.038, -0.609, 0.15)),
                new Line((0.388, -0.334, 0.5), (0.388, -0.609, 0.5)),

                new Line((0.038, -0.581, 0.875), (0.038, -0.650, 0.875)),
                new Line((-0.337, -0.581, 0.5), (-0.337, -0.650, 0.5)),
                new Line((0.038, -0.581, 0.125), (0.038, -0.650, 0.125)),
                new Line((0.413, -0.581, 0.5), (0.413, -0.650, 0.5)),
            };

            var circlework = new List<Circle> {
                new Circle(new Transform((0.038, -0.3, 0.5), Vector3.YAxis, 0.0), 0.375),

                new Circle(new Transform((0.038, -0.334, 0.5), Vector3.YAxis, 0.0), 0.375),
                new Circle(new Transform((0.038, -0.334, 0.5), Vector3.YAxis, 0.0), 0.35),

                new Circle(new Transform((0.038, -0.581, 0.5), Vector3.YAxis, 0.0), 0.375),
                new Circle(new Transform((0.038, -0.581, 0.5), Vector3.YAxis, 0.0), 0.35),
                new Circle(new Transform((0.038, -0.581, 0.5), Vector3.YAxis, 0.0), 0.325),
                new Circle(new Transform((0.038, -0.581, 0.5), Vector3.YAxis, 0.0), 0.3),

                new Circle(new Transform((0.038, -0.609, 0.5), Vector3.YAxis, 0.0), 0.3),
                new Circle(new Transform((0.038, -0.609, 0.5), Vector3.YAxis, 0.0), 0.35),

                new Circle(new Transform((0.038, -0.65, 0.5), Vector3.YAxis, 0.0), 0.3),
                new Circle(new Transform((0.038, -0.65, 0.5), Vector3.YAxis, 0.0), 0.375),
            };

            var lines = new List<CurveRepresentation>();
            foreach (var line in linework)
            {
                lines.Add(new CurveRepresentation(ScaleLineFromPoint(line, this.Transform.Origin, scale), true));
            }
            foreach (var circle in circlework)
            {
                lines.Add(new CurveRepresentation(ScaleCircleFromPoint(circle, this.Transform.Origin, scale), true));
            }

            return lines;
        }

        public Line ScaleLineFromPoint(Line line, Vector3 origin, double scaleFactor)
        {
            // Translate to Origin
            Vector3 translatedStart = line.Start;
            Vector3 translatedEnd = line.End;

            // Scale
            Vector3 scaledTranslatedStart = translatedStart * scaleFactor;
            Vector3 scaledTranslatedEnd = translatedEnd * scaleFactor;

            // Translate Back
            Vector3 scaledStart = scaledTranslatedStart;
            Vector3 scaledEnd = scaledTranslatedEnd;

            return new Line(scaledStart, scaledEnd);
        }

        public Circle ScaleCircleFromPoint(Circle circle, Vector3 origin, double scaleFactor)
        {
            // Translate to Origin
            Vector3 translatedStart = circle.Center;

            // Scale
            Vector3 scaledTranslatedStart = translatedStart * scaleFactor;

            // Translate Back
            Vector3 scaledStart = scaledTranslatedStart;

            double scaledRadius = circle.Radius * scaleFactor;
            return new Circle(new Transform(scaledStart, Vector3.YAxis, 0.0), scaledRadius);
        }

    }
}