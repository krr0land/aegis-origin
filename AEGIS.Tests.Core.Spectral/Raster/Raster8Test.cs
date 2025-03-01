﻿/// <copyright file="Raster8Test.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Raster;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Collections.Spectral
{
    /// <summary>
    /// Test fixture for the <see cref="Raster8"/> class.
    /// </summary>
    [TestFixture]
    public class Raster8Test
    {
        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void Raster8ConstructorTest()
        {
            // normal parameters

            for (Int32 numberOfBands = 0; numberOfBands < 10; numberOfBands++)
                for (Int32 numberOfRows = 0; numberOfRows < 1000; numberOfRows += 250)
                    for (Int32 numberOfColumns = 0; numberOfColumns < 1000; numberOfColumns += 250)
                    {
                        Raster8 raster = new Raster8(null, numberOfBands, numberOfRows, numberOfColumns, 8, null);

                        Assert.AreEqual(numberOfBands, raster.NumberOfBands);
                        Assert.AreEqual(numberOfRows, raster.NumberOfRows);
                        Assert.AreEqual(numberOfColumns, raster.NumberOfColumns);
                        Assert.AreEqual(8, raster.RadiometricResolution);
                        Assert.AreEqual(RasterFormat.Integer, raster.Format);
                        Assert.IsFalse(raster.IsMapped);
                        Assert.IsTrue(raster.IsReadable);
                        Assert.IsTrue(raster.IsWritable);
                        Assert.IsInstanceOf<RasterFactory>(raster.Factory);
                        Assert.IsTrue(raster.HistogramValues.All(values => values.Sum() == raster.NumberOfColumns * raster.NumberOfRows && values[0] == raster.NumberOfColumns * raster.NumberOfRows));
                    }


            // argument out of range exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, -1, 1, 1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, -1, 1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, -1, 1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, -1, null); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { Raster8 raster = new Raster8(null, 1, 1, 1, 65, null); });
        }

        /// <summary>
        /// Test case for value setting and getting.
        /// </summary>
        [Test]
        public void Raster8ValueTest()
        {
            Raster8 raster = new Raster8(null, 3, 9, 27, 8, null);

            // single values

            for (Int32 bandIndex = 0; bandIndex < raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                    {
                        raster.SetValue(rowIndex, columnIndex, bandIndex, (UInt32)(bandIndex * rowIndex * columnIndex));

                        Assert.AreEqual((bandIndex * rowIndex * columnIndex) % (Byte.MaxValue + 1), raster.GetValue(rowIndex, columnIndex, bandIndex), 0);
                    }


            // test case 2: multiple values

            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                {
                    raster.SetValues(rowIndex, columnIndex, new UInt32[] { (UInt32)rowIndex, (UInt32)columnIndex, (UInt32)(rowIndex * columnIndex) });

                    Assert.AreEqual(rowIndex, raster.GetValues(rowIndex, columnIndex)[0], 0);
                    Assert.AreEqual(columnIndex, raster.GetValues(rowIndex, columnIndex)[1], 0);
                    Assert.AreEqual(rowIndex * columnIndex, raster.GetValues(rowIndex, columnIndex)[2], 0);
                }


            // test case 3: argument out of range exceptions

            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(-1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(raster.NumberOfRows, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, raster.NumberOfColumns, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.GetValue(0, 0, raster.NumberOfBands));

            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(-1, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(raster.NumberOfRows, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, -1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, raster.NumberOfColumns, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, 0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => raster.SetValue(0, 0, raster.NumberOfBands, 0));
        }

        /// <summary>
        /// Test case for the histogram.
        /// </summary>
        [Test]
        public void Raster8HistogramTest()
        {
            Int32[] histogramValues = new Int32[Byte.MaxValue + 1];
            Random random = new Random();

            Raster8 raster = new Raster8(null, 1, 100, 100, 8, null);
            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                {
                    Byte value = (Byte)random.Next();
                    raster.SetValue(rowIndex, columnIndex, 0, value);

                    histogramValues[value]++;
                }

            for (Int32 valueIndex = 0; valueIndex < raster.GetHistogramValues(0).Count; valueIndex++)
            {
                Assert.AreEqual(histogramValues[valueIndex], raster.GetHistogramValues(0)[valueIndex]);
            }
        }

        #endregion
    }
}
