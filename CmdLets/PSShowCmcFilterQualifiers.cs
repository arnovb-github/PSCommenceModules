using PSCommenceModules.Base;
using System.Management.Automation;
using Vovin.CmcLibNet.Database;

namespace PSCommenceModules.CmdLets
{

    [Cmdlet(VerbsCommon.Show, "CmcFilterQualifiers")]
    public class ShowCmcFilterQualifiers : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            foreach (var o in EnumHelper.ListEnum<FilterQualifier>())
            {
                WriteObject(o);
            }
        }
    }
}