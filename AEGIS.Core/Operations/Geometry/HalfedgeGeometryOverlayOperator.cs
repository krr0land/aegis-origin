﻿/// <copyright file="HalfedgeGeometryOverlayOperator.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Topology;

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents an operator computing spatial overlays using the halfedge topology model.
    /// </summary>
    public class HalfedgeGeometryOverlayOperator : IGeometryOverlayOperator
    {
        // Source: http://edndoc.esri.com/arcsde/9.0/general_topics/understand_spatial_relations.htm

        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private readonly IGeometryFactory _geometryFactory;

        /// <summary>
        /// Identifiers in the first operand.
        /// </summary>
        private ISet<Int32> _aIdentifiers;

        /// <summary>
        /// Identifiers in the second operand.
        /// </summary>
        private ISet<Int32> _bIdentifiers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HalfedgeGeometryOverlayOperator" /> class.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        public HalfedgeGeometryOverlayOperator(IGeometryFactory factory = null)
        {
            _geometryFactory = factory;
        }

        #endregion

        #region IGeometryOverlayOperator methods

        /// <summary>
        /// Computes the difference between the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The difference between the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry Difference(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            try
            {
                return CreateResult(geometry, otherGeometry, face => face.Identifiers.All(id => !_bIdentifiers.Contains(id)));
            }
            catch (NotSupportedException) 
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Computes the intersection of the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The intersection of the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry Intersection(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            // check whether the envelopes are disjoint
            if (geometry.Envelope.Disjoint(otherGeometry.Envelope))
                return null;

            try
            {
                return CreateResult(geometry, otherGeometry, face => 
                    face.Identifiers.Any(id => _aIdentifiers.Contains(id)) &&
                    face.Identifiers.Any(id => _bIdentifiers.Contains(id)));
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Computes the symmetric difference between the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The symmetric difference between the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry SymmetricDifference(IGeometry geometry, IGeometry otherGeometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            try
            {
                return CreateResult(geometry, otherGeometry, 
                    face => face.Identifiers.All(id => _aIdentifiers.Contains(id) && !_bIdentifiers.Contains(id)) ||
                            face.Identifiers.All(id => !_aIdentifiers.Contains(id) && _bIdentifiers.Contains(id)));
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
            }
        }

        /// <summary>
        /// Computes the union of the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The union of the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry Union(IGeometry geometry, IGeometry otherGeometry)
        {

            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (otherGeometry == null)
                throw new ArgumentNullException("otherGeometry", "The other geometry is null.");

            try
            {
                return CreateResult(geometry, otherGeometry, face => true);
            }
            catch (NotSupportedException)
            {
                throw new NotSupportedException("The operation is not supported with the specified geometry type.");
            }
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates the result of the overlay operation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <param name="predicate">The overlay predicate.</param>
        /// <returns>The result of the overlay.</returns>
        private IGeometry CreateResult(IGeometry geometry, IGeometry otherGeometry, Func<IFace, Boolean> predicate)
        {
            // merge the geometries into a single graph
            HalfedgeGraph graph = new HalfedgeGraph(new HalfedgeGraph.FixedIdentifierProvider(1));
            graph.MergeGeometry(geometry);
            _aIdentifiers = new HashSet<Int32>(graph.Vertices.SelectMany(v => v.Identifiers));

            HalfedgeGraph otherGraph = new HalfedgeGraph(new HalfedgeGraph.FixedIdentifierProvider(2));
            otherGraph.MergeGeometry(otherGeometry);
            _bIdentifiers = new HashSet<Int32>(otherGraph.Vertices.SelectMany(v => v.Identifiers));

            graph.MergeGraph(otherGraph);

            // query the results
            IGeometryFactory factory = _geometryFactory ?? geometry.Factory;
            List<IPolygon> result = graph.Faces.Where(predicate).Select(face => face.ToGeometry(factory)).ToList();

            if (result.Count == 0)
                return null;
            if (result.Count == 1)
                return result[0];

            return factory.CreateGeometryCollection(result);
        }

        #endregion
    }
}
