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
    /// Ϊ��ʵ��Զ��ͼƬ��ʾ����Ҫʵ��IImageWeb�ӿ�
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
