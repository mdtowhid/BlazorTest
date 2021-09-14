using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Test.Pages
{
    public class DisplayEmployeeBase : ComponentBase
    {
        public Employee Employee { get; set; }
        public List<Employee> Employees = new List<Employee>();
        const string noteKey = "note";
        public string Text {get; set;}
        public int t { get; set; }
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public ILocalStorageService localStore { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Id = Id ?? "1";
            t = Convert.ToInt32(Id);

            Text = await localStore.GetItemAsync<string>(noteKey);
            if (Text != null)
            {
                Employees = JsonConvert.DeserializeObject<List<Employee>>(Text);

                if (Employees.Count > 0)
                {
                    foreach (Employee emp in Employees)
                    {
                        if (emp.Id == t)
                        {
                            Employee = emp;
                            break;
                        }
                    }
                }
            }
        }
    }
}
