﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holorama.Logic.Abstract
{
    /// <summary>
    /// General options for space division. See <see cref="IFactory{T}"/> and <see cref="ISpaceDivision"/>.
    /// </summary>
    public class SpaceDivisionOptions
    {
        /// <summary>
        /// Size in target image units of largest desired area of division
        /// </summary>
        public double? LargestAreaSize { get; set; }

        /// <summary>
        /// Size in target image units of smallest desired area of division
        /// </summary>
        public double? SmallestAreaSize { get; set; }

    }
}
