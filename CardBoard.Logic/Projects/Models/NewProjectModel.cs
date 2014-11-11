using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Fields;

namespace CardBoard.Projects.Models
{
    public class NewProjectModel
    {
        private Independent<string> _identifier = new Independent<string>();

        public string Identifier
        {
            get { return _identifier.Value; }
            set { _identifier.Value = value; }
        }

        public bool Valid
        {
            get { return NormalizeIdentifier().Length > 3; }
        }

        public string NormalizeIdentifier()
        {
            return new string(Identifier
                .Where(c => char.IsLetterOrDigit(c))
                .Select(c => char.ToLower(c))
                .ToArray());
        }
    }
}
