#Region "Copyright Syncfusion Inc. 2001 - 2011"
' Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
' Use of this code is subject to the terms of our license.
' A copy of the current license can be obtained at any time by e-mailing
' licensing@syncfusion.com. Any infringement will be prosecuted under
' applicable laws. 
#End Region

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Syncfusion.Windows.Forms
Imports Syncfusion.Windows.Forms.Tools

Namespace LocalizationDemo
    Friend Class Localizer
        Implements ILocalizationProvider

        Public Function GetLocalizedString(ByVal culture As System.Globalization.CultureInfo, ByVal name As String) As String Implements Syncfusion.Windows.Forms.ILocalizationProvider.GetLocalizedString
            Select Case name
                '				#Region "Menu Package"
                Case ToolsResourceIdentifiers.CustomizeMenu
                    Return "Customize"
                Case ToolsResourceIdentifiers.CustomMenu
                    Return "Benutzerdefinierte"
                Case ToolsResourceIdentifiers.NewMenu
                    Return "Neue"
                Case ToolsResourceIdentifiers.AddOrRemoveButtons
                    Return "Schaltflächen hinzufügen oder entfernen"
                Case ToolsResourceIdentifiers.Add
                    Return "Hinzufügen"
                Case ToolsResourceIdentifiers.Delete
                    Return "Löschen"
                Case ToolsResourceIdentifiers.Rename
                    Return "Umbenennen"
                Case ToolsResourceIdentifiers.DeleteAll
                    Return "AlleLöschen"
                Case ToolsResourceIdentifiers.ResetToolBarMenu
                    Return "Symbolleiste zurücksetzen"
                    '				#End Region

                    '				#Region "BarCustomization Dialog"
                Case ToolsResourceIdentifiers.BarCustomizationDialogButtonReset
                    Return "Reset"
                Case ToolsResourceIdentifiers.BarCustomizationDialogCommands
                    Return "Löschen"
                Case ToolsResourceIdentifiers.BarCustomizationDialogNew
                    Return "Neue"
                Case ToolsResourceIdentifiers.BarCustomizationDialogTabCommands
                    Return "Befehle"
                Case ToolsResourceIdentifiers.BarCustomizationDialogModify
                    Return "Ändern"
                Case ToolsResourceIdentifiers.BarCustomizationDialogCategories
                    Return "Kategorien"
                Case ToolsResourceIdentifiers.BarCustomizationDialogTabOptions
                    Return "Optionen"
                Case ToolsResourceIdentifiers.BarCustomizationDialogResetCustomization
                    Return "Reset Anpassung"
                Case ToolsResourceIdentifiers.BarCustomizationDialogResetPartialMenus
                    Return "Reset Teilweise Menüs"
                Case ToolsResourceIdentifiers.BarCustomizationDialogLargeIcons
                    Return "Große Symbole verwenden"
                Case ToolsResourceIdentifiers.BarCustomizationDialogExpandAfterDelay
                    Return "Expand Nach Delay"
                Case ToolsResourceIdentifiers.BarCustomizationDialogPersonalizedMenus
                    Return "personalisierte Menüs"
                Case ToolsResourceIdentifiers.BarCustomizationDialogAlwaysFullMenu
                    Return "Immer anzeigen Menu"
                Case ToolsResourceIdentifiers.BarCustomizationDialogClose
                    Return "Schließen"
                Case ToolsResourceIdentifiers.BarCustomizationDialogCaption
                    Return "Anpassungsdialogfeld"
                Case ToolsResourceIdentifiers.BarCustomizationDialogDelete
                    Return "Löschen"
                Case ToolsResourceIdentifiers.BarCustomizationDialogOther
                    Return "Andere"
                Case ToolsResourceIdentifiers.BarCustomizationDialogTabToolbars, ToolsResourceIdentifiers.BarCustomizationDialogToolbars
                    Return "Symbolleisten"
                    '				#End Region

                    '				#Region "Color Editor"
                Case ResourceIdentifiers.ColorEditorPaletteTab
                    Return "Farbenskala"
                Case ResourceIdentifiers.ColorEditorStandardTab
                    Return "Niveau"
                Case ResourceIdentifiers.ColorEditorSystemTab
                    Return "Anlage"
                    '				#End Region

                    '				#Region "Menu notifications"
                Case ToolsResourceIdentifiers.RecentlyUsedItemsResetConfirm
                    Return "Damit wird die Aufzeichnung der Befehle, die Sie in dieser Anwendung verwendet haben und die Standardeinstellungen wiederherstellen sichtbare Befehle in den Menüs und Symbolleisten eingestellt. Es wird nicht explizite Anpassungen. Sind Sie sicher, dass dies zu tun?"
                Case ToolsResourceIdentifiers.RecentlyUsedItemsResetConfirmTitle
                    Return "Bestätigen zurückgesetzt"
                Case ToolsResourceIdentifiers.NotifyRecentlyUsedItemsReset
                    Return "Ihre aufgezeichneten Nutzungsdaten wurde entfernt. Die Menüs und Befehle werden Sie das nächste Mal öffnen diese Anwendung wiederhergestellt werden."
                Case ToolsResourceIdentifiers.CustomizationResetConfirm
                    Return "Dadurch werden alle expliziten Anpassungen auf die Menüs und Symbolleisten getan. Sind Sie sicher, dass dies zu tun?"
                Case ToolsResourceIdentifiers.NotifyCustomizationReset
                    Return "Maßgeschneiderte Informationen zurückgesetzt wird. Allerdings werden die Menüs und Befehle nur in den Grundzustand wiederhergestellt werden Sie das nächste Mal öffnen diese Anwendung."
                Case ToolsResourceIdentifiers.CustomizationResetConfirmTitle
                    Return "Bestätigen zurückgesetzt"
                Case ToolsResourceIdentifiers.NotifyCustomizationResetTitle
                    Return "Customization Informationen zum Zurücksetzen"
                    '				#End Region

                Case ResourceIdentifiers.Retry
                    Return "Wiederholen"
                Case ResourceIdentifiers.Cancel
                    Return "Abbrechen"

                Case Else
                    Return String.Empty
            End Select
        End Function

    End Class
End Namespace


