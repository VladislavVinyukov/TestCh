using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        private List<Employee> employees;
        private List<Vacation> vacations;

        public HomeController()
        {
            // Генерация случайных сотрудников
            employees = GenerateRandomEmployees(100);

            // Генерация графика отпусков
            vacations = GenerateRandomVacations(employees);
        }

        // Генерация случайных сотрудников
        private List<Employee> GenerateRandomEmployees(int count)
        {
            List<Employee> employees = new List<Employee>();
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                Employee employee = new Employee
                {
                    FullName = "Employee " + i,
                    Gender = random.Next(2) == 0 ? "Male" : "Female",
                    Position = "Position " + random.Next(10),
                    Age = random.Next(20, 60)
                };

                employees.Add(employee);
            }

            return employees;
        }

        // Генерация случайных отпусков
        private List<Vacation> GenerateRandomVacations(List<Employee> employees)
        {
            List<Vacation> vacations = new List<Vacation>();
            Random random = new Random();
            int currentYear = DateTime.Now.Year;

            foreach (Employee employee in employees)
            {
                DateTime startDate = new DateTime(currentYear, random.Next(1, 13), random.Next(1, 29));
                DateTime endDate = startDate.AddDays(random.Next(7, 15));

                Vacation vacation = new Vacation
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Employee = employee
                };

                vacations.Add(vacation);
            }

            return vacations;
        }

        // Запрос нового отпуска текущего сотрудника
        [HttpGet]
        public ActionResult NewVacation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewVacation(Vacation vacation)
        {
            // Добавление нового отпуска в список
            vacations.Add(vacation);

            return RedirectToAction("Index");
        }

        // Вывод информации о пересечениях отпусков
        public ActionResult Index()
        {
            var intersectedDepartmentVacations = vacations.Where(v => v.Employee.Age < 30);
            var intersectedFemaleVacations = vacations.Where(v => v.Employee.Age > 30 && v.Employee.Age < 50 && v.Employee.Gender == "Female");
            var intersectedSeniorVacations = vacations.Where(v => v.Employee.Age > 50);
            var nonIntersectedVacations = vacations.Except(intersectedDepartmentVacations)
                                                  .Except(intersectedFemaleVacations)
                                                  .Except(intersectedSeniorVacations);

            ViewBag.IntersectedDepartmentVacations = intersectedDepartmentVacations;
            ViewBag.IntersectedFemaleVacations = intersectedFemaleVacations;
            ViewBag.IntersectedSeniorVacations = intersectedSeniorVacations;
            ViewBag.NonIntersectedVacations = nonIntersectedVacations;

            return View();
        }
    }
}
