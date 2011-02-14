using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace graphml_graphdrawing_org.xmlns.Example
{
    class Program
    {
        class Generator : X
        {
            public T.graphml Generate()
            {
                return
                    graphml
                        [key_(id: "d0")]
                        [graph_(edgedefault: "directed")
                            [desc_("XXX")]
                        ];
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
