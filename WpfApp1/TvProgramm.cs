namespace TVProgram
{
    public class TvProgramm
    {
        private string date;
        private string program;
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        public string Program
        {
            get { return program; }
            set { program = value; }
        }

        public TvProgramm(string Date, string Program)
        {
            this.Date = Date;
            this.Program = Program;
        }
    }
}

