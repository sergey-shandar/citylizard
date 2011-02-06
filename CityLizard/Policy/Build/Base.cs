namespace CityLizard.Policy.Build
{
    using C = CodeDom;

    public class Base: C.Code
    {
        public void Do()
        {
            var u = 
                Unit()
                    [Namespace("CityLizard.Policy")
                        [Type("Base", IsStruct: true)]
                    ];
        }
    }
}
