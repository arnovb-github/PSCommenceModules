﻿using PoshCommence.Base;
using PoshCommence.Base.Extensions;
using System;
using System.Management.Automation;
using Vovin.CmcLibNet.Attributes;
using Vovin.CmcLibNet.Database;

namespace PoshCommence.CmdLets
{
    /* A more generalized cmdlet for creating filters, aka with a single cmdlet */
    /* this uses switch parameters for every filter type
     * This approach comes very close to the dynamic parameter approach,
     * but until a user specifies any of the switch parameters,
     * he will see all the parameters in all parameter sets.
     * There is no intuitive way of telling him he needs to set one of them first
     * This is suboptimal UX
     * It is also a little hacky, because we recreate the enum as separate switch parameters
     * There is also the problem of not having argument completion because you cannot specify those
     * to be specfic to a parameter set
     */
    [Obsolete]
    [Cmdlet(VerbsCommon.Get, "CmcFilter3", DefaultParameterSetName = FilterTypeParameterSetName.F)]
    internal class GetFilter3 : PSCmdlet //internal because we do not want to expose this, but keep the code
    {
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.F)]
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTI)]
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCTI)]
        private int clauseNumber;
        [Parameter(Position = 0, Mandatory = true)]
        public int ClauseNumber
        {
            get { return clauseNumber; }
            set { clauseNumber = value; }
        }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.F)]
        public SwitchParameter F { get; set; }
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTI)]
        public SwitchParameter CTI { get; set; }
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        public SwitchParameter CTCF { get; set; }
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCTI)]
        public SwitchParameter CTCTI { get; set; }


        private string connection;
        [Parameter(Position = 2, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        [Parameter(Position = 2, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTI)]
        [Parameter(Position = 2, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCTI)]
        [ArgumentCompleter(typeof(ConnectionNameArgumentCompleter))]
        public string Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        private string categoryName;
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTI)]
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCTI)]
        [ArgumentCompleter(typeof(CategoryNameArgumentCompleter))]
        public string ToCategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        private string connection2;
        [Parameter(Position = 4, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCTI)]
        [ArgumentCompleter(typeof(ConnectionNameArgumentCompleter))]
        public string Connection2
        {
            get { return connection2; }
            set { connection2 = value; }
        }

        private string toCategoryName2;
        [Parameter(Position = 5, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCTI)]
        [ArgumentCompleter(typeof(CategoryNameArgumentCompleter))]
        public string ToCategoryName2
        {
            get { return toCategoryName2; }
            set { toCategoryName2 = value; }
        }

        // we cannot autocomplete here because we do not know
        // -(F) what category we are working in
        // -(CTCF) can't specify completer for specific ParameterSetname
        // We could display all database fields but that's probably worse than no auto-complete.
        private string fieldName;
        [Parameter(Position = 2, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.F)]
        [Parameter(Position = 4, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        private FilterQualifier qualifier;
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.F)]
        [Parameter(Position = 5, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        public FilterQualifier Qualifier
        {
            get { return qualifier; }
            set { qualifier = value; }
        }

        private string fieldValue;
        [Parameter(Position = 4, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.F)]
        [Parameter(Position = 6, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        public string FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        private string fieldValue2;
        [Parameter(Position = 5, Mandatory = false, ParameterSetName = FilterTypeParameterSetName.F)]
        [Parameter(Position = 7, Mandatory = false, ParameterSetName = FilterTypeParameterSetName.CTCF)]
        public string FieldValue2
        {
            get { return fieldValue2; }
            set { fieldValue2 = value; }
        }

        private string item;
        [Parameter(Position = 4, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTI)]
        [Parameter(Position = 6, Mandatory = true, ParameterSetName = FilterTypeParameterSetName.CTCTI)]
        public string Item
        {
            get { return item; }
            set { item = value; }
        }

        private bool matchCase;
        [Parameter(ParameterSetName = FilterTypeParameterSetName.F)]
        [Parameter(ParameterSetName = FilterTypeParameterSetName.CTCF)]
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
            ICursorFilter filter = null;
            if (F.IsPresent)
            {
                var f = new CursorFilterTypeF(clauseNumber)
                {
                    FieldName = fieldName,
                    Qualifier = qualifier,
                    MatchCase = matchCase
                };
                if (qualifier == FilterQualifier.Between)
                {
                    f.FilterBetweenStartValue = fieldValue;
                    f.FilterBetweenEndValue = fieldValue2;
                }
                if (qualifier.GetAttribute<FilterValuesAttribute>()?.Number == 1)
                {
                    f.FieldValue = fieldValue;
                }
                filter = f;
            }
            if (CTI.IsPresent)
            {
                var cti = new CursorFilterTypeCTI(clauseNumber)
                {
                    Connection = connection,
                    Category = categoryName,
                    Item = item
                };
                filter = cti;
            }
            if ((CTCF.IsPresent))
            {
                var ctcf = new CursorFilterTypeCTCF(clauseNumber)
                {
                    Connection = connection,
                    Category = categoryName,
                    FieldName = fieldName,
                    Qualifier = qualifier,
                    MatchCase = matchCase
                };
                if (qualifier == FilterQualifier.Between)
                {
                    ctcf.FilterBetweenStartValue = fieldValue;
                    ctcf.FilterBetweenEndValue = fieldValue2;
                }
                if (qualifier.GetAttribute<FilterValuesAttribute>()?.Number == 1)
                {
                    ctcf.FieldValue = fieldValue;
                }
                filter = ctcf;
            }
            if (CTCTI.IsPresent)
            {
                ICursorFilterTypeCTCTI ctcti = new CursorFilterTypeCTCTI(clauseNumber)
                {
                    Connection = connection,
                    Category = categoryName,
                    Connection2 = connection2,
                    Category2 = toCategoryName2,
                    Item = item
                };
                filter = ctcti;
            }
            filter.Except = except;
            filter.OrFilter = orFilter;
            WriteVerbose($"Resulting filterstring is: {filter}, conjunction is: [{(filter.OrFilter ? "OR" : "AND")}]");
            WriteObject(filter, false);
        }
    }
}