using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using servicio.Data;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly MyAppContext myAppContext;

        public AnalyticsController(MyAppContext myAppContext)
        {
            this.myAppContext = myAppContext;
        }

        [HttpGet("genderCounts")]
        public IActionResult GetGenderCounts()
        {
            var maleStudents = myAppContext.Students.Count(s => s.Gender == "varon");
            var femaleStudents = myAppContext.Students.Count(s => s.Gender == "mujer");
            var maleGuardians = myAppContext.LegalGuardians.Count(g => g.Gender == "varon");
            var femaleGuardians = myAppContext.LegalGuardians.Count(g => g.Gender == "mujer");

            return Ok(new
            {
                MaleStudents = maleStudents,
                FemaleStudents = femaleStudents,
                MaleGuardians = maleGuardians,
                FemaleGuardians = femaleGuardians
            });
        }

        [HttpGet("registeredStudents")]
        public IActionResult GetStudentsCounts()
        {
            var totalStudents = myAppContext.Students.Count();

            var currentDate = DateTime.Now;
            var startDate = currentDate.AddMonths(-8);
            var months = new List<DateTime>();

            for (int i = 7; i >= 0; i--)
            {
                months.Add(currentDate.AddMonths(-i).Date);
            }

            var monthlyCounts = months.Select(month =>
            {
                var studentCount = myAppContext.Students
                    .Where(s => s.CreatedAt.Year == month.Year && s.CreatedAt.Month == month.Month)
                    .Count();

                return new MonthlyCountDTO
                {
                    Year = month.Year,
                    Month = month.Month,
                    Count = studentCount
                };
            }).ToList();

            var firstMonthCount = monthlyCounts.FirstOrDefault()?.Count ?? 0;
            var lastMonthCount = monthlyCounts.LastOrDefault()?.Count ?? 0;

            decimal percentageChange = 0;
            if (firstMonthCount > 0)
            {
                percentageChange = (lastMonthCount-firstMonthCount/firstMonthCount) * 100;
            }
            else
            {
                percentageChange = 0;
            }

            var result = new StatisticsDTO
            {
                Total = totalStudents,
                PercentageChange = percentageChange,
                MonthlyCounts = monthlyCounts
            };

            return Ok(result);
        }

        [HttpGet("registeredTeachers")]
        public IActionResult GetTeachersCounts()
        {
            var totalTeachers = myAppContext.Docentes.Where(t => t.Estado == true).Count();

            var currentDate = DateTime.Now;
            var startDate = currentDate.AddMonths(-8);
            var months = new List<DateTime>();

            for (int i = 7; i >= 0; i--)
            {
                months.Add(currentDate.AddMonths(-i).Date);
            }

            var monthlyCounts = months.Select(month =>
            {
                var teachersCount = myAppContext.Docentes
                    .Where(s => s.CreatedAt.Year == month.Year && s.CreatedAt.Month == month.Month && s.Estado == true)
                    .Count();

                return new MonthlyCountDTO
                {
                    Year = month.Year,
                    Month = month.Month,
                    Count = teachersCount
                };
            }).ToList();

            var firstMonthCount = monthlyCounts.FirstOrDefault()?.Count ?? 0;
            var lastMonthCount = monthlyCounts.LastOrDefault()?.Count ?? 0;

            decimal percentageChange = 0;
            if (firstMonthCount > 0)
            {
                percentageChange = (lastMonthCount - firstMonthCount / firstMonthCount) * 100;
            }
            else
            {
                percentageChange = 0;
            }

            var result = new StatisticsDTO
            {
                Total = totalTeachers,
                PercentageChange = percentageChange,
                MonthlyCounts = monthlyCounts
            };

            return Ok(result);
        }

        [HttpGet("registeredStudentsByGender")]
        public IActionResult GetStudentsCountsByGender()
        {
            var totalStudents = myAppContext.Students.Count();

            var currentDate = DateTime.Now;
            var startDate = currentDate.AddMonths(-8);
            var months = new List<DateTime>();

            for (int i = 8; i >= 0; i--)
            {
                months.Add(currentDate.AddMonths(-i).Date);
            }

            var malemonthlyCounts = months.Select(month =>
            {
                var maleStudentCount = myAppContext.Students
                    .Where(s => s.CreatedAt.Year == month.Year && s.CreatedAt.Month == month.Month && s.Gender == "varon")
                    .Count();

                var femaleStudentCount = myAppContext.Students
                    .Where(s => s.CreatedAt.Year == month.Year && s.CreatedAt.Month == month.Month && s.Gender == "mujer")
                    .Count();

                return new MonthlyCountDTO
                {
                    Year = month.Year,
                    Month = month.Month,
                    Count = maleStudentCount
                };
            }).ToList();

            var femalemonthlyCounts = months.Select(month =>
            {
                var femaleStudentCount = myAppContext.Students
                    .Where(s => s.CreatedAt.Year == month.Year && s.CreatedAt.Month == month.Month && s.Gender == "mujer")
                    .Count();

                return new MonthlyCountDTO
                {
                    Year = month.Year,
                    Month = month.Month,
                    Count = femaleStudentCount
                };
            }).ToList();

            var result = new MonthlyGenderStudentCountDTO
            {
                Total = totalStudents,
                MaleStudentCount = malemonthlyCounts,
                FemaleStudentCount = femalemonthlyCounts
            };

            return Ok(result);
        }

    }
}
