using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.GraphML.Example
{
    using G = graphml_graphdrawing_org.xmlns;

    class Program
    {
        class Generator : G.X
        {
            public G.X.T.graphml Generate()
            {
                return
                    graphml
                        [key_(id: "d0")]
                        [graph_(edgedefault: "directed")
                            [desc_()]
                        ];
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
