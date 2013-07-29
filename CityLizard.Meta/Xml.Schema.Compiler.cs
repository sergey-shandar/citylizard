namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using XS = System.Xml.Schema;
    using C = System.Collections.Generic;
    using CD = System.CodeDom;
    using S = System;
    using A = System.CodeDom.MemberAttributes;

    using F = Fsm;

    using System.Linq;

    public class Compiler //: D
    {
        C.Dictionary<int, Fsm.Dfa<int>.State> dictionaryX = null;
    }
}
