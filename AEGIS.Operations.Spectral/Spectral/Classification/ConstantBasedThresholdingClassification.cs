﻿/// <copyright file="ConstantBasedThresholdingClassification.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents a threshold based spectral classification using the specified constants.
    /// </summary>
    [OperationMethodImplementation("AEGIS::253120", "Constant based spectral thresholding")]
    public class ConstantBasedThresholdingClassification : BasicThresholdingClassification
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantBasedThresholdingClassification" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public ConstantBasedThresholdingClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantBasedThresholdingClassification" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public ConstantBasedThresholdingClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.ConstantBasedThresholdClassification, parameters)
        {
            for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
            {
                _lowerThresholdValues[bandIndex] = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.LowerThresholdBoundary));
                _upperThresholdValues[bandIndex] = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.UpperThresholdBoundary));

                if (_lowerThresholdValues[bandIndex] > _upperThresholdValues[bandIndex])
                {
                    Double temp = _upperThresholdValues[bandIndex];
                    _upperThresholdValues[bandIndex] = _lowerThresholdValues[bandIndex];
                    _lowerThresholdValues[bandIndex] = temp;
                }
            }
        }

        #endregion
    }
}
