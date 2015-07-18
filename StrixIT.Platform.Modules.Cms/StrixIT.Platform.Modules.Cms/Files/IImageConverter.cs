//-----------------------------------------------------------------------
// <copyright file="IImageConverter.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the image services that can be used by the platform.
    /// </summary>
    public interface IImageConverter
    {
        /// <summary>
        /// Gets the path for a thumbnail, creating the thumbnail if it does not yet exist.
        /// </summary>
        /// <param name="path">The image path</param>
        /// <param name="width">The thumbnail width</param>
        /// <param name="height">The thumbnail height</param>
        /// <returns>The thumbnail path</returns>
        string GetThumbPath(string path, int width, int height);

        /// <summary>
        /// Gets the path to an image, creating a thumbnail when it does not yet exists.
        /// </summary>
        /// <param name="path">The image file path</param>
        /// <param name="width">The image width to resize to, if appliccable</param>
        /// <param name="height">The image height to resize to, if appliccable</param>
        /// <param name="keepAspectRatio">True if the apsect ratio for the image should be kept intact, false otherwise</param>
        /// <returns>The Base64 string for the image</returns>
        string ImageAsPath(string path, int width, int height, bool keepAspectRatio = true);

        /// <summary>
        /// Converts an image to a Base64 string.
        /// </summary>
        /// <param name="path">The image file path</param>
        /// <param name="width">The image width to resize to, if appliccable</param>
        /// <param name="height">The image height to resize to, if appliccable</param>
        /// <param name="keepAspectRatio">True if the apsect ratio for the image should be kept intact, false otherwise</param>
        /// <returns>The Base64 string for the image</returns>
        string ImageAsBase64(string path, int width, int height, bool keepAspectRatio = true);

        /// <summary>
        /// Resizes an image.
        /// </summary>
        /// <param name="path">The image file path</param>
        /// <param name="width">The image width to resize to, if appliccable</param>
        /// <param name="height">The image height to resize to, if appliccable</param>
        /// <param name="overwrite">True if the image should be overwritten by the resized image, false otherwise</param>
        /// <param name="keepAspectRatio">True if the apsect ratio for the image should be kept intact, false otherwise</param>
        /// <returns>The resized image</returns>
        byte[] Resize(string path, int width, int height, bool overwrite = false, bool keepAspectRatio = true);
    }
}