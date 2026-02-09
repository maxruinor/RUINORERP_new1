using System;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// A lightweight wrapper to hold either binary image data or a path to an image.
    /// This enables two storage modes for an image cell: in-memory binary data or a path reference.
    /// </summary>
    [Serializable]
    public class ImageCellValue
    {
        // In-memory image data (preferred for editing/display)
        public byte[] ImageData { get; set; }

        // Path/identifier to the image (used for legacy or external storage)
        public string ImagePath { get; set; }

        // Convenience: is the value currently storing binary data?
        public bool IsBinary => ImageData != null && ImageData.Length > 0;
    }
}
