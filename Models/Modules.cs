namespace Trial.Models
{
    public class Modules
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int NoOfCredits { get; set; }

        public int NoofWeeks { get; set; }

        public int ClassHrsPerWeek { get; set; }
        public int TotalhoursOfStudy { get; set; }
        public int RemainingHoursOfStudy { get; set; }
        public string Studentid { get; set; }

        public Modules()
        {

        }

        public Modules(string code, string name, int noOfCredits, int classHrsPerWeek, int totalhoursOfStudy, int remainingHoursOfStudy, string Studentid)
        {
            Code = code;
            Name = name;
            NoOfCredits = noOfCredits;
            ClassHrsPerWeek = classHrsPerWeek;
            TotalhoursOfStudy = totalhoursOfStudy;
            RemainingHoursOfStudy = remainingHoursOfStudy;
            this.Studentid = Studentid;

        }

        public override string ToString()
        {
            return Code;
        }
    }
}
