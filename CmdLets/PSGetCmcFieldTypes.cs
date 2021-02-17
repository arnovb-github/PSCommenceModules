using PoshCommence.Base;
using System.Management.Automation;
using Vovin.CmcLibNet.Database;

namespace PoshCommence.CmdLets
{

    [Cmdlet(VerbsCommon.Get, "CmcFieldTypes")]
    public class GetCmcFieldTypes : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            foreach (var o in EnumHelper.ListEnum<CommenceFieldType>())
            {
                WriteObject(o);
            }
        }
    }
}