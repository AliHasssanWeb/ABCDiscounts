using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.Shared.DataConfig
{
    public class StatesofAmerica
    {

        public List<StatesTable> Get()
        {
            List<StatesTable> states = new List<StatesTable>()
        {
                new StatesTable() {StateName = "Alabama" },
                new StatesTable() {StateName = "Alabama" },  
                new StatesTable() {StateName = "Arizona" },  
                new StatesTable() {StateName = "Arkansas" },
                new StatesTable() {StateName = "California" }, 
                new StatesTable() {StateName = "Colorado" }, 
                new StatesTable() {StateName = "Connecticut" }, 
                new StatesTable() {StateName = "Delaware" }, 
                new StatesTable() {StateName = "Florida" }, 
                new StatesTable() {StateName = "Georgia" }, 
                new StatesTable() {StateName = "Hawaii" }, 
                new StatesTable() {StateName = "Idaho" },  
                new StatesTable() {StateName = "Illinois" }, 
                new StatesTable() {StateName = "Indiana" }, 
                new StatesTable() {StateName = "Iowa" }, 
                new StatesTable() {StateName = "Kansas" }, 
                new StatesTable() {StateName = "Kentucky" },  
                new StatesTable() {StateName = "Maine" }, 
                new StatesTable() {StateName = "Maryland" }, 
                new StatesTable() {StateName = "Massachusetts[" }, 
                new StatesTable() {StateName = "Michigan" }, 
                new StatesTable() {StateName = "Mississippi" }, 
                new StatesTable() {StateName = "Missouri" }, 
                new StatesTable() {StateName = "Montana" }, 
                new StatesTable() {StateName = "Nevada" }, 
                new StatesTable() {StateName = "Nebraska" }, 
                new StatesTable() {StateName = "New Hampshire" }, 
                new StatesTable() {StateName = "New Jersey" },  
                new StatesTable() {StateName = "New Mexico" }, 
                new StatesTable() {StateName = "New York" }, 
                new StatesTable() {StateName = "North Carolina" }, 
                new StatesTable() {StateName = "North Dakota" },
                new StatesTable() {StateName = "Ohio" },
                new StatesTable() {StateName = "Oklahoma" },
                new StatesTable() {StateName = "Oregon" },
                new StatesTable() {StateName = "Pennsylvania" },
                new StatesTable() {StateName = "Rhode Island" },
                new StatesTable() {StateName = "South Dakota" },
                new StatesTable() {StateName = "Tennessee" },
                new StatesTable() {StateName = "Texas" },
                new StatesTable() {StateName = " Utah" },
                new StatesTable() {StateName = "Vermont" },
                new StatesTable() {StateName = "Virginia[" },
                new StatesTable() {StateName = "Washington" },
                new StatesTable() {StateName = "West Virginia" },
                new StatesTable() {StateName = "Wisconsin" },
                new StatesTable() {StateName = "Wyoming" },
                new StatesTable() {StateName = "Alaska" }

        };
            return states.ToList();
        }


        public class StatesTable
        {
            public string StateName { get; set; }
        }
    }
}
