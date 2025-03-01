﻿/// <copyright file="GeometryStreamFormats.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.IO
{
    [IdentifiedObjectCollection(typeof(GeometryStreamFormat))]
    public static class GeometryStreamFormats
    {        
        #region Query fields

        private static GeometryStreamFormat[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GeometryStreamFormat" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GeometryStreamFormat" /> instances within the collection.</value>
        public static IList<GeometryStreamFormat> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(GeometryStreamFormats).GetProperties().
                                                         Where(property => property.Name != "All").
                                                         Select(property => property.GetValue(null, null) as GeometryStreamFormat).
                                                         ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeometryStreamFormat" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="GeometryStreamFormat" /> instances that match the specified identifier.</returns>
        public static IList<GeometryStreamFormat> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }
        /// <summary>
        /// Returns all <see cref="GeometryStreamFormat" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="GeometryStreamFormat" /> instances that match the specified name.</returns>
        public static IList<GeometryStreamFormat> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => System.Text.RegularExpressions.Regex.IsMatch(alias, name, System.Text.RegularExpressions.RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static GeometryStreamFormat _shapefile;
      
        private static GeometryStreamFormat _lasfile;

        private static GeometryStreamFormat _lazfile;

        private static GeometryStreamFormat _geojsonfile;

        private static GeometryStreamFormat _topojsonfile;

        #endregion

        #region Public static fields

        /// <summary>
        /// Esri shapefile.
        /// </summary>
        public static GeometryStreamFormat Shapefile
        { 
            get
            {
                return _shapefile ?? (_shapefile =
                    new GeometryStreamFormat("AEGIS::610101", "Esri shapefile", 
                                             "© 1997, 1998 Environmental Systems Research Institute (ESRI), Inc.", new String[] { "Shapefile" }, "1.0", 
                                             new String[] { "shp" }, null, 
                                             new Type[] { typeof(IPoint), typeof(IGeometryCollection<IPoint>), typeof(ILineString), typeof(IPolygon), typeof(IGeometryCollection<IPolygon>) },
                                             new GeometryModel[] { GeometryModel.Spatial2D, GeometryModel.Spatial3D }));
            }
        }

        /// <summary>
        /// LASer file
        /// </summary>
        public static GeometryStreamFormat Lasfile
        {
            get
            {
                return _lasfile ?? (_lasfile =
                           new GeometryStreamFormat("AEGIS::610104", "LAS file",
                               "@ 2013, American Society for Photogrammetry & Remote Sensing (asprs)", null, "1.4",
                               new String[] { "las" }, null,
                               new Type[] { typeof(IPoint) },
                               new GeometryModel[] { GeometryModel.Spatial3D }));
            }
        }

        /// <summary>
        /// Compressed LASer file
        /// </summary>
        public static GeometryStreamFormat Lazfile
        {
            get
            {
                return _lazfile ?? (_lazfile =
                           new GeometryStreamFormat("AEGIS::610105", "LAZ file",
                               "LAZ is a compressed version of the LAS format", null, null,
                               new String[] { "laz" }, null,
                               new Type[] { typeof(IPoint) },
                               new GeometryModel[] { GeometryModel.Spatial3D }));
            }
        }

        /// <summary>
        /// GeoJSON file
        /// </summary>
        public static GeometryStreamFormat GeoJson
        {
            get
            {
                return _geojsonfile ?? (_geojsonfile =
                           new GeometryStreamFormat("AEGIS::610102", "GeoJSON file", "1.0", new String[] { "json", "geojson" }, new String[] { "application/vnd.geo+json" },
                               new Type[] { typeof(IPoint), typeof(IGeometryCollection<IPoint>), typeof(ILineString), typeof(IGeometryCollection<ILineString>), typeof(IPolygon), typeof(IGeometryCollection<IPolygon>), typeof(IGeometryCollection<IGeometry>) },
                               new GeometryModel[] { GeometryModel.Spatial2D, GeometryModel.Spatial3D }));
            }
        }

        /// <summary>
        /// TopoJSON file
        /// </summary>
        public static GeometryStreamFormat TopoJson
        {
            get
            {
                return _topojsonfile ?? (_topojsonfile =
                           new GeometryStreamFormat("AEGIS::610103", "TopoJSON file", "1.0", new String[] { "json", "topojson" }, new String[] { "application/vnd.topo+json" },
                               new Type[] { typeof(IPoint), typeof(IGeometryCollection<IPoint>), typeof(ILineString), typeof(IGeometryCollection<ILineString>), typeof(IPolygon), typeof(IGeometryCollection<IPolygon>), typeof(IGeometryCollection<IGeometry>) },
                               new GeometryModel[] { GeometryModel.Spatial2D, GeometryModel.Spatial3D }));
            }
        }

        #endregion
    }
}
