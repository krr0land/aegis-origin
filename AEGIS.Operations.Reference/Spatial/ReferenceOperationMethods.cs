﻿/// <copyright file="ReferenceOperationMethods.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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

namespace ELTE.AEGIS.Operations.Spatial
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationMethod" /> instances for reference operations.
    /// </summary>
    [IdentifiedObjectCollection(typeof(OperationMethod))]
    public static class ReferenceOperationMethods
    {
        #region Query fields

        private static OperationMethod[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationMethod" /> instances in the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationMethod" /> instances in the collection.</value>
        public static IList<OperationMethod> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(ReferenceOperationMethods).GetProperties().
                                                    Where(property => property.Name != "All").
                                                    Select(property => property.GetValue(null, null) as OperationMethod).
                                                    ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified identifier.</returns>
        public static IList<OperationMethod> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified name.</returns>
        public static IList<OperationMethod> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static OperationMethod _referenceTransformation;

        #endregion

        #region Public static properties

        /// <summary>
        /// Reference system transformation.
        /// </summary>
        public static OperationMethod ReferenceTransformation
        {
            get
            {
                return _referenceTransformation ?? (_referenceTransformation =
                    new OperationMethod("AEGIS::212901", "Reference system transformation",
                                        "Transforms the specified geometry the a new reference system.", null, "1.0",
                                        false,
                                        typeof(IGeometry),
                                        typeof(IGeometry),
                                        GeometryModel.Spatial2D | GeometryModel.Spatial3D | GeometryModel.SpatioTemporal2D | GeometryModel.SpatioTemporal3D,
                                        ExecutionMode.OutPlace,
                                        ExecutionDomain.Local | ExecutionDomain.Remote | ExecutionDomain.External,
                                        ReferenceOperationParameters.TargetReferenceSystem,
                                        OperationParameters.GeometryFactory,
                                        OperationParameters.MetadataPreservation));
            }
        }

        #endregion
    }
}
