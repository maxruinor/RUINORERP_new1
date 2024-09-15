using System;

namespace SourceGrid.Cells.Models
{
    /// <summary>
    /// Interface for informations about an image.
    /// </summary>
    public interface IImage : IModel
    {
        /// <summary>
        /// Get the image of the specified cell. 
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        System.Drawing.Image GetImage(CellContext cellContext);
    }

    /// <summary>
    /// 为了实际远程图片显示，需要实现IImageWeb接口
    /// </summary>
    public interface IImageWeb : IModel
    {
        /// <summary>
        /// Get the image of the specified cell. 
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        System.Drawing.Image GetImage(CellContext cellContext);
    }

}
