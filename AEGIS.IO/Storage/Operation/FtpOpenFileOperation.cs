﻿/// <copyright file="FtpOpenFileOperation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Marcell Lipp</author>

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represents an FTP file opening operation.
    /// </summary>
    class FtpOpenFileOperation : FtpFileSystemOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpOpenFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        public FtpOpenFileOperation(Uri path) : base(path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpOpenFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The file system authentication.</param>
        public FtpOpenFileOperation(Uri path, IFileSystemAuthentication authentication) : base(path, authentication)
        {
        }

        /// <summary>
        /// Sets the properties of the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void SetupRequest(System.Net.FtpWebRequest request)
        {
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.KeepAlive = true;
        }

        /// <summary>
        /// Creates the result of the operation.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        protected override async Task<FileSystemOperationResult> CreateResultAsync(FtpWebResponse response)
        {
            return await Task.Run(() => new StreamOperationResult(response.GetResponseStream()));
        }
    }
}
