namespace TVProgram
{
    public class Channel
    {
        private string id;
        private string name;
        private string logo;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Logo
        {
            get { return logo; }
            set { logo = value; }
        }
        public Channel(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
            this.Logo = "http://www.americanlayout.com/wp/wp-content/uploads/2012/08/C-To-Go-300x300.png";
        }
        public Channel(string Id, string Name, string Logo)
        {
            this.Id = Id;
            this.Name = Name;
            this.Logo = Logo;
        }
    }
}