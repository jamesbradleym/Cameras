using Elements;
using Elements.Geometry;
using System.Collections.Generic;

namespace Cameras
{
  public static class Cameras
  {
    /// <summary>
    /// The Cameras function.
    /// </summary>
    /// <param name="model">The input model.</param>
    /// <param name="input">The arguments to the execution.</param>
    /// <returns>A CamerasOutputs instance containing computed results and the model with any new elements.</returns>
    public static CamerasOutputs Execute(Dictionary<string, Model> inputModels, CamerasInputs input)
    {
      // Your code here.
      var output = new CamerasOutputs();

      var cameras = input.Overrides.Cameras.CreateElements(
          input.Overrides.Additions.Cameras,
          input.Overrides.Removals.Cameras,
          (add) => new ViewCamera(add),
          (camera, identity) => camera.Match(identity),
          (camera, edit) => camera.Update(edit)
      );

      output.Model.AddElements(cameras);

      var viewScopes = cameras
        .Where(x => x.AdditionalProperties.ContainsKey("View Scope") && x.AdditionalProperties["View Scope"] is ViewScope)
        .Select(x => x.AdditionalProperties["View Scope"] as ViewScope)
        .ToList() as List<ViewScope>;

      output.Model.AddElements(viewScopes);

      return output;
    }
  }
}