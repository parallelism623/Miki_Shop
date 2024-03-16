using Microsoft.AspNetCore.Authorization;

namespace Ntier.BLL.Extentions
{
    public class RequirementAuthorizationExtensions : IAuthorizationRequirement
    {
        public string _param { set; get; } = null;
        public IEnumerable<string> _params { get; set; } = null;
        public RequirementAuthorizationExtensions(string param) {
            var subStr = param.Split(",").ToList();
            if (subStr.Count() == 1)
            {
                _param = subStr[0];
            }
            if (subStr.Count() > 1)
            {
                _params = subStr;
            }
        }

    }
}
