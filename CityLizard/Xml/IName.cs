﻿namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    public interface IName : INode
    {
        string Namespace { get; }
        string Name { get; }
    }
}