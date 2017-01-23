using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP {
    public class SolverStats {

        public long NumberSolutions {get; set; }
        public long Failures { get; set; }
        public long Branches { get; set; }
        public long WallTime { get; set; }

    }
}
