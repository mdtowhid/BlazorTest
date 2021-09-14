using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Blazor.Test.Pages
{
    public class IndexBase : ComponentBase
    {
        
        public Employee Employee { get; set; } = new Employee();
        public List<Employee> Employees = new List<Employee>();
        const string noteKey = "note";
        string noteContent;
        protected void HandleFormSubmit()
        {
            Employee.Name = "Nameeeee";
        }

        public string ErrorMessage { get; set; } = "";

        public void AddEmployee()
        {
            Random random = new Random();
            Employee.Id = random.Next(1, 50);
            var x = Employee.DateTime1.Value.Date;
            var y = Employee.DateTime2.Value.Date;
            if(y < x)
            {
                ErrorMessage = $"DateTime 1 cannot be less than Datetime 2 X={x} and Y={y}";
            }
            else
            {
                Employees.Add(Employee);
                Employee = new Employee();
                ErrorMessage = "";
                UpdateLocalStorage();
            }
        }

        public async void DeleteEmployeeRow(int id)
        {
            Employee employee = null;
            foreach (Employee emp in Employees)
            {
                if(emp.Id == id)
                {
                    employee = emp;
                    break;
                }
            }
            Employees.Remove(employee);
            UpdateLocalStorage();
            GetLocalStorageData();
        }

        

        [Inject]
        public ILocalStorageService localStore { get; set; }

        public async void UpdateLocalStorage()
        {
            if (Employees.Count > 0)
            {
                var json = JsonConvert.SerializeObject(Employees);
                await localStore.SetItemAsync(noteKey, json);
            }
        }

        public async void GetLocalStorageData()
        {
            var d = await localStore.GetItemAsync<string>(noteKey);
            if (d != null)
            {
                Employees = JsonConvert.DeserializeObject<List<Employee>>(d);
            }
        }

        protected async override Task OnInitializedAsync()
        {
            var d = await localStore.GetItemAsync<string>(noteKey);
            if (d != null)
            {
                Employees = JsonConvert.DeserializeObject<List<Employee>>(d);
            }
        }
    }
}
