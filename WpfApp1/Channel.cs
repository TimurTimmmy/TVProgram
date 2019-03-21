namespace TVProgram
{
    class Channel
    {
        public Channel(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}