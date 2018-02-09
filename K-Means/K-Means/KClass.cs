// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KClass.cs" company="no">
// no  
// </copyright>
// <summary>
//   Defines the KClass type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace K_Means
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// The k class.
    /// </summary>
    public class KClass
    {
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public List<Point> Points { get; set; }

        /// <summary>
        /// Gets or sets the center.
        /// </summary>
        public Point Center { get; set; }

        /// <summary>
        /// Gets or sets the far.
        /// </summary>
        public Point Far { get; set; }

        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        public double Max { get; set; }
    }
}