using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class lookup
    {
        public long Id { get; set; }

        public int MajorCode { get; set; } 
        public int MinorCode { get; set; }

        [MaxLength(50)] 
        public string Name { get; set; }






    }
}
/*
 * 
 * 
 * 1=>Developers    // position ? Department type? 
 * 2=>manager      // position ? Department type? 
 * 3=>finance     //  position ? Department type? 
 * 4-tichnical   // position ? Department type? 
 * **********************************************************
 * 
 * 
 * 
 * 
 * 
 * */