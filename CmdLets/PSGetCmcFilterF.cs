using PoshCommence.Base.Extensions;
using System;
using System.Management.Automation;
using Vovin.CmcLibNet.Attributes;
using Vovin.CmcLibNet.Database;

namespace PoshCommence.CmdLets
{
    [Obsolete]
    [Cmdlet(VerbsCommon.Get, "CmcFilterF")]
    internal class GetFilterF  : PSCmdlet //internal because I do not want to expose this but keep the code for now
    {
        private int clauseNumber;
        [Parameter(Position = 0, Mandatory = true)]
        public int ClauseNumber
        {
            get { return clauseNumber; }
            set { clauseNumber = value; }
        }

        // we cannot autocomplete here because we do not know what category
        // we are working in
        // We could display all database fields but that's probably worse than no auto-complete.
        private string fieldName;
        [Parameter(Position = 1, Mandatory = true)]
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        private FilterQualifier qualifier;
        [Parameter(Position = 2, Mandatory = true)]
        public FilterQualifier Qualifier
        {
            get { return qualifier; }
            set { qualifier = value; }
        }

        private string fieldValue;
        [Parameter(Position = 3, Mandatory = true)]
        public string FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
        
        private string fieldValue2;
        [Parameter(Position = 4, Mandatory = false)]
        public string FieldValue2
        {
            get { return fieldValue2; }
            set { fieldValue2 = value; }
        }

        private bool matchCase;
        [Parameter()]
        public SwitchParameter MatchCase
        {
            get { return matchCase; }
            set { matchCase = value; }
        }

        private bool except;
        [Parameter()]
        public SwitchParameter Except
        {
            get { return except; }
            set { except = value; }
        }

        private bool orFilter;
        [Parameter()]
        public SwitchParameter OrFilter
        {
            get { return orFilter; }
            set { orFilter = value; }
        }
        
        protected override void ProcessRecord()
        {
            ICursorFilterTypeF f = new CursorFilterTypeF(clauseNumber);
            f.Except = except;
            f.FieldName = fieldName;
            f.Qualifier = qualifier;
            
            if (qualifier == FilterQualifier.Between)
            {
                f.FilterBetweenStartValue = fieldValue;
                f.FilterBetweenEndValue = fieldValue2;
            }
            if (qualifier.GetAttribute<FilterValuesAttribute>()?.Number == 1)
            {
                f.FieldValue = fieldValue;
            }
            f.MatchCase = matchCase;
            f.OrFilter = orFilter;
            WriteVerbose($"Resulting filterstring is: {f}, conjunction is: [{(f.OrFilter ?  "OR" : "AND")}]");
            WriteObject(f, false);
        }
    }

}