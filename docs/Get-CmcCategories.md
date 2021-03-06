---
external help file: PoshCommence.dll-Help.xml
Module Name: PoshCommence
online version:
schema: 2.0.0
---

# Get-CmcCategories

## SYNOPSIS
Get categories from Commence

## SYNTAX

```
Get-CmcCategories [<CommonParameters>]
```

## DESCRIPTION
Returns the category definitions from the currently active Commence database.

## EXAMPLES

### Example 1
```powershell
Get-CmcCategories
```

Returns the category definitions of the currently active Commence database.

### Example 2
```powershell
(Get-CmcCategories).Name
```

Show all category names.

### Example 3
```powershell
Get-CmcCategories | Where-Object { $_.Clarified -eq $true } | select Name, ClarifyField, ClarifySeparator
```

Show all clarified categories and their clarify field and separator.

## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
