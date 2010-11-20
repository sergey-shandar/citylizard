namespace CityLizard.Xml.Linked.Element
{
    /// <summary>
    /// The not mixed XML element.
    /// </summary>
    public abstract class NotMixed: NotEmpty
    {
        /// <summary>
        /// Returns Type.NotMixed.
        /// </summary>
        public override Type Type
        {
            get { return Type.NotMixed; }
        }
    }
}
