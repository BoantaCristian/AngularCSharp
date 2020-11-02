using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class Details
    {
        [Key]
        public int Id { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Age { get; set; }
        public string BodyType { get; set; }
        public double MassIndex { get; set; }
        public string BodyState { get; set; }
        public double IdealWeight { get; set; }
        public string Activity { get; set; }
        public double BasalMetabolism { get; set; }
        public double CaloricRatio { get; set; }
        public double Glucid { get; set; }
        public double Lipid { get; set; }
        public double Protein { get; set; }
        public double GlucidGrams { get; set; }
        public double LipidGrams { get; set; }
        public double ProteindGrams { get; set; }
        public double Breakfast { get; set; }
        public double Snack1 { get; set; }
        public double Launch { get; set; }
        public double Snack2 { get; set; }
        public double Dinner { get; set; }
        public double BreakfastGlucid { get; set; }
        public double Snack1Glucid { get; set; }
        public double LaunchGlucid { get; set; }
        public double Snack2Glucid { get; set; }
        public double DinnerGlucid { get; set; }
        public double BreakfastLipid { get; set; }
        public double Snack1Lipid { get; set; }
        public double LaunchLipid { get; set; }
        public double Snack2Lipid { get; set; }
        public double DinnerLipid { get; set; }
        public double BreakfastProtein { get; set; }
        public double Snack1Protein { get; set; }
        public double LaunchProtein { get; set; }
        public double Snack2Protein { get; set; }
        public double DinnerProtein { get; set; }
        public virtual User User { get; set; }
    }
}
