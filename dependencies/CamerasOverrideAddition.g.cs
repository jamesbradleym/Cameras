using Elements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Cameras
{
	/// <summary>
	/// Override metadata for CamerasOverrideAddition
	/// </summary>
	public partial class CamerasOverrideAddition : IOverride
	{
        public static string Name = "Cameras Addition";
        public static string Dependency = null;
        public static string Context = "[*discriminator=Elements.ViewCamera]";
		public static string Paradigm = "Edit";

        /// <summary>
        /// Get the override name for this override.
        /// </summary>
        public string GetName() {
			return Name;
		}

		public object GetIdentity() {

			return Identity;
		}

	}

}