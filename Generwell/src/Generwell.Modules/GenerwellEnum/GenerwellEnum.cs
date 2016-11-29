using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.GenerwellEnum
{
    public enum GenerwellEnum
    {
        Well = 1,
        Facility = 4,
        Pipelilne = 6,
        Project = 9
    }
    public enum GenerwellEnumNumber
    {
        First = 1,
        Fourth = 4,
        Sixth = 6,
        Nineth = 9
    }

    public enum Menu
    {
        Task = 1,
        Well = 2,
        Facility = 3,
        Pipeline = 4,
        Project = 5,
        Map = 6
    }

    public enum PageOrder
    {
        Welllisting = 1,
        WellLineReports = 2,
        WellDetails = 3,
        Tasklisting = 4,
        TaskDetails = 5,
        Pipeline = 6,
        Project = 7,
        Map = 12
    }

}
