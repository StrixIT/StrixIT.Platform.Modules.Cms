#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IImageConverter.cs" company="StrixIT">
// Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

#endregion Apache License

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the image services that can be used by the platform.
    /// </summary>
    public interface IImageConverter
    {
        #region Public Methods

        /// <summary>
        /// Gets the path for a thumbnail, creating the thumbnail if it does not yet exist.
        /// </summary>
        /// <param name="path">The image path</param>
        /// <param name="width">The thumbnail width</param>
        /// <param name="height">The thumbnail height</param>
        /// <returns>The thumbnail path</returns>
        string GetThumbPath(string path, int width, int height);

        /// <summary>
        /// Converts an image to a Base64 string.
        /// </summary>
        /// <param name="path">The image file path</param>
        /// <param name="width">The image width to resize to, if appliccable</param>
        /// <param name="height">The image height to resize to, if appliccable</param>
        /// <param name="keepAspectRatio">
        /// True if the apsect ratio for the image should be kept intact, false otherwise
        /// </param>
        /// <returns>The Base64 string for the image</returns>
        string ImageAsBase64(string path, int width, int height, bool keepAspectRatio = true);

        /// <summary>
        /// Gets the path to an image, creating a thumbnail when it does not yet exists.
        /// </summary>
        /// <param name="path">The image file path</param>
        /// <param name="width">The image width to resize to, if appliccable</param>
        /// <param name="height">The image height to resize to, if appliccable</param>
        /// <param name="keepAspectRatio">
        /// True if the apsect ratio for the image should be kept intact, false otherwise
        /// </param>
        /// <returns>The Base64 string for the image</returns>
        string ImageAsPath(string path, int width, int height, bool keepAspectRatio = true);

        /// <summary>
        /// Resizes an image.
        /// </summary>
        /// <param name="path">The image file path</param>
        /// <param name="width">The image width to resize to, if appliccable</param>
        /// <param name="height">The image height to resize to, if appliccable</param>
        /// <param name="overwrite">
        /// True if the image should be overwritten by the resized image, false otherwise
        /// </param>
        /// <param name="keepAspectRatio">
        /// True if the apsect ratio for the image should be kept intact, false otherwise
        /// </param>
        /// <returns>The resized image</returns>
        byte[] Resize(string path, int width, int height, bool overwrite = false, bool keepAspectRatio = true);

        #endregion Public Methods
    }
}