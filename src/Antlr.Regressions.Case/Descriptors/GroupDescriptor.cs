namespace Kingdom.Antlr.Regressions.Case
{
    public class GroupDescriptor
    {
        public string Name { get; set; }

        public string RenderString()
        {
            const string group = nameof(group);
            return $"{group} = {Name};";
        }
    }
}
